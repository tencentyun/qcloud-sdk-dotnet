using System;
using System.Collections.Generic;
using System.Text;
using COSXML.Model.Object;
using COSXML.Utils;
using COSXML.Model;
using System.IO;
using COSXML.Log;
using COSXML.CosException;
using System.Xml.Serialization;
using COSXML.Common;
using System.Threading;

namespace COSXML.Transfer
{
    public sealed class COSXMLDownloadTask : COSXMLTask
    {
        // user params
        private string localDir;
        private string localFileName;
        private long localFileOffset = 0;
        private long rangeStart = 0L;
        private long rangeEnd = -1L;
        private int maxTasks = 5;
        private long sliceSize = 5 * 1024 * 1024;
        private long divisionSize = 20 * 1024 * 1024;
        private bool enableCrc64Check = false;

        // concurrency control
        private volatile int activeTasks = 0;
        private int maxRetries = 3;
        private Object syncExit = new Object();
        private bool isExit = false;
        private static ReaderWriterLockSlim resumableFileWriteLock = new ReaderWriterLockSlim();
        private Dictionary<int, DownloadSliceStruct> sliceList = new Dictionary<int, DownloadSliceStruct>();
        private static ReaderWriterLockSlim targetFileLock = new ReaderWriterLockSlim();

        // internal requests
        private HeadObjectRequest headObjectRequest;
        private GetObjectRequest getObjectRequest;
        private string localFileCrc64;

        // resumable info
        private string resumableTaskFile = null;
        private DownloadResumableInfo resumableInfo = null;
        private bool resumable = false;
        private HashSet<string> tmpFilePaths = new HashSet<string>();
        private HashSet<int> sliceToRemove = null;

        // global exception
        private COSXML.CosException.CosClientException gClientExp = null;

        public COSXMLDownloadTask(string bucket, string key, string localDir, string localFileName)
            : base(bucket, key)
        {
            if (localDir.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                this.localDir = localDir;
            }
            else
            {
                this.localDir = localDir + System.IO.Path.DirectorySeparatorChar;
            }
            this.localFileName = localFileName;
        }

        public COSXMLDownloadTask(GetObjectRequest request)
            : base(request.Bucket, request.Key)
        {
            this.getObjectRequest = request;
            if (request.localDir.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                this.localDir = request.localDir;
            }
            else
            {
                this.localDir = request.localDir + System.IO.Path.DirectorySeparatorChar;
            }
            this.localFileName = request.localFileName;
        }

        public void SetRange(long rangeStart, long rangeEnd)
        {
            this.rangeStart = rangeStart;
            this.rangeEnd = rangeEnd;
        }

        public void SetLocalFileOffset(long localFileOffset)
        {
            this.localFileOffset = localFileOffset;
        }

        public void SetResumableDownload(bool resumable)
        {
            this.resumable = resumable;
        }

        public void SetResumableTaskFile(string file)
        {
            this.resumableTaskFile = file;
        }

        public string GetLocalFileCrc64()
        {
            return localFileCrc64;
        }

        public void SetMaxTasks(int maxTasks)
        {
            if (maxTasks <= 0) 
            {
                throw new COSXML.CosException.CosClientException((int) CosClientError.InvalidArgument, "max tasks cannot be negative or zero");
                return;
            }
            this.maxTasks = maxTasks;
        }

        public void SetSliceSize(long sliceSize)
        {
            if (sliceSize <= 0)
            {
                throw new COSXML.CosException.CosClientException((int) CosClientError.InvalidArgument, "slice size cannot be negative or zero");
                return;
            }
            this.sliceSize = sliceSize;
        }

        public void SetDivisionSize(long divisionSize)
        {
            if (divisionSize <= 0)
            {
                throw new COSXML.CosException.CosClientException((int) CosClientError.InvalidArgument, "division size cannot be negative or zero");
                return;
            }
            this.divisionSize = divisionSize;
        }

        private void SetEnableCRC64Check(bool enableCrc64Check)
        {
            this.enableCrc64Check = enableCrc64Check;
        }

        private void ComputeSliceList(HeadObjectResult result)
        {
            // slice list can be not empty, if use pause&resume, skip it
            if (this.sliceList.Count != 0)
            {
                return;
            }
            long contentLength = result.size;
            rangeEnd = rangeEnd == -1L || (rangeEnd > contentLength) ? contentLength - 1 : rangeEnd;
            if (rangeEnd - rangeStart + 1 < this.divisionSize)
            {
                DownloadSliceStruct slice = new DownloadSliceStruct();
                slice.partNumber = 1;
                slice.sliceStart = rangeStart;
                slice.sliceEnd = rangeEnd;
                this.sliceList.Add(slice.partNumber, slice);
            }
            else
            {
                long sliceCount = ((rangeEnd - rangeStart) / this.sliceSize) + 1;
                for (int i = 0; i < sliceCount; i++)
                {
                    DownloadSliceStruct slice = new DownloadSliceStruct();
                    slice.partNumber = i + 1;
                    slice.sliceStart = rangeStart + i * this.sliceSize;
                    slice.sliceEnd = 
                        (slice.sliceStart + this.sliceSize > rangeEnd)
                        ? rangeEnd : slice.sliceStart + this.sliceSize - 1;
                    this.sliceList.Add(slice.partNumber, slice);
                }
            }
        }

        internal void Download()
        {
            UpdateTaskState(TaskState.Waiting);
            //对象是否存在
            headObjectRequest = new HeadObjectRequest(bucket, key);

            cosXmlServer.HeadObject(headObjectRequest, delegate (CosResult cosResult)
            {
                lock (syncExit)
                {

                    if (isExit)
                    {
                        return;
                    }
                }

                if (UpdateTaskState(TaskState.Running))
                {
                    HeadObjectResult result = cosResult as HeadObjectResult;
                    ComputeSliceList(result);
                    // resolv resumeInfo
                    if (resumable)
                    {
                        if (resumableTaskFile == null)
                        {
                            resumableTaskFile = localDir + localFileName + ".cosresumabletask";
                        }
                        ResumeDownloadInPossible(result, localDir + localFileName);
                    }
                    // concurrent download
                    GetObject(result.crc64ecma);
                } 

            },
            
            delegate (CosClientException clientEx, CosServerException serverEx)
            {
                lock (syncExit)
                {

                    if (isExit)
                    {
                        return;
                    }
                }
                if (UpdateTaskState(TaskState.Failed))
                {
                    if (failCallback != null)
                    {
                        failCallback(clientEx, serverEx);
                    }
                }

            });
        }

        // resolve resumable task file, continue in proper position
        private void ResumeDownloadInPossible(HeadObjectResult result, string localFile)
        {
            this.resumableInfo = DownloadResumableInfo.LoadFromResumableFile(resumableTaskFile);
            if (this.resumableInfo != null)
            {
                if ((result.crc64ecma == null || result.crc64ecma == resumableInfo.crc64ecma) && 
                    this.resumableInfo.eTag == result.eTag && 
                    this.resumableInfo.lastModified == result.lastModified && 
                    this.resumableInfo.contentLength == result.size)
                {
                    // load parts downloaded
                    if (this.resumableInfo.slicesDownloaded != null)
                    {
                        // process downloaded parts
                        foreach (DownloadSliceStruct downloadedSlice in resumableInfo.slicesDownloaded)
                        {
                            // remove from current queue
                            DownloadSliceStruct calculatedSlice;
                            bool ret = this.sliceList.TryGetValue(downloadedSlice.partNumber, out calculatedSlice);
                            if (!ret) {
                                // resumable file broken
                                break;
                            }
                            if (calculatedSlice.sliceStart == downloadedSlice.sliceStart
                                && calculatedSlice.sliceEnd == downloadedSlice.sliceEnd)
                            {
                                if (this.sliceToRemove == null)
                                    this.sliceToRemove = new HashSet<int>();
                                this.sliceToRemove.Add(downloadedSlice.partNumber);
                            }
                            // add to merging list
                            string tmpFileName = localDir + "." + localFileName + ".cosresumable." + downloadedSlice.partNumber;
                            this.tmpFilePaths.Add(tmpFileName);
                        }
                    }
                    else
                    {
                        this.resumableInfo.slicesDownloaded = new List<DownloadSliceStruct>();
                    }
                }
            }
            else
            {
                this.resumableInfo = new DownloadResumableInfo();
                this.resumableInfo.contentLength = result.size;
                this.resumableInfo.crc64ecma = result.crc64ecma;
                this.resumableInfo.eTag = result.eTag;
                this.resumableInfo.lastModified = result.lastModified;
                this.resumableInfo.slicesDownloaded = new List<DownloadSliceStruct>();
                resumableInfo.Persist(resumableTaskFile);
            }
        }

        private bool CompareCrc64(string localFile, string crc64ecma)
        {
            Crc64.InitECMA();
            String hash = String.Empty;

            using (FileStream fs = File.Open(localFile, FileMode.Open))
            {
                byte[] buffer = new byte[2048];
                int bytesRead;
                ulong crc = 0;

                while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0) 
                {
                    ulong partCrc = Crc64.Compute(buffer, 0, bytesRead);
                    if (crc == 0) 
                    {
                        crc = partCrc;
                    }
                    else 
                    {
                        crc = Crc64.Combine(crc, partCrc, bytesRead);
                    }
                }

                localFileCrc64 = crc.ToString();
                return localFileCrc64 == crc64ecma;
            }
        }

        // actual get object requests with concurrency control
        private void GetObject(string crc64ecma)
        {
            lock (syncExit) 
            {
                if (isExit) 
                {
                    return;
                }
            }
            // create dir if not exist
            DirectoryInfo dirInfo = new DirectoryInfo(localDir);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            // concurrency control
            AutoResetEvent resetEvent = new AutoResetEvent(false);
            int retries = 0;
            // return last response
            GetObjectResult downloadResult = null;
            if (sliceToRemove == null)
                sliceToRemove = new HashSet<int>();
            while (sliceList.Count != 0 && retries < maxRetries)
            {
                retries += 1;
                foreach (int partNumber in sliceList.Keys)
                {
                    if (sliceToRemove.Contains(partNumber))
                    {
                        continue;
                    }
                    DownloadSliceStruct slice;
                    bool get_state = sliceList.TryGetValue(partNumber, out slice);
                    if (activeTasks >= maxTasks)
                    {
                        resetEvent.WaitOne();
                    }
                    lock (syncExit)
                    {
                        if (isExit)
                        {
                            return;
                        }
                    }
                    string tmpFileName = "." + localFileName + ".cosresumable." + slice.partNumber;
                    // clear remainance
                    FileInfo tmpFileInfo = new FileInfo(localDir + tmpFileName);
                    if (tmpFileInfo.Exists 
                        && tmpFileInfo.Length != (slice.sliceEnd - slice.sliceStart + 1) 
                        && localFileOffset != 0)
                    {
                        System.IO.File.Delete(localDir + tmpFileName);
                    }
                    getObjectRequest = new GetObjectRequest(bucket, key, localDir, tmpFileName);
                    tmpFilePaths.Add(localDir + tmpFileName);
                    getObjectRequest.SetRange(slice.sliceStart, slice.sliceEnd);
                    if (progressCallback != null && this.sliceList.Count == 1)
                    {
                        getObjectRequest.SetCosProgressCallback(delegate(long completed, long total)
                            {
                                progressCallback(completed, total);
                            }
                        );
                    }
                    Interlocked.Increment(ref activeTasks);
                    cosXmlServer.GetObject(getObjectRequest,
                        delegate (CosResult result)
                        {
                            Interlocked.Decrement(ref activeTasks);
                            lock (syncExit)
                            {
                                if (isExit)
                                {
                                    return;
                                }
                            }
                            sliceToRemove.Add(partNumber);
                            if (progressCallback != null && this.sliceList.Count > 1)
                            {
                                long completed = sliceToRemove.Count * this.sliceSize;
                                long total = rangeEnd - rangeStart;
                                if (completed > total)
                                    completed = total;
                                progressCallback(completed, total);
                            }
                            downloadResult = result as GetObjectResult;
                            resetEvent.Set();
                            if (resumable)
                            {
                                // flush done parts
                                this.resumableInfo.slicesDownloaded.Add(slice);
                                this.resumableInfo.Persist(resumableTaskFile);
                            }
                        }, 
                        delegate (CosClientException clientEx, CosServerException serverEx)
                        {
                            Interlocked.Decrement(ref activeTasks);
                            lock (syncExit)
                            {
                                if (isExit)
                                {
                                    return;
                                }
                            }
                            // server 4xx throw and stop
                            if (serverEx != null && serverEx.statusCode < 500) {
                                throw serverEx;
                                if (failCallback != null)
                                {
                                    failCallback(null, serverEx);
                                }
                                return;
                            }
                            // client cannot connect, just retry
                            if (clientEx != null)
                                gClientExp = clientEx;
                            resetEvent.Set();
                        }
                    );
                }
                while (activeTasks != 0)
                {
                    Thread.Sleep(100);
                }
                // remove success parts
                foreach (int partNumber in sliceToRemove)
                {
                    sliceList.Remove(partNumber);
                }
            }
            if (this.sliceList.Count != 0) 
            {
                if (gClientExp != null)
                {
                    throw gClientExp;
                }
                COSXML.CosException.CosClientException clientEx = new COSXML.CosException.CosClientException
                    ((int)CosClientError.InternalError, "max retries " + retries + " excceed, download fail");
                throw clientEx;
                if (UpdateTaskState(TaskState.Failed))
                {
                    if (failCallback != null)
                    {
                        failCallback(clientEx, null);
                    }
                }
                return;
            } 
            // file merge
            FileMode fileMode = FileMode.OpenOrCreate;
            FileInfo localFileInfo = new FileInfo(localDir + localFileName);
            if (localFileInfo.Exists && localFileOffset == 0 && localFileInfo.Length != rangeEnd - rangeStart + 1)
                fileMode = FileMode.Truncate;
            using (var outputStream = File.Open(localDir + localFileName, fileMode))
            {
                outputStream.Seek(localFileOffset, 0);
                // sort
                List<string> tmpFileList = new List<string>(this.tmpFilePaths);
                tmpFileList.Sort(delegate(string x, string y){
                    int partNumber1 = int.Parse(x.Split(new string[]{"cosresumable."}, StringSplitOptions.None)[1]);
                    int partNumber2 = int.Parse(y.Split(new string[]{"cosresumable."}, StringSplitOptions.None)[1]);
                    return partNumber1 - partNumber2;
                });
                foreach (var inputFilePath in tmpFileList)
                {
                    // tmp not exist, clear everything and ask for retry
                    if (!File.Exists(inputFilePath))
                    {
                        // check if download already completed
                        if (File.Exists(localDir + localFileName))
                        {
                            FileInfo fileInfo = new FileInfo(localDir + localFileName);
                            if (fileInfo.Length == rangeEnd - rangeStart + 1)
                            {
                                foreach (var tmpFile in tmpFileList)
                                {
                                    System.IO.File.Delete(tmpFile);
                                }
                                if (resumableTaskFile != null)
                                {
                                    System.IO.File.Delete(resumableTaskFile);
                                }
                                break;
                            }
                        }
                        // not completed, report fatal error
                        foreach (var tmpFile in tmpFileList)
                        {
                            System.IO.File.Delete(tmpFile);
                        }
                        if (resumableTaskFile != null)
                        {
                            System.IO.File.Delete(resumableTaskFile);
                        }
                        if (File.Exists(localDir + localFileName))
                        {
                            System.IO.File.Delete(localDir + localFileName);
                        }
                        COSXML.CosException.CosClientException clientEx = new COSXML.CosException.CosClientException
                            ((int)CosClientError.InternalError, "local tmp file not exist, could be concurrent writing same file" 
                            + inputFilePath +" download again");
                        throw clientEx;
                        if (failCallback != null)
                        {
                            failCallback(clientEx, null);
                        }
                        break;
                    }
                    using (var inputStream = File.OpenRead(inputFilePath))
                    {
                        FileInfo info = new FileInfo(inputFilePath);
                        inputStream.CopyTo(outputStream);
                    }
                }
                tmpFileList.Clear();
                tmpFilePaths.Clear();
                if (UpdateTaskState(TaskState.Completed))
                {
                    var dir = new DirectoryInfo(localDir);
                    foreach (var file in dir.EnumerateFiles("." + localFileName + ".cosresumable.*")) {
                        file.Delete();
                    }
                    if (resumableTaskFile != null)
                    {
                        FileInfo info = new FileInfo(resumableTaskFile);
                        if (info.Exists)
                        {
                            info.Delete();
                        }
                    }
                    DownloadTaskResult downloadTaskResult = new DownloadTaskResult();
                    downloadTaskResult.SetResult(downloadResult);
                    outputStream.Close();
                    if (successCallback != null)
                    {
                        successCallback(downloadTaskResult);
                    }
                    return;
                } else {
                    // 容灾 return
                    DownloadTaskResult downloadTaskResult = new DownloadTaskResult();
                    downloadTaskResult.SetResult(downloadResult);
                    outputStream.Close();
                    if (successCallback != null)
                    {
                        successCallback(downloadTaskResult);
                    }
                }
            }
            return;
        }

        private void RealCancle()
        {
            //cancle request
            cosXmlServer.Cancel(headObjectRequest);
            cosXmlServer.Cancel(getObjectRequest);
            // Cancel success, remove one task
            Interlocked.Decrement(ref activeTasks);
            // wait for tasks to finish
            while (activeTasks > 0)
            {
                Thread.Sleep(100);
            }
        }

        private void Clear()
        {
            // delete tmp files
            var dir = new DirectoryInfo(localDir);
            foreach (var file in dir.EnumerateFiles("." + localFileName + ".cosresumable.*")) {
                file.Delete();
            }
            // delete resumable file
            if (resumableTaskFile != null)
            {
                FileInfo info = new FileInfo(resumableTaskFile);
                if (info.Exists)
                {
                    info.Delete();
                }
            }
        }

        public override void Pause()
        {

            if (UpdateTaskState(TaskState.Pause))
            {
                //exit download
                lock (syncExit) 
                { 
                    isExit = true; 
                }
                //cancle request
                RealCancle();
            }
        }

        public override void Cancel()
        {

            if (UpdateTaskState(TaskState.Cancel))
            {
                //exit copy
                lock (syncExit) 
                { 
                    isExit = true; 
                }

                //cancle request
                RealCancle();
                //clear recoder
                Clear();
            }
        }

        public override void Resume()
        {

            if (UpdateTaskState(TaskState.Resume))
            {
                lock (syncExit)
                {
                    //continue to download
                    isExit = false;
                }
                Download();
            }
        }

        public class DownloadTaskResult : CosResult
        {
            public string eTag;

            public void SetResult(GetObjectResult result)
            {
                this.eTag = result.eTag;
                this.httpCode = result.httpCode;
                this.httpMessage = result.httpMessage;
                this.responseHeaders = result.responseHeaders;
            }

            public override string GetResultInfo()
            {

                return base.GetResultInfo() + ("\n : ETag: " + eTag);
            }
        }

        public sealed class DownloadResumableInfo 
        {
            public string lastModified;

            public long contentLength;

            public string eTag;

            public string crc64ecma;

            public List<DownloadSliceStruct> slicesDownloaded = new List<DownloadSliceStruct>();

            public static DownloadResumableInfo LoadFromResumableFile(string taskFile)
            {
                try
                {
                    using (FileStream stream = File.OpenRead(taskFile))
                    {
                        DownloadResumableInfo resumableInfo = XmlParse.Deserialize<DownloadResumableInfo>(stream);
                        return resumableInfo;
                    }
                }
                catch (System.Exception)
                {
                    return null;
                }
            }

            public void Persist(string taskFile)
            {
                try
                {
                    resumableFileWriteLock.EnterWriteLock();
                    string xml = XmlBuilder.Serialize(this);
                    using (FileStream stream = File.Create(taskFile))
                    {
                        byte[] info = new UTF8Encoding(true).GetBytes(xml);
                        stream.Write(info, 0, info.Length);
                    }
                }
                catch (Exception)
                {
                    return;
                }
                finally
                {
                    resumableFileWriteLock.ExitWriteLock();
                }
            }
        }
    }
}

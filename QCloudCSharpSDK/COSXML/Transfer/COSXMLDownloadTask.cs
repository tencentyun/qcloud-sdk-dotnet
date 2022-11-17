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
        private long sliceSize = 10 * 1024 * 1024;
        private long divisionSize = 20 * 1024 * 1024;
        private bool enableCrc64Check = false;
        private long singleTaskTimeoutMs = 30 * 1000;

        // concurrency control
        private volatile int activeTasks = 0;
        private int maxRetries = 3;
        private Object syncExit = new Object();
        private bool isExit = false;
        private static ReaderWriterLockSlim resumableFileWriteLock = new ReaderWriterLockSlim();
        private Dictionary<int, DownloadSliceStruct> sliceList = new Dictionary<int, DownloadSliceStruct>();

        // internal requests
        private HeadObjectRequest headObjectRequest;
        private GetObjectRequest getObjectRequest;
        private string localFileCrc64;

        // list of ongoing getObjectRequest
        private List<GetObjectRequest> getObjectRequestsList;

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

        public void SetEnableCRC64Check(bool enableCrc64Check)
        {
            this.enableCrc64Check = enableCrc64Check;
        }

        public void SetSingleTaskTimeoutMs(long singleTaskTimeoutMs)
        {
            if (singleTaskTimeoutMs > 0)
            {
                this.singleTaskTimeoutMs = singleTaskTimeoutMs;
            }
        }

        public void SetMaxRetries(int maxRetries)
        {
            if (maxRetries > 0)
            {
                this.maxRetries = maxRetries;
            }
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
                    ConcurrentGetObject(result.crc64ecma);
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

        // 发起多线程下载
        private void ConcurrentGetObject(string crc64ecma)
        {
            lock (syncExit) 
            {
                if (isExit) 
                {
                    return;
                }
            }
            // 保障目标路径是存在的
            DirectoryInfo dirInfo = new DirectoryInfo(localDir);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            // 控制任务数
            AutoResetEvent resetEvent = new AutoResetEvent(false);
            // 记录成功的分片
            if (sliceToRemove == null)
            {
                sliceToRemove = new HashSet<int>();
            }
            // 记录子任务
            if (getObjectRequestsList == null)
            {
                getObjectRequestsList = new List<GetObjectRequest>();
            }
            int retries = 0;
            // 只抛出最后一条服务端回包
            GetObjectResult downloadResult = null;
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
                    /*
                        以下几种场景需要清理临时文件
                        1. 一次下载中, 重试未下载完成的文件
                        2. Pause之后重入下载, 清理Pause前未下载完成的文件
                        3. 断点续传中, 清理未下载完成的单个分块
                    */
                    FileInfo tmpFileInfo = new FileInfo(localDir + tmpFileName);
                    if (tmpFileInfo.Exists 
                        && tmpFileInfo.Length != (slice.sliceEnd - slice.sliceStart + 1) 
                        && localFileOffset != 0)
                    {
                        System.IO.File.Delete(localDir + tmpFileName);
                    }
                    GetObjectRequest subGetObjectRequest = new GetObjectRequest(bucket, key, localDir, tmpFileName);
                    tmpFilePaths.Add(localDir + tmpFileName);
                    subGetObjectRequest.SetRange(slice.sliceStart, slice.sliceEnd);
                    getObjectRequestsList.Add(subGetObjectRequest);
                    // 计算出来只有一个分块, 而且不是Resume或重试剩的一个, 即不走并发下载, 用GetObject的进度回调给客户端
                    if (progressCallback != null && this.sliceList.Count == 1 && sliceToRemove.Count == 0)
                    {
                        subGetObjectRequest.SetCosProgressCallback(delegate(long completed, long total)
                            {
                                progressCallback(completed, total);
                            }
                        );
                    }
                    Interlocked.Increment(ref activeTasks);
                    cosXmlServer.GetObject(subGetObjectRequest,
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
                            // 对服务端返回的4xx, 不重试, 直接抛异常
                            if (serverEx != null && serverEx.statusCode < 500) {
                                throw serverEx;
                                if (failCallback != null)
                                {
                                    failCallback(null, serverEx);
                                }
                                return;
                            }
                            // 对客户端异常, 全部都重试
                            if (clientEx != null) {
                                gClientExp = clientEx;
                            }
                            resetEvent.Set();
                        }
                    );
                }
                long waitTimeMs = 0;
                int lastActiveTasks = activeTasks;
                while (activeTasks != 0)
                {
                    lock (syncExit)
                    {
                        if (isExit)
                        {
                            return;
                        }
                    }
                    Thread.Sleep(100);
                    if (lastActiveTasks == activeTasks) {
                        /*
                            兼容一种子任务既不成功，也不失败，完全hang住的场景
                            在 .net core + 丢包率高的使用场景下会概率性复现
                            当activeTasks一直不变时，累加一个等待的时间
                            超出 singleTaskTimeoutMs 时，全部清理掉进入下一轮重试
                        */
                        waitTimeMs += 100;
                    } else {
                        waitTimeMs = 0;
                        lastActiveTasks = activeTasks;
                    }
                    if (waitTimeMs > singleTaskTimeoutMs) {
                        foreach(GetObjectRequest subGetObjectRequest in getObjectRequestsList) {
                            try {
                                cosXmlServer.Cancel(subGetObjectRequest);
                            } catch (Exception e) {
                                ;
                            }
                        }
                        getObjectRequestsList.Clear();
                        activeTasks = 0;
                        break;
                    }
                }
                // 从下载列表中移除成功分块
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
            // 预期每个分块都下载完成了, 开始顺序合并
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
                outputStream.Close();
                // 合并完成后，默认进行文件大小的检查
                FileInfo completedFileInfo = new FileInfo(localDir + localFileName);
                if (completedFileInfo.Length != rangeEnd - rangeStart + 1) {
                    COSXML.CosException.CosClientException clientEx = new COSXML.CosException.CosClientException
                        ((int)CosClientError.InternalError, "local File Length " + completedFileInfo.Length + 
                        " does not equals to applied download length " + (rangeEnd - rangeStart + 1) + ", try again");
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
                // 按需进行CRC64的检查
                if (enableCrc64Check) {
                    if (!CompareCrc64(localDir + localFileName, crc64ecma)) {
                        COSXML.CosException.CosClientException clientEx = new COSXML.CosException.CosClientException
                        ((int)CosClientError.CRC64ecmaCheckFailed, "local File Crc64 " + 
                        " does not equals to crc64ecma on cos, try download again");
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
                }
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
                    //outputStream.Close();
                    DownloadTaskResult downloadTaskResult = new DownloadTaskResult();
                    downloadTaskResult.SetResult(downloadResult);
                    if (successCallback != null)
                    {
                        successCallback(downloadTaskResult);
                    }
                    return;
                } else {
                    // 容灾 return
                    DownloadTaskResult downloadTaskResult = new DownloadTaskResult();
                    downloadTaskResult.SetResult(downloadResult);
                    //outputStream.Close();
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
            // 停止可能进行中的Head请求
            cosXmlServer.Cancel(headObjectRequest);
            // 停止可能进行中的下载线程
            foreach (GetObjectRequest subGetObjectRequest in getObjectRequestsList) {
                cosXmlServer.Cancel(subGetObjectRequest);
            }
            getObjectRequestsList.Clear();
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
                // throw exception if requested
                if (throwExceptionIfCancelled) {
                    throw new CosClientException((int)CosClientError.UserCancelled, "Download Task Cancelled by user");
                }
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

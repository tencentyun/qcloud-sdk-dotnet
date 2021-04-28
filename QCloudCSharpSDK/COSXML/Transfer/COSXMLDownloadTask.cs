using System;
using System.Collections.Generic;
using System.Text;
using COSXML.Model.Object;
using COSXML.Utils;
using COSXML.Model;
using System.IO;
using COSXML.CosException;
using System.Xml.Serialization;
using System.Text;
using COSXML.Common;

namespace COSXML.Transfer
{
    public sealed class COSXMLDownloadTask : COSXMLTask
    {
        private string localDir;

        private string localFileName;

        private long localFileOffset;

        private long rangeStart = -1L;

        private long rangeEnd = -1L;

        private HeadObjectRequest headObjectRequest;

        private GetObjectRequest getObjectRequest;

        private Object syncExit = new Object();

        private bool isExit = false;

        private bool resumable = false;
        private string resumableTaskFile = null;

        private string localFileCrc64;

        public COSXMLDownloadTask(string bucket, string key, string localDir, string localFileName)
            : base(bucket, key)
        {
            this.localDir = localDir;
            this.localFileName = localFileName;
        }

        public COSXMLDownloadTask(GetObjectRequest request)
            : base(request.Bucket, request.Key)
        {
            this.getObjectRequest = request;
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

        internal void Download()
        {
            UpdateTaskState(TaskState.Waiting);
            //对象是否存在
            headObjectRequest = new HeadObjectRequest(bucket, key);

            if (getObjectRequest == null)
            {
                getObjectRequest = new GetObjectRequest(bucket, key, localDir, localFileName);
            }

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
                    //计算range
                    if (resumable) 
                    {
                        if (resumableTaskFile == null)
                        {
                            resumableTaskFile = getObjectRequest.GetSaveFilePath() + ".cosresumabletask";
                        }
                        resumeDownloadInPossible(result, getObjectRequest.GetSaveFilePath());
                    }

                    //download
                    GetObject(getObjectRequest, result.crc64ecma);
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

        private void resumeDownloadInPossible(HeadObjectResult result, string localFile)
        {
            DownloadResumableInfo resumableInfo = DownloadResumableInfo.loadFromResumableFile(resumableTaskFile);
            
            if (resumableInfo != null)
            {
                if ((result.crc64ecma == null || result.crc64ecma == resumableInfo.crc64ecma) && 
                resumableInfo.eTag == result.eTag && 
                resumableInfo.lastModified == result.lastModified && 
                resumableInfo.contentLength == result.size)
                {
                    // remote file remain unchange after last download
                    FileInfo localFileInfo = new FileInfo(localFile);
                    if (localFileInfo.Exists && localFileInfo.Length < result.size)
                    {
                        rangeStart = localFileInfo.Length;
                    }
                }
            }
            else
            {
                resumableInfo = new DownloadResumableInfo();
                resumableInfo.contentLength = result.size;
                resumableInfo.crc64ecma = result.crc64ecma;
                resumableInfo.eTag = result.eTag;
                resumableInfo.lastModified = result.lastModified;

                resumableInfo.persist(resumableTaskFile);
            }
        }

        private bool compareCrc64(string localFile, string crc64ecma)
        {
            CRC64.InitECMA();
            String hash = String.Empty;

            using (FileStream fs = File.Open(localFile, FileMode.Open))
            {
                byte[] buffer = new byte[2048];
                int bytesRead;
                ulong crc = 0;
                while((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0) {
                    ulong partCrc = CRC64.Compute(buffer, 0, bytesRead);
                    if (crc == 0) 
                    {
                        crc = partCrc;
                    }
                    else 
                    {
                        crc = CRC64.Combine(crc, partCrc, bytesRead);
                    }
                }
                localFileCrc64 = crc.ToString();
                return localFileCrc64 == crc64ecma;
            }
        }

        private void GetObject(GetObjectRequest getObjectRequest, string crc64ecma)
        {

            if (progressCallback != null)
            {
                getObjectRequest.SetCosProgressCallback(progressCallback);
            }

            getObjectRequest.SetRange(rangeStart, rangeEnd);
            getObjectRequest.SetLocalFileOffset(localFileOffset);
            cosXmlServer.GetObject(getObjectRequest, delegate (CosResult result)
            {
                if (resumableTaskFile != null)
                {
                    FileInfo info = new FileInfo(resumableTaskFile);
                    if (info.Exists)
                    {
                        info.Delete();
                    }
                }

                lock (syncExit)
                {

                    if (isExit)
                    {

                        return;
                    }
                }

                if (resumable && crc64ecma != null && !compareCrc64(getObjectRequest.GetSaveFilePath(), crc64ecma))
                {
                    // crc64 is not match
                    if (UpdateTaskState(TaskState.Failed))
                    {

                        if (failCallback != null)
                        {
                            failCallback(new CosClientException((int) CosClientError.IOError, "crc64 not match"), null);
                        }
                    }
                }
                else if (UpdateTaskState(TaskState.Completed))
                {
                    GetObjectResult getObjectResult = result as GetObjectResult;
                    DownloadTaskResult downloadTaskResult = new DownloadTaskResult();

                    downloadTaskResult.SetResult(getObjectResult);

                    if (successCallback != null)
                    {
                        successCallback(downloadTaskResult);
                    }
                }
            }
            
            , delegate (CosClientException clientEx, CosServerException serverEx)
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

        private void RealCancle()
        {
            //cancle request
            cosXmlServer.Cancel(headObjectRequest);
            cosXmlServer.Cancel(getObjectRequest);
        }

        private void Clear()
        {

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

            public static DownloadResumableInfo loadFromResumableFile(string taskFile)
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

            public void persist(string taskFile)
            {
                string xml = XmlBuilder.Serialize(this);
                using (FileStream stream = File.Create(taskFile))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(xml);
                    stream.Write(info, 0, info.Length);
                }
            }
        }
    }
}

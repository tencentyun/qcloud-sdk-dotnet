using System;
using System.Collections.Generic;
using System.Text;
using COSXML.Model.Object;
using COSXML.Utils;
using COSXML.Model;
using COSXML.CosException;

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
                    //计算range

                    //download
                    GetObject();
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

        private void GetObject()
        {

            if (getObjectRequest == null)
            {
                getObjectRequest = new GetObjectRequest(bucket, key, localDir, localFileName);
            }

            if (progressCallback != null)
            {
                getObjectRequest.SetCosProgressCallback(progressCallback);
            }

            getObjectRequest.SetRange(rangeStart, rangeEnd);
            getObjectRequest.SetLocalFileOffset(localFileOffset);
            cosXmlServer.GetObject(getObjectRequest, delegate (CosResult result)
            {
                lock (syncExit)
                {

                    if (isExit)
                    {

                        return;
                    }
                }

                if (UpdateTaskState(TaskState.Completed))
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
    }
}

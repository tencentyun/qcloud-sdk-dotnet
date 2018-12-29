using System;
using System.Collections.Generic;
using System.Text;
using COSXML.Model.Object;
using COSXML.Utils;
using COSXML.Model;
using COSXML.CosException;
/**
* Copyright (c) 2018 Tencent Cloud. All rights reserved.
* 11/29/2018 4:58:58 PM
* bradyxiao
*/
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

        public COSXMLDownloadTask(string bucket, string region, string key, string localDir, string localFileName)
            : base(bucket, region, key)
        {
            this.localDir = localDir;
            this.localFileName = localFileName;
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
            UpdateTaskState(TaskState.WAITTING);
            //对象是否存在
            headObjectRequest = new HeadObjectRequest(bucket, key);
            headObjectRequest.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
            cosXmlServer.HeadObject(headObjectRequest, delegate(CosResult cosResult)
            {
                lock (syncExit)
                {
                    if (isExit)
                    {
                        return;
                    }
                }
                if (UpdateTaskState(TaskState.RUNNING))
                {
                    HeadObjectResult result = cosResult as HeadObjectResult;
                    //计算range

                    //download
                    GetObject();
                }

            },
            delegate(CosClientException clientEx, CosServerException serverEx)
            {
                lock (syncExit)
                {
                    if (isExit)
                    {
                        return;
                    }
                }
                if (UpdateTaskState(TaskState.FAILED))
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
            getObjectRequest = new GetObjectRequest(bucket, key, localDir, localFileName);
            getObjectRequest.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
            if (progressCallback != null)
            {
                getObjectRequest.SetCosProgressCallback(progressCallback);
            }
            getObjectRequest.SetRange(rangeStart, rangeEnd);
            getObjectRequest.SetLocalFileOffset(localFileOffset);
            cosXmlServer.GetObject(getObjectRequest, delegate(CosResult result)
            {
                lock (syncExit)
                {
                    if (isExit)
                    {
                        return;
                    }
                }
                if (UpdateTaskState(TaskState.COMPLETED))
                {
                    GetObjectResult getObjectResult = result as GetObjectResult;
                    DownloadTaskResult downloadTaskResult = new DownloadTaskResult();
                    downloadTaskResult.SetResult(getObjectResult);

                    if (successCallback != null)
                    {
                        successCallback(downloadTaskResult);
                    }
                }
            }, delegate(CosClientException clientEx, CosServerException serverEx)
            {
                lock (syncExit)
                {
                    if (isExit)
                    {
                        return;
                    }
                }
                if (UpdateTaskState(TaskState.FAILED))
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
            if (UpdateTaskState(TaskState.PAUSE))
            {
                lock (syncExit) { isExit = true; }//exit download
                //cancle request
                RealCancle();
            }
        }

        public override void Cancel()
        {
            if (UpdateTaskState(TaskState.CANCEL))
            {
                lock (syncExit) { isExit = true; }//exit copy
                //cancle request
                RealCancle();
                //clear recoder
                Clear();
            }
        }

        public override void Resume()
        {
            if (UpdateTaskState(TaskState.RESUME))
            {
                lock (syncExit)
                {
                    isExit = false;//continue to download
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

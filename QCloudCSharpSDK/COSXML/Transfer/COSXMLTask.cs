using System;
using System.Collections.Generic;
using System.Text;
using COSXML.Model;
using COSXML.Model.Object;
using COSXML.CosException;
/**
* Copyright (c) 2018 Tencent Cloud. All rights reserved.
* 11/29/2018 5:08:32 PM
* bradyxiao
*/
namespace COSXML.Transfer
{
    public abstract class COSXMLTask
    {
        public COSXML.Callback.OnProgressCallback progressCallback;

        public COSXML.Callback.OnSuccessCallback<CosResult> successCallback;

        public COSXML.Callback.OnFailedCallback failCallback;

        public OnInternalHandleBeforExcute onInternalHandle;

        public OnState onState;

        protected static CosXml cosXmlServer;

        protected string bucket;

        protected string region;

        protected string key;

        protected bool isNeedMd5 = true;

        protected TaskState taskState;
        protected Object syncTaskState = new Object();

        public static void InitCosXmlServer(CosXml cosXml)
        {
            cosXmlServer = cosXml;
        }

        public COSXMLTask(string bucket, string region, string key)
        {
            this.bucket = bucket;
            this.region = region;
            this.key = key;
        }

        public abstract void Pause();

        public abstract void Cancel();

        public abstract void Resume();

        protected bool UpdateTaskState(TaskState newTaskState)
        {
            bool result = false;
            lock (syncTaskState)
            {
                switch (newTaskState)
                {
                    case TaskState.WAITTING:
                        taskState = newTaskState;
                        if (onState != null) onState(taskState);
                        result = true;
                        break;
                    case TaskState.RUNNING:
                        if (taskState == TaskState.WAITTING)
                        {
                            taskState = newTaskState;
                            if (onState != null) onState(taskState);
                            result = true;
                        }
                        break;
                    case TaskState.COMPLETED:
                        if (taskState == TaskState.RUNNING)
                        {
                            taskState = newTaskState;
                            if (onState != null) onState(taskState);
                            result = true;
                        }
                        break;
                    case TaskState.FAILED:
                        if (taskState == TaskState.WAITTING || taskState == TaskState.RUNNING)
                        {
                            taskState = newTaskState;
                            if (onState != null) onState(taskState);
                            result = true;
                        }
                        break;
                    case TaskState.PAUSE:
                        if (taskState == TaskState.WAITTING || taskState == TaskState.RUNNING)
                        {
                            taskState = newTaskState;
                            if (onState != null) onState(taskState);
                            result = true;
                        }
                        break;
                    case TaskState.CANCEL:
                        if (taskState != TaskState.COMPLETED || taskState != TaskState.CANCEL)
                        {
                            taskState = newTaskState;
                            if (onState != null) onState(taskState);
                            result = true;
                        }
                        break;
                    case TaskState.RESUME:
                        if (taskState == TaskState.PAUSE || taskState == TaskState.FAILED)
                        {
                            taskState = newTaskState;
                            if (onState != null) onState(taskState);
                            result = true;
                        }
                        break;
                }
            }
            return result;
            
        }

    }

    internal class SliceStruct
    {
        public int partNumber;
        public bool isAlreadyUpload;
        public long sliceStart;
        public long sliceEnd;
        public long sliceLength;
        public string eTag;
    }

    public enum TaskState
    {
        WAITTING = 0,
        RUNNING,
        COMPLETED,
        FAILED,
        CANCEL,
        PAUSE,
        RESUME,
    }

    public delegate void OnState(TaskState taskState);

    public delegate void OnInternalHandleBeforExcute(CosRequest cosRequest);

    internal interface OnMultipartUploadStateListener
    {
        void OnInit();
        void OnPart();
        void OnCompleted(CompleteMultipartUploadResult result);
        void OnFailed(CosClientException clientEx, CosServerException serverEx);
    }

}

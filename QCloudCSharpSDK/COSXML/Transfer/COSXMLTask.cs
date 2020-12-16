using System;
using System.Collections.Generic;
using System.Text;
using COSXML.Model;
using COSXML.Model.Object;
using COSXML.CosException;
using System.Threading.Tasks;

namespace COSXML.Transfer
{
    public abstract class COSXMLTask
    {
        public COSXML.Callback.OnProgressCallback progressCallback;

        public COSXML.Callback.OnSuccessCallback<CosResult> successCallback;

        public COSXML.Callback.OnFailedCallback failCallback;

        public OnInternalHandleBeforExcute onInternalHandle;

        public OnState onState;

        protected CosXml cosXmlServer;

        protected string bucket;

        protected string region;

        protected string key;

        protected Dictionary<string, string> customHeaders;

        protected TaskState taskState;

        protected Object syncTaskState = new Object();

        public void InitCosXmlServer(CosXml cosXml)
        {
            cosXmlServer = cosXml;

            if (this.region == null)
            {
                this.region = cosXml.GetConfig().Region;
            }
        }

        public COSXMLTask(string bucket, string region, string key)
        {
            this.bucket = bucket;
            this.region = region;
            this.key = key;
        }


        public COSXMLTask(string bucket, string key)
        {
            this.bucket = bucket;
            this.key = key;
        }

        public abstract void Pause();

        public abstract void Cancel();

        public abstract void Resume();

        public TaskState State()
        {
            return taskState;
        }

        protected void SetHeaders(Dictionary<string, string> headers)
        {
            this.customHeaders = headers;
        }

        protected bool UpdateTaskState(TaskState newTaskState)
        {
            bool result = false;

            lock (syncTaskState)
            {

                switch (newTaskState)
                {
                    case TaskState.Waiting:
                        taskState = newTaskState;
                        result = true;
                        break;
                    case TaskState.Running:

                        if (taskState == TaskState.Waiting)
                        {
                            taskState = newTaskState;
                            result = true;
                        }
                        break;
                    case TaskState.Completed:

                        if (taskState == TaskState.Running)
                        {
                            taskState = newTaskState;
                            result = true;
                        }
                        break;
                    case TaskState.Failed:

                        if (taskState == TaskState.Waiting || taskState == TaskState.Running)
                        {
                            taskState = newTaskState;
                            result = true;
                        }
                        break;
                    case TaskState.Pause:

                        if (taskState == TaskState.Waiting || taskState == TaskState.Running)
                        {
                            taskState = newTaskState;
                            result = true;
                        }
                        break;
                    case TaskState.Cancel:

                        if (taskState != TaskState.Completed || taskState != TaskState.Cancel)
                        {
                            taskState = newTaskState;
                            result = true;
                        }
                        break;
                    case TaskState.Resume:

                        if (taskState == TaskState.Pause || taskState == TaskState.Failed)
                        {
                            taskState = newTaskState;
                            result = true;
                        }
                        break;
                }
            }

            if (result && onState != null)
            {
                onState(taskState);
            }

            return result;

        }


        /// <summary>
        /// 等待任务
        /// </summary>
        /// <param name="task"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Task<T> AsyncTask<T>() where T : CosResult
        {
            return NewTaskCompletion<T>().Task;
        }

        private TaskCompletionSource<T> NewTaskCompletion<T>() where T : CosResult
        {
            var t = new TaskCompletionSource<T>();

            successCallback = delegate (CosResult cosResult)
            {
                t.TrySetResult(cosResult as T);
            };

            failCallback = delegate (CosClientException clientException, CosServerException serverException)
            {

                if (clientException != null)
                {
                    t.TrySetException(clientException);
                }
                else
                {
                    t.TrySetException(serverException);
                }
            };

            return t;
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
        Waiting = 0,

        Running,

        Completed,

        Failed,

        Cancel,

        Pause,

        Resume,
    }

    public delegate void OnState(TaskState taskState);

    public delegate void OnInternalHandleBeforExcute(CosRequest cosRequest);

    internal interface IOnMultipartUploadStateListener
    {
        void OnInit();

        void OnPart();

        void OnCompleted(CompleteMultipartUploadResult result);

        void OnFailed(CosClientException clientEx, CosServerException serverEx);
    }

}

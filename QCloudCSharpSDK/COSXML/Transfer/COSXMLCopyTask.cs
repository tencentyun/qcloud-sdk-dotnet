using System;
using System.Collections.Generic;
using System.Text;
using COSXML.Model.Tag;
using COSXML.Model.Object;
using COSXML.Utils;
using COSXML.Model;
using COSXML.CosException;
using COSXML.Log;
using COSXML.Common;

namespace COSXML.Transfer
{
    public sealed class COSXMLCopyTask : COSXMLTask, IOnMultipartUploadStateListener
    {
        private long divisionSize;

        private long sliceSize;

        private CopySourceStruct copySource;

        private HeadObjectRequest headObjectRequest;

        private DeleteObjectRequest deleteObjectRequest;

        private long sourceSize;

        private CopyObjectRequest copyObjectRequest;

        private Object syncExit = new Object();

        private bool isExit = false;

        private InitMultipartUploadRequest initMultiUploadRequest;

        private string uploadId;

        private ListPartsRequest listPartsRequest;

        private List<UploadPartCopyRequest> uploadCopyCopyRequestList;

        private List<SliceStruct> sliceList;

        private Object syncPartCopyCount = new object();

        private int sliceCount;

        private CompleteMultipartUploadRequest completeMultiUploadRequest;

        private AbortMultipartUploadRequest abortMultiUploadRequest;

        public bool CompleteOnAllPartsCopyed { get; set; } = true;

        public COSXMLCopyTask(string bucket, string key, CopySourceStruct copySource)
            : base(bucket, key)
        {
            this.copySource = copySource;
        }

        internal void SetDivision(long divisionSize, long sliceSize)
        {
            this.divisionSize = divisionSize;
            this.sliceSize = sliceSize;
        }

        internal void Copy()
        {
            UpdateTaskState(TaskState.Waiting);
            //源对象是否存在
            if (copySource == null)
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
                        failCallback(new CosClientException((int)CosClientError.InvalidArgument, "copySource = null"), null);
                    }

                }

                //error
                return;
            }

            headObjectRequest = new HeadObjectRequest(copySource.bucket, copySource.key);
            headObjectRequest.Region = copySource.region;
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

                    //源对象的长度
                    sourceSize = result.size;

                    if (sourceSize > divisionSize)
                    {
                        MultiPartCopy();
                    }
                    else
                    {
                        SimpleCopy();
                    }
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

        private void SimpleCopy()
        {
            copyObjectRequest = new CopyObjectRequest(bucket, key);
            copyObjectRequest.SetCopyMetaDataDirective(Common.CosMetaDataDirective.Copy);
            copyObjectRequest.SetCopySource(copySource);
            cosXmlServer.CopyObject(copyObjectRequest, 
                delegate (CosResult cosResult)
                {
                    lock (syncExit)
                    {

                        if (isExit)
                        {

                            if (taskState == TaskState.Cancel)
                            {
                                DeleteObject();
                            }

                            return;
                        }
                    }

                    if (UpdateTaskState(TaskState.Completed))
                    {
                        CopyObjectResult result = cosResult as CopyObjectResult;
                        CopyTaskResult copyTaskResult = new CopyTaskResult();

                        copyTaskResult.SetResult(result);

                        if (successCallback != null)
                        {
                            successCallback(copyTaskResult);
                        }
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
                }
            );
        }

        private void MultiPartCopy()
        {

            if (uploadId != null)
            {
                //list -> partCopy -> complete
                ListParts();
            }
            else
            {
                // init -> partCopy - > complete
                InitMultiUploadPart();
            }

        }

        private void InitMultiUploadPart()
        {
            initMultiUploadRequest = new InitMultipartUploadRequest(bucket, key);
            cosXmlServer.InitMultipartUpload(initMultiUploadRequest, delegate (CosResult cosResult)
            {
                lock (syncExit)
                {

                    if (isExit)
                    {

                        return;
                    }
                }

                InitMultipartUploadResult result = cosResult as InitMultipartUploadResult;

                uploadId = result.initMultipartUpload.uploadId;
                //计算分片块
                ComputeSliceNums();
                //通知执行PartCopy
                OnInit();

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
                    OnFailed(clientEx, serverEx);
                }
            });
        }

        private void ListParts()
        {
            listPartsRequest = new ListPartsRequest(bucket, key, uploadId);
            cosXmlServer.ListParts(listPartsRequest, delegate (CosResult cosResult)
            {
                lock (syncExit)
                {

                    if (isExit)
                    {

                        return;
                    }
                }

                ListPartsResult result = cosResult as ListPartsResult;

                //更新分块
                UpdateSliceNums(result);
                //通知执行PartCopy
                OnInit();

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
                    OnFailed(clientEx, serverEx);
                }
            });
        }

        private void PartCopy()
        {
            int size = sliceList.Count;


            sliceCount = size;
            uploadCopyCopyRequestList = new List<UploadPartCopyRequest>(size);

            for (int i = 0; i < size; i++)
            {

                if (isExit)
                {

                    return;
                }

                SliceStruct sliceStruct = sliceList[i];

                if (!sliceStruct.isAlreadyUpload)
                {
                    UploadPartCopyRequest uploadPartCopyRequest = new UploadPartCopyRequest(bucket, key, sliceStruct.partNumber, uploadId);

                    uploadPartCopyRequest.SetCopySource(copySource);
                    uploadPartCopyRequest.SetCopyRange(sliceStruct.sliceStart, sliceStruct.sliceEnd);
                    uploadCopyCopyRequestList.Add(uploadPartCopyRequest);
                    cosXmlServer.PartCopy(uploadPartCopyRequest, delegate (CosResult result)
                    {
                        lock (syncExit)
                        {

                            if (isExit)
                            {

                                return;
                            }
                        }

                        UploadPartCopyResult uploadPartCopyResult = result as UploadPartCopyResult;

                        sliceStruct.eTag = uploadPartCopyResult.copyPart.eTag;

                        lock (syncPartCopyCount)
                        {
                            sliceCount--;

                            if (sliceCount == 0)
                            {
                                OnPart();

                                return;
                            }
                        }
                    }, delegate (CosClientException clientEx, CosServerException serverEx)
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
                            OnFailed(clientEx, serverEx);
                        }

                        return;
                    });
                }
                else
                {
                    lock (syncPartCopyCount)
                    {
                        sliceCount--;

                        if (sliceCount == 0)
                        {
                            OnPart();

                            return;
                        }
                    }
                }
            }
        }

        private void ComputeSliceNums()
        {
            int count = (int)(sourceSize / sliceSize);

            sliceList = new List<SliceStruct>(count > 0 ? count : 1);
            int i = 1;


            for (; i < count; i++)
            {
                SliceStruct sliceStruct = new SliceStruct();

                sliceStruct.partNumber = i;
                sliceStruct.isAlreadyUpload = false;
                sliceStruct.sliceStart = (i - 1) * sliceSize;
                sliceStruct.sliceEnd = i * sliceSize - 1;
                sliceList.Add(sliceStruct);
            }

            SliceStruct lastSliceStruct = new SliceStruct();

            lastSliceStruct.partNumber = i;
            lastSliceStruct.isAlreadyUpload = false;
            lastSliceStruct.sliceStart = (i - 1) * sliceSize;
            lastSliceStruct.sliceEnd = sourceSize - 1;
            sliceList.Add(lastSliceStruct);
        }

        private void UpdateSliceNums(ListPartsResult listPartsResult)
        {

            try
            {

                if (listPartsResult.listParts.parts != null)
                {
                    //获取原来的parts并提取partNumber
                    Dictionary<int, SliceStruct> sourceParts = new Dictionary<int, SliceStruct>(sliceList.Count);

                    foreach (SliceStruct sliceStruct in sliceList)
                    {
                        sourceParts.Add(sliceStruct.partNumber, sliceStruct);
                    }

                    foreach (ListParts.Part part in listPartsResult.listParts.parts)
                    {
                        int partNumber = -1;

                        bool parse = int.TryParse(part.partNumber, out partNumber);


                        if (!parse)
                        {
                            throw new ArgumentException("ListParts.Part parse error");
                        }

                        SliceStruct sliceStruct = sourceParts[partNumber];

                        sliceStruct.isAlreadyUpload = true;
                        sliceStruct.eTag = part.eTag;
                    }
                }
            }
            catch (Exception ex)
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
                    OnFailed(new CosClientException((int)CosClientError.InternalError, ex.Message, ex), null);
                }
            }

        }


        private void CompleteMultipartUpload()
        {
            completeMultiUploadRequest = new CompleteMultipartUploadRequest(bucket, key, uploadId);

            foreach (SliceStruct sliceStruct in sliceList)
            {
                completeMultiUploadRequest.SetPartNumberAndETag(sliceStruct.partNumber, sliceStruct.eTag);
            }

            cosXmlServer.CompleteMultiUpload(completeMultiUploadRequest, delegate (CosResult result)
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
                    CompleteMultipartUploadResult completeMultiUploadResult = result as CompleteMultipartUploadResult;

                    OnCompleted(completeMultiUploadResult);
                }
            }, delegate (CosClientException clientEx, CosServerException serverEx)
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
                    OnFailed(clientEx, serverEx);
                }

            });
        }

        public void OnInit()
        {
            //获取了uploadId
            PartCopy();
        }

        public void OnPart()
        {
            if (CompleteOnAllPartsCopyed)
            {
                //获取了 part ETag
                CompleteMultipartUpload();
            }

        }

        public void OnCompleted(CompleteMultipartUploadResult result)
        {
            uploadId = null;
            //lock (syncExit)
            //{
            //    isExit = true;
            //}
            //success
            if (successCallback != null)
            {
                CopyTaskResult copyTaskResult = new CopyTaskResult();

                copyTaskResult.SetResult(result);
                successCallback(copyTaskResult);
            }
        }

        public void OnFailed(CosClientException clientEx, CosServerException serverEx)
        {

            if (!isExit)
            {
                lock (syncExit)
                {
                    isExit = true;
                }
            }

            if (failCallback != null)
            {
                failCallback(clientEx, serverEx);
            }
        }


        private void Abort()
        {
            abortMultiUploadRequest = new AbortMultipartUploadRequest(bucket, key, uploadId);
            cosXmlServer.AbortMultiUpload(abortMultiUploadRequest, 
                delegate (CosResult cosResult) 
                { 

                },

                delegate (CosClientException cosClientException, CosServerException cosServerException) 
                { 
                    DeleteObject(); 
                }
            );
        }

        private void DeleteObject()
        {
            deleteObjectRequest = new DeleteObjectRequest(bucket, key);
            cosXmlServer.DeleteObject(deleteObjectRequest, 
                delegate (CosResult cosResult) 
                { 

                },

                delegate (CosClientException cosClientException, CosServerException cosServerException) 
                { 

                }
            );
        }

        private void RealCancle()
        {
            //cancle request
            cosXmlServer.Cancel(headObjectRequest);
            cosXmlServer.Cancel(copyObjectRequest);
            cosXmlServer.Cancel(initMultiUploadRequest);
            cosXmlServer.Cancel(completeMultiUploadRequest);

            if (uploadCopyCopyRequestList != null)
            {

                foreach (UploadPartCopyRequest uploadPartCopyRequest in uploadCopyCopyRequestList)
                {
                    cosXmlServer.Cancel(uploadPartCopyRequest);
                }
            }
        }

        public override void Pause()
        {

            if (UpdateTaskState(TaskState.Pause))
            {
                //exit copy
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
                //abort
                Abort();
                uploadId = null;
            }
        }

        public override void Resume()
        {

            if (UpdateTaskState(TaskState.Resume))
            {
                lock (syncExit)
                {
                    isExit = false;
                }

                Copy();
            }
        }

        public class CopyTaskResult : CosResult
        {
            public string eTag;

            public void SetResult(CopyObjectResult result)
            {
                this.eTag = result.copyObject.eTag;
                this.httpCode = result.httpCode;
                this.httpMessage = result.httpMessage;
                this.responseHeaders = result.responseHeaders;
            }

            public void SetResult(CompleteMultipartUploadResult result)
            {
                this.eTag = result.completeResult.eTag;
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

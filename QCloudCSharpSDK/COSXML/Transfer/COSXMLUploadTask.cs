using System;
using System.Collections.Generic;
using COSXML.Model;
using COSXML.CosException;
using COSXML.Model.Object;
using System.IO;
using COSXML.Common;
using COSXML.Utils;
using COSXML.Model.Tag;
using COSXML.Model.Bucket;
using System.Threading;

namespace COSXML.Transfer
{
    public sealed class COSXMLUploadTask : COSXMLTask, IOnMultipartUploadStateListener
    {
        private long divisionSize;

        private long sliceSize;

        private const int MAX_ACTIVIE_TASKS = 2;

        private volatile int activieTasks = 0;

        private long sendOffset = 0L;
        // 实际要发送的总长度，类似于content-length
        private long sendContentLength = -1L;

        private string srcPath;

        private PutObjectRequest putObjectRequest;

        private DeleteObjectRequest deleteObjectRequest;

        private Object syncExit = new Object();

        private bool isExit = false;

        private ListPartsRequest listPartsRequest;

        private InitMultipartUploadRequest initMultiUploadRequest;

        private string uploadId;

        private Dictionary<long, long> uploadPartRequestMap;

        private List<UploadPartRequest> uploadPartRequestList;

        private List<SliceStruct> sliceList;

        private Object syncPartCopyCount = new object();

        private int sliceCount;

        private long hasReceiveDataLength = 0;

        private object syncProgress = new Object();

        private CompleteMultipartUploadRequest completeMultiUploadRequest;

        private AbortMultipartUploadRequest abortMultiUploadRequest;

        private ListMultiUploadsRequest listMultiUploadsRequest;

        public int MaxConcurrent { private get; set; } = MAX_ACTIVIE_TASKS;

        public bool UseResumableUpload { private get; set; } = true;

        public string StorageClass { private get; set; }

        public COSXMLUploadTask(string bucket, string key)
            : base(bucket, key)
        {
        }

        public COSXMLUploadTask(PutObjectRequest request)
            : base(request.Bucket, request.Region, request.Key)
        {
            SetHeaders(request.GetRequestHeaders());
        }

        internal void SetDivision(long divisionSize, long sliceSize)
        {
            this.divisionSize = divisionSize;
            this.sliceSize = sliceSize;
        }

        public void SetSrcPath(string srcPath)
        {
            SetSrcPath(srcPath, -1L, -1L);
        }

        public void SetSrcPath(string srcPath, long fileOffset, long contentLength)
        {
            this.srcPath = srcPath;
            this.sendOffset = fileOffset >= 0 ? fileOffset : 0;
            this.sendContentLength = contentLength >= 0 ? contentLength : -1L;
        }

        public void SetUploadId(string uploadId)
        {
            this.uploadId = uploadId;
        }

        public string GetUploadId()
        {
            return uploadId;
        }

        internal void Upload()
        {
            //UpdateTaskState(TaskState.WAITTING);
            taskState = TaskState.Waiting;
            hasReceiveDataLength = 0;
            FileInfo fileInfo = null;
            long sourceLength = 0;

            try
            {
                fileInfo = new FileInfo(srcPath);
                sourceLength = fileInfo.Length;
            }
            catch (Exception ex)
            {
                lock (syncExit)
                {

                    if (isExit) return;
                }

                if (UpdateTaskState(TaskState.Failed))
                {

                    if (failCallback != null)
                    {
                        failCallback(new CosClientException((int)CosClientError.InvalidArgument, ex.Message, ex), null);
                    }
                }
                //error
                return;
            }

            if (sendContentLength == -1L || (sendContentLength + sendOffset > sourceLength))
            {
                sendContentLength = sourceLength - sendOffset;
            }

            taskState = TaskState.Running;

            if (sendContentLength > divisionSize)
            {
                MultiUpload();
            }
            else
            {
                SimpleUpload();
            }

        }

        private void SimpleUpload()
        {
            putObjectRequest = new PutObjectRequest(bucket, key, srcPath, sendOffset, sendContentLength);

            if (customHeaders != null)
            {
                putObjectRequest.SetRequestHeaders(customHeaders);
            }

            if (progressCallback != null)
            {
                putObjectRequest.SetCosProgressCallback(progressCallback);
            }

            if (StorageClass != null) putObjectRequest.SetCosStorageClass(StorageClass);

            cosXmlServer.PutObject(putObjectRequest, delegate (CosResult cosResult)
            {
                lock (syncExit)
                {

                    if (isExit) {
                        if (taskState == TaskState.Cancel) DeleteObject();
                        return;
                    }
                }

                if (UpdateTaskState(TaskState.Completed))
                {
                    PutObjectResult result = cosResult as PutObjectResult;
                    UploadTaskResult uploadTaskResult = new UploadTaskResult();

                    uploadTaskResult.SetResult(result);

                    if (successCallback != null)
                    {
                        successCallback(uploadTaskResult);
                    }
                }

            },
            
            delegate (CosClientException clientEx, CosServerException serverEx) {
                lock (syncExit) {
                    if (isExit) return;
                }
                if (UpdateTaskState(TaskState.Failed)) {
                    if (failCallback != null) failCallback(clientEx, serverEx);
                }
            });
        }

        private void MultiUpload()
        {
            ComputeSliceNums();

            if (uploadId != null)
            {
                ListMultiParts();
            }
            else
            {
                if (UseResumableUpload)
                {
                    CheckResumeblaUpload();
                }
                else
                {
                    InitMultiUploadPart();
                }
            }
        }

        private void InitMultiUploadPart()
        {
            initMultiUploadRequest = new InitMultipartUploadRequest(bucket, key);

            if (customHeaders != null)
            {
                initMultiUploadRequest.SetRequestHeaders(customHeaders);
            }

            if (StorageClass != null) 
            {
                initMultiUploadRequest.SetCosStorageClass(StorageClass);
            }

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
                //通知执行PartCopy
                OnInit();

            },
            
            delegate (CosClientException clientEx, CosServerException serverEx)
            {
                lock (syncExit) {
                    if (isExit)  return; 
                }
                if (UpdateTaskState(TaskState.Failed)) OnFailed(clientEx, serverEx);
            });
        }

        private void CheckResumeblaUpload()
        {
            listMultiUploadsRequest = new ListMultiUploadsRequest(bucket);
            listMultiUploadsRequest.SetPrefix(key);
            cosXmlServer.ListMultiUploads(listMultiUploadsRequest, delegate (CosResult cosResult)
            {
                // 取最新符合条件的uploadId
                ListMultiUploadsResult result = cosResult as ListMultiUploadsResult;
                var uploads = result.listMultipartUploads;
                if (uploads.uploads != null && uploads.uploads.Count > 0) 
                {
                    for (int i = uploads.uploads.Count - 1; i >= 0; i--)
                    {
                        var upload = uploads.uploads[i];
                        if (upload.key != key) continue;
                        CheckAllUploadParts(upload.uploadID);
                        return;
                    }
                } else {
                    InitMultiUploadPart();
                }
            },
            delegate (CosClientException clientEx, CosServerException serverEx)
            {
                lock (syncExit) {
                    if (isExit) return;
                }
                if (UpdateTaskState(TaskState.Failed)) OnFailed(clientEx, serverEx);
            });
        }

        public void TestCheckAllUploadParts(string upId)
        {
            isExit = false;
            CheckAllUploadParts(upId);
        }

        public void TestDeleteObject()
        {
            DeleteObject();
        }
        
        private void CheckAllUploadParts(string uploadId)
        {
            bool checkSucc = true;
            listPartsRequest = new ListPartsRequest(bucket, key, uploadId);
            cosXmlServer.ListParts(listPartsRequest, delegate (CosResult cosResult)
            {
                lock (syncExit) {
                    if (isExit) return;
                }

                ListPartsResult result = cosResult as ListPartsResult;

                Dictionary<int, SliceStruct> sourceParts = new Dictionary<int, SliceStruct>(sliceList.Count);

                foreach (SliceStruct sliceStruct in sliceList)
                {
                    sourceParts.Add(sliceStruct.partNumber, sliceStruct);
                }
                //检查已上传块的ETag和本地ETag是否一致
                foreach (ListParts.Part part in result.listParts.parts)
                {
                    int partNumber = -1;

                    bool parse = int.TryParse(part.partNumber, out partNumber);

                    if (!parse)
                    {
                        throw new ArgumentException("ListParts.Part parse error");
                    }

                    SliceStruct sliceStruct = sourceParts[partNumber];

                    //计算本地ETag
                    if (!CompareSliceMD5(srcPath, sliceStruct.sliceStart, sliceStruct.sliceLength, part.eTag))
                        checkSucc = false;
                }
                if (checkSucc) {
                    this.uploadId = uploadId;
                    UpdateSliceNums(result);
                    OnInit();
                } else {
                    InitMultiUploadPart();
                }
            },
            delegate (CosClientException clientEx, CosServerException serverEx)
            {
                lock(syncExit) {
                    if (isExit) return;
                }

                if (UpdateTaskState(TaskState.Failed))
                {
                    OnFailed(clientEx, serverEx);
                }
            });
        }

        private void ListMultiParts()
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

                //指定uploadId时, 对已上传分块做校验, 已通过校验的分块会纳入续传范围
                UpdateSliceNums(result);
                //跳过Init流程
                OnInit();

            },
            delegate (CosClientException clientEx, CosServerException serverEx) {
                lock (syncExit) {
                    if (isExit) return;
                }
                if (UpdateTaskState(TaskState.Failed))  OnFailed(clientEx, serverEx);
            });
        }

        private void UploadPart()
        {
            activieTasks = 0;
            int size = sliceList.Count;

            sliceCount = size;
            uploadPartRequestMap = new Dictionary<long, long>(size);
            uploadPartRequestList = new List<UploadPartRequest>(size);

            AutoResetEvent resetEvent = new AutoResetEvent(false);
            
            for (int i = 0; i < size; i++)
            {

                if (activieTasks > MaxConcurrent)
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

                SliceStruct sliceStruct = sliceList[i];

                if (!sliceStruct.isAlreadyUpload)
                {
                    UploadPartRequest uploadPartRequest = new UploadPartRequest(bucket, key, sliceStruct.partNumber, uploadId, srcPath,
                        sliceStruct.sliceStart, sliceStruct.sliceLength);

                    if (customHeaders != null && customHeaders.ContainsKey(CosRequestHeaderKey.X_COS_TRAFFIC_LIMIT))
                    {
                        string trafficLimit = customHeaders[CosRequestHeaderKey.X_COS_TRAFFIC_LIMIT];

                        uploadPartRequest.LimitTraffic(Convert.ToInt64(trafficLimit));
                    }

                    //打印进度
                    uploadPartRequest.SetCosProgressCallback(
                        delegate (long completed, long total)
                        {
                            lock (syncProgress)
                            {
                                long dataLen = hasReceiveDataLength + completed - uploadPartRequestMap[sliceStruct.partNumber];

                                UpdateProgress(dataLen, sendContentLength, false);
                                hasReceiveDataLength = dataLen;
                                uploadPartRequestMap[sliceStruct.partNumber] = completed;
                            }
                        }
                    );

                    uploadPartRequestMap.Add(sliceStruct.partNumber, 0);
                    uploadPartRequestList.Add(uploadPartRequest);


                    Interlocked.Increment(ref activieTasks);

                    cosXmlServer.UploadPart(uploadPartRequest, delegate (CosResult result)
                    {
                        Interlocked.Decrement(ref activieTasks);
                        UploadPartResult uploadPartResult = result as UploadPartResult;

                        sliceStruct.eTag = uploadPartResult.eTag;
                        lock (syncPartCopyCount)
                        {
                            sliceCount--;

                            if (sliceCount == 0)
                            {
                                OnPart();
                            }
                            if (uploadPartRequest != null && uploadPartRequestList.Contains(uploadPartRequest)) 
                            {
                                uploadPartRequestList.Remove(uploadPartRequest);
                            }
                        }

                        resetEvent.Set();

                    }, delegate (CosClientException clientEx, CosServerException serverEx)
                    {
                        Interlocked.Decrement(ref activieTasks);

                        if (UpdateTaskState(TaskState.Failed))
                        {
                            OnFailed(clientEx, serverEx);
                        }

                        resetEvent.Set();
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

        private void UpdateProgress(long complete, long total, bool isCompleted)
        {

            lock (syncExit)
            {

                if (isExit)
                {

                    return;
                }
            }

            if (complete < total)
            {

                if (progressCallback != null)
                {
                    progressCallback(complete, total);
                }
            }
            else
            {

                if (isCompleted)
                {

                    if (progressCallback != null)
                    {
                        progressCallback(complete, total);
                    }
                }
                else
                {

                    if (progressCallback != null)
                    {
                        progressCallback(total - 1, total);
                    }
                }
            }

        }

        private void CompleteMultipartUpload()
        {
            completeMultiUploadRequest = new CompleteMultipartUploadRequest(bucket, key, uploadId);

            foreach (SliceStruct sliceStruct in sliceList)
            {
                // partNumberEtag 有序的
                // partNumberEtag 有序的
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

        private void ComputeSliceNums()
        {
            int count = (int)(sendContentLength / sliceSize);

            if (count >= 10000) {
                throw new CosClientException((int)CosClientError.UserCancelled, "分块传输设置的分片太小导致分片超过10000，请调大分片大小");
            }
            sliceList = new List<SliceStruct>(count > 0 ? count : 1);
            // partNumber >= 1
            // partNumber >= 1
            int i = 1;

            for (; i < count; i++)
            {
                SliceStruct sliceStruct = new SliceStruct();

                sliceStruct.partNumber = i;
                sliceStruct.isAlreadyUpload = false;
                sliceStruct.sliceStart = sendOffset + (i - 1) * sliceSize;
                sliceStruct.sliceLength = sliceSize;
                sliceStruct.sliceEnd = sendOffset + i * sliceSize - 1;
                sliceList.Add(sliceStruct);
            }

            SliceStruct lastSliceStruct = new SliceStruct();

            lastSliceStruct.partNumber = i;
            lastSliceStruct.isAlreadyUpload = false;
            lastSliceStruct.sliceStart = sendOffset + (i - 1) * sliceSize;
            lastSliceStruct.sliceLength = sendContentLength - (i - 1) * sliceSize;
            lastSliceStruct.sliceEnd = sendOffset + sendContentLength - 1;
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
                        lock (syncProgress)
                        {
                            long size = 0L;

                            long.TryParse(part.size, out size);
                            hasReceiveDataLength += size;
                        }
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

        public bool TestCompareSliceMD5(string localFile, long offset, long length, string crc64ecma)
        {
            return CompareSliceMD5(localFile, offset, length, crc64ecma);
        }
        
        private bool CompareSliceMD5(string localFile, long offset, long length, string crc64ecma)
        {
            Crc64.InitECMA();
            String hash = String.Empty;

            try
            {
                using (FileStream fs = File.OpenRead(localFile))
                {
                    fs.Seek(offset, SeekOrigin.Begin);
                    string md5 = DigestUtils.GetMD5HexString(fs, length);
                    fs.Close();
                    crc64ecma = crc64ecma.Trim('"');
                    return md5 == crc64ecma;
                }
            } 
            catch (Exception e)
            {
                return false;
            }

        }

        public void OnInit()
        {
            //获取了uploadId
            UploadPart();
        }

        public void OnPart()
        {
            //获取了 part ETag
            CompleteMultipartUpload();
        }

        public void OnCompleted(CompleteMultipartUploadResult result)
        {
            UpdateProgress(sendContentLength, sendContentLength, true);
            //lock (syncExit)
            //{
            //    isExit = true;
            //}
            if (successCallback != null)
            {
                UploadTaskResult uploadTaskResult = new UploadTaskResult();

                uploadTaskResult.SetResult(result);
                successCallback(uploadTaskResult);
            }
        }

        public void OnFailed(CosClientException clientEx, CosServerException serverEx)
        {
            lock (syncExit)
            {
                isExit = true;
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
            cosXmlServer.Cancel(putObjectRequest);
            cosXmlServer.Cancel(initMultiUploadRequest);
            cosXmlServer.Cancel(completeMultiUploadRequest);
            cosXmlServer.Cancel(listPartsRequest);

            if (uploadPartRequestList != null)
            {

                foreach (UploadPartRequest uploadPartRequest in uploadPartRequestList)
                {
                    cosXmlServer.Cancel(uploadPartRequest);
                }
            }
        }

        public override void Pause()
        {

            if (UpdateTaskState(TaskState.Pause))
            {
                //exit upload
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
                //exit upload
                lock (syncExit) 
                { 
                    isExit = true; 
                }

                //cancle request
                RealCancle();
                //abort
                Abort();
                uploadId = null;
                // throw exception if requested
                if (throwExceptionIfCancelled) {
                    throw new CosClientException((int)CosClientError.UserCancelled, "Upload Task Cancelled by user");
                }
            }
        }

        public override void Resume()
        {

            if (UpdateTaskState(TaskState.Resume))
            {
                lock (syncExit)
                {
                    //continue to upload
                    //continue to upload
                    isExit = false;
                }
                
                Upload();
            }
        }

        public class UploadTaskResult : CosResult
        {
            public string eTag;

            public void SetResult(PutObjectResult result)
            {
                this.eTag = result.eTag;
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

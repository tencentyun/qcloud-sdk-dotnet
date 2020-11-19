using System;
using System.Collections.Generic;

using System.Threading.Tasks;
using System.Text;
using COSXML.Network;
using COSXML.Model;
using COSXML.Model.CI;
using COSXML.Model.Service;
using COSXML.Model.Object;
using COSXML.Model.Bucket;
using COSXML.Auth;
using COSXML.Log;
using COSXML.Utils;
using COSXML.CosException;
using COSXML.Common;
using COSXML.Model.Tag;
using COSXML.Callback;

namespace COSXML
{
    /// <summary>
    /// CosXml 实现类
    /// </summary>
    public class CosXmlServer : CosXml
    {
        private CosXmlConfig config;

        private QCloudCredentialProvider qcloudCredentailProvider;

        private HttpClient httpClient;

        /// <summary>
        /// 创建一个新实例
        /// </summary>
        /// <param name="config">服务配置</param>
        /// <param name="qcloudCredentailProvider">凭证提供者</param>
        public CosXmlServer(CosXmlConfig config, QCloudCredentialProvider qcloudCredentailProvider)
        {

            if (config != null)
            {
                this.config = config;
            }
            else
            {
                this.config = new CosXmlConfig.Builder().Build();
            }

            if (this.config.IsDebugLog)
            {
                QLog.AddLogAdapter(new LogImpl());
            }

            this.qcloudCredentailProvider = qcloudCredentailProvider;
            HttpClient.Init(this.config.HttpConfig, this.qcloudCredentailProvider);
            httpClient = HttpClient.GetInstance();
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <returns></returns>
        public CosXmlConfig GetConfig()
        {

            return config;
        }

        private void CheckAppidAndRegion(CosRequest request)
        {
            request.serviceConfig = config;

            if (request.IsHttps == null)
            {
                request.IsHttps = config.IsHttps;
            }

            if (request is GetServiceRequest)
            {

                return;
            }

            if (request.APPID == null)
            {
                request.APPID = config.Appid;
            }

            if (request is ObjectRequest)
            {

                if (((ObjectRequest)request).Region == null)
                {
                    ((ObjectRequest)request).Region = config.Region;
                }

                return;
            }

            if (request is BucketRequest)
            {

                if (((BucketRequest)request).Region == null)
                {
                    ((BucketRequest)request).Region = config.Region;
                }

                return;
            }

        }

        public Task<T> ExecuteAsync<T>(CosRequest request) where T : CosResult
        {
            T result = Activator.CreateInstance<T>();

            CheckAppidAndRegion(request);

            var t = new TaskCompletionSource<T>();


            Schedue(request, result, delegate (CosResult cosResult)
            {
                t.TrySetResult(result as T);
            }, delegate (CosClientException clientException, CosServerException serverException)
            {

                if (clientException != null)
                {
                    t.TrySetException(clientException);
                }
                else
                {
                    t.TrySetException(serverException);
                }
            });

            return t.Task;
        }

        public T Execute<T>(CosRequest request) where T : CosResult
        {
            T result = Activator.CreateInstance<T>();


            return (T)Excute(request, result);
        }

        private T Execute<T>(CosRequest request, T result) where T : CosResult
        {

            return (T)Excute(request, result);
        }

        private CosResult Excute(CosRequest request, CosResult result)
        {
            CheckAppidAndRegion(request);
            httpClient.Excute(request, result);

            return result;
        }

        private void Schedue(CosRequest request, CosResult result, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback)
        {
            CheckAppidAndRegion(request);
            httpClient.Schedue(request, result, successCallback, failCallback);
        }

        public Model.Service.GetServiceResult GetService(Model.Service.GetServiceRequest request)
        {

            return (Model.Service.GetServiceResult)Excute(request, new Model.Service.GetServiceResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void GetService(Model.Service.GetServiceRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new GetServiceResult(), successCallback, failCallback);
        }

        public PutBucketResult PutBucket(PutBucketRequest request)
        {

            return (Model.Bucket.PutBucketResult)Excute(request, new Model.Bucket.PutBucketResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void PutBucket(PutBucketRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new PutBucketResult(), successCallback, failCallback);
        }

        public DeleteBucketResult DeleteBucket(DeleteBucketRequest request)
        {

            return (Model.Bucket.DeleteBucketResult)Excute(request, new Model.Bucket.DeleteBucketResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void DeleteBucket(DeleteBucketRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new DeleteBucketResult(), successCallback, failCallback);
        }

        public HeadBucketResult HeadBucket(HeadBucketRequest request)
        {

            return (Model.Bucket.HeadBucketResult)Excute(request, new Model.Bucket.HeadBucketResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void HeadBucket(HeadBucketRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new HeadBucketResult(), successCallback, failCallback);
        }

        public GetBucketResult GetBucket(GetBucketRequest request)
        {

            return (Model.Bucket.GetBucketResult)Excute(request, new Model.Bucket.GetBucketResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void GetBucket(GetBucketRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new GetBucketResult(), successCallback, failCallback);
        }

        public GetBucketLocationResult GetBucketLocation(GetBucketLocationRequest request)
        {

            return (Model.Bucket.GetBucketLocationResult)Excute(request, new Model.Bucket.GetBucketLocationResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void GetBucketLocation(GetBucketLocationRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new GetBucketLocationResult(), successCallback, failCallback);
        }

        public PutBucketACLResult PutBucketACL(PutBucketACLRequest request)
        {

            return (Model.Bucket.PutBucketACLResult)Excute(request, new Model.Bucket.PutBucketACLResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void PutBucketACL(PutBucketACLRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new PutBucketACLResult(), successCallback, failCallback);
        }

        public GetBucketACLResult GetBucketACL(GetBucketACLRequest request)
        {

            return (Model.Bucket.GetBucketACLResult)Excute(request, new Model.Bucket.GetBucketACLResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void GetBucketACL(GetBucketACLRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new GetBucketACLResult(), successCallback, failCallback);
        }

        public PutBucketCORSResult PutBucketCORS(PutBucketCORSRequest request)
        {

            return (Model.Bucket.PutBucketCORSResult)Excute(request, new Model.Bucket.PutBucketCORSResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void PutBucketCORS(PutBucketCORSRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new PutBucketCORSResult(), successCallback, failCallback);
        }

        public GetBucketCORSResult GetBucketCORS(GetBucketCORSRequest request)
        {

            return (Model.Bucket.GetBucketCORSResult)Excute(request, new Model.Bucket.GetBucketCORSResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void GetBucketCORS(GetBucketCORSRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new GetBucketCORSResult(), successCallback, failCallback);
        }

        public DeleteBucketCORSResult DeleteBucketCORS(DeleteBucketCORSRequest request)
        {

            return (Model.Bucket.DeleteBucketCORSResult)Excute(request, new Model.Bucket.DeleteBucketCORSResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void DeleteBucketCORS(DeleteBucketCORSRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new DeleteBucketCORSResult(), successCallback, failCallback);
        }

        public PutBucketLifecycleResult PutBucketLifecycle(PutBucketLifecycleRequest request)
        {

            return (Model.Bucket.PutBucketLifecycleResult)Excute(request, new Model.Bucket.PutBucketLifecycleResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void PutBucketLifecycle(PutBucketLifecycleRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new PutBucketLifecycleResult(), successCallback, failCallback);
        }

        public GetBucketLifecycleResult GetBucketLifecycle(GetBucketLifecycleRequest request)
        {

            return (Model.Bucket.GetBucketLifecycleResult)Excute(request, new Model.Bucket.GetBucketLifecycleResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void GetBucketLifecycle(GetBucketLifecycleRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new GetBucketLifecycleResult(), successCallback, failCallback);
        }

        public DeleteBucketLifecycleResult DeleteBucketLifecycle(DeleteBucketLifecycleRequest request)
        {

            return (Model.Bucket.DeleteBucketLifecycleResult)Excute(request, new Model.Bucket.DeleteBucketLifecycleResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void DeleteBucketLifecycle(DeleteBucketLifecycleRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new DeleteBucketLifecycleResult(), successCallback, failCallback);
        }

        public PutBucketReplicationResult PutBucketReplication(PutBucketReplicationRequest request)
        {

            return (Model.Bucket.PutBucketReplicationResult)Excute(request, new Model.Bucket.PutBucketReplicationResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void PutBucketReplication(PutBucketReplicationRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new PutBucketReplicationResult(), successCallback, failCallback);
        }

        public GetBucketReplicationResult GetBucketReplication(GetBucketReplicationRequest request)
        {

            return (Model.Bucket.GetBucketReplicationResult)Excute(request, new Model.Bucket.GetBucketReplicationResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void GetBucketReplication(GetBucketReplicationRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new GetBucketReplicationResult(), successCallback, failCallback);
        }

        public DeleteBucketReplicationResult DeleteBucketReplication(DeleteBucketReplicationRequest request)
        {

            return (Model.Bucket.DeleteBucketReplicationResult)Excute(request, new Model.Bucket.DeleteBucketReplicationResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void DeleteBucketReplication(DeleteBucketReplicationRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new DeleteBucketReplicationResult(), successCallback, failCallback);
        }

        public PutBucketVersioningResult PutBucketVersioning(PutBucketVersioningRequest request)
        {

            return (Model.Bucket.PutBucketVersioningResult)Excute(request, new Model.Bucket.PutBucketVersioningResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void PutBucketVersioning(PutBucketVersioningRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new PutBucketVersioningResult(), successCallback, failCallback);
        }

        public GetBucketVersioningResult GetBucketVersioning(GetBucketVersioningRequest request)
        {

            return (Model.Bucket.GetBucketVersioningResult)Excute(request, new Model.Bucket.GetBucketVersioningResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void GetBucketVersioning(GetBucketVersioningRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new GetBucketVersioningResult(), successCallback, failCallback);
        }

        public ListBucketVersionsResult ListBucketVersions(ListBucketVersionsRequest request)
        {

            return (Model.Bucket.ListBucketVersionsResult)Excute(request, new Model.Bucket.ListBucketVersionsResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void ListBucketVersions(ListBucketVersionsRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new ListBucketVersionsResult(), successCallback, failCallback);
        }

        public ListMultiUploadsResult ListMultiUploads(ListMultiUploadsRequest request)
        {

            return (Model.Bucket.ListMultiUploadsResult)Excute(request, new Model.Bucket.ListMultiUploadsResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void ListMultiUploads(ListMultiUploadsRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new ListMultiUploadsResult(), successCallback, failCallback);
        }

        public DeleteBucketPolicyResult DeleteBucketPolicy(DeleteBucketPolicyRequest request)
        {

            return (Model.Bucket.DeleteBucketPolicyResult)Excute(request, new Model.Bucket.DeleteBucketPolicyResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void DeleteBucketPolicy(DeleteBucketPolicyRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new DeleteBucketPolicyResult(), successCallback, failCallback);
        }

        public PutObjectResult PutObject(PutObjectRequest request)
        {

            return (Model.Object.PutObjectResult)Excute(request, new Model.Object.PutObjectResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void PutObject(PutObjectRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new PutObjectResult(), successCallback, failCallback);
        }

        public HeadObjectResult HeadObject(HeadObjectRequest request)
        {

            return (Model.Object.HeadObjectResult)Excute(request, new Model.Object.HeadObjectResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void HeadObject(HeadObjectRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new HeadObjectResult(), successCallback, failCallback);
        }

        public GetObjectResult GetObject(GetObjectRequest request)
        {

            return (Model.Object.GetObjectResult)Excute(request, new Model.Object.GetObjectResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void GetObject(GetObjectRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new GetObjectResult(), successCallback, failCallback);
        }

        public PutObjectACLResult PutObjectACL(PutObjectACLRequest request)
        {

            return (Model.Object.PutObjectACLResult)Excute(request, new Model.Object.PutObjectACLResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void PutObjectACL(PutObjectACLRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new PutObjectACLResult(), successCallback, failCallback);
        }

        public GetObjectACLResult GetObjectACL(GetObjectACLRequest request)
        {

            return (Model.Object.GetObjectACLResult)Excute(request, new Model.Object.GetObjectACLResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void GetObjectACL(GetObjectACLRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new GetObjectACLResult(), successCallback, failCallback);
        }

        public DeleteObjectResult DeleteObject(DeleteObjectRequest request)
        {

            return (Model.Object.DeleteObjectResult)Excute(request, new Model.Object.DeleteObjectResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void DeleteObject(DeleteObjectRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new DeleteObjectResult(), successCallback, failCallback);
        }

        public DeleteMultiObjectResult DeleteMultiObjects(DeleteMultiObjectRequest request)
        {

            return (Model.Object.DeleteMultiObjectResult)Excute(request, new Model.Object.DeleteMultiObjectResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void DeleteMultiObjects(DeleteMultiObjectRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new DeleteMultiObjectResult(), successCallback, failCallback);
        }

        public InitMultipartUploadResult InitMultipartUpload(InitMultipartUploadRequest request)
        {

            return (Model.Object.InitMultipartUploadResult)Excute(request, new Model.Object.InitMultipartUploadResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void InitMultipartUpload(InitMultipartUploadRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new InitMultipartUploadResult(), successCallback, failCallback);
        }

        public ListPartsResult ListParts(ListPartsRequest request)
        {

            return (Model.Object.ListPartsResult)Excute(request, new Model.Object.ListPartsResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void ListParts(ListPartsRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new ListPartsResult(), successCallback, failCallback);
        }

        public UploadPartResult UploadPart(UploadPartRequest request)
        {

            return (Model.Object.UploadPartResult)Excute(request, new Model.Object.UploadPartResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void UploadPart(UploadPartRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new UploadPartResult(), successCallback, failCallback);
        }

        public CompleteMultipartUploadResult CompleteMultiUpload(CompleteMultipartUploadRequest request)
        {

            return (Model.Object.CompleteMultipartUploadResult)Excute(request, new Model.Object.CompleteMultipartUploadResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void CompleteMultiUpload(CompleteMultipartUploadRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new CompleteMultipartUploadResult(), successCallback, failCallback);
        }

        public AbortMultipartUploadResult AbortMultiUpload(AbortMultipartUploadRequest request)
        {

            return (Model.Object.AbortMultipartUploadResult)Excute(request, new Model.Object.AbortMultipartUploadResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void AbortMultiUpload(AbortMultipartUploadRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new AbortMultipartUploadResult(), successCallback, failCallback);
        }

        public CopyObjectResult CopyObject(CopyObjectRequest request)
        {

            return (Model.Object.CopyObjectResult)Excute(request, new Model.Object.CopyObjectResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void CopyObject(CopyObjectRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new CopyObjectResult(), successCallback, failCallback);
        }

        public UploadPartCopyResult PartCopy(UploadPartCopyRequest request)
        {

            return (Model.Object.UploadPartCopyResult)Excute(request, new Model.Object.UploadPartCopyResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void PartCopy(UploadPartCopyRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new UploadPartCopyResult(), successCallback, failCallback);
        }

        public OptionObjectResult OptionObject(OptionObjectRequest request)
        {

            return (Model.Object.OptionObjectResult)Excute(request, new Model.Object.OptionObjectResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void OptionObject(OptionObjectRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new OptionObjectResult(), successCallback, failCallback);
        }

        public PostObjectResult PostObject(PostObjectRequest request)
        {

            return (Model.Object.PostObjectResult)Excute(request, new Model.Object.PostObjectResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void PostObject(PostObjectRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new PostObjectResult(), successCallback, failCallback);
        }

        public RestoreObjectResult RestoreObject(RestoreObjectRequest request)
        {

            return (Model.Object.RestoreObjectResult)Excute(request, new Model.Object.RestoreObjectResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void RestoreObject(RestoreObjectRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new RestoreObjectResult(), successCallback, failCallback);
        }

        public string GenerateSign(string method, string key, Dictionary<string, string> queryParameters, Dictionary<string, string> headers, long signDurationSecond)
        {

            try
            {
                string signTime = null;

                if (signDurationSecond > 0)
                {
                    long currentTimeSecond = TimeUtils.GetCurrentTime(TimeUnit.Seconds);

                    signTime = String.Format("{0};{1}", currentTimeSecond, currentTimeSecond + signDurationSecond);
                }

                Dictionary<string, string> encodeQuery = null;

                if (queryParameters != null)
                {
                    encodeQuery = new Dictionary<string, string>(queryParameters.Count);

                    foreach (KeyValuePair<string, string> keyValuePair in queryParameters)
                    {
                        encodeQuery[keyValuePair.Key] = URLEncodeUtils.Encode(keyValuePair.Value);
                    }
                }

                return CosXmlSigner.GenerateSign(method, key, encodeQuery, headers, signTime, qcloudCredentailProvider.GetQCloudCredentials());
            }
            catch (CosClientException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new CosClientException((int)CosClientError.InvalidArgument, ex.Message, ex);
            }
        }

        public string GenerateSignURL(PreSignatureStruct preSignatureStruct)
        {

            try
            {

                if (preSignatureStruct.httpMethod == null)
                {
                    throw new CosClientException((int)CosClientError.InvalidArgument, "httpMethod = null");
                }

                if (preSignatureStruct.key == null)
                {
                    throw new CosClientException((int)CosClientError.InvalidArgument, "key = null");
                }

                StringBuilder urlBuilder = new StringBuilder();

                if (preSignatureStruct.isHttps)
                {
                    urlBuilder.Append("https://");
                }
                else
                {
                    urlBuilder.Append("http://");
                }

                if (preSignatureStruct.host == null)
                {

                    if (preSignatureStruct.bucket == null)
                    {
                        throw new CosClientException((int)CosClientError.InvalidArgument, "bucket = null");
                    }

                    if (preSignatureStruct.bucket.EndsWith("-" + preSignatureStruct.appid))
                    {
                        urlBuilder.Append(preSignatureStruct.bucket);
                    }
                    else
                    {
                        urlBuilder.Append(preSignatureStruct.bucket).Append("-")
                            .Append(preSignatureStruct.appid);
                    }

                    urlBuilder.Append(".cos.")
                        .Append(preSignatureStruct.region)
                        .Append(".myqcloud.com");
                }
                else
                {
                    urlBuilder.Append(preSignatureStruct.host);
                }

                if (!preSignatureStruct.key.StartsWith("/"))
                {
                    preSignatureStruct.key = "/" + preSignatureStruct.key;
                }

                urlBuilder.Append(preSignatureStruct.key);

                string sign = GenerateSign(preSignatureStruct.httpMethod, preSignatureStruct.key,
                    preSignatureStruct.queryParameters, preSignatureStruct.headers, preSignatureStruct.signDurationSecond);

                StringBuilder queryBuilder = new StringBuilder();

                if (preSignatureStruct.queryParameters != null && preSignatureStruct.queryParameters.Count > 0)
                {

                    foreach (KeyValuePair<string, string> keyValuePair in preSignatureStruct.queryParameters)
                    {
                        queryBuilder.Append(keyValuePair.Key).Append('=').Append(URLEncodeUtils.Encode(keyValuePair.Value));
                        queryBuilder.Append('&');
                    }
                }

                queryBuilder.Append(sign);
                urlBuilder.Append("?").Append(queryBuilder.ToString());

                return urlBuilder.ToString();
            }
            catch (CosClientException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new CosClientException((int)CosClientError.InvalidArgument, ex.Message, ex);
            }
        }

        public string GetAccessURL(CosRequest request)
        {

            try
            {
                CheckAppidAndRegion(request);
                request.CheckParameters();
                StringBuilder urlBuilder = new StringBuilder();

                if (request.IsHttps != null && (bool)request.IsHttps)
                {
                    urlBuilder.Append("https://");
                }
                else
                {
                    urlBuilder.Append("http://");
                }

                urlBuilder.Append(request.GetHost());
                urlBuilder.Append(request.RequestPath);

                return urlBuilder.ToString();
            }
            catch (CosClientException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new CosClientException((int)CosClientError.InvalidArgument, ex.Message, ex);
            }
        }

        public void Cancel(CosRequest cosRequest)
        {

            if (cosRequest != null)
            {
                cosRequest.Cancel();
            }
        }

        public GetObjectBytesResult GetObject(GetObjectBytesRequest request)
        {

            return (Model.Object.GetObjectBytesResult)Excute(request, new Model.Object.GetObjectBytesResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void GetObject(GetObjectBytesRequest request, OnSuccessCallback<CosResult> successCallback, OnFailedCallback failCallback)
        {
            Schedue(request, new GetObjectBytesResult(), successCallback, failCallback);
        }

        public PutBucketWebsiteResult PutBucketWebsite(PutBucketWebsiteRequest request)
        {

            return (Model.Bucket.PutBucketWebsiteResult)Excute(request, new Model.Bucket.PutBucketWebsiteResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void PutBucketWebsiteAsync(PutBucketWebsiteRequest request, OnSuccessCallback<CosResult> successCallback, OnFailedCallback failCallback)
        {
            Schedue(request, new Model.Bucket.PutBucketWebsiteResult(), successCallback, failCallback);
        }

        public GetBucketWebsiteResult GetBucketWebsite(GetBucketWebsiteRequest request)
        {

            return (Model.Bucket.GetBucketWebsiteResult)Excute(request, new Model.Bucket.GetBucketWebsiteResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void GetBucketWebsiteAsync(GetBucketWebsiteRequest request, OnSuccessCallback<CosResult> successCallback, OnFailedCallback failCallback)
        {
            Schedue(request, new Model.Bucket.GetBucketWebsiteResult(), successCallback, failCallback);
        }

        public DeleteBucketWebsiteResult DeleteBucketWebsite(DeleteBucketWebsiteRequest request)
        {

            return (Model.Bucket.DeleteBucketWebsiteResult)Excute(request, new Model.Bucket.DeleteBucketWebsiteResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void DeleteBucketWebsiteAsync(DeleteBucketWebsiteRequest request, OnSuccessCallback<CosResult> successCallback, OnFailedCallback failCallback)
        {
            Schedue(request, new Model.Bucket.DeleteBucketWebsiteResult(), successCallback, failCallback);
        }

        public PutBucketLoggingResult PutBucketLogging(PutBucketLoggingRequest request)
        {

            return (Model.Bucket.PutBucketLoggingResult)Excute(request, new Model.Bucket.PutBucketLoggingResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void PutBucketLoggingAsync(PutBucketLoggingRequest request, OnSuccessCallback<CosResult> successCallback, OnFailedCallback failCallback)
        {
            Schedue(request, new Model.Bucket.PutBucketLoggingResult(), successCallback, failCallback);
        }

        public GetBucketLoggingResult GetBucketLogging(GetBucketLoggingRequest request)
        {

            return (Model.Bucket.GetBucketLoggingResult)Excute(request, new Model.Bucket.GetBucketLoggingResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void GetBucketLoggingAsync(GetBucketLoggingRequest request, OnSuccessCallback<CosResult> successCallback, OnFailedCallback failCallback)
        {
            Schedue(request, new Model.Bucket.GetBucketLoggingResult(), successCallback, failCallback);
        }

        public PutBucketInventoryResult PutBucketInventory(PutBucketInventoryRequest request)
        {

            return (Model.Bucket.PutBucketInventoryResult)Excute(request, new Model.Bucket.PutBucketInventoryResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void PutBucketInventoryAsync(PutBucketInventoryRequest request, OnSuccessCallback<CosResult> successCallback, OnFailedCallback failCallback)
        {
            Schedue(request, new Model.Bucket.PutBucketInventoryResult(), successCallback, failCallback);
        }

        public GetBucketInventoryResult GetBucketInventory(GetBucketInventoryRequest request)
        {

            return (Model.Bucket.GetBucketInventoryResult)Excute(request, new Model.Bucket.GetBucketInventoryResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void GetBucketInventoryAsync(GetBucketInventoryRequest request, OnSuccessCallback<CosResult> successCallback, OnFailedCallback failCallback)
        {
            Schedue(request, new Model.Bucket.GetBucketInventoryResult(), successCallback, failCallback);
        }

        public DeleteBucketInventoryResult DeleteBucketInventory(DeleteBucketInventoryRequest request)
        {

            return (Model.Bucket.DeleteBucketInventoryResult)Excute(request, new Model.Bucket.DeleteBucketInventoryResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void DeleteInventoryAsync(DeleteBucketInventoryRequest request, OnSuccessCallback<CosResult> successCallback, OnFailedCallback failCallback)
        {
            Schedue(request, new Model.Bucket.DeleteBucketInventoryResult(), successCallback, failCallback);
        }

        public ListBucketInventoryResult ListBucketInventory(ListBucketInventoryRequest request)
        {

            return (Model.Bucket.ListBucketInventoryResult)Excute(request, new Model.Bucket.ListBucketInventoryResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void ListBucketInventoryAsync(ListBucketInventoryRequest request, OnSuccessCallback<CosResult> successCallback, OnFailedCallback failCallback)
        {
            Schedue(request, new Model.Bucket.ListBucketInventoryResult(), successCallback, failCallback);
        }

        public PutBucketTaggingResult PutBucketTagging(PutBucketTaggingRequest request)
        {

            return (Model.Bucket.PutBucketTaggingResult)Excute(request, new Model.Bucket.PutBucketTaggingResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void PutBucketTaggingAsync(PutBucketTaggingRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new Model.Bucket.PutBucketTaggingResult(), successCallback, failCallback);
        }

        public GetBucketTaggingResult GetBucketTagging(GetBucketTaggingRequest request)
        {

            return (Model.Bucket.GetBucketTaggingResult)Excute(request, new Model.Bucket.GetBucketTaggingResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void GetBucketTaggingAsync(GetBucketTaggingRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new Model.Bucket.GetBucketTaggingResult(), successCallback, failCallback);
        }

        public DeleteBucketTaggingResult DeleteBucketTagging(DeleteBucketTaggingRequest request)
        {

            return (Model.Bucket.DeleteBucketTaggingResult)Excute(request, new Model.Bucket.DeleteBucketTaggingResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void DeleteBucketTaggingAsync(DeleteBucketTaggingRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new Model.Bucket.DeleteBucketTaggingResult(), successCallback, failCallback);
        }

        public PutBucketDomainResult PutBucketDomain(PutBucketDomainRequest request)
        {

            return (Model.Bucket.PutBucketDomainResult)Excute(request, new Model.Bucket.PutBucketDomainResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void PutBucketDomainAsync(PutBucketDomainRequest request, OnSuccessCallback<CosResult> successCallback, OnFailedCallback failCallback)
        {
            Schedue(request, new Model.Bucket.PutBucketDomainResult(), successCallback, failCallback);
        }

        public GetBucketDomainResult GetBucketDomain(GetBucketDomainRequest request)
        {

            return (Model.Bucket.GetBucketDomainResult)Excute(request, new Model.Bucket.GetBucketDomainResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        public void GetBucketDomainAsync(GetBucketDomainRequest request, OnSuccessCallback<CosResult> successCallback, OnFailedCallback failCallback)
        {
            Schedue(request, new Model.Bucket.GetBucketDomainResult(), successCallback, failCallback);
        }

        SelectObjectResult CosXml.SelectObject(SelectObjectRequest request)
        {

            return (Model.Object.SelectObjectResult)Excute(request, new Model.Object.SelectObjectResult());
        }

        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        void CosXml.SelectObjectAsync(SelectObjectRequest request, OnSuccessCallback<CosResult> successCallback, OnFailedCallback failCallback)
        {
            Schedue(request, new Model.Object.SelectObjectResult(), successCallback, failCallback);
        }

        public GetBucketIntelligentTieringResult GetBucketIntelligentTieringConfiguration(GetBucketIntelligentTieringRequest request)
        {

            try
            {

                return Execute(request, new GetBucketIntelligentTieringResult());
            }
            catch (CosServerException e)
            {

                if (e.statusCode == 404)
                {

                    return new GetBucketIntelligentTieringResult();
                }

                throw e;
            }
        }

        public CosResult PutBucketIntelligentTiering(PutBucketIntelligentTieringRequest request)
        {

            return Execute(request, new CosResult());
        }

        public SensitiveContentRecognitionResult SensitiveContentRecognition(SensitiveContentRecognitionRequest request)
        {

            return Execute(request, new SensitiveContentRecognitionResult());
        }

        /// <summary>
        /// 图片处理
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ImageProcessResult ImageProcess(ImageProcessRequest request)
        {

            return Execute(request, new ImageProcessResult());
        }
    }
}

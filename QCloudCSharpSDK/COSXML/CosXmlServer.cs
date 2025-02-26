using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
using System.Diagnostics.CodeAnalysis;

#if !COMPATIBLE
using System.Threading.Tasks;
#endif

namespace COSXML
{
    /// <summary>
    /// CosXml 实现类
    /// </summary>
    public class CosXmlServer : CosXml
    {
        private CosXmlConfig config;

        private QCloudCredentialProvider credentialProvider;

        private HttpClient httpClient;

        /// <summary>
        /// 创建一个新实例
        /// </summary>
        /// <param name="config">服务配置</param>
        /// <param name="qcloudCredentailProvider">凭证提供者</param>
        public CosXmlServer(CosXmlConfig config, QCloudCredentialProvider qcloudCredentailProvider)
        {

            if (config == null)
            {
                throw new CosClientException((int) CosClientError.InvalidArgument, "Config is null.");
            }
            
            this.config = config;

            if (this.config.IsDebugLog)
            {
                QLog.AddLogAdapter(new LogImpl());
            }

            this.credentialProvider = qcloudCredentailProvider;
            httpClient = HttpClient.GetInstance();
            httpClient.Init(this.config.HttpConfig);
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

#if !COMPATIBLE
        public Task<T> ExecuteAsync<T>(CosRequest request) where T : CosResult
        {
            T result = Activator.CreateInstance<T>();

            CheckAppidAndRegion(request);

            var t = new TaskCompletionSource<T>();


            Schedue(request, result,
                delegate(CosResult cosResult)
                {
                    t.TrySetResult(result as T);
                }

                , delegate(CosClientException clientException, CosServerException serverException)
                {

                    if (clientException != null)
                    {
                        t.TrySetException(clientException);
                    }
                    else
                    {
                        t.TrySetException(serverException);
                    }
                }
            );

            return t.Task;
        }
#endif

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
            httpClient.Excute(request, result, credentialProvider);

            return result;
        }

        private void Schedue(CosRequest request, CosResult result, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback)
        {
            CheckAppidAndRegion(request);
            httpClient.Schedue(request, result, successCallback, failCallback, credentialProvider);
        }

        public Model.Service.GetServiceResult GetService(Model.Service.GetServiceRequest request)
        {

            return (Model.Service.GetServiceResult)Excute(request, new Model.Service.GetServiceResult());
        }

         
        public void GetService(Model.Service.GetServiceRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new GetServiceResult(), successCallback, failCallback);
        }

        public PutBucketResult PutBucket(PutBucketRequest request)
        {

            return (Model.Bucket.PutBucketResult)Excute(request, new Model.Bucket.PutBucketResult());
        }

         
        public void PutBucket(PutBucketRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new PutBucketResult(), successCallback, failCallback);
        }

        public DeleteBucketResult DeleteBucket(DeleteBucketRequest request)
        {

            return (Model.Bucket.DeleteBucketResult)Excute(request, new Model.Bucket.DeleteBucketResult());
        }

         
        public void DeleteBucket(DeleteBucketRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new DeleteBucketResult(), successCallback, failCallback);
        }

        public HeadBucketResult HeadBucket(HeadBucketRequest request)
        {

            return (Model.Bucket.HeadBucketResult)Excute(request, new Model.Bucket.HeadBucketResult());
        }

        public bool DoesBucketExist(DoesBucketExistRequest request)
        {
            try {
                CosResult result = Excute(request, new Model.Bucket.HeadBucketResult());
                if (result.httpCode == 200) {
                    return true;
                }
                return false;
            }
            catch (CosServerException serverEx) {
                if (serverEx.statusCode == 404) {
                    return false;
                } else {
                    throw serverEx;
                }
            }
            catch (Exception e) {
                throw e;
            }
        }

         
        public void HeadBucket(HeadBucketRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new HeadBucketResult(), successCallback, failCallback);
        }

        public GetBucketResult GetBucket(GetBucketRequest request)
        {

            return (Model.Bucket.GetBucketResult)Excute(request, new Model.Bucket.GetBucketResult());
        }

         
        public void GetBucket(GetBucketRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new GetBucketResult(), successCallback, failCallback);
        }
        
        public GetBucketPolicyResult GetBucketPolicy(GetBucketPolicyRequest request)
        { 
            return (Model.Bucket.GetBucketPolicyResult)Excute(request, new Model.Bucket.GetBucketPolicyResult());
        }
        
        public PutBucketPolicyResult PutBucketPolicy(PutBucketPolicyRequest request)
        {
            return (Model.Bucket.PutBucketPolicyResult)Excute(request, new Model.Bucket.PutBucketPolicyResult());
        }
        
        public PutBucketACLResult PutBucketACL(PutBucketACLRequest request)
        {

            return (Model.Bucket.PutBucketACLResult)Excute(request, new Model.Bucket.PutBucketACLResult());
        }

         
        public void PutBucketACL(PutBucketACLRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new PutBucketACLResult(), successCallback, failCallback);
        }

        public GetBucketACLResult GetBucketACL(GetBucketACLRequest request)
        {

            return (Model.Bucket.GetBucketACLResult)Excute(request, new Model.Bucket.GetBucketACLResult());
        }

         
        public void GetBucketACL(GetBucketACLRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new GetBucketACLResult(), successCallback, failCallback);
        }

        public PutBucketCORSResult PutBucketCORS(PutBucketCORSRequest request)
        {

            return (Model.Bucket.PutBucketCORSResult)Excute(request, new Model.Bucket.PutBucketCORSResult());
        }

         
        public void PutBucketCORS(PutBucketCORSRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new PutBucketCORSResult(), successCallback, failCallback);
        }

        public GetBucketCORSResult GetBucketCORS(GetBucketCORSRequest request)
        {

            return (Model.Bucket.GetBucketCORSResult)Excute(request, new Model.Bucket.GetBucketCORSResult());
        }

         
        public void GetBucketCORS(GetBucketCORSRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new GetBucketCORSResult(), successCallback, failCallback);
        }

        public DeleteBucketCORSResult DeleteBucketCORS(DeleteBucketCORSRequest request)
        {

            return (Model.Bucket.DeleteBucketCORSResult)Excute(request, new Model.Bucket.DeleteBucketCORSResult());
        }

         
        public void DeleteBucketCORS(DeleteBucketCORSRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new DeleteBucketCORSResult(), successCallback, failCallback);
        }

        public PutBucketLifecycleResult PutBucketLifecycle(PutBucketLifecycleRequest request)
        {

            return (Model.Bucket.PutBucketLifecycleResult)Excute(request, new Model.Bucket.PutBucketLifecycleResult());
        }

         
        public void PutBucketLifecycle(PutBucketLifecycleRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new PutBucketLifecycleResult(), successCallback, failCallback);
        }

        public GetBucketLifecycleResult GetBucketLifecycle(GetBucketLifecycleRequest request)
        {

            return (Model.Bucket.GetBucketLifecycleResult)Excute(request, new Model.Bucket.GetBucketLifecycleResult());
        }

         
        public void GetBucketLifecycle(GetBucketLifecycleRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new GetBucketLifecycleResult(), successCallback, failCallback);
        }

        public DeleteBucketLifecycleResult DeleteBucketLifecycle(DeleteBucketLifecycleRequest request)
        {

            return (Model.Bucket.DeleteBucketLifecycleResult)Excute(request, new Model.Bucket.DeleteBucketLifecycleResult());
        }

         
        public void DeleteBucketLifecycle(DeleteBucketLifecycleRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new DeleteBucketLifecycleResult(), successCallback, failCallback);
        }

        public PutBucketReplicationResult PutBucketReplication(PutBucketReplicationRequest request)
        {

            return (Model.Bucket.PutBucketReplicationResult)Excute(request, new Model.Bucket.PutBucketReplicationResult());
        }

         
        public void PutBucketReplication(PutBucketReplicationRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new PutBucketReplicationResult(), successCallback, failCallback);
        }

        public GetBucketReplicationResult GetBucketReplication(GetBucketReplicationRequest request)
        {

            return (Model.Bucket.GetBucketReplicationResult)Excute(request, new Model.Bucket.GetBucketReplicationResult());
        }

         
        public void GetBucketReplication(GetBucketReplicationRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new GetBucketReplicationResult(), successCallback, failCallback);
        }

        public DeleteBucketReplicationResult DeleteBucketReplication(DeleteBucketReplicationRequest request)
        {

            return (Model.Bucket.DeleteBucketReplicationResult)Excute(request, new Model.Bucket.DeleteBucketReplicationResult());
        }

         
        public void DeleteBucketReplication(DeleteBucketReplicationRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new DeleteBucketReplicationResult(), successCallback, failCallback);
        }

        public PutBucketVersioningResult PutBucketVersioning(PutBucketVersioningRequest request)
        {

            return (Model.Bucket.PutBucketVersioningResult)Excute(request, new Model.Bucket.PutBucketVersioningResult());
        }

         
        public void PutBucketVersioning(PutBucketVersioningRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new PutBucketVersioningResult(), successCallback, failCallback);
        }

        public GetBucketVersioningResult GetBucketVersioning(GetBucketVersioningRequest request)
        {

            return (Model.Bucket.GetBucketVersioningResult)Excute(request, new Model.Bucket.GetBucketVersioningResult());
        }

         
        public void GetBucketVersioning(GetBucketVersioningRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new GetBucketVersioningResult(), successCallback, failCallback);
        }

        public ListBucketVersionsResult ListBucketVersions(ListBucketVersionsRequest request)
        {

            return (Model.Bucket.ListBucketVersionsResult)Excute(request, new Model.Bucket.ListBucketVersionsResult());
        }

         
        public void ListBucketVersions(ListBucketVersionsRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new ListBucketVersionsResult(), successCallback, failCallback);
        }

        public ListMultiUploadsResult ListMultiUploads(ListMultiUploadsRequest request)
        {

            return (Model.Bucket.ListMultiUploadsResult)Excute(request, new Model.Bucket.ListMultiUploadsResult());
        }

        public PutBucketRefererResult PutBucketReferer(PutBucketRefererRequest request)
        {

            return (Model.Bucket.PutBucketRefererResult)Excute(request, new Model.Bucket.PutBucketRefererResult());
        }

        public void PutBucketReferer(PutBucketRefererRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new PutBucketRefererResult(), successCallback, failCallback);
        }

        public GetBucketRefererResult GetBucketReferer(GetBucketRefererRequest request)
        {

            return (Model.Bucket.GetBucketRefererResult)Excute(request, new Model.Bucket.GetBucketRefererResult());
        }

        public void GetBucketReferer(GetBucketRefererRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new GetBucketRefererResult(), successCallback, failCallback);
        }
         
        public void ListMultiUploads(ListMultiUploadsRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new ListMultiUploadsResult(), successCallback, failCallback);
        }

        public DeleteBucketPolicyResult DeleteBucketPolicy(DeleteBucketPolicyRequest request)
        {

            return (Model.Bucket.DeleteBucketPolicyResult)Excute(request, new Model.Bucket.DeleteBucketPolicyResult());
        }

         
        public void DeleteBucketPolicy(DeleteBucketPolicyRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new DeleteBucketPolicyResult(), successCallback, failCallback);
        }

        public PutObjectResult PutObject(PutObjectRequest request)
        {

            return (Model.Object.PutObjectResult)Excute(request, new Model.Object.PutObjectResult());
        }

         
        public void PutObject(PutObjectRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new PutObjectResult(), successCallback, failCallback);
        }

         
        public void AppendObject(AppendObjectRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback) 
        {
            Schedue(request, new AppendObjectResult(), successCallback, failCallback);
        }
        
        public AppendObjectResult AppendObject(AppendObjectRequest request) 
        {

            return (Model.Object.AppendObjectResult)Excute(request, new Model.Object.AppendObjectResult());
        }

        public HeadObjectResult HeadObject(HeadObjectRequest request)
        {

            return (Model.Object.HeadObjectResult)Excute(request, new Model.Object.HeadObjectResult());
        }

         
        public void HeadObject(HeadObjectRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new HeadObjectResult(), successCallback, failCallback);
        }

        public bool DoesObjectExist(DoesObjectExistRequest request)
        {
            try {
                CosResult result = Excute(request, new Model.Object.HeadObjectResult());
                if (result.httpCode == 200) {
                    return true;
                }
                return false;
            }
            catch (CosServerException serverEx) {
                if (serverEx.statusCode == 404) {
                    return false;
                }
                else {
                    throw serverEx;
                }
            }
            catch (Exception e) {
                throw e;
            }
        }

        public GetObjectResult GetObject(GetObjectRequest request)
        {

            return (Model.Object.GetObjectResult)Excute(request, new Model.Object.GetObjectResult());
        }

         
        public void GetObject(GetObjectRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new GetObjectResult(), successCallback, failCallback);
        }

        public PutObjectACLResult PutObjectACL(PutObjectACLRequest request)
        {

            return (Model.Object.PutObjectACLResult)Excute(request, new Model.Object.PutObjectACLResult());
        }

         
        public void PutObjectACL(PutObjectACLRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new PutObjectACLResult(), successCallback, failCallback);
        }

        public GetObjectACLResult GetObjectACL(GetObjectACLRequest request)
        {

            return (Model.Object.GetObjectACLResult)Excute(request, new Model.Object.GetObjectACLResult());
        }

         
        public void GetObjectACL(GetObjectACLRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new GetObjectACLResult(), successCallback, failCallback);
        }

        public PutObjectTaggingResult PutObjectTagging(PutObjectTaggingRequest request)
        {
            return (Model.Object.PutObjectTaggingResult)Excute(request, new Model.Object.PutObjectTaggingResult());
        }

        public void PutObjectTagging(PutObjectTaggingRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new PutObjectTaggingResult(), successCallback, failCallback);
        }

        public GetObjectTaggingResult GetObjectTagging(GetObjectTaggingRequest request)
        {
            return (Model.Object.GetObjectTaggingResult)Excute(request, new Model.Object.GetObjectTaggingResult());
        }

        public void GetObjectTagging(GetObjectTaggingRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new GetObjectTaggingResult(), successCallback, failCallback);
        }

        public DeleteObjectTaggingResult DeleteObjectTagging(DeleteObjectTaggingRequest request)
        {
            return (Model.Object.DeleteObjectTaggingResult)Excute(request, new Model.Object.DeleteObjectTaggingResult());
        }

        public void DeleteObjectTagging(DeleteObjectTaggingRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new DeleteObjectTaggingResult(), successCallback, failCallback);
        }

        public DeleteObjectResult DeleteObject(DeleteObjectRequest request)
        {

            return (Model.Object.DeleteObjectResult)Excute(request, new Model.Object.DeleteObjectResult());
        }

         
        public void DeleteObject(DeleteObjectRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new DeleteObjectResult(), successCallback, failCallback);
        }

        public DeleteMultiObjectResult DeleteMultiObjects(DeleteMultiObjectRequest request)
        {

            return (Model.Object.DeleteMultiObjectResult)Excute(request, new Model.Object.DeleteMultiObjectResult());
        }

         
        public void DeleteMultiObjects(DeleteMultiObjectRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new DeleteMultiObjectResult(), successCallback, failCallback);
        }

        public InitMultipartUploadResult InitMultipartUpload(InitMultipartUploadRequest request)
        {

            return (Model.Object.InitMultipartUploadResult)Excute(request, new Model.Object.InitMultipartUploadResult());
        }

         
        public void InitMultipartUpload(InitMultipartUploadRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new InitMultipartUploadResult(), successCallback, failCallback);
        }

        public ListPartsResult ListParts(ListPartsRequest request)
        {

            return (Model.Object.ListPartsResult)Excute(request, new Model.Object.ListPartsResult());
        }

         
        public void ListParts(ListPartsRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new ListPartsResult(), successCallback, failCallback);
        }

        public UploadPartResult UploadPart(UploadPartRequest request)
        {

            return (Model.Object.UploadPartResult)Excute(request, new Model.Object.UploadPartResult());
        }

         
        public void UploadPart(UploadPartRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new UploadPartResult(), successCallback, failCallback);
        }

        public CompleteMultipartUploadResult CompleteMultiUpload(CompleteMultipartUploadRequest request)
        {

            return (Model.Object.CompleteMultipartUploadResult)Excute(request, new Model.Object.CompleteMultipartUploadResult());
        }

         
        public void CompleteMultiUpload(CompleteMultipartUploadRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new CompleteMultipartUploadResult(), successCallback, failCallback);
        }

        public AbortMultipartUploadResult AbortMultiUpload(AbortMultipartUploadRequest request)
        {

            return (Model.Object.AbortMultipartUploadResult)Excute(request, new Model.Object.AbortMultipartUploadResult());
        }

         
        public void AbortMultiUpload(AbortMultipartUploadRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new AbortMultipartUploadResult(), successCallback, failCallback);
        }

        public CopyObjectResult CopyObject(CopyObjectRequest request)
        {

            return (Model.Object.CopyObjectResult)Excute(request, new Model.Object.CopyObjectResult());
        }

         
        public void CopyObject(CopyObjectRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new CopyObjectResult(), successCallback, failCallback);
        }

        public UploadPartCopyResult PartCopy(UploadPartCopyRequest request)
        {

            return (Model.Object.UploadPartCopyResult)Excute(request, new Model.Object.UploadPartCopyResult());
        }

         
        public void PartCopy(UploadPartCopyRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new UploadPartCopyResult(), successCallback, failCallback);
        }

        public OptionObjectResult OptionObject(OptionObjectRequest request)
        {

            return (Model.Object.OptionObjectResult)Excute(request, new Model.Object.OptionObjectResult());
        }

         
        public void OptionObject(OptionObjectRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new OptionObjectResult(), successCallback, failCallback);
        }

        public PostObjectResult PostObject(PostObjectRequest request)
        {

            return (Model.Object.PostObjectResult)Excute(request, new Model.Object.PostObjectResult());
        }

         
        public void PostObject(PostObjectRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new PostObjectResult(), successCallback, failCallback);
        }

        public RestoreObjectResult RestoreObject(RestoreObjectRequest request)
        {

            return (Model.Object.RestoreObjectResult)Excute(request, new Model.Object.RestoreObjectResult());
        }

         
        public void RestoreObject(RestoreObjectRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new RestoreObjectResult(), successCallback, failCallback);
        }

        public string GenerateSign(string method, string key, Dictionary<string, string> queryParameters, 
                                   Dictionary<string, string> headers, long signDurationSecond, long keyDurationSecond)
        {

            try
            {
                string signTime = null;
                long currentTimeSecond = TimeUtils.GetCurrentTime(TimeUnit.Seconds);

                if (signDurationSecond > 0)
                {
                    signTime = String.Format("{0};{1}", currentTimeSecond, currentTimeSecond + signDurationSecond);
                }

                string keyTime = null;
                if (keyDurationSecond > 0)
                {
                    keyTime = String.Format("{0};{1}", currentTimeSecond, currentTimeSecond + keyDurationSecond);
                }

                Dictionary<string, string> encodeQuery = null;

                if (queryParameters != null)
                {
                    encodeQuery = new Dictionary<string, string>(queryParameters.Count);

                    foreach (KeyValuePair<string, string> keyValuePair in queryParameters)
                    {
                        if (keyValuePair.Key == null || keyValuePair.Key == "")
                        {
                            continue;
                        }
                        else if (keyValuePair.Value == null)
                        {
                            encodeQuery[URLEncodeUtils.Encode(keyValuePair.Key).ToLower()] = URLEncodeUtils.Encode("");
                        }
                        else 
                        {
                            encodeQuery[URLEncodeUtils.Encode(keyValuePair.Key).ToLower()] = URLEncodeUtils.Encode(keyValuePair.Value);
                        } 
                    }
                }

                return CosXmlSigner.GenerateSign(method, key, encodeQuery, headers, signTime, keyTime, credentialProvider.GetQCloudCredentialsCompat(null));
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
                    StringBuilder host = new StringBuilder();

                    if (preSignatureStruct.bucket == null)
                    {
                        throw new CosClientException((int)CosClientError.InvalidArgument, "bucket = null");
                    }

                    if (preSignatureStruct.bucket.EndsWith("-" + preSignatureStruct.appid))
                    {
                        host.Append(preSignatureStruct.bucket);
                    }
                    else
                    {
                        host.Append(preSignatureStruct.bucket).Append("-")
                            .Append(preSignatureStruct.appid);
                    }

                    host.Append(".cos.")
                        .Append(preSignatureStruct.region)
                        .Append(".myqcloud.com");

                    urlBuilder.Append(host.ToString());

                    // host 入签
                    if (preSignatureStruct.signHost)
                    {
                        if (preSignatureStruct.headers == null)
                        {
                            preSignatureStruct.headers = new Dictionary<string, string>(); 
                        }
                        if (!preSignatureStruct.headers.ContainsKey("host"))
                            preSignatureStruct.headers.Add("host", host.ToString());
                    }
                }
                else
                {
                    urlBuilder.Append(preSignatureStruct.host);
                    // host 入签
                    if (preSignatureStruct.signHost) {
                        if (preSignatureStruct.headers == null)
                        {
                            preSignatureStruct.headers = new Dictionary<string, string>(); 
                        }
                        preSignatureStruct.headers.Add("host", preSignatureStruct.host);
                    }
                }

                if (!preSignatureStruct.key.StartsWith("/"))
                {
                    preSignatureStruct.key = "/" + preSignatureStruct.key;
                }

                urlBuilder.Append(preSignatureStruct.key.Replace("+", "%2B"));

                string sign = GenerateSign(preSignatureStruct.httpMethod, preSignatureStruct.key,
                    preSignatureStruct.queryParameters, preSignatureStruct.headers, 
                    preSignatureStruct.signDurationSecond, preSignatureStruct.keyDurationSecond);

                StringBuilder queryBuilder = new StringBuilder();

                if (preSignatureStruct.queryParameters != null && preSignatureStruct.queryParameters.Count > 0)
                {
                    foreach (KeyValuePair<string, string> keyValuePair in preSignatureStruct.queryParameters)
                    {
                        if (keyValuePair.Key == null || keyValuePair.Key == "")
                            continue;
                        queryBuilder.Append(URLEncodeUtils.Encode(keyValuePair.Key)).Append('=').Append(URLEncodeUtils.Encode(keyValuePair.Value));
                        queryBuilder.Append('&');
                    }
                }
                
                // 针对需要二次 Encode 的 request Param 特殊处理
                Regex rgx = new Regex("q-url-param-list=.*&q-signature");
                string paramlist = rgx.Match(sign).ToString().Split('=')[1].ToString().Split('&')[0].ToString();
                paramlist = paramlist.Trim('&');
                paramlist = URLEncodeUtils.Encode(paramlist).ToLower();
                string encodedStr = "q-url-param-list=" + paramlist + "&q-signature";
                sign = rgx.Replace(sign, encodedStr);

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

        public void GetObject(GetObjectBytesRequest request, OnSuccessCallback<CosResult> successCallback, OnFailedCallback failCallback)
        {
            Schedue(request, new GetObjectBytesResult(), successCallback, failCallback);
        }

        public string GetObjectUrl(string bucket, string key) 
        {
            string http_prefix = config.IsHttps ? "https://" : "http://";
            StringBuilder domainSuffix = new StringBuilder();
            // 优先自定义域名
            if (this.config.host != null) {
                domainSuffix.Append(this.config.host).Append("/").Append(key);
                return http_prefix + domainSuffix.ToString();
            }
            // endpoint
            else if (this.config.endpointSuffix != null) {
                domainSuffix.Append(".").Append(this.config.endpointSuffix).Append("/").Append(key);
            }
            // 默认域名
            else {
                domainSuffix.Append(".cos.");
                domainSuffix.Append(this.config.Region).Append(".myqcloud.com/").Append(key);
            }
            return http_prefix + bucket + domainSuffix.ToString();
        }

        public PutBucketWebsiteResult PutBucketWebsite(PutBucketWebsiteRequest request)
        {

            return (Model.Bucket.PutBucketWebsiteResult)Excute(request, new Model.Bucket.PutBucketWebsiteResult());
        }

         
        public void PutBucketWebsiteAsync(PutBucketWebsiteRequest request, OnSuccessCallback<CosResult> successCallback, OnFailedCallback failCallback)
        {
            Schedue(request, new Model.Bucket.PutBucketWebsiteResult(), successCallback, failCallback);
        }

        public GetBucketWebsiteResult GetBucketWebsite(GetBucketWebsiteRequest request)
        {

            return (Model.Bucket.GetBucketWebsiteResult)Excute(request, new Model.Bucket.GetBucketWebsiteResult());
        }

         
        public void GetBucketWebsiteAsync(GetBucketWebsiteRequest request, OnSuccessCallback<CosResult> successCallback, OnFailedCallback failCallback)
        {
            Schedue(request, new Model.Bucket.GetBucketWebsiteResult(), successCallback, failCallback);
        }

        public DeleteBucketWebsiteResult DeleteBucketWebsite(DeleteBucketWebsiteRequest request)
        {

            return (Model.Bucket.DeleteBucketWebsiteResult)Excute(request, new Model.Bucket.DeleteBucketWebsiteResult());
        }

         
        public void DeleteBucketWebsiteAsync(DeleteBucketWebsiteRequest request, OnSuccessCallback<CosResult> successCallback, OnFailedCallback failCallback)
        {
            Schedue(request, new Model.Bucket.DeleteBucketWebsiteResult(), successCallback, failCallback);
        }

        public PutBucketLoggingResult PutBucketLogging(PutBucketLoggingRequest request)
        {

            return (Model.Bucket.PutBucketLoggingResult)Excute(request, new Model.Bucket.PutBucketLoggingResult());
        }

         
        public void PutBucketLoggingAsync(PutBucketLoggingRequest request, OnSuccessCallback<CosResult> successCallback, OnFailedCallback failCallback)
        {
            Schedue(request, new Model.Bucket.PutBucketLoggingResult(), successCallback, failCallback);
        }

        public GetBucketLoggingResult GetBucketLogging(GetBucketLoggingRequest request)
        {

            return (Model.Bucket.GetBucketLoggingResult)Excute(request, new Model.Bucket.GetBucketLoggingResult());
        }

         
        public void GetBucketLoggingAsync(GetBucketLoggingRequest request, OnSuccessCallback<CosResult> successCallback, OnFailedCallback failCallback)
        {
            Schedue(request, new Model.Bucket.GetBucketLoggingResult(), successCallback, failCallback);
        }

        public PutBucketInventoryResult PutBucketInventory(PutBucketInventoryRequest request)
        {

            return (Model.Bucket.PutBucketInventoryResult)Excute(request, new Model.Bucket.PutBucketInventoryResult());
        }

         
        public void PutBucketInventoryAsync(PutBucketInventoryRequest request, OnSuccessCallback<CosResult> successCallback, OnFailedCallback failCallback)
        {
            Schedue(request, new Model.Bucket.PutBucketInventoryResult(), successCallback, failCallback);
        }

        public GetBucketInventoryResult GetBucketInventory(GetBucketInventoryRequest request)
        {

            return (Model.Bucket.GetBucketInventoryResult)Excute(request, new Model.Bucket.GetBucketInventoryResult());
        }

         
        public void GetBucketInventoryAsync(GetBucketInventoryRequest request, OnSuccessCallback<CosResult> successCallback, OnFailedCallback failCallback)
        {
            Schedue(request, new Model.Bucket.GetBucketInventoryResult(), successCallback, failCallback);
        }

        public DeleteBucketInventoryResult DeleteBucketInventory(DeleteBucketInventoryRequest request)
        {

            return (Model.Bucket.DeleteBucketInventoryResult)Excute(request, new Model.Bucket.DeleteBucketInventoryResult());
        }

         
        public void DeleteInventoryAsync(DeleteBucketInventoryRequest request, OnSuccessCallback<CosResult> successCallback, OnFailedCallback failCallback)
        {
            Schedue(request, new Model.Bucket.DeleteBucketInventoryResult(), successCallback, failCallback);
        }

        public ListBucketInventoryResult ListBucketInventory(ListBucketInventoryRequest request)
        {

            return (Model.Bucket.ListBucketInventoryResult)Excute(request, new Model.Bucket.ListBucketInventoryResult());
        }

         
        public void ListBucketInventoryAsync(ListBucketInventoryRequest request, OnSuccessCallback<CosResult> successCallback, OnFailedCallback failCallback)
        {
            Schedue(request, new Model.Bucket.ListBucketInventoryResult(), successCallback, failCallback);
        }

        public PutBucketTaggingResult PutBucketTagging(PutBucketTaggingRequest request)
        {

            return (Model.Bucket.PutBucketTaggingResult)Excute(request, new Model.Bucket.PutBucketTaggingResult());
        }

         
        public void PutBucketTaggingAsync(PutBucketTaggingRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new Model.Bucket.PutBucketTaggingResult(), successCallback, failCallback);
        }

        public GetBucketTaggingResult GetBucketTagging(GetBucketTaggingRequest request)
        {

            return (Model.Bucket.GetBucketTaggingResult)Excute(request, new Model.Bucket.GetBucketTaggingResult());
        }

         
        public void GetBucketTaggingAsync(GetBucketTaggingRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new Model.Bucket.GetBucketTaggingResult(), successCallback, failCallback);
        }

        public DeleteBucketTaggingResult DeleteBucketTagging(DeleteBucketTaggingRequest request)
        {

            return (Model.Bucket.DeleteBucketTaggingResult)Excute(request, new Model.Bucket.DeleteBucketTaggingResult());
        }

         
        public void DeleteBucketTaggingAsync(DeleteBucketTaggingRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback)
        {
            Schedue(request, new Model.Bucket.DeleteBucketTaggingResult(), successCallback, failCallback);
        }

        public PutBucketDomainResult PutBucketDomain(PutBucketDomainRequest request)
        {

            return (Model.Bucket.PutBucketDomainResult)Excute(request, new Model.Bucket.PutBucketDomainResult());
        }

         
        public void PutBucketDomainAsync(PutBucketDomainRequest request, OnSuccessCallback<CosResult> successCallback, OnFailedCallback failCallback)
        {
            Schedue(request, new Model.Bucket.PutBucketDomainResult(), successCallback, failCallback);
        }

        public GetBucketDomainResult GetBucketDomain(GetBucketDomainRequest request)
        {

            return (Model.Bucket.GetBucketDomainResult)Excute(request, new Model.Bucket.GetBucketDomainResult());
        }

         
        public void GetBucketDomainAsync(GetBucketDomainRequest request, OnSuccessCallback<CosResult> successCallback, OnFailedCallback failCallback)
        {
            Schedue(request, new Model.Bucket.GetBucketDomainResult(), successCallback, failCallback);
        }

        SelectObjectResult CosXml.SelectObject(SelectObjectRequest request)
        {

            return (Model.Object.SelectObjectResult)Excute(request, new Model.Object.SelectObjectResult());
        }

         
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

        /// <summary>
        /// 下载时进行二维码识别
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public QRCodeRecognitionResult QRCodeRecognition(QRCodeRecognitionRequest request)
        {
            return Execute(request, new QRCodeRecognitionResult());
        }

        /// <summary>
        /// 获取媒体文件某个时间的截图
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public GetSnapshotResult GetSnapshot(GetSnapshotRequest request)
        {
            return Execute(request, new GetSnapshotResult());
        }

        /// <summary>
        /// 获取媒体文件的信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public GetMediaInfoResult GetMediaInfo(GetMediaInfoRequest request)
        {
            return Execute(request, new GetMediaInfoResult());
        }

        /// <summary>
        /// 提交视频审核任务
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public SubmitCensorJobResult SubmitVideoCensorJob(SubmitVideoCensorJobRequest request)
        {
            request.Region = this.GetConfig().Region;
            return Execute(request, new SubmitCensorJobResult());
        }

        /// <summary>
        /// 获取视频审核任务结果
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public GetVideoCensorJobResult GetVideoCensorJob(GetVideoCensorJobRequest request)
        {
            request.Region = this.GetConfig().Region;
            return Execute(request, new GetVideoCensorJobResult());
        }

        /// <summary>
        /// 提交音频审核任务
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public SubmitCensorJobResult SubmitAudioCensorJob(SubmitAudioCensorJobRequest request)
        {
            request.Region = this.GetConfig().Region;
            return Execute(request, new SubmitCensorJobResult());
        }

        /// <summary>
        /// 获取音频审核任务结果
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public GetAudioCensorJobResult GetAudioCensorJob(GetAudioCensorJobRequest request)
        {
            request.Region = this.GetConfig().Region;
            return Execute(request, new GetAudioCensorJobResult());
        }

        /// <summary>
        /// 提交文本审核任务
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public SubmitCensorJobResult SubmitTextCensorJob(SubmitTextCensorJobRequest request)
        {
            request.Region = this.GetConfig().Region;
            return Execute(request, new SubmitCensorJobResult());
        }

        /// <summary>
        /// 提交文本审核任务, 支持同步返回
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public SubmitTextCensorJobsResult SubmitTextCensorJobSync(SubmitTextCensorJobRequest request)
        {
            request.Region = this.GetConfig().Region;
            return Execute(request, new SubmitTextCensorJobsResult());
        }

        /// <summary>
        /// 获取文本审核任务结果
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public GetTextCensorJobResult GetTextCensorJob(GetTextCensorJobRequest request)
        {
            request.Region = this.GetConfig().Region;
            return Execute(request, new GetTextCensorJobResult());
        }

        /// <summary>
        /// 提交文档审核任务
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public SubmitCensorJobResult SubmitDocumentCensorJob(SubmitDocumentCensorJobRequest request)
        {
            request.Region = this.GetConfig().Region;
            return Execute(request, new SubmitCensorJobResult());
        }
        /// <summary>
        /// 提交文档转码任务
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CreateDocProcessJobsResult CreateDocProcessJobs(CreateDocProcessJobsRequest request)
        {
            request.Region = this.GetConfig().Region;
            return Execute(request, new CreateDocProcessJobsResult());
        }
        /// <summary>
        /// 提交文档转码任务
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Obsolete]
        public SubmitDocumentProcessJobResult SubmitDocumentProcessJob(SubmitDocumentProcessJobRequest request)
        {
            request.Region = this.GetConfig().Region;
            return Execute(request, new SubmitDocumentProcessJobResult());
        }
        /// <summary>
        /// 查询指定的文档转码任务
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DescribeDocProcessJobResult DescribeDocProcessJob(DescribeDocProcessJobRequest request)
        {
            request.Region = this.GetConfig().Region;
            return Execute(request, new DescribeDocProcessJobResult());
        }
        /// <summary>
        /// 拉取符合条件的文档转码任务
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DescribeDocProcessJobsResult DescribeDocProcessJobs(DescribeDocProcessJobsRequest request)
        {
            request.Region = this.GetConfig().Region;
            return Execute(request, new DescribeDocProcessJobsResult());
        }

        /// <summary>
        /// 获取文档审核任务结果
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public GetDocumentCensorJobResult GetDocumentCensorJob(GetDocumentCensorJobRequest request)
        {
            request.Region = this.GetConfig().Region;
            return Execute(request, new GetDocumentCensorJobResult());
        }

        /// <summary>
        /// 获取媒体bucket列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DescribeMediaBucketsResult DescribeMediaBuckets(DescribeMediaBucketsRequest request)
        {
            request.Region = this.GetConfig().Region;
            return Execute(request, new DescribeMediaBucketsResult());
        }
        /// <summary>
        /// 多文件打包压缩功能可以将您的多个文件，打包为 zip 等压缩包格式，以提交任务的方式进行多文件打包压缩，异步返回打包后的文件，该接口属于 POST 请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CreateFileZipProcessJobsResult createFileZipProcessJobs(CreateFileZipProcessJobsRequest request)
        {
            request.Region = this.GetConfig().Region;
            return Execute(request, new CreateFileZipProcessJobsResult());
        }

        /// <summary>
        /// 本接口用于主动查询指定的多文件打包压缩任务结果
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DescribeFileZipProcessJobsResult describeFileZipProcessJobs(DescribeFileZipProcessJobsRequest request)
        {   
            request.Region = this.GetConfig().Region;
            return Execute(request, new DescribeFileZipProcessJobsResult());
        }
        /// <summary>
        /// 本接口用于文档转 HTML 同步预览链接
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public String createDocPreview(CreateDocPreviewRequest request)
        {

            PreSignatureStruct signatureStruct = new PreSignatureStruct();

            signatureStruct.bucket = request.Bucket;
            signatureStruct.appid = this.GetConfig().Appid;
            signatureStruct.region =this.GetConfig().Region;
            signatureStruct.key = request.Key;
            signatureStruct.httpMethod = "GET";
            signatureStruct.isHttps = true;
            signatureStruct.signDurationSecond = request.GetSignExpired();
            string docPreviewUrl = GenerateSignURL(signatureStruct);
            foreach (var param in request.GetRequestParamters())
            {
                docPreviewUrl += "&" + param.Key + "=" + param.Value;
            }
            return  docPreviewUrl;
        }

    }
}

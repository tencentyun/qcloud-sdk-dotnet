using System;
using System.Collections.Generic;
using System.Text;
using COSXML.Model.Service;
using COSXML.Model.Bucket;
using COSXML.Model.Object;
using COSXML.Model.CI;
using COSXML.Model;
using COSXML.Model.Tag;
#if !COMPATIBLE
using System.Threading.Tasks;
#endif

namespace COSXML
{

    /// <summary>
    /// COS 接口类，实现 COS XML 的所有功能。
    /// </summary>
    public interface CosXml
    {
        /// <summary>
        /// 获取配置项
        /// </summary>
        /// <returns></returns>
        CosXmlConfig GetConfig();

        /// <summary>
        /// 生成签名串
        /// </summary>
        /// <param name="method">http method</param>
        /// <param name="key">http url path</param>
        /// <param name="queryParameters">http url query</param>
        /// <param name="headers">http header</param>
        /// <param name="signDurationSecond">sign time</param>
        /// <returns></returns>
        string GenerateSign(string method, string key, Dictionary<string, string> queryParameters, 
                            Dictionary<string, string> headers, long signDurationSecond, long keyDurationSecond);

        /// <summary>
        /// 生成预签名URL
        /// </summary>
        /// <param name="preSignatureStruct">签名结构体</param>
        /// <returns></returns>
        string GenerateSignURL(PreSignatureStruct preSignatureStruct);

        /// <summary>
        /// 获取存储桶列表
        /// </summary>
        /// <param name="request"> <see href="COSXML.Model.Service.GetServiceRequest"/>GetServiceRequest </param>
        /// <returns><see href="COSXML.Model.Service.GetServiceResult"/>GetServiceResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        GetServiceResult GetService(GetServiceRequest request);

        /// <summary>
        /// 获取存储桶列表的异步方法
        /// </summary>
        /// <param name="request">GetServiceRequest</param>
        /// <param name="successCallback">OnSuccessCallback</param>
        /// <param name="failCallback">OnFailedCallback</param>
         
        void GetService(GetServiceRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 创建存储桶
        /// </summary>
        /// <param name="request">PutBucketRequest</param>
        /// <returns>PutBucketResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        PutBucketResult PutBucket(PutBucketRequest request);

        /// <summary>
        /// 创建存储桶的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void PutBucket(PutBucketRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 检索存储桶
        /// </summary>
        /// <param name="request">HeadBucketRequest</param>
        /// <returns>HeadBucketResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        HeadBucketResult HeadBucket(HeadBucketRequest request);

        /// <summary>
        /// 检索存储桶的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void HeadBucket(HeadBucketRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 检查存储桶是否存在（只有同步方法）
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        bool DoesBucketExist(DoesBucketExistRequest request);

        /// <summary>
        /// 列出存储桶下的文件
        /// </summary>
        /// <param name="request">GetBucketRequest</param>
        /// <returns>GetBucketResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        GetBucketResult GetBucket(GetBucketRequest request);

        /// <summary>
        /// 列出存储桶下文件的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void GetBucket(GetBucketRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 删除存储桶
        /// </summary>
        /// <param name="request">DeleteBucketRequest</param>
        /// <returns>DeleteBucketResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        DeleteBucketResult DeleteBucket(DeleteBucketRequest request);

        /// <summary>
        /// 删除存储桶的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void DeleteBucket(DeleteBucketRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 获取存储桶策略
        /// </summary>
        /// <param name="request">GetBucketRequest</param>
        /// <returns>GetBucketPolicyResult</returns>
        GetBucketPolicyResult GetBucketPolicy(GetBucketPolicyRequest request);

        /// <summary>
        /// 设置存储桶策略
        /// </summary>
        /// <param name="request">PutBucketRequest</param>
        /// <returns>PutBucketPolicyResult</returns>
        PutBucketPolicyResult PutBucketPolicy(PutBucketPolicyRequest request);
        
        /// <summary>
        /// 删除存储桶策略
        /// </summary>
        /// <param name="request">DeleteBucketRequest</param>
        /// <returns>DeleteBucketPolicyResult</returns>
        DeleteBucketPolicyResult DeleteBucketPolicy(DeleteBucketPolicyRequest request);
        
        /// <summary>
        /// 设置存储桶权限
        /// </summary>
        /// <param name="request">PutBucketACLRequest</param>
        /// <returns>PutBucketACLResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        PutBucketACLResult PutBucketACL(PutBucketACLRequest request);

        /// <summary>
        /// 设置存储桶权限的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void PutBucketACL(PutBucketACLRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 查询存储桶权限
        /// </summary>
        /// <param name="request">GetBucketACLRequest</param>
        /// <returns>GetBucketACLResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        GetBucketACLResult GetBucketACL(GetBucketACLRequest request);

        /// <summary>
        /// 查询存储桶权限的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void GetBucketACL(GetBucketACLRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 设置存储桶的跨域规则
        /// </summary>
        /// <param name="request">PutBucketCORSRequest</param>
        /// <returns>PutBucketCORSResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        PutBucketCORSResult PutBucketCORS(PutBucketCORSRequest request);

        /// <summary>
        /// 设置存储桶跨域规则的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void PutBucketCORS(PutBucketCORSRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 获取存储桶跨域规则
        /// </summary>
        /// <param name="request">GetBucketCORSRequest</param>
        /// <returns>GetBucketCORSResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        GetBucketCORSResult GetBucketCORS(GetBucketCORSRequest request);

        /// <summary>
        /// 获取存储桶跨域规则的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void GetBucketCORS(GetBucketCORSRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 删除存储桶跨域规则
        /// </summary>
        /// <param name="request">DeleteBucketCORSRequest</param>
        /// <returns>DeleteBucketCORSResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        DeleteBucketCORSResult DeleteBucketCORS(DeleteBucketCORSRequest request);

        /// <summary>
        /// 删除存储桶跨域规则的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void DeleteBucketCORS(DeleteBucketCORSRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 设置存储桶生命周期
        /// </summary>
        /// <param name="request">PutBucketLifecycleRequest</param>
        /// <returns>PutBucketLifecycleResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception> 
        PutBucketLifecycleResult PutBucketLifecycle(PutBucketLifecycleRequest request);

        /// <summary>
        /// 设置存储桶生命周期的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void PutBucketLifecycle(PutBucketLifecycleRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 获取存储桶生命周期规则
        /// </summary>
        /// <param name="request">GetBucketLifecycleRequest</param>
        /// <returns>GetBucketLifecycleResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception> 
        GetBucketLifecycleResult GetBucketLifecycle(GetBucketLifecycleRequest request);

        /// <summary>
        /// 获取存储桶生命周期规则的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void GetBucketLifecycle(GetBucketLifecycleRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 删除存储桶生命周期规则
        /// </summary>
        /// <param name="request">DeleteBucketLifecycleRequest</param>
        /// <returns>DeleteBucketLifecycleResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception> 
        DeleteBucketLifecycleResult DeleteBucketLifecycle(DeleteBucketLifecycleRequest request);

        /// <summary>
        /// 删除存储桶生命周期规则的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void DeleteBucketLifecycle(DeleteBucketLifecycleRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 设置存储桶跨地域复制规则
        /// </summary>
        /// <param name="request">PutBucketReplicationRequest</param>
        /// <returns>PutBucketReplicationResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception> 
        PutBucketReplicationResult PutBucketReplication(PutBucketReplicationRequest request);

        /// <summary>
        /// 设置存储桶跨地域复制规则的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void PutBucketReplication(PutBucketReplicationRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 获取存储桶跨地域复制规则
        /// </summary>
        /// <param name="request">GetBucketReplicationRequest</param>
        /// <returns>GetBucketReplicationResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception> 
        GetBucketReplicationResult GetBucketReplication(GetBucketReplicationRequest request);

        /// <summary>
        /// 获取存储桶跨地域复制规则的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void GetBucketReplication(GetBucketReplicationRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 删除存储桶跨地域复制规则
        /// </summary>
        /// <param name="request">DeleteBucketReplicationRequest</param>
        /// <returns>DeleteBucketReplicationResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception> 
        DeleteBucketReplicationResult DeleteBucketReplication(DeleteBucketReplicationRequest request);

        /// <summary>
        /// 删除存储桶跨地域复制规则的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void DeleteBucketReplication(DeleteBucketReplicationRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 设置存储桶多版本状态
        /// </summary>
        /// <param name="request">PutBucketVersioningRequest</param>
        /// <returns>PutBucketVersioningResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception> 
        PutBucketVersioningResult PutBucketVersioning(PutBucketVersioningRequest request);

        /// <summary>
        /// 设置存储桶多版本状态的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void PutBucketVersioning(PutBucketVersioningRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 获取存储桶多版本状态
        /// </summary>
        /// <param name="request">GetBucketVersioningRequest</param>
        /// <returns>GetBucketVersioningResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception> 
        GetBucketVersioningResult GetBucketVersioning(GetBucketVersioningRequest request);

        /// <summary>
        /// 获取存储桶多版本状态的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void GetBucketVersioning(GetBucketVersioningRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 设置存储桶防盗链
        /// </summary>
        /// <param name="request">PutBucketRefererRequest</param>
        /// <returns>PutBucketRefererResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception> 
        PutBucketRefererResult PutBucketReferer(PutBucketRefererRequest request);

        /// <summary>
        /// 设置存储桶防盗链的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void PutBucketReferer(PutBucketRefererRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 获取存储桶防盗链
        /// </summary>
        /// <param name="request">GetBucketRefererRequest</param>
        /// <returns>GetBucketRefererResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception> 
        GetBucketRefererResult GetBucketReferer(GetBucketRefererRequest request);

        /// <summary>
        /// 获取存储桶防盗链的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void GetBucketReferer(GetBucketRefererRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 列出多版本对象列表
        /// </summary>
        /// <param name="request">ListBucketVersionsRequest</param>
        /// <returns>ListBucketVersionsResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception> 
        ListBucketVersionsResult ListBucketVersions(ListBucketVersionsRequest request);

        /// <summary>
        /// 列出多版本对象列表的异步方法
        /// </summary>
        /// <param name="request">ListBucketVersionsRequest</param>
        /// <param name="successCallback">successCallback</param>
        /// <param name="failCallback">failCallback</param>
         
        void ListBucketVersions(ListBucketVersionsRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 列出所有未完成的分片上传任务
        /// </summary>
        /// <param name="request">ListMultiUploadsRequest</param>
        /// <returns>ListMultiUploadsResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        ListMultiUploadsResult ListMultiUploads(ListMultiUploadsRequest request);

        /// <summary>
        /// 列出所有未完成的分片上传任务的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void ListMultiUploads(ListMultiUploadsRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 简单上传对象
        /// </summary>
        /// <param name="request">PutObjectRequest</param>
        /// <returns>PutObjectResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        PutObjectResult PutObject(PutObjectRequest request);

        /// <summary>
        /// 简单上传对象的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void PutObject(PutObjectRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 检索对象属性
        /// </summary>
        /// <param name="request">HeadObjectRequest</param>
        /// <returns>HeadObjectResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        HeadObjectResult HeadObject(HeadObjectRequest request);

        /// <summary>
        /// 检索对象属性的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void HeadObject(HeadObjectRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 检查对象是否存在(只支持同步方法)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        bool DoesObjectExist(DoesObjectExistRequest request);

        /// <summary>
        /// 下载对象
        /// </summary>
        /// <param name="request">GetObjectRequest</param>
        /// <returns>GetObjectResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        GetObjectResult GetObject(GetObjectRequest request);

        /// <summary>
        /// 下载对象的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void GetObject(GetObjectRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 下载对象到内存中
        /// </summary>
        /// <param name="request"></param>
        /// <returns>GetObjectBytesResult</returns>
        GetObjectBytesResult GetObject(GetObjectBytesRequest request);

        /// <summary>
        /// 下载对象到内存中的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void GetObject(GetObjectBytesRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 获取对象访问URL
        /// </summary>
        /// <param name="bucket">bucket</param>
        /// <param name="key">object key</param>
        /// <returns></returns>
        string GetObjectUrl(string bucket, string key);

        /// <summary>
        /// 设置对象 ACL 权限
        /// </summary>
        /// <param name="request">PutObjectACLRequest</param>
        /// <returns>PutObjectACLResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        PutObjectACLResult PutObjectACL(PutObjectACLRequest request);

        /// <summary>
        /// 设置对象 ACL 权限的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void PutObjectACL(PutObjectACLRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// Append上传对象
        /// </summary>
        /// <param name="request"></param>
        AppendObjectResult AppendObject(AppendObjectRequest request);

        /// <summary>
        /// 获取对象 ACL 权限
        /// </summary>
        /// <param name="request">GetObjectACLRequest</param>
        /// <returns>GetObjectACLResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        GetObjectACLResult GetObjectACL(GetObjectACLRequest request);

        /// <summary>
        /// 获取对象 ACL 权限的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void GetObjectACL(GetObjectACLRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 设置对象标签的同步方法
        /// </summary>
        /// <param name="request"></param>
        PutObjectTaggingResult PutObjectTagging(PutObjectTaggingRequest request);

        /// <summary>
        /// 设置对象标签的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void PutObjectTagging(PutObjectTaggingRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 获取对象标签的同步方法
        /// </summary>
        /// <param name="request"></param>
        GetObjectTaggingResult GetObjectTagging(GetObjectTaggingRequest request);

        /// <summary>
        /// 获取对象标签的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void GetObjectTagging(GetObjectTaggingRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 删除对象标签的同步方法
        /// </summary>
        /// <param name="request"></param>
        DeleteObjectTaggingResult DeleteObjectTagging(DeleteObjectTaggingRequest request);

        /// <summary>
        /// 删除对象标签的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void DeleteObjectTagging(DeleteObjectTaggingRequest request, Callback.OnSuccessCallback<CosResult> successCallback, Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="request">DeleteObjectRequest</param>
        /// <returns>DeleteObjectResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        DeleteObjectResult DeleteObject(DeleteObjectRequest request);

        /// <summary>
        /// 删除对象的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void DeleteObject(DeleteObjectRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 批量删除对象
        /// </summary>
        /// <param name="request">DeleteMultiObjectRequest</param>
        /// <returns>DeleteMultiObjectResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        DeleteMultiObjectResult DeleteMultiObjects(DeleteMultiObjectRequest request);

        /// <summary>
        /// 批量删除对象的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void DeleteMultiObjects(DeleteMultiObjectRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 初始化分片上传任务
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        InitMultipartUploadResult InitMultipartUpload(InitMultipartUploadRequest request);

        /// <summary>
        /// 初始化分片上传任务的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void InitMultipartUpload(InitMultipartUploadRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 列出已上传的分片
        /// </summary>
        /// <param name="request">ListPartsRequest</param>
        /// <returns>ListPartsResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        ListPartsResult ListParts(ListPartsRequest request);

        /// <summary>
        /// 列出已上传的分片的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void ListParts(ListPartsRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 上传分片
        /// </summary>
        /// <param name="request">UploadPartRequest</param>
        /// <returns>UploadPartResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        UploadPartResult UploadPart(UploadPartRequest request);

        /// <summary>
        /// 上传分片的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void UploadPart(UploadPartRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 完成分片上传
        /// </summary>
        /// <param name="request">CompleteMultiUploadRequest</param>
        /// <returns>CompleteMultiUploadResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        CompleteMultipartUploadResult CompleteMultiUpload(CompleteMultipartUploadRequest request);

        /// <summary>
        /// 完成分片上传的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void CompleteMultiUpload(CompleteMultipartUploadRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 终止分片上传
        /// </summary>
        /// <param name="request">AbortMultiUploadRequest</param>
        /// <returns>AbortMultiUploadResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        AbortMultipartUploadResult AbortMultiUpload(AbortMultipartUploadRequest request);

        /// <summary>
        /// 终止分片上传的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void AbortMultiUpload(AbortMultipartUploadRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 复制对象
        /// </summary>
        /// <param name="request">CopyObjectRequest</param>
        /// <returns>CopyObjectResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        CopyObjectResult CopyObject(CopyObjectRequest request);

        /// <summary>
        /// 复制对象的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void CopyObject(CopyObjectRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 拷贝一个分片
        /// </summary>
        /// <param name="request">UploadPartCopyRequest</param>
        /// <returns>UploadPartCopyResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        UploadPartCopyResult PartCopy(UploadPartCopyRequest request);

        /// <summary>
        /// 拷贝一个分片的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void PartCopy(UploadPartCopyRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 对象的跨域预请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        OptionObjectResult OptionObject(OptionObjectRequest request);

        /// <summary>
        /// 对象的跨域预请求的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void OptionObject(OptionObjectRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// POST 方式上传对象
        /// </summary>
        /// <param name="request">PostObjectRequest</param>
        /// <returns>PostObjectResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        PostObjectResult PostObject(PostObjectRequest request);

        /// <summary>
        /// POST 方式上传对象的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void PostObject(PostObjectRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 对一个归档存储（ARCHIVE）类型的对象进行恢复
        /// </summary>
        /// <param name="request">RestoreObjectRequest</param>
        /// <returns>RestoreObjectResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        RestoreObjectResult RestoreObject(RestoreObjectRequest request);

        /// <summary>
        /// 对一个归档存储（ARCHIVE）类型的对象进行恢复的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void RestoreObject(RestoreObjectRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 设置存储桶静态网站
        /// </summary>
        /// <param name="request"></param>
        /// <returns>设置结果</returns>
        PutBucketWebsiteResult PutBucketWebsite(PutBucketWebsiteRequest request);

        /// <summary>
        /// 设置存储桶静态网站的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void PutBucketWebsiteAsync(PutBucketWebsiteRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 获取存储桶静态网站设置
        /// </summary>
        /// <param name="request"></param>
        /// <returns>静态</returns>
        GetBucketWebsiteResult GetBucketWebsite(GetBucketWebsiteRequest request);

        /// <summary>
        /// 获取存储桶静态网站设置的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void GetBucketWebsiteAsync(GetBucketWebsiteRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 删除存储桶静态网站设置
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        DeleteBucketWebsiteResult DeleteBucketWebsite(DeleteBucketWebsiteRequest request);

        /// <summary>
        /// 删除存储桶静态网站设置的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void DeleteBucketWebsiteAsync(DeleteBucketWebsiteRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 设置存储桶自定义域名
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        PutBucketDomainResult PutBucketDomain(PutBucketDomainRequest request);

        /// <summary>
        /// 设置存储桶自定义域名的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void PutBucketDomainAsync(PutBucketDomainRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 获取存储桶自定义域名
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        GetBucketDomainResult GetBucketDomain(GetBucketDomainRequest request);

        /// <summary>
        /// 获取存储桶自定义域名的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void GetBucketDomainAsync(GetBucketDomainRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 设置存储桶日志服务
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        PutBucketLoggingResult PutBucketLogging(PutBucketLoggingRequest request);

        /// <summary>
        /// 设置存储桶日志服务的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void PutBucketLoggingAsync(PutBucketLoggingRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 获取存储桶日志服务设置
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        GetBucketLoggingResult GetBucketLogging(GetBucketLoggingRequest request);

        /// <summary>
        /// 获取存储桶日志服务设置的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void GetBucketLoggingAsync(GetBucketLoggingRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 设置存储桶清单任务
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        PutBucketInventoryResult PutBucketInventory(PutBucketInventoryRequest request);

        /// <summary>
        /// 设置存储桶清单任务的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void PutBucketInventoryAsync(PutBucketInventoryRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 获取存储桶清单任务
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        GetBucketInventoryResult GetBucketInventory(GetBucketInventoryRequest request);

        /// <summary>
        /// 获取存储桶清单任务的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void GetBucketInventoryAsync(GetBucketInventoryRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 删除存储桶清单任务
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        DeleteBucketInventoryResult DeleteBucketInventory(DeleteBucketInventoryRequest request);

        /// <summary>
        /// 删除存储桶清单任务的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void DeleteInventoryAsync(DeleteBucketInventoryRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 列出存储桶所有清单任务
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ListBucketInventoryResult ListBucketInventory(ListBucketInventoryRequest request);

        /// <summary>
        /// 列出存储桶所有清单任务的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void ListBucketInventoryAsync(ListBucketInventoryRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 设置存储桶标签
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        PutBucketTaggingResult PutBucketTagging(PutBucketTaggingRequest request);

        /// <summary>
        /// 设置存储桶标签的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void PutBucketTaggingAsync(PutBucketTaggingRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 获取存储桶标签
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        GetBucketTaggingResult GetBucketTagging(GetBucketTaggingRequest request);

        /// <summary>
        /// 获取存储桶标签的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void GetBucketTaggingAsync(GetBucketTaggingRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 删除存储桶标签
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        DeleteBucketTaggingResult DeleteBucketTagging(DeleteBucketTaggingRequest request);

        /// <summary>
        /// 删除存储桶标签的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void DeleteBucketTaggingAsync(DeleteBucketTaggingRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 检索对象
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        SelectObjectResult SelectObject(SelectObjectRequest request);

        /// <summary>
        /// 检索对象的异步方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
         
        void SelectObjectAsync(SelectObjectRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// 获取智能分层配置
        /// </summary>
        /// <param name="request"></param>
        GetBucketIntelligentTieringResult GetBucketIntelligentTieringConfiguration(GetBucketIntelligentTieringRequest request);

        /// <summary>
        /// 设置存储桶智能分层
        /// </summary>
        /// <param name="request"></param>
        CosResult PutBucketIntelligentTiering(PutBucketIntelligentTieringRequest request);

        /// <summary>
        /// 内容识别
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        SensitiveContentRecognitionResult SensitiveContentRecognition(SensitiveContentRecognitionRequest request);

        /// <summary>
        /// 图片处理
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ImageProcessResult ImageProcess(ImageProcessRequest request);

        /// <summary>
        /// 下载时进行二维码识别
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        QRCodeRecognitionResult QRCodeRecognition(QRCodeRecognitionRequest request);

        /// <summary>
        /// 获取媒体文件某个时间的截图
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        GetSnapshotResult GetSnapshot(GetSnapshotRequest request);

        /// <summary>
        /// 获取媒体文件的信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        GetMediaInfoResult GetMediaInfo(GetMediaInfoRequest request);

        /// <summary>
        /// 提交视频审核任务
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        SubmitCensorJobResult SubmitVideoCensorJob(SubmitVideoCensorJobRequest request);

        /// <summary>
        /// 获取视频审核任务结果
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        GetVideoCensorJobResult GetVideoCensorJob(GetVideoCensorJobRequest request);

        /// <summary>
        /// 提交音频审核任务
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        SubmitCensorJobResult SubmitAudioCensorJob(SubmitAudioCensorJobRequest request);

        /// <summary>
        /// 获取音频审核任务结果
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        GetAudioCensorJobResult GetAudioCensorJob(GetAudioCensorJobRequest request);

        /// <summary>
        /// 提交文本审核任务
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        SubmitCensorJobResult SubmitTextCensorJob(SubmitTextCensorJobRequest request);
        
        /// <summary>
        /// 提交文本审核任务, 支持同步返回
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        SubmitTextCensorJobsResult SubmitTextCensorJobSync(SubmitTextCensorJobRequest request);

        /// <summary>
        /// 获取文本审核任务结果
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        GetTextCensorJobResult GetTextCensorJob(GetTextCensorJobRequest request);

        /// <summary>
        /// 提交文档审核任务
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        SubmitCensorJobResult SubmitDocumentCensorJob(SubmitDocumentCensorJobRequest request);

        /// <summary>
        /// 提交文档转码任务
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Obsolete]
        SubmitDocumentProcessJobResult SubmitDocumentProcessJob(SubmitDocumentProcessJobRequest request);

        /// <summary>
        /// 提交文档转码任务
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        CreateDocProcessJobsResult CreateDocProcessJobs(CreateDocProcessJobsRequest request);

        /// <summary>
        /// 查询指定的文档转码任务
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        DescribeDocProcessJobResult DescribeDocProcessJob(DescribeDocProcessJobRequest request);
        /// <summary>
        /// 拉取符合条件的文档转码任务
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        DescribeDocProcessJobsResult DescribeDocProcessJobs(DescribeDocProcessJobsRequest request);

        /// <summary>
        /// 获取文档审核任务结果
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        GetDocumentCensorJobResult GetDocumentCensorJob(GetDocumentCensorJobRequest request);

        /// <summary>
        /// 获取媒体bucket列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        DescribeMediaBucketsResult DescribeMediaBuckets(DescribeMediaBucketsRequest request);

        /// <summary>
        /// 多文件打包压缩功能可以将您的多个文件，打包为 zip 等压缩包格式，以提交任务的方式进行多文件打包压缩，异步返回打包后的文件，该接口属于 POST 请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        CreateFileZipProcessJobsResult createFileZipProcessJobs(CreateFileZipProcessJobsRequest request);

        /// <summary>
        /// 本接口用于主动查询指定的多文件打包压缩任务结果
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        DescribeFileZipProcessJobsResult describeFileZipProcessJobs(DescribeFileZipProcessJobsRequest request);
        /// <summary>
        /// 本接口用于获取文档转html url
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        String createDocPreview(CreateDocPreviewRequest request);
        /// <summary>
        /// 取消请求
        /// </summary>
        /// <param name="cosRequest"></param>
        void Cancel(CosRequest cosRequest);

        /// <summary>
        /// 同步执行请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        T Execute<T>(CosRequest request) where T : CosResult;

        #if !COMPATIBLE
        /// <summary>
        /// 异步执行请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<T> ExecuteAsync<T>(CosRequest request) where T : CosResult;
        #endif

    }
}

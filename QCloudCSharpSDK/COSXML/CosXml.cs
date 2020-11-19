using System;
using System.Collections.Generic;
using System.Text;
using COSXML.Model.Service;
using COSXML.Model.Bucket;
using COSXML.Model.Object;
using COSXML.Model.CI;
using COSXML.Model;
using COSXML.Model.Tag;
using System.Threading.Tasks;

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
        string GenerateSign(string method, string key, Dictionary<string, string> queryParameters, Dictionary<string, string> headers, long signDurationSecond);

        /// <summary>
        /// 生成预签名URL
        /// </summary>
        /// <param name="preSignatureStruct">签名结构体</param>
        /// <returns></returns>
        string GenerateSignURL(PreSignatureStruct preSignatureStruct);

        /// <summary>
        /// 获取存储桶列表
        /// </summary>
        /// <param name="request"> <see cref="COSXML.Model.Service.GetServiceRequest"/>GetServiceRequest </param>
        /// <returns><see cref="COSXML.Model.Service.GetServiceResult"/>GetServiceResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        GetServiceResult GetService(GetServiceRequest request);

        /// <summary>
        /// 获取存储桶列表的异步方法
        /// </summary>
        /// <param name="request">GetServiceRequest</param>
        /// <param name="successCallback">OnSuccessCallback</param>
        /// <param name="failCallback">OnFailedCallback</param>
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        void HeadBucket(HeadBucketRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        void DeleteBucket(DeleteBucketRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        void GetBucketVersioning(GetBucketVersioningRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        void HeadObject(HeadObjectRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        void GetObject(GetObjectBytesRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        void PutObjectACL(PutObjectACLRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
        void GetObjectACL(GetObjectACLRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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
        [Obsolete("方法已废弃，请使用 ExecuteAsync 实现异步请求。")]
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

        /// <summary>
        /// 异步执行请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<T> ExecuteAsync<T>(CosRequest request) where T : CosResult;
    }
}

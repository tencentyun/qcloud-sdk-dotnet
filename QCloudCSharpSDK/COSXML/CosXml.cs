using System;
using System.Collections.Generic;
using System.Text;
using COSXML.Model.Service;
using COSXML.Model.Bucket;
using COSXML.Model.Object;
using COSXML.Model;
using COSXML.Model.Tag;

namespace COSXML
{ 

    public interface CosXml
    {
        /// <summary>
        /// 生成签名串
        /// </summary>
        /// <param name="method">http method</param>
        /// <param name="path">http url path</param>
        /// <param name="queryParameters">http url query</param>
        /// <param name="headers">http header</param>
        /// <param name="signTime">sign time</param>
        /// <returns></returns>
        string GenerateSign(string method, string key, Dictionary<string, string> queryParameters, Dictionary<string, string> headers,
            long signDurationSecond);

        /// <summary>
        /// 生成预签名URL
        /// </summary>
        /// <param name="request"></param>
        /// <param name="queryParameters"></param>
        /// <param name="headers"></param>
        /// <param name="signTime"></param>
        /// <returns></returns>
        string GenerateSignURL(PreSignatureStruct preSignatureStruct);

        string GetAccessURL(CosRequest request);

        /// <summary>
        /// get service for cos
        /// </summary>
        /// <param name="request"> <see cref="COSXML.Model.Service.GetServiceRequest"/>GetServiceRequest </param>
        /// <returns><see cref="COSXML.Model.Service.GetServiceResult"/>GetServiceResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        GetServiceResult GetService(GetServiceRequest request);

        /// <summary>
        /// asynchronous get service for cos
        /// </summary>
        /// <param name="request">GetServiceRequest</param>
        /// <param name="successCallback">OnSuccessCallback</param>
        /// <param name="failCallback">OnFailedCallback</param>
        void GetService(GetServiceRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// put bucket for cos
        /// </summary>
        /// <param name="request">PutBucketRequest</param>
        /// <returns>PutBucketResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        PutBucketResult PutBucket(PutBucketRequest request);

        /// <summary>
        /// asynchronous put bucket for cos
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void PutBucket(PutBucketRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// Head bucket for cos
        /// </summary>
        /// <param name="request">HeadBucketRequest</param>
        /// <returns>HeadBucketResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        HeadBucketResult HeadBucket(HeadBucketRequest request);

        /// <summary>
        /// asynchronous Head bucket for cos
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void HeadBucket(HeadBucketRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// Get bucket for cos
        /// </summary>
        /// <param name="request">GetBucketRequest</param>
        /// <returns>GetBucketResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        GetBucketResult GetBucket(GetBucketRequest request);

        /// <summary>
        /// asynchronous Get bucket for cos
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void GetBucket(GetBucketRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// delete bucket for cos
        /// </summary>
        /// <param name="request">DeleteBucketRequest</param>
        /// <returns>DeleteBucketResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        DeleteBucketResult DeleteBucket(DeleteBucketRequest request);

        /// <summary>
        /// asynchronous delete bucket for cos
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void DeleteBucket(DeleteBucketRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// get bucket location for cos
        /// </summary>
        /// <param name="request">GetBucketLocationRequest</param>
        /// <returns>GetBucketLocationResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        GetBucketLocationResult GetBucketLocation(GetBucketLocationRequest request);

        void GetBucketLocation(GetBucketLocationRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// put bucket acl for cos
        /// </summary>
        /// <param name="request">PutBucketACLRequest</param>
        /// <returns>PutBucketACLResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        PutBucketACLResult PutBucketACL(PutBucketACLRequest request);

        void PutBucketACL(PutBucketACLRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// get bucket acl for cos
        /// </summary>
        /// <param name="request">GetBucketACLRequest</param>
        /// <returns>GetBucketACLResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        GetBucketACLResult GetBucketACL(GetBucketACLRequest request);

        void GetBucketACL(GetBucketACLRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// delete bucket policy for cos
        /// </summary>
        /// <param name="request">DeleteBucketPolicyRequest</param>
        /// <returns>DeleteBucketPolicyResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        DeleteBucketPolicyResult DeleteBucketPolicy(DeleteBucketPolicyRequest request);

        void DeleteBucketPolicy(DeleteBucketPolicyRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// put bucket cros for cos
        /// </summary>
        /// <param name="request">PutBucketCORSRequest</param>
        /// <returns>PutBucketCORSResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        PutBucketCORSResult PutBucketCORS(PutBucketCORSRequest request);

        void PutBucketCORS(PutBucketCORSRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// get bucket cros for cos
        /// </summary>
        /// <param name="request">GetBucketCORSRequest</param>
        /// <returns>GetBucketCORSResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        GetBucketCORSResult GetBucketCORS(GetBucketCORSRequest request);

        void GetBucketCORS(GetBucketCORSRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// delete bucket cros for cos
        /// </summary>
        /// <param name="request">DeleteBucketCORSRequest</param>
        /// <returns>DeleteBucketCORSResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        DeleteBucketCORSResult DeleteBucketCORS(DeleteBucketCORSRequest request);

        void DeleteBucketCORS(DeleteBucketCORSRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// put bucket lifecycle for cos
        /// </summary>
        /// <param name="request">PutBucketLifecycleRequest</param>
        /// <returns>PutBucketLifecycleResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception> 
        PutBucketLifecycleResult PutBucketLifecycle(PutBucketLifecycleRequest request);

        void PutBucketLifecycle(PutBucketLifecycleRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// get bucket lifecycle for cos
        /// </summary>
        /// <param name="request">GetBucketLifecycleRequest</param>
        /// <returns>GetBucketLifecycleResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception> 
        GetBucketLifecycleResult GetBucketLifecycle(GetBucketLifecycleRequest request);

        void GetBucketLifecycle(GetBucketLifecycleRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// delete bucket lifecycle for cos
        /// </summary>
        /// <param name="request">DeleteBucketLifecycleRequest</param>
        /// <returns>DeleteBucketLifecycleResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception> 
        DeleteBucketLifecycleResult DeleteBucketLifecycle(DeleteBucketLifecycleRequest request);

        void DeleteBucketLifecycle(DeleteBucketLifecycleRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// put bucket replication for cos
        /// </summary>
        /// <param name="request">PutBucketReplicationRequest</param>
        /// <returns>PutBucketReplicationResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception> 
        PutBucketReplicationResult PutBucketReplication(PutBucketReplicationRequest request);

        void PutBucketReplication(PutBucketReplicationRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// get bucket replication for cos
        /// </summary>
        /// <param name="request">GetBucketReplicationRequest</param>
        /// <returns>GetBucketReplicationResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception> 
        GetBucketReplicationResult GetBucketReplication(GetBucketReplicationRequest request);

        void GetBucketReplication(GetBucketReplicationRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// delete bucket replication for cos
        /// </summary>
        /// <param name="request">DeleteBucketReplicationRequest</param>
        /// <returns>DeleteBucketReplicationResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception> 
        DeleteBucketReplicationResult DeleteBucketReplication(DeleteBucketReplicationRequest request);

        void DeleteBucketReplication(DeleteBucketReplicationRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// put bucket versioning for cos
        /// </summary>
        /// <param name="request">PutBucketVersioningRequest</param>
        /// <returns>PutBucketVersioningResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception> 
        PutBucketVersioningResult PutBucketVersioning(PutBucketVersioningRequest request);

        void PutBucketVersioning(PutBucketVersioningRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// get bucket versioning for cos
        /// </summary>
        /// <param name="request">GetBucketVersioningRequest</param>
        /// <returns>GetBucketVersioningResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception> 
        GetBucketVersioningResult GetBucketVersioning(GetBucketVersioningRequest request);

        void GetBucketVersioning(GetBucketVersioningRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// list bucket versions for cos
        /// </summary>
        /// <param name="request">ListBucketVersionsRequest</param>
        /// <returns>ListBucketVersionsResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception> 
        ListBucketVersionsResult ListBucketVersions(ListBucketVersionsRequest request);

        void ListBucketVersions(ListBucketVersionsRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// list multiUploads for cos
        /// </summary>
        /// <param name="request">ListMultiUploadsRequest</param>
        /// <returns>ListMultiUploadsResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        ListMultiUploadsResult ListMultiUploads(ListMultiUploadsRequest request);

        void ListMultiUploads(ListMultiUploadsRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// put object to cos
        /// </summary>
        /// <param name="request">PutObjectRequest</param>
        /// <returns>PutObjectResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        PutObjectResult PutObject(PutObjectRequest request);

        void PutObject(PutObjectRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// head object for cos
        /// </summary>
        /// <param name="request">HeadObjectRequest</param>
        /// <returns>HeadObjectResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        HeadObjectResult HeadObject(HeadObjectRequest request);

        void HeadObject(HeadObjectRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// get object for cos
        /// </summary>
        /// <param name="request">GetObjectRequest</param>
        /// <returns>GetObjectResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        GetObjectResult GetObject(GetObjectRequest request);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successCallback"></param>
        /// <param name="failCallback"></param>
        void GetObject(GetObjectRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);



        /// <summary>
        /// put object acl for cos
        /// </summary>
        /// <param name="request">PutObjectACLRequest</param>
        /// <returns>PutObjectACLResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        PutObjectACLResult PutObjectACL(PutObjectACLRequest request);

        void PutObjectACL(PutObjectACLRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// get object acl for cos
        /// </summary>
        /// <param name="request">GetObjectACLRequest</param>
        /// <returns>GetObjectACLResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        GetObjectACLResult GetObjectACL(GetObjectACLRequest request);

        void GetObjectACL(GetObjectACLRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// delete object for cos
        /// </summary>
        /// <param name="request">DeleteObjectRequest</param>
        /// <returns>DeleteObjectResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        DeleteObjectResult DeleteObject(DeleteObjectRequest request);

        void DeleteObject(DeleteObjectRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// delete multi objects for cos
        /// </summary>
        /// <param name="request">DeleteMultiObjectRequest</param>
        /// <returns>DeleteMultiObjectResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        DeleteMultiObjectResult DeleteMultiObjects(DeleteMultiObjectRequest request);

        void DeleteMultiObjects(DeleteMultiObjectRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// init multiupload for a object to cos
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        InitMultipartUploadResult InitMultipartUpload(InitMultipartUploadRequest request);

        void InitMultipartUpload(InitMultipartUploadRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// list all has been uploaded parts of a object for cos
        /// </summary>
        /// <param name="request">ListPartsRequest</param>
        /// <returns>ListPartsResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        ListPartsResult ListParts(ListPartsRequest request);

        void ListParts(ListPartsRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// upload a part of a object to cos
        /// </summary>
        /// <param name="request">UploadPartRequest</param>
        /// <returns>UploadPartResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        UploadPartResult UploadPart(UploadPartRequest request);

        void UploadPart(UploadPartRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// complete all parts of a object for cos 
        /// </summary>
        /// <param name="request">CompleteMultiUploadRequest</param>
        /// <returns>CompleteMultiUploadResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        CompleteMultiUploadResult CompleteMultiUpload(CompleteMultiUploadRequest request);

        void CompleteMultiUpload(CompleteMultiUploadRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// abort has been parts of a object in cos
        /// </summary>
        /// <param name="request">AbortMultiUploadRequest</param>
        /// <returns>AbortMultiUploadResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        AbortMultiUploadResult AbortMultiUpload(AbortMultiUploadRequest request);

        void AbortMultiUpload(AbortMultiUploadRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// copy object to another object for cos
        /// </summary>
        /// <param name="request">CopyObjectRequest</param>
        /// <returns>CopyObjectResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        CopyObjectResult CopyObject(CopyObjectRequest request);

        void CopyObject(CopyObjectRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// copy part object for cos
        /// </summary>
        /// <param name="request">UploadPartCopyRequest</param>
        /// <returns>UploadPartCopyResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        UploadPartCopyResult PartCopy(UploadPartCopyRequest request);

        void PartCopy(UploadPartCopyRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// option object for cos
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        OptionObjectResult OptionObject(OptionObjectRequest request);

        void OptionObject(OptionObjectRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// post object to cos
        /// </summary>
        /// <param name="request">PostObjectRequest</param>
        /// <returns>PostObjectResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        PostObjectResult PostObject(PostObjectRequest request);

        void PostObject(PostObjectRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        /// <summary>
        /// restore object for cos
        /// </summary>
        /// <param name="request">RestoreObjectRequest</param>
        /// <returns>RestoreObjectResult</returns>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        RestoreObjectResult RestoreObject(RestoreObjectRequest request);

        void RestoreObject(RestoreObjectRequest request, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback);

        void Cancel(CosRequest cosRequest);
    }
}

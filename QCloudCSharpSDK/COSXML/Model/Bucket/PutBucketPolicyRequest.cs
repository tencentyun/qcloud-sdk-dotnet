using COSXML.Common;

namespace COSXML.Model.Bucket
{
    /// <summary>
    /// 设置存储桶的策略
    /// https://cloud.tencent.com/document/product/436/8282
    /// </summary>
    public sealed class PutBucketPolicyRequest : BucketRequest
    {
        /// <summary>
        /// 存储桶权限策略，格式参考 https://cloud.tencent.com/document/product/436/8282
        /// </summary>
        private string bucketPolicy;
        
        public PutBucketPolicyRequest(string bucket)
            : base(bucket)
        {
            this.method = CosRequestMethod.PUT;
            this.queryParameters.Add("policy", null);
        }

        public override Network.RequestBody GetRequestBody()
        {
            return GetJsonRequestBody(bucketPolicy);
        }
        
        public void SetBucketPolicy(string bucketPolicyStr)
        {
            bucketPolicy = bucketPolicyStr;
        }
    }
}

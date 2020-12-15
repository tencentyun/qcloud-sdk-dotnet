using COSXML.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace COSXML.Model.Bucket
{
    /// <summary>
    /// 获取 Bucket 权限策略
    /// <see cref="https://cloud.tencent.com/document/product/436/8276"/>
    /// </summary>
    public sealed class GetBucketPolicyRequest : BucketRequest
    {
        public GetBucketPolicyRequest(string bucket)
            : base(bucket)
        {
            this.method = CosRequestMethod.GET;
            this.queryParameters.Add("policy", null);
        }
    }
}

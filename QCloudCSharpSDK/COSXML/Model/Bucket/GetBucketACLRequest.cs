using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;

namespace COSXML.Model.Bucket
{
    /// <summary>
    /// 获取 Bucket 的访问权限控制列表
    /// <see cref="https://cloud.tencent.com/document/product/436/7733"/>
    /// </summary>
    public sealed class GetBucketACLRequest : BucketRequest
    {
        public GetBucketACLRequest(string bucket)
            : base(bucket)
        {
            this.method = CosRequestMethod.GET;
            this.queryParameters.Add("acl", null);
        }
    }
}

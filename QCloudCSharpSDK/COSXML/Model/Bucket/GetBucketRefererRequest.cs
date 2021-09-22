using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;

namespace COSXML.Model.Bucket
{
    /// <summary>
    /// 查询 Bucket 的防盗链配置
    /// <see href="https://cloud.tencent.com/document/product/436/32493"/>
    /// </summary>
    public sealed class GetBucketRefererRequest : BucketRequest
    {
        public GetBucketRefererRequest(string bucket)
            : base(bucket)
        {
            this.method = CosRequestMethod.GET;
            this.queryParameters.Add("referer", null);
        }
    }
}

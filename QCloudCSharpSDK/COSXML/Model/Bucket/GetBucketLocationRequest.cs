using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;

namespace COSXML.Model.Bucket
{
    /// <summary>
    /// 获取 Bucket 地域信息
    /// <see cref="https://cloud.tencent.com/document/product/436/8275"/>
    /// </summary>
    public sealed class GetBucketLocationRequest : BucketRequest
    {
        public GetBucketLocationRequest(string bucket) : base(bucket)
        {
            this.method = CosRequestMethod.GET;
            this.queryParameters.Add("location", null);
        }
    }
}

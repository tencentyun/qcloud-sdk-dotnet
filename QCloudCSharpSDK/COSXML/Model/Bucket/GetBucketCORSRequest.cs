using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;

namespace COSXML.Model.Bucket
{
    /// <summary>
    /// 获取 Bucket CORS 配置信息
    /// <see cref="https://cloud.tencent.com/document/product/436/8274"/>
    /// </summary>
    public sealed class GetBucketCORSRequest : BucketRequest
    {
        public GetBucketCORSRequest(string bucket)
            : base(bucket)
        {
            this.method = CosRequestMethod.GET;
            this.queryParameters.Add("cors", null);
        }
    }
}

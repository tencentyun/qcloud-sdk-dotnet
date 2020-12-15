using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;

namespace COSXML.Model.Bucket
{
    /// <summary>
    /// 删除 Bucket CORS
    /// <see cref="https://cloud.tencent.com/document/product/436/8283"/>
    /// </summary>
    public sealed class DeleteBucketCORSRequest : BucketRequest
    {
        public DeleteBucketCORSRequest(string bucket) : base(bucket)
        {
            this.method = CosRequestMethod.DELETE;
            this.queryParameters.Add("cors", null);
        }
    }
}

using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;

namespace COSXML.Model.Bucket
{
    /// <summary>
    /// 检索 Bucket 是否存在
    /// <see cref="https://cloud.tencent.com/document/product/436/7735"/>
    /// </summary>
    public sealed class HeadBucketRequest : BucketRequest
    {
        public HeadBucketRequest(string bucket)
            : base(bucket)
        {
            this.method = CosRequestMethod.HEAD;
        }
    }
}

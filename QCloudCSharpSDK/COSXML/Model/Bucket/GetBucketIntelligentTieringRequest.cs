using System;

using System.Text;
using COSXML.Common;
using COSXML.Model.Tag;
using COSXML.Network;
using COSXML.CosException;

namespace COSXML.Model.Bucket
{
    /// <summary>
    /// 设置 Bucket 生命周期
    /// <see cref="https://cloud.tencent.com/document/product/436/8280"/>
    /// </summary>
    public sealed class GetBucketIntelligentTieringRequest : BucketRequest
    {

        public GetBucketIntelligentTieringRequest(string bucket)
            : base(bucket)
        {
            this.method = CosRequestMethod.GET;
            this.queryParameters.Add("intelligenttiering", null);
        }
    }
}

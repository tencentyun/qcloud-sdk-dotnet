using System;
using System.Text;

using COSXML.Common;
using COSXML.Model.Tag;
using COSXML.Network;

namespace COSXML.Model.Bucket
{
    /// <summary>
    /// 检索 Bucket 是否存在
    /// <see cref="https://cloud.tencent.com/document/product/436/7735"/>
    /// </summary>
    public sealed class PutBucketDomainRequest : BucketRequest
    {
        private DomainConfiguration domain;

        public PutBucketDomainRequest(string bucket, DomainConfiguration domain)
            : base(bucket)
        {
            this.method = CosRequestMethod.PUT;
            this.domain = domain;
            this.queryParameters.Add("domain", null);
        }

        public override Network.RequestBody GetRequestBody()
        {
            return GetXmlRequestBody(domain);
        }
    }
}

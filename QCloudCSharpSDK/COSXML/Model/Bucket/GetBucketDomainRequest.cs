using COSXML.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace COSXML.Model.Bucket
{
    public sealed class GetBucketDomainRequest : BucketRequest
    {
        public GetBucketDomainRequest(string bucket) : base(bucket)
        {
            this.method = CosRequestMethod.GET;
            this.queryParameters.Add("domain", null);
        }
    }
}

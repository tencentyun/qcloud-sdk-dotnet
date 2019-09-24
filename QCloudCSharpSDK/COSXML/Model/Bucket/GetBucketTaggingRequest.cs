using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;

namespace COSXML.Model.Bucket
{
    public sealed class GetBucketTaggingRequest : BucketRequest
    {
        public GetBucketTaggingRequest(string bucket) : base(bucket)
        {
            this.method = CosRequestMethod.GET;
            this.queryParameters.Add("tagging", null);
        }
    }
}

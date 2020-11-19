using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;

namespace COSXML.Model.Bucket
{
    public sealed class GetBucketVersioningRequest : BucketRequest
    {
        public GetBucketVersioningRequest(string bucket) : base(bucket)
        {
            this.method = CosRequestMethod.GET;
            this.queryParameters.Add("versioning", null);
        }
    }
}

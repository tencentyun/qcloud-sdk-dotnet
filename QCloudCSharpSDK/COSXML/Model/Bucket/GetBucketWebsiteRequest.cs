using COSXML.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSXML.Model.Bucket
{
    public sealed class GetBucketWebsiteRequest :BucketRequest
    {
        public GetBucketWebsiteRequest(string bucket) : base(bucket)
        {
            this.method = CosRequestMethod.GET;
            this.queryParameters.Add("website", null);
        }
    }
}

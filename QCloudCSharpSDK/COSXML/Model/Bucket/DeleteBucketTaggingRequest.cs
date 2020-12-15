using COSXML.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSXML.Model.Bucket
{
    public sealed class DeleteBucketTaggingRequest : BucketRequest
    {
        public DeleteBucketTaggingRequest(string bucket) : base(bucket)
        {
            this.method = CosRequestMethod.DELETE;
            this.queryParameters.Add("tagging", null);
        }

    }
}

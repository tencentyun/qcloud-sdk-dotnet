using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;

namespace COSXML.Model.Bucket
{
    public sealed class DeleteBucketReplicationRequest : BucketRequest
    {
        public DeleteBucketReplicationRequest(string bucket)
            : base(bucket)
        {
            this.method = CosRequestMethod.DELETE;
            this.queryParameters.Add("replication", null);
        }
    }
}

using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Model.Tag;
using COSXML.Transfer;

namespace COSXML.Model.Bucket
{
    public sealed class GetBucketReplicationResult : CosDataResult<ReplicationConfiguration>
    {
        public ReplicationConfiguration replicationConfiguration {get => _data; }
    }
}

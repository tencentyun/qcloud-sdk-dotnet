using COSXML.Model.Tag;
using COSXML.Transfer;
using System;
using System.Collections.Generic;
using System.Text;

namespace COSXML.Model.Bucket
{
    public sealed class GetBucketLoggingResult : CosDataResult<BucketLoggingStatus>
    {
        public BucketLoggingStatus bucketLoggingStatus {get => _data; }
    }
}

using COSXML.Common;
using COSXML.Model.Tag;
using COSXML.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace COSXML.Model.Bucket
{
    public sealed class PutBucketLoggingRequest : BucketRequest
    {
        private BucketLoggingStatus bucketLoggingStatus;

        public PutBucketLoggingRequest(string bucket) : base(bucket)
        {
            this.method = CosRequestMethod.PUT;
            this.queryParameters.Add("logging", null);
            this.bucketLoggingStatus = new BucketLoggingStatus();
        }

        public void SetTarget(string targetBucket, string targetPrefix)
        {

            if (targetPrefix == null && targetPrefix == null)
            {

                return;
            }

            if (bucketLoggingStatus.loggingEnabled == null)
            {
                bucketLoggingStatus.loggingEnabled = new BucketLoggingStatus.LoggingEnabled();
            }

            if (targetBucket != null)
            {
                bucketLoggingStatus.loggingEnabled.targetBucket = targetBucket;
            }

            if (targetPrefix != null)
            {
                bucketLoggingStatus.loggingEnabled.targetPrefix = targetPrefix;
            }
        }

        public override Network.RequestBody GetRequestBody()
        {
            return GetXmlRequestBody(bucketLoggingStatus);
        }
    }
}

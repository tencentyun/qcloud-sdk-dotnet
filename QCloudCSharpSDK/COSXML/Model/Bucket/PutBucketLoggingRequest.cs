using COSXML.Common;
using COSXML.Model.Tag;
using COSXML.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSXML.Model.Bucket
{
    public sealed class PutBucketLoggingRequest : BucketRequest
    {
        private BucketLoggingStatus bucketLoggingStatus;
        public PutBucketLoggingRequest(string bucket):base(bucket)
        {
            this.method = CosRequestMethod.PUT;
            this.queryParameters.Add("logging", null);
        }

        public void SetTarget(string targetBucket, string targetPrefix)
        {
            if (targetPrefix == null && targetPrefix == null) return;
            if (bucketLoggingStatus.loggingEnabled == null)
            {
                bucketLoggingStatus.loggingEnabled = new BucketLoggingStatus.LoggingEnabled();
            }
            if(targetBucket != null) bucketLoggingStatus.loggingEnabled.targetBucket = targetBucket;
            if(targetPrefix != null) bucketLoggingStatus.loggingEnabled.targetPrefix = targetPrefix;
        }

        public override Network.RequestBody GetRequestBody()
        {
            string content = Transfer.XmlBuilder.BuildBucketLogging(bucketLoggingStatus);
            byte[] data = Encoding.UTF8.GetBytes(content);
            ByteRequestBody body = new ByteRequestBody(data);
            return body;
        }
    }
}

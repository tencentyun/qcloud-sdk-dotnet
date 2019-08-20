using COSXML.Model.Tag;
using COSXML.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSXML.Model.Bucket
{
    public sealed class GetBucketLoggingResult : CosResult
    {
        public BucketLoggingStatus bucketLoggingStatus;
        internal override void ParseResponseBody(System.IO.Stream inputStream, string contentType, long contentLength)
        {
            bucketLoggingStatus = new BucketLoggingStatus();
            XmlParse.ParseBucketLoggingStatus(inputStream, bucketLoggingStatus);
        }

        public override string GetResultInfo()
        {
            return base.GetResultInfo() + (bucketLoggingStatus == null ? "" : "\n" + bucketLoggingStatus.GetInfo());
        }
    }
}

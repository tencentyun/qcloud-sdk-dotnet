using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Model.Tag;
using COSXML.Transfer;

namespace COSXML.Model.Bucket
{
    public sealed class GetBucketReplicationResult : CosResult
    {
        public ReplicationConfiguration replicationConfiguration;

        internal override void ParseResponseBody(System.IO.Stream inputStream, string contentType, long contentLength)
        {
            replicationConfiguration = new ReplicationConfiguration();
            XmlParse.ParseReplicationConfiguration(inputStream, replicationConfiguration);
        }

        public override string GetResultInfo()
        {
            return base.GetResultInfo() + (replicationConfiguration == null ? "" : "\n" + replicationConfiguration.GetInfo()); 
        }
    }
}

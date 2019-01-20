using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;
using COSXML.Model.Tag;
using COSXML.Network;
using COSXML.CosException;

namespace COSXML.Model.Bucket
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class PutBucketReplicationRequest : BucketRequest
    {
        private ReplicationConfiguration replicationConfiguration;

        public PutBucketReplicationRequest(string bucket)
            : base(bucket)
        {
            this.method = CosRequestMethod.PUT;
            this.needMD5 = true;
            this.queryParameters.Add("replication", null);
            replicationConfiguration = new ReplicationConfiguration();
            replicationConfiguration.rules = new List<ReplicationConfiguration.Rule>();
        }

        public override Network.RequestBody GetRequestBody()
        {
            string content = Transfer.XmlBuilder.BuildReplicationConfiguration(replicationConfiguration);
            byte[] data = Encoding.UTF8.GetBytes(content);
            ByteRequestBody body = new ByteRequestBody(data);
            return body;
        }

        public void SetReplicationConfiguration(string ownerUin, string subUin, List<RuleStruct> ruleStructs)
        {
            SetReplicationConfigurationWithRole(ownerUin, subUin);
            if (ruleStructs != null)
            {
                foreach (RuleStruct ruleStruct in ruleStructs)
                {
                    SetReplicationConfigurationWithRule(ruleStruct);
                }
            }
        }
        
        private void SetReplicationConfigurationWithRole(string ownerUin, string subUin)
        {
            if (ownerUin != null && subUin != null)
            {
                string role = "qcs::cam::uin/" + ownerUin + ":uin/" + subUin;
                replicationConfiguration.role = role;
            }
        }

        private void SetReplicationConfigurationWithRule(RuleStruct ruleStruct)
        {
            if (ruleStruct != null)
            {
                ReplicationConfiguration.Rule rule = new ReplicationConfiguration.Rule();
                rule.id = ruleStruct.id;
                rule.status = ruleStruct.isEnable ? "Enabled" : "Disabled";
                rule.prefix = ruleStruct.prefix;
                ReplicationConfiguration.Destination destination = new ReplicationConfiguration.Destination();
                destination.storageClass = ruleStruct.storageClass;
                StringBuilder bucket = new StringBuilder();
                bucket.Append("qcs:id/0:cos:").Append(ruleStruct.region).Append(":appid/")
                        .Append(ruleStruct.appid).Append(":").Append(ruleStruct.bucket);
                destination.bucket = bucket.ToString();
                rule.destination = destination;
                replicationConfiguration.rules.Add(rule);
            }
        }

        public override void CheckParameters()
        {
            base.CheckParameters();
            if (replicationConfiguration.rules.Count == 0) throw new CosClientException((int)CosClientError.INVALID_ARGUMENT, "replicationConfiguration.rules.Count = 0");
        }

        public sealed class RuleStruct
        {
            public string appid;
            public string region;
            public string bucket;
            public string storageClass;
            public string id;
            public string prefix;
            public bool isEnable;
        }
    }

}

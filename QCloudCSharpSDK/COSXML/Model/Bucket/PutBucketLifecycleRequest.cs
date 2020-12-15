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
    /// 设置 Bucket 生命周期
    /// <see cref="https://cloud.tencent.com/document/product/436/8280"/>
    /// </summary>
    public sealed class PutBucketLifecycleRequest : BucketRequest
    {
        private LifecycleConfiguration lifecycleConfiguration;

        public PutBucketLifecycleRequest(string bucket)
            : base(bucket)
        {
            this.method = CosRequestMethod.PUT;
            this.queryParameters.Add("lifecycle", null);
            lifecycleConfiguration = new LifecycleConfiguration();
            lifecycleConfiguration.rules = new List<LifecycleConfiguration.Rule>();
        }

        public override Network.RequestBody GetRequestBody()
        {
            return GetXmlRequestBody(lifecycleConfiguration);
        }

        /// <summary>
        /// 设置生命周期规则
        /// <see cref="Model.Tag.LifecycleConfiguration.Rule"/>
        /// </summary>
        /// <param name="rule"></param>
        public void SetRule(LifecycleConfiguration.Rule rule)
        {

            if (rule != null)
            {
                lifecycleConfiguration.rules.Add(rule);
            }
        }

        /// <summary>
        /// 设置生命周期规则
        /// <see cref="Model.Tag.LifecycleConfiguration.Rule"/>
        /// </summary>
        /// <param name="rules"></param>
        public void SetRules(List<LifecycleConfiguration.Rule> rules)
        {

            if (rules != null)
            {
                lifecycleConfiguration.rules.AddRange(rules);
            }
        }

        public override void CheckParameters()
        {
            base.CheckParameters();

            if (lifecycleConfiguration.rules.Count == 0)
            {
                throw new CosClientException((int)CosClientError.InvalidArgument, "lifecycleConfiguration.rules.Count = 0");
            }
        }
    }
}

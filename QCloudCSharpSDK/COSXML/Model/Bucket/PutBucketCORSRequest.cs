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
    /// 设置 Bucket CORS
    /// <see cref="https://cloud.tencent.com/document/product/436/8279"/>
    /// </summary>
    public sealed class PutBucketCORSRequest : BucketRequest
    {
        /// <summary>
        /// CORS 配置信息
        /// <see cref="Model.Tag.CORSConfiguration"/>
        /// </summary>
        private CORSConfiguration corsConfiguration;

        public PutBucketCORSRequest(string bucket)
            : base(bucket)
        {
            this.method = CosRequestMethod.PUT;
            this.queryParameters.Add("cors", null);
            corsConfiguration = new CORSConfiguration();
            corsConfiguration.corsRules = new List<CORSConfiguration.CORSRule>();
        }

        public override Network.RequestBody GetRequestBody()
        {
            return GetXmlRequestBody(corsConfiguration);
        }

        /// <summary>
        /// 设置 CORS 规则
        /// <see cref="Model.Tag.CORSConfiguration.CORSRule"/>
        /// </summary>
        /// <param name="corsRule"></param>
        public void SetCORSRule(CORSConfiguration.CORSRule corsRule)
        {

            if (corsRule != null)
            {
                corsConfiguration.corsRules.Add(corsRule);
            }
        }

        /// <summary>
        /// 设置 CORS 规则
        /// <see cref="Model.Tag.CORSConfiguration.CORSRule"/>
        /// </summary>
        /// <param name="corsRules"></param>
        public void SetCORSRules(List<CORSConfiguration.CORSRule> corsRules)
        {

            if (corsRules != null)
            {
                corsConfiguration.corsRules.AddRange(corsRules);
            }
        }

        public override void CheckParameters()
        {
            base.CheckParameters();

            if (corsConfiguration.corsRules.Count == 0)
            {
                throw new CosClientException((int)CosClientError.InvalidArgument, "corsConfiguration.corsRules.Count = 0");
            }
        }
    }
}

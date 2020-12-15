using System;

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
    public sealed class PutBucketIntelligentTieringRequest : BucketRequest
    {
        private IntelligentTieringConfiguration configuration;

        public PutBucketIntelligentTieringRequest(string bucket, IntelligentTieringConfiguration configuration)
            : base(bucket)
        {

            if (String.IsNullOrEmpty(configuration.Status))
            {
                configuration.Status = "Enabled";
            }
            
            if (configuration.Transition == null)
            {
                configuration.Transition = new IntelligentTieringConfiguration.IntelligentTieringTransition();
            }

            if (configuration.Transition.Days < 1)
            {
                configuration.Transition.Days = 30;
            }

            if (configuration.Transition.RequestFrequent < 1)
            {
                configuration.Transition.RequestFrequent = 1;
            }

            this.method = CosRequestMethod.PUT;
            this.queryParameters.Add("intelligenttiering", null);
            this.configuration = configuration;
        }

        public override Network.RequestBody GetRequestBody()
        {
            return GetXmlRequestBody(configuration);
        }

        public override void CheckParameters()
        {
            base.CheckParameters();

            if (String.IsNullOrEmpty(configuration.Status))
            {
                throw new CosClientException((int)CosClientError.InvalidArgument, "Status 不能为空");
            }
        }
    }
}

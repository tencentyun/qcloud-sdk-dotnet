using COSXML.Common;
using COSXML.Model.Tag;
using COSXML.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace COSXML.Model.Bucket
{
    /// <summary>
    /// 设置 Bucket 防盗链
    /// <see href="https://cloud.tencent.com/document/product/436/32492"/>
    /// </summary>
    public sealed class PutBucketRefererRequest : BucketRequest
    {

        public RefererConfiguration refererConfiguration;

        public PutBucketRefererRequest(string bucket) : base(bucket)
        {
            this.method = CosRequestMethod.PUT;
            this.queryParameters.Add("referer", null);
        }

        public void SetRefererConfiguration(RefererConfiguration refererConfiguration) {

            this.refererConfiguration = refererConfiguration;
        }

        public override Network.RequestBody GetRequestBody()
        {
            return GetXmlRequestBody(refererConfiguration);
        }

    }
}

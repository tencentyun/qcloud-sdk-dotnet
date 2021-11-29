using System;
using System.Text;

using COSXML.Common;
using COSXML.Model.Tag;
using COSXML.Network;

namespace COSXML.Model.Bucket
{
    /// <summary>
    /// 添加存储桶标签
    /// <see href="https://cloud.tencent.com/document/product/436/7735"/>
    /// </summary>
    public sealed class PutBucketTaggingRequest : BucketRequest
    {
        private Tagging tagging;

        public PutBucketTaggingRequest(string bucket)
            : base(bucket)
        {
            this.method = CosRequestMethod.PUT;
            this.tagging = new Tagging();
            this.queryParameters.Add("tagging", null);
        }

        public void AddTag(string key, string value)
        {
            this.tagging.AddTag(key, value);
        }


        public override Network.RequestBody GetRequestBody()
        {
            return GetXmlRequestBody(tagging);
        }
    }
}

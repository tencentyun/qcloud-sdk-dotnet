using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;
using COSXML.Utils;
using COSXML.Model.Tag;

namespace COSXML.Model.Object
{
    /// <summary>
    /// 设置对象标签
    /// <see href="https://cloud.tencent.com/document/product/436/42997"/>
    /// </summary>
    public sealed class PutObjectTaggingRequest : ObjectRequest
    {
        private Tagging tagging;

        public PutObjectTaggingRequest(string bucket, string key)
            : base(bucket, key)
        {
            this.method = CosRequestMethod.PUT;
            this.queryParameters.Add("tagging", null);
            this.tagging = new Tagging();
        }

        /// <summary>
        /// 向指定版本的对象添加标签
        /// </summary>
        /// <param name="versionId"></param>
        public void SetVersionId(string versionId)
        {

            if (versionId != null)
            {
                SetQueryParameter(CosRequestHeaderKey.VERSION_ID, versionId);
            }
        }

        /// <summary>
        /// 向标签键值对增加内容
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
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

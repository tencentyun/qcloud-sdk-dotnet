using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;
using COSXML.Utils;
using COSXML.Model.Tag;

namespace COSXML.Model.Object
{
    /// <summary>
    /// 获取对象标签
    /// <see href="https://cloud.tencent.com/document/product/436/42998"/>
    /// </summary>
    public sealed class GetObjectTaggingRequest : ObjectRequest
    {
        public GetObjectTaggingRequest(string bucket, string key)
            : base(bucket, key)
        {
            this.method = CosRequestMethod.GET;
            this.queryParameters.Add("tagging", null);
        }

        /// <summary>
        /// 指定获取对象某个版本的对象标签
        /// </summary>
        /// <param name="versionId"></param>
        public void SetVersionId(string versionId)
        {

            if (versionId != null)
            {
                SetQueryParameter(CosRequestHeaderKey.VERSION_ID, versionId);
            }
        }

    }
}

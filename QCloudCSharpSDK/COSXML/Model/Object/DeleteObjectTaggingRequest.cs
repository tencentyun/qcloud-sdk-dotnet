using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;
using COSXML.Utils;
using COSXML.Model.Tag;

namespace COSXML.Model.Object
{
    /// <summary>
    /// 删除对象标签
    /// <see href="https://cloud.tencent.com/document/product/436/42999"/>
    /// </summary>
    public sealed class DeleteObjectTaggingRequest : ObjectRequest
    {
        public DeleteObjectTaggingRequest(string bucket, string key)
            : base(bucket, key)
        {
            this.method = CosRequestMethod.DELETE;
            this.queryParameters.Add("tagging", null);
        }

        /// <summary>
        /// 删除对象某个版本的对象标签
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

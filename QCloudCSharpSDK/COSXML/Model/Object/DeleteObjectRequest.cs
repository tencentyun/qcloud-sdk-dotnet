using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;

namespace COSXML.Model.Object
{
    /// <summary>
    /// 删除对象
    /// <see cref="https://cloud.tencent.com/document/product/436/7743"/>
    /// </summary>
    public sealed class DeleteObjectRequest : ObjectRequest
    {
        public DeleteObjectRequest(string bucket, string key)
            : base(bucket, key)
        {
            this.method = CosRequestMethod.DELETE;
        }

        /// <summary>
        /// 删除指定版本的对象
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

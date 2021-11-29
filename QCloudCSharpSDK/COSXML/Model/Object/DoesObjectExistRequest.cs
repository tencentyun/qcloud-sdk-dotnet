using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;

namespace COSXML.Model.Object
{
    /// <summary>
    /// 检查对象是否存在, 封装了Head接口
    /// <see href="https://cloud.tencent.com/document/product/436/7745"/>
    /// </summary>
    public sealed class DoesObjectExistRequest : ObjectRequest
    {
        public DoesObjectExistRequest(string bucket, string key)
            : base(bucket, key)
        {
            this.method = CosRequestMethod.HEAD;
        }

        /// <summary>
        /// 特定版本的对象
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

using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;

namespace COSXML.Model.Object
{
    /// <summary>
    /// 检索 对象 的 meta 信息数据
    /// <see cref="https://cloud.tencent.com/document/product/436/7745"/>
    /// </summary>
    public sealed class HeadObjectRequest : ObjectRequest
    {
        public HeadObjectRequest(string bucket, string key)
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
        /// <summary>
        /// 当 Object 在指定时间后被修改，则返回对应 Object 的 meta 信息，否则返回 304
        /// </summary>
        /// <param name="ifModifiedSince"></param>
        public void SetIfModifiedSince(string ifModifiedSince)
        {
            if (ifModifiedSince != null)
            {
                SetRequestHeader(CosRequestHeaderKey.IF_MODIFIED_SINCE, ifModifiedSince);
            }
        }

    }
}

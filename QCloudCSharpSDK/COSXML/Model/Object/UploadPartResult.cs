using System;
using System.Collections.Generic;

using System.Text;
using System.IO;

namespace COSXML.Model.Object
{
    /// <summary>
    /// 分片上传返回的结果
    /// <see cref="https://cloud.tencent.com/document/product/436/7750"/>
    /// </summary>
    public sealed class UploadPartResult : CosResult
    {
        /// <summary>
        /// 分片块的eTag
        /// </summary>
        public string eTag;

        internal override void InternalParseResponseHeaders()
        {
            List<string> values;

            this.responseHeaders.TryGetValue("ETag", out values);

            if (values != null && values.Count > 0)
            {
                eTag = values[0];
            }
        }
    }
}

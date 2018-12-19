using System;
using System.Collections.Generic;

using System.Text;

namespace COSXML.Model.Object
{
    /// <summary>
    /// 简单上传对象返回的结果
    /// <see cref="https://cloud.tencent.com/document/product/436/7749"/>
    /// </summary>
    public sealed class PutObjectResult : CosResult
    {
        /// <summary>
        /// 对象的eTag
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

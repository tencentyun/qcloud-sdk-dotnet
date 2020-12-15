using System;
using System.Collections.Generic;

using System.Text;
using System.IO;
using COSXML.CosException;
using COSXML.Common;

namespace COSXML.Model.Object
{
    /// <summary>
    /// 下载对象返回的结果
    /// <see cref="https://cloud.tencent.com/document/product/436/7753"/>
    /// </summary>
    public sealed class GetObjectResult : CosResult
    {
        /// <summary>
        /// 对象的 eTag
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

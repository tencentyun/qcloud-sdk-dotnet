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
    /// <see href="https://cloud.tencent.com/document/product/436/7753"/>
    /// </summary>
    public sealed class GetObjectResult : CosResult
    {
        /// <summary>
        /// 对象的 eTag
        /// </summary>
        public string eTag;

        /// <summary>
        /// 对象的 crc64
        /// </summary>
        public string crc64ecma;

        internal override void InternalParseResponseHeaders()
        {
            List<string> values;

            this.responseHeaders.TryGetValue("ETag", out values);

            if (values != null && values.Count > 0)
            {
                eTag = values[0];
            }

            this.responseHeaders.TryGetValue("x-cos-hash-crc64ecma", out values);

            if (values != null && values.Count > 0)
            {
                crc64ecma = values[0];
            }
        }
    }
}

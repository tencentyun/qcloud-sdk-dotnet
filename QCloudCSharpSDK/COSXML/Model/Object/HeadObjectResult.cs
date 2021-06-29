using System;
using System.Collections.Generic;

using System.Text;

namespace COSXML.Model.Object
{
    /// <summary>
    /// 检索对象返回的结果
    /// <see href="https://cloud.tencent.com/document/product/436/7745"/>
    /// </summary>
    public sealed class HeadObjectResult : CosResult
    {

        /// <summary>
        /// Object 的存储级别，枚举值：STANDARD,STANDARD_IA
        /// <see href="Common.CosStorageClass"/>
        /// </summary>
        public string cosStorageClass;

        /// <summary>
        /// 对象的长度
        /// </summary>
        public long size;

        /// <summary>
        /// 对象的eTag
        /// </summary>
        public string eTag;

        /// <summary>
        /// 对象的 crc64ecma
        /// </summary>
        public string crc64ecma;

        /// <summary>
        /// 对象的 lastModified
        /// </summary>
        public string lastModified;

        internal override void InternalParseResponseHeaders()
        {
            List<string> values;

            this.responseHeaders.TryGetValue("x-cos-storage-class", out values);

            if (values != null && values.Count > 0)
            {
                cosStorageClass = values[0];
            }

            this.responseHeaders.TryGetValue("Content-Length", out values);

            if (values != null && values.Count > 0)
            {
                long.TryParse(values[0], out size);
            }

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

            this.responseHeaders.TryGetValue("Last-Modified", out values);

            if (values != null && values.Count > 0)
            {
                lastModified = values[0];
            }
        }
    }
}

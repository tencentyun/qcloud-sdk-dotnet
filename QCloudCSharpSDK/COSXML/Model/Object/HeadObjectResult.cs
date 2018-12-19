using System;
using System.Collections.Generic;

using System.Text;

namespace COSXML.Model.Object
{
    /// <summary>
    /// 检索对象返回的结果
    /// <see cref="https://cloud.tencent.com/document/product/436/7745"/>
    /// </summary>
    public sealed class HeadObjectResult : CosResult
    {
        /// <summary>
        /// 用来表示 Object 是否可以被追加上传，枚举值：normal 或者 appendable
        /// </summary>
        public string cosObjectType;
        /// <summary>
        /// Object 的存储级别，枚举值：STANDARD,STANDARD_IA
        /// <see cref="Common.CosStorageClass"/>
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

        internal override void InternalParseResponseHeaders()
        {
            List<string> values = new List<string>();
            this.responseHeaders.TryGetValue("x-cos-object-type", out values);
            if (values != null && values.Count > 0)
            {
                cosObjectType = values[0];
            }
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
        }
    }
}

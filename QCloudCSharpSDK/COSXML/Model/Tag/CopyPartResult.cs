using System;
using System.Xml.Serialization;

using System.Text;

namespace COSXML.Model.Tag
{
    /// <summary>
    /// 复制结果返回信息
    /// <see cref="https://cloud.tencent.com/document/product/436/10881"/>
    /// </summary>
    [XmlRoot("CopyPartResult")]
    public sealed class CopyPartResult
    {
        /// <summary>
        /// 返回文件的 MD5 算法校验值。ETag 的值可以用于检查 Object 的内容是否发生变化
        /// </summary>
        [XmlElement("ETag")]
        public string eTag;

        /// <summary>
        /// 返回文件最后修改时间，GMT 格式
        /// </summary>
        [XmlElement("LastModified")]
        public string lastModified;

    }
}

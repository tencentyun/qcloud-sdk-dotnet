using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;

namespace COSXML.Model.Tag
{
    /// <summary>
    /// 本次分块上传的所有信息
    /// <see cref="https://cloud.tencent.com/document/product/436/7742"/>
    /// </summary>
    [XmlRoot]
    public sealed class CompleteMultipartUpload
    {
        /// <summary>
        /// 本次分块上传中每个块的信息
        /// <see cref="Part"/>
        /// </summary>
        [XmlElement("Part")]
        public List<Part> parts;

        public sealed class Part
        {
            /// <summary>
            /// 块编号
            /// </summary>
            [XmlElement("PartNumber")]
            public int partNumber;

            /// <summary>
            /// 每个块文件的 eTag 值
            /// </summary>
            [XmlElement("ETag")]
            public string eTag;
        }
    }
}

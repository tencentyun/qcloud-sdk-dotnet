using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;

namespace COSXML.Model.Tag
{
    /// <summary>
    /// 分片上传完成结果
    /// <see cref="https://cloud.tencent.com/document/product/436/7742"/>
    /// </summary>
    [XmlRoot("CompleteMultipartUploadResult")]
    public sealed class CompleteResult
    {
        /// <summary>
        /// 创建的Object的外网访问域名
        /// </summary>
        [XmlElement("Location")]
        public string location;

        /// <summary>
        /// 分块上传的目标Bucket
        /// </summary>
        [XmlElement("Bucket")]
        public string bucket;

        /// <summary>
        /// Object的名称
        /// </summary>
        [XmlElement("Key")]
        public string key;

        /// <summary>
        /// 合并后对象的唯一标签值，该值不是对象内容的 MD5 校验值，仅能用于检查对象唯一性
        /// </summary>
        [XmlElement("ETag")]
        public string eTag;


        public string GetInfo()
        {
            StringBuilder stringBuilder = new StringBuilder("{CompleteMultipartUploadResult:\n");

            stringBuilder.Append("Location:").Append(location).Append("\n");
            stringBuilder.Append("Bucket:").Append(bucket).Append("\n");
            stringBuilder.Append("Key:").Append(key).Append("\n");
            stringBuilder.Append("ETag:").Append(eTag).Append("\n");
            stringBuilder.Append("}");

            return stringBuilder.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;

namespace COSXML.Model.Tag
{
    /// <summary>
    /// 初始化上传返回的信息
    /// <see cref="https://cloud.tencent.com/document/product/436/7746"/>
    /// </summary>
    [XmlRoot("InitiateMultipartUploadResult")]
    public sealed class InitiateMultipartUpload
    {
        /// <summary>
        /// 分片上传的目标 Bucket，由用户自定义字符串和系统生成appid数字串由中划线连接而成，如：mybucket-1250000000.
        /// </summary>
        [XmlElement("Bucket")]
        public string bucket;

        /// <summary>
        /// Object 的名称
        /// </summary>
        [XmlElement("Key")]
        public string key;

        /// <summary>
        /// 在后续上传中使用的 ID
        /// </summary>
        [XmlElement("UploadId")]
        public string uploadId;
    }
}

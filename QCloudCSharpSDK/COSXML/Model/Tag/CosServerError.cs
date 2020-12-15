using System;
using System.Xml.Serialization;

using System.Text;

namespace COSXML.Model.Tag
{
    /// <summary>
    /// cos server 返回的错误信息
    /// <see cref="https://cloud.tencent.com/document/product/436/7730"/>
    /// </summary>
    [XmlRoot("Error")]
    public sealed class CosServerError
    {
        /// <summary>
        /// 错误码
        /// </summary>
        [XmlElement("Code")]
        public string code;

        /// <summary>
        /// 错误信息
        /// </summary>
        [XmlElement("Message")]
        public string message;

        /// <summary>
        /// 资源地址
        /// </summary>
        [XmlElement("Resource")]
        public string resource;

        /// <summary>
        /// 请求ID
        /// </summary>
        [XmlElement("RequestId")]
        public string requestId;

        /// <summary>
        /// 错误ID
        /// </summary>
        [XmlElement("TraceId")]
        public string traceId;
    }
}

using System.Xml.Serialization;
using System.Collections.Generic;

namespace COSXML.Model.Tag
{
    /// <summary>
    /// 图片审核结果
    /// </summary>
    [XmlRoot("Response")]
    public sealed class QRRecognitionResult
    {

        /// <summary>
        /// 二维码识别结果
        /// </summary>
        [XmlElement]
        public int CodeStatus;

        /// <summary>
        /// 二维码识别结果
        /// </summary>
        [XmlElement]
        public QRcodeRecognitionInfo QRcodeInfo;

        /// <summary>
        /// 处理后的图片 base64数据，请求参数 cover 为1时返回
        /// </summary>
        [XmlElement]
        public string ResultImage;

        /// <summary>
        /// 二维码识别结果
        /// </summary>
        public sealed class QRcodeRecognitionInfo
        {
            /// <summary>
            /// 二维码的内容
            /// </summary>
            [XmlElement]
            public string CodeUrl;

            /// <summary>
            /// 图中识别到的二维码位置坐标
            /// </summary>
            [XmlElement]
            public CodeLocationInfo CodeLocation;
        }

        /// <summary>
        /// 二维码位置坐标
        /// </summary>
        public sealed class CodeLocationInfo
        {
            /// <summary>
            /// 二维码的内容
            /// </summary>
            [XmlElement]
            public List<string> Point;
        }
    }
}
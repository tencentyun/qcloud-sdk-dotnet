using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;

namespace COSXML.Model.Tag
{
    /// <summary>
    /// 文本审核请求body
    /// <see href="https://cloud.tencent.com/document/product/436/56289"/>
    /// </summary>
    [XmlRoot("Request")]
    public sealed class TextCensorJobInfo
    {
        [XmlElement("Input")]
        public Input input;

        [XmlElement("Conf")]
        public Conf conf;

        public string GetInfo()
        {
            StringBuilder stringBuilder = new StringBuilder("{Request:\n");

            stringBuilder.Append(input.GetInfo()).Append("\n");

            stringBuilder.Append(conf.GetInfo()).Append("\n");

            stringBuilder.Append("}");

            return stringBuilder.ToString();
        }

        public sealed class Input
        {
            [XmlElement("Object")]
            public string obj;

            [XmlElement("Content")]
            public string content;

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{Input:\n");
                if (obj != null)
                {
                    stringBuilder.Append("Object:" + obj + "\n");
                }
                if (content != null)
                {
                    stringBuilder.Append("Content:" + content + "\n");
                }
                stringBuilder.Append("}");
                return stringBuilder.ToString();
            }
        }


        public sealed class Conf
        {
            [XmlElement("DetectType")]
            public string detectType;

            [XmlElement("Callback")]
            public string callback;

            [XmlElement("BizType")]
            public string bizType;

            public string GetInfo()
            { 
                StringBuilder stringBuilder = new StringBuilder("{Conf:\n");

                stringBuilder.Append("DetectType:").Append(detectType).Append("\n");
                if (callback != null)
                {
                    stringBuilder.Append("Callback:").Append(callback).Append("\n");
                }
                if (bizType != null)
                {
                    stringBuilder.Append("BizType:").Append(bizType).Append("\n");
                }
                stringBuilder.Append("}");

                return stringBuilder.ToString();
            }
        }

    }

}

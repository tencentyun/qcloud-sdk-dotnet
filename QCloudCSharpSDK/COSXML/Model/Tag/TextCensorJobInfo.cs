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

            [XmlElement("Url")]
            public string url;

            [XmlElement("DataId")]
            public string dataId;
/*
            [XmlElement("UserInfo")]
            public UserInfo userInfo;
*/
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
                if (url != null)
                {
                    stringBuilder.Append("Url:" + url + "\n");
                }
                if (dataId != null)
                {
                    stringBuilder.Append("DataId:" + dataId + "\n");
                }

                //stringBuilder.Append(userInfo.GetInfo().Append("\n"));

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
/*
        public sealed class UserInfo
        {
            [XmlElement("TokenId")]
            public string tokenId;

            [XmlElement("Nickname")]
            public string niekName;

            [XmlElement("DeviceId")]
            public string deviceId;

            [XmlElement("AppId")]
            public string appId;

            [XmlElement("Room")]
            public string room;

            [XmlElement("IP")]
            public string ip;

            [XmlElement("Type")]
            public string type;

            public string GetInfo()
            { 
                StringBuilder stringBuilder = new StringBuilder("{UserInfo:\n");

                if (tokenId != null)
                {
                    stringBuilder.Append("TokenId:").Append(tokenId).Append("\n");
                }
                if (niekName != null)
                {
                    stringBuilder.Append("Nickname:").Append(niekName).Append("\n");
                }
                if (deviceId != null)
                {
                    stringBuilder.Append("DeviceId:").Append(deviceId).Append("\n");
                }
                if (appId != null)
                {
                    stringBuilder.Append("AppId:").Append(appId).Append("\n");
                }
                if (room != null)
                {
                    stringBuilder.Append("Room:").Append(room).Append("\n");
                }
                if (ip != null)
                {
                    stringBuilder.Append("IP:").Append(ip).Append("\n");
                }
                if (type != null)
                {
                    stringBuilder.Append("Type:").Append(type).Append("\n");
                }
                stringBuilder.Append("}");

                return stringBuilder.ToString();
            }
        }
*/
    }

}

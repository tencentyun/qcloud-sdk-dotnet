using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;

namespace COSXML.Model.Tag
{
    /// <summary>
    /// 视频审核
    /// <see href="https://cloud.tencent.com/document/product/436/7733"/>
    /// </summary>
    [XmlRoot("Request")]
    public sealed class VideoCencorJobInfo
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

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{Input:\n");
                stringBuilder.Append("Object:" + obj + "\n");
                stringBuilder.Append("}");
                return stringBuilder.ToString();
            }
        }


        public sealed class Conf
        {
            [XmlElement("DetectType")]
            public string detectType;

            [XmlElement("Snapshot")]
            public Snapshot snapshot;

            [XmlElement("Callback")]
            public string callback;

            [XmlElement("CallbackVersion")]
            public string callbackVersion;

            [XmlElement("BizType")]
            public string bizType;

            [XmlElement("DetectContent")]
            public string detectContent;

            public string GetInfo()
            { 
                StringBuilder stringBuilder = new StringBuilder("{Conf:\n");

                stringBuilder.Append("DetectType:").Append(detectType).Append("\n");
                stringBuilder.Append("Snapshot:").Append(snapshot.GetInfo()).Append("\n");
                if (detectContent != null)
                {
                    stringBuilder.Append("DetectContent:").Append(detectContent).Append("\n");
                }
                if (callback != null)
                {
                    stringBuilder.Append("Callback:").Append(callback).Append("\n");
                }
                if (callbackVersion != null)
                {
                    stringBuilder.Append("CallbackVersion:").Append(callbackVersion).Append("\n");
                }
                if (bizType != null)
                {
                    stringBuilder.Append("BizType:").Append(bizType).Append("\n");
                }
                stringBuilder.Append("}");

                return stringBuilder.ToString();
            }
        }

        public sealed class Snapshot
        {
            [XmlElement("Mode")]
            public string mode = "";

            [XmlElement("Count")]
            public string count = "";

            [XmlElement("TimeInterval")]
            public string timeInterval = "";

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{Snapshot:\n");

                if (mode != null)
                {
                    stringBuilder.Append("Mode:").Append(mode).Append("\n");
                }
                if (count != null)
                {
                    stringBuilder.Append("Count:").Append(count).Append("\n");
                }
                if (timeInterval != null)
                {
                    stringBuilder.Append("TimeInterval:").Append(timeInterval).Append("\n");
                }
                stringBuilder.Append("}");

                return stringBuilder.ToString();
            }
            
        }
    }

}

using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;

namespace COSXML.Model.Tag
{
    /// <summary>
    /// 音频审核结果
    /// <see href="https://cloud.tencent.com/document/product/436/54064"/>
    /// </summary>
    [XmlRoot("Response")]
    public sealed class AudioCensorResult
    {
        [XmlElement]
        public Detail JobsDetail;

        public sealed class Detail
        {
            [XmlElement]
            public string Code;

            [XmlElement]
            public string Message;

            [XmlElement]
            public string JobId;

            [XmlElement]
            public string State;

            [XmlElement]
            public string CreationTime;

            [XmlElement]
            public string Object;

            [XmlElement]
            public string Url;

            [XmlElement]
            public string Result;

            [XmlElement]
            public string AudioText;

            [XmlElement]
            public Info PornInfo;

            [XmlElement]
            public Info TerrorismInfo;

            [XmlElement]
            public Info PoliticsInfo;

            [XmlElement]
            public Info AdsInfo;

            [XmlElement]
            public List<SectionInfo> Section;     
        }       

        public sealed class Info
        {
            [XmlElement]
            public string HitFlag;

            [XmlElement]
            public string Score;

            [XmlElement]
            public string Label;
        }

        public sealed class SectionInfo
        {
            [XmlElement]
            public string Url;
            
            [XmlElement]
            public string Text;

            [XmlElement]
            public string OffsetTime;

            [XmlElement]
            public string Duration;

            [XmlElement]
            public SectionInfoDetail PornInfo;

            [XmlElement]
            public SectionInfoDetail TerrorismInfo;

            [XmlElement]
            public SectionInfoDetail PoliticsInfo;

            [XmlElement]
            public SectionInfoDetail AdsInfo;
        }

        public sealed class SectionInfoDetail
        {
            [XmlElement]
            public string HitFlag;

            [XmlElement]
            public string Score;

            [XmlElement]
            public string Keywords;
            
        }
    }

}

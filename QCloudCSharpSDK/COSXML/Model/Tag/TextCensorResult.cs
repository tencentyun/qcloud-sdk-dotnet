using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;

namespace COSXML.Model.Tag
{
    /// <summary>
    /// 文本审核结果
    /// <see href="https://cloud.tencent.com/document/product/436/56288"/>
    /// </summary>
    [XmlRoot("Response")]
    public sealed class TextCensorResult
    {
        [XmlElement]
        public Detail JobsDetail;

        [XmlElement]
        public string NonExistJobIds;

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
            public string SectionCount;

            [XmlElement]
            public string Result;

            [XmlElement]
            public Info PornInfo;

            [XmlElement]
            public Info TerrorismInfo;

            [XmlElement]
            public Info PoliticsInfo;

            [XmlElement]
            public Info AdsInfo;

            [XmlElement]
            public Info IllegalInfo;

            [XmlElement]
            public Info AbuseInfo;

            [XmlElement]
            public List<SectionInfo> Section;
        }       

        public sealed class Info
        {
            [XmlElement]
            public string HitFlag;

            [XmlElement]
            public string Count;
        }

        public sealed class SectionInfo
        {
            [XmlElement]
            public string StartByte;
            
            [XmlElement]
            public SectionInfoDetail PornInfo;

            [XmlElement]
            public SectionInfoDetail TerrorismInfo;

            [XmlElement]
            public SectionInfoDetail PoliticsInfo;

            [XmlElement]
            public SectionInfoDetail AdsInfo;

            [XmlElement]
            public SectionInfoDetail IllegalInfo;

            [XmlElement]
            public SectionInfoDetail AbuseInfo;
            
        }

        public sealed class SectionInfoDetail
        {
            [XmlElement]
            public string Code;

            [XmlElement]
            public string HitFlag;

            [XmlElement]
            public string Score;

            [XmlElement]
            public string Keywords;
            
        }
    }

}

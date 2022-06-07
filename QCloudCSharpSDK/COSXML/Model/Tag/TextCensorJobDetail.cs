using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;

namespace COSXML.Model.Tag
{
    /// <summary>
    /// 审核任务提交回执
    /// <see href="https://cloud.tencent.com/document/product/436/47316"/>
    /// </summary>
    [XmlRoot("Response")]
    public sealed class TextCensorJobsResponse
    {
        [XmlElement]
        public JobsDetailContent JobsDetail;

        public sealed class JobsDetailContent
        {
            [XmlElement]
            public string JobId;

            [XmlElement]
            public string State;

            [XmlElement]
            public string CreationTime;

            [XmlElement]
            public string Code;

            [XmlElement]
            public string DataId;

            [XmlElement]
            public string Message;

            [XmlElement]
            public string Content;

            [XmlElement]
            public string Label;

            [XmlElement]
            public string Result;

            [XmlElement]
            public string SectionCount;

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

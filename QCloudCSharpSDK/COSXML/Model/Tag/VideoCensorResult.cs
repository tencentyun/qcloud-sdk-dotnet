using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;

namespace COSXML.Model.Tag
{
    /// <summary>
    /// 视频审核结果
    /// <see href="https://cloud.tencent.com/document/product/436/47317"/>
    /// </summary>
    [XmlRoot("Response")]
    public sealed class VideoCensorResult
    {
        [XmlElement("JobsDetail")]
        public JobsDetail jobsDetail;

        [XmlElement]
        public string NonExistJobIds;

        public sealed class JobsDetail
        {
            [XmlElement]
            public string Code;

            [XmlElement]
            public string Message;

            [XmlElement]
            public string State;

            [XmlElement]
            public string CreationTime;

            [XmlElement]
            public string Object;

            [XmlElement]
            public string SnapshotCount;

            [XmlElement]
            public int Result;

            [XmlElement]
            public Info PornInfo;

            [XmlElement]
            public Info TerrorismInfo;

            [XmlElement]
            public Info PoliticsInfo;

            [XmlElement]
            public Info AdsInfo;

            [XmlElement]
            public List<SnapshotDetail> Snapshot;

            [XmlElement]
            public List<AudioSectionDetail> AudioSection;     
        }       

        public sealed class Info
        {
            [XmlElement]
            public int HitFlag;

            [XmlElement]
            public int Count;
        }

        public sealed class SnapshotDetail
        {
            [XmlElement]
            public string Url;

            [XmlElement]
            public int SnapshotTime;
            
            [XmlElement]
            public string Text;

            [XmlElement]
            public SnapshotSectionInfoDetail PornInfo;

            [XmlElement]
            public SnapshotSectionInfoDetail TerrorismInfo;

            [XmlElement]
            public SnapshotSectionInfoDetail PoliticsInfo;

            [XmlElement]
            public SnapshotSectionInfoDetail AdsInfo;
        }

        public sealed class AudioSectionDetail
        {
            [XmlElement]
            public string Url;
            
            [XmlElement]
            public string Text;

            [XmlElement]
            public int OffsetTime;

            [XmlElement]
            public int Duration;

            [XmlElement]
            public AudioSectionInfoDetail PornInfo;

            [XmlElement]
            public AudioSectionInfoDetail TerrorismInfo;

            [XmlElement]
            public AudioSectionInfoDetail PoliticsInfo;

            [XmlElement]
            public AudioSectionInfoDetail AdsInfo;
        }

        public sealed class SnapshotSectionInfoDetail
        {
            [XmlElement]
            public int HitFlag;

            [XmlElement]
            public int Score;

            [XmlElement]
            public string Label;

            [XmlElement]
            public string SubLabel;
            
        }

        public sealed class AudioSectionInfoDetail
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

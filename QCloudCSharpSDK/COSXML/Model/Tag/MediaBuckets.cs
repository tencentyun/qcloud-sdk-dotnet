using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;

namespace COSXML.Model.Tag
{
    /// <summary>
    /// 媒体bucket接口返回值
    /// <see href="https://cloud.tencent.com/document/product/436/48988"/>
    /// </summary>
    [XmlRoot("Response")]
    public sealed class MediaBuckets
    {
        [XmlElement]
        public string RequestId;

        [XmlElement]
        public int TotalCount;

        [XmlElement]
        public int PageNumber;

        [XmlElement]
        public int PageSize;

        [XmlElement]
        public List<MediaBucketListInfo> MediaBucketList;

        public sealed class MediaBucketListInfo 
        {
            [XmlElement]
            public string BucketId;

            [XmlElement]
            public string Name;

            [XmlElement]
            public string Region;

            [XmlElement]
            public string CreateTime;
        }

    }

}

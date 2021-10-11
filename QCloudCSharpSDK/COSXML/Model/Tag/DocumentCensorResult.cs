using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;

namespace COSXML.Model.Tag
{
    /// <summary>
    /// 文档审核结果
    /// <see href="https://cloud.tencent.com/document/product/436/59382"/>
    /// </summary>
    [XmlRoot("Response")]
    public sealed class DocumentCensorResult
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
            public string Suggestion;

            [XmlElement]
            public string CreationTime;

            [XmlElement]
            public string Url;

            [XmlElement]
            public string PageCount;

            [XmlElement]
            public LabelsInfo Labels;

            [XmlElement]
            public PageSegmentInfo PageSegment;

        }       

        public sealed class LabelsInfo
        {            
            [XmlElement]
            public LabelsInfoDetail PornInfo;

            [XmlElement]
            public LabelsInfoDetail TerrorismInfo;

            [XmlElement]
            public LabelsInfoDetail PoliticsInfo;

            [XmlElement]
            public LabelsInfoDetail AdsInfo;
        }

        public sealed class LabelsInfoDetail
        {
            [XmlElement]
            public string HitFlag;

            [XmlElement]
            public string Score;            
        }

        public sealed class PageSegmentInfo
        {
            [XmlElement]
            public ResultsInfo Results;
        }

        public sealed class ResultsInfo
        {
            [XmlElement]
            public string Url;

            [XmlElement]
            public string Text;

            [XmlElement]
            public string PageNumber;

            [XmlElement]
            public string SheetNumber;

            [XmlElement]
            public PageSegmentInfoDetail PornInfo;

            [XmlElement]
            public PageSegmentInfoDetail TerrorismInfo;

            [XmlElement]
            public PageSegmentInfoDetail PoliticsInfo;

            [XmlElement]
            public PageSegmentInfoDetail AdsInfo;
        }

        public sealed class PageSegmentInfoDetail
        {
            [XmlElement]
            public string HitFlag;

            [XmlElement]
            public string SubLabel;
            
            [XmlElement]
            public string Score;
            
            [XmlElement]
            public OcrResultsDetail OcrResults;
            
            [XmlElement]
            public ObjectResultsDetail ObjectResults;
                        
        }

        public sealed class OcrResultsDetail
        {
            [XmlElement]
            public string Text;

            [XmlElement]
            public string Keywords;
        }

        public sealed class ObjectResultsDetail
        {
            [XmlElement]
            public string Name;
        }
    }
}

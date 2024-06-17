using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace COSXML.Model.Tag
{
    /// <summary>
    /// 拉取符合条件的文档转码任务返回https://cloud.tencent.com/document/product/460/46944
    /// </summary>
    [XmlRoot("Response")]
    public sealed class ListDocProcessResponse
    {
        /// <summary>
        /// 任务的详细信息
        /// </summary>
        [XmlElement("JobsDetail")]
        // [XmlArrayItem("JobsDetail")]
        public List<DocProcessResponse.Detail> JobsDetail;
        [XmlElement]
        public String NextToken;
    }
}

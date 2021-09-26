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
    public sealed class CensorJobsResponse
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
        }
    }

}

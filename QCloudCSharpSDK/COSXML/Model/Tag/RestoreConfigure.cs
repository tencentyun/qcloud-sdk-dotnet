using System;
using System.Xml.Serialization;

using System.Text;
using COSXML.Common;
using COSXML.Utils;

namespace COSXML.Model.Tag
{
    [XmlRoot("RestoreRequest")]
    public sealed class RestoreConfigure
    {
        /// <summary>
        /// 设置临时副本的过期时间
        /// </summary>
        [XmlElement("Days")]
        public int days;

        /// <summary>
        /// 归档存储工作参数配置
        /// <see cref="CASJobParameters"/>
        /// </summary>
        [XmlElement("CASJobParameters")]
        public CASJobParameters casJobParameters;

        public sealed class CASJobParameters
        {
            /// <summary>
            /// 恢复数据时，Tier 可以指定为 CAS 支持的三种恢复类型，分别为 Expedited、Standard、Bulk
            /// <see cref="Tier"/>
            /// </summary>
            [XmlElement("Tier")]
            public Tier tier = Tier.Standard;
        }

        public enum Tier
        {
            [CosValue("Expedited")]
            Expedited = 0,

            [CosValue("Standard")]
            Standard,

            [CosValue("Bulk")]
            Bulk
        }
    }
}

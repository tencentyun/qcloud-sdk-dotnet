using System;
using System.Xml.Serialization;

namespace COSXML.Model.Tag
{
    /// <summary>
    /// 授予者信息
    /// </summary>
    [XmlRoot]
    public sealed class IntelligentTieringConfiguration
    {
        /// <summary>
        /// 说明智能分层存储配置是否开启，枚举值：Enabled
        /// </summary>
        [XmlElement]
        public string Status;

        [XmlElement]
        public IntelligentTieringTransition Transition;

        public sealed class IntelligentTieringTransition 
        {
            
            /// <summary>
            /// 智能分层存储配置中标准层数据转换为低频层数据的天数限制
            /// </summary>
            [XmlElement]
            public int Days;


            /// <summary>
            /// 指定智能分层存储配置中标准层数据转换为低频层数据的访问次数限制，默认值为0次
            /// </summary>
            public int RequestFrequent;
        }

        public Boolean IsEnabled()
        {

            return "Enabled".Equals(Status);
        }
    }
}

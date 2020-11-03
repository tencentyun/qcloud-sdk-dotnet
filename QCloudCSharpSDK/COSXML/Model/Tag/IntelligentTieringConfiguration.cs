using System;
using System.Collections.Generic;
using System.Text;

namespace COSXML.Model.Tag
{
    /// <summary>
    /// 授予者信息
    /// </summary>
    public sealed class IntelligentTieringConfiguration
    {
        /// <summary>
        /// 说明智能分层存储配置是否开启，枚举值：Enabled
        /// </summary>
        public string Status;

        /// <summary>
        /// 智能分层存储配置中标准层数据转换为低频层数据的天数限制
        /// </summary>
        public int Days;

        /// <summary>
        /// 指定智能分层存储配置中标准层数据转换为低频层数据的访问次数限制，默认值为0次
        /// </summary>
        public int RequestFrequent;

        public Boolean isEnabled() {
            return "Enabled".Equals(Status);
        }
    }
}

using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;
using COSXML.Utils;
/**
* Copyright (c) 2018 Tencent Cloud. All rights reserved.
* 11/5/2018 11:46:17 AM
* bradyxiao
*/
namespace COSXML.Model.Tag
{
    public sealed class RestoreConfigure
    {
        /// <summary>
        /// 设置临时副本的过期时间
        /// </summary>
        public int days;
        /// <summary>
        /// 归档存储工作参数配置
        /// <see cref="CASJobParameters"/>
        /// </summary>
        public CASJobParameters casJobParameters;

        public string GetInfo()
        {
            StringBuilder stringBuilder = new StringBuilder("{RestoreRequest:\n");
            stringBuilder.Append("Days:").Append(days).Append("\n");
            if(casJobParameters != null)stringBuilder.Append(casJobParameters.GetInfo()).Append("\n");
            stringBuilder.Append("}");
            return stringBuilder.ToString();
        }

        public sealed class CASJobParameters
        {
            /// <summary>
            /// 恢复数据时，Tier 可以指定为 CAS 支持的三种恢复类型，分别为 Expedited、Standard、Bulk
            /// <see cref="Tier"/>
            /// </summary>
            public Tier tier = Tier.Standard;

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{CASJobParameters:\n");
                stringBuilder.Append("Tier:").Append(EnumUtils.GetValue(tier)).Append("\n");
                stringBuilder.Append("}");
                return stringBuilder.ToString();
            }
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

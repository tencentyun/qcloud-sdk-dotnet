using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Utils;
/**
* Copyright (c) 2018 Tencent Cloud. All rights reserved.
* 11/1/2018 9:48:15 PM
* bradyxiao
*/
namespace COSXML.Common
{
    public enum CosRegion
    {
        /// <summary>
        /// 北京一区（华北）
        /// </summary>
        [CosValue("ap-beijing-1")]
        AP_Beijing_1 = 0,

        /// <summary>
        /// 北京
        /// </summary>
        [CosValue("ap-beijing")]
        AP_Beijing,

        /// <summary>
        /// 上海（华东）
        /// </summary>
        [CosValue("ap-shanghai")]
        AP_Shanghai,

        /// <summary>
        /// 广州（华南）
        /// </summary>
        [CosValue("ap-guangzhou")]
        AP_Guangzhou,

        /// <summary>
        /// 
        /// </summary>
        [CosValue("ap-guangzhou-2")]
        AP_Guangzhou_2,

        /// <summary>
        /// 成都（西南）
        /// </summary>
        [CosValue("ap-chengdu")]
        AP_Chengdu,

        /// <summary>
        /// 新加坡
        /// </summary>
        [CosValue("ap-singapore")]
        AP_Singapore,

        /// <summary>
        /// 香港
        /// </summary>
        [CosValue("ap-hongkong")]
        AP_Hongkong,

        /// <summary>
        /// 多伦多
        /// </summary>
        [CosValue("na-toronto")]
        NA_Toronto,

        /// <summary>
        /// 法兰克福
        /// </summary>
        [CosValue("eu-frankfurt")]
        EU_Frankfurt,

        [CosValue("ap-chongqing")]
        AP_Chongqing,

        /// <summary>
        /// 孟买
        /// </summary>
        [CosValue("ap-mumbai")]
        AP_Mumbai,

        /// <summary>
        /// 首尔
        /// </summary>
        [CosValue("ap-seoul")]
        AP_Seoul,

        /// <summary>
        /// 硅谷
        /// </summary>
        [CosValue("na-siliconvalley")]
        NA_Siliconvalley,

        /// <summary>
        /// 弗吉尼亚
        /// </summary>
        [CosValue("na-ashburn")]
        NA_Ashburn,

        /// <summary>
        /// 曼谷
        /// </summary>
        [CosValue("ap-bangkok")]
        AP_Bangkok,

        /// <summary>
        /// 莫斯科
        /// </summary>
        [CosValue("eu-moscow")]
        EU_Moscow,

        /// <summary>
        /// 东京
        /// </summary>
        [CosValue("ap-tokyo")]
        AO_Tokyo,
    }
}

using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;
using COSXML.Utils;
/**
* Copyright (c) 2018 Tencent Cloud. All rights reserved.
* 11/2/2018 1:58:05 PM
* bradyxiao
*/
namespace COSXML.Common
{
    public enum CosStorageClass
    {
        /// <summary>
        /// 标准储存
        /// </summary>
        [CosValue("Standard")]
        STANDARD = 0,

        /// <summary>
        /// 低频存储
        /// </summary>
        [CosValue("Standard_IA")]
        STANDARD_IA = 1,

        /// <summary>
        /// 归档储存
        /// </summary>
        [CosValue("ARCHIVE")]
        ARCHIVE
    }
}

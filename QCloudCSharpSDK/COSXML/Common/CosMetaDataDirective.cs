using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;
using COSXML.Utils;
/**
* Copyright (c) 2018 Tencent Cloud. All rights reserved.
* 11/2/2018 2:49:33 PM
* bradyxiao
*/
namespace COSXML.Common
{
    public enum CosMetaDataDirective
    {
        [CosValue("Copy")]
        COPY = 0,

        [CosValue("Replaced")]
        REPLACED
    }
}

using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;
using COSXML.Utils;

namespace COSXML.Common
{
    public enum CosMetaDataDirective
    {
        [CosValue("Copy")]
        Copy = 0,

        [CosValue("Replaced")]
        Replaced
    }
}

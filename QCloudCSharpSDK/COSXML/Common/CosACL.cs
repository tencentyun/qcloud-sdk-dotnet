using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Utils;

namespace COSXML.Common
{
    public enum CosACL
    {
        /// <summary>
        /// 私有读写
        /// </summary>
        [CosValue("private")]
        Private = 0,

        /// <summary>
        /// 私有写公有读
        /// </summary>
        [CosValue("public-read")]
        PublicRead,

        /// <summary>
        /// 公有读写
        /// </summary>
        [CosValue("public-read-write")]
        PublicReadWrite,
    }
}

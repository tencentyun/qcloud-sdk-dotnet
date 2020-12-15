using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;
using COSXML.Utils;

namespace COSXML.Common
{
    public enum CosGrantPermission
    {
        /// <summary>
        /// 只读权限
        /// </summary>
        [CosValue("READ")]
        Read = 0,

        /// <summary>
        /// 只写权限
        /// </summary>
        [CosValue("WRITE")]
        Write,

        /// <summary>
        /// 读写权限
        /// </summary>
        [CosValue("FULL_CONTROL")]
        FullControl
    }
}

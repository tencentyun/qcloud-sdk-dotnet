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
        READ = 0,

        /// <summary>
        /// 只写权限
        /// </summary>
        [CosValue("WRITE")]
        WRITE,

        /// <summary>
        /// 读写权限
        /// </summary>
        [CosValue("FULL_CONTROL")]
        FULL_CONTROL
    }
}

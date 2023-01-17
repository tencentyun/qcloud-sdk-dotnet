using System;
using System.IO;
using System.Collections.Generic;
using COSXML.Model.Tag;
using COSXML.Transfer;

namespace COSXML.Model.CI
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SubmitCensorJobResult : CosDataResult<CensorJobsResponse>
    {

        /// <summary>
        /// 图片审核结果
        /// </summary>
        /// <value></value>
        public CensorJobsResponse censorJobsResponse { 
            get {return _data; } 
        }
    }
}

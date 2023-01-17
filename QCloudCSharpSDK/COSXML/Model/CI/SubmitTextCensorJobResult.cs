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
    public sealed class SubmitTextCensorJobsResult : CosDataResult<TextCensorJobsResponse>
    {

        /// <summary>
        /// 文本审核结果
        /// </summary>
        /// <value></value>
        public TextCensorJobsResponse textCensorJobsResponse { 
            get {return _data; } 
        }
    }
}

using System;
using System.IO;
using System.Collections.Generic;
using COSXML.Model.Tag;
using COSXML.Transfer;

namespace COSXML.Model.CI
{
    /// <summary>
    /// 文本审核结果
    /// </summary>
    public sealed class GetTextCensorJobResult : CosDataResult<TextCensorJobResult>
    {
        /// <summary>
        /// 文本审核结果
        /// </summary>
        /// <value></value>
        public TextCensorJobResult resultStruct { 
            get {return _data; } 
        }
    }
}

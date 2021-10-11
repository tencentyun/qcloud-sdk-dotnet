using System;
using System.IO;
using System.Collections.Generic;
using COSXML.Model.Tag;
using COSXML.Transfer;

namespace COSXML.Model.CI
{
    /// <summary>
    /// 文档审核结果
    /// </summary>
    public sealed class GetDocumentCensorJobResult : CosDataResult<DocumentCensorResult>
    {
        /// <summary>
        /// 文档审核结果
        /// </summary>
        /// <value></value>
        public DocumentCensorResult resultStruct { 
            get {return _data; } 
        }
    }
}

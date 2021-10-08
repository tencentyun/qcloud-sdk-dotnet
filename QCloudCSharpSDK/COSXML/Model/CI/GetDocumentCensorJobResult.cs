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
    public sealed class GetDocumentCensorJobResult : CosDataResult<DocumentCensorJobResult>
    {
        /// <summary>
        /// 文档审核结果
        /// </summary>
        /// <value></value>
        public DocumentCensorJobResult resultStruct { 
            get {return _data; } 
        }
    }
}

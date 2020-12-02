using System;
using System.IO;
using System.Collections.Generic;
using COSXML.Model.Tag;
using COSXML.Transfer;

namespace COSXML.Model.CI
{
    /// <summary>
    /// 图片审核结果
    /// </summary>
    public sealed class SensitiveContentRecognitionResult : CosDataResult<SensitiveRecognitionResult>
    {

        /// <summary>
        /// 图片审核结果
        /// </summary>
        /// <value></value>
        public SensitiveRecognitionResult recognitionResult { get => _data;  }
    }
}

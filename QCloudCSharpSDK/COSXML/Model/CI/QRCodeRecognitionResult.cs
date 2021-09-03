using System;
using System.IO;
using System.Collections.Generic;
using COSXML.Model.Tag;
using COSXML.Transfer;

namespace COSXML.Model.CI
{
    /// <summary>
    /// 二维码识别结果
    /// </summary>
    public sealed class QRCodeRecognitionResult : CosDataResult<QRRecognitionResult>
    {

        /// <summary>
        /// 二维码识别结果
        /// </summary>
        /// <value></value>
        public QRRecognitionResult recognitionResult { 
            get {return _data; } 
        }
    }
}

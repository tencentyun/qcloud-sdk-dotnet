using System;
using System.IO;
using System.Collections.Generic;
using COSXML.Model.Tag;
using COSXML.Transfer;

namespace COSXML.Model.CI
{
    /// <summary>
    /// 图片处理结果
    /// </summary>
    public sealed class ImageProcessResult : CosResult
    {

        /// <summary>
        /// 图片处理结果
        /// </summary>
        /// <value></value>
        public PicOperationUploadResult uploadResult {get; private set;} 

        internal override void ParseResponseBody(Stream inputStream, string contentType, long contentLength)
        {
            uploadResult = XmlParse.ParsePicOpeartionResult(inputStream);
        }
    }
}

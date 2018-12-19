using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Model.Tag;
using COSXML.Transfer;

namespace COSXML.Model.Object
{
    /// <summary>
    /// 完成整个分块上传返回的结果
    /// <see cref="https://cloud.tencent.com/document/product/436/7742"/>
    /// </summary>
    public sealed class CompleteMultiUploadResult : CosResult
    {
        /// <summary>
        /// Complete返回信息
        /// <see cref="Model.Tag.CompleteMultipartUploadResult"/>
        /// </summary>
        public CompleteMultipartUploadResult completeMultipartUpload;

        internal override void ParseResponseBody(System.IO.Stream inputStream, string contentType, long contentLength)
        {
            completeMultipartUpload = new CompleteMultipartUploadResult();
            XmlParse.ParseCompleteMultipartUploadResult(inputStream, completeMultipartUpload);
        }

        public override string GetResultInfo()
        {
            return base.GetResultInfo() + (completeMultipartUpload == null ? "" : "\n" + completeMultipartUpload.GetInfo());
        }
    }
}

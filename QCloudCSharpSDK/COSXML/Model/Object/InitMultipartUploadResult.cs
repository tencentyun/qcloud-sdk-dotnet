using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Model.Tag;
using COSXML.Transfer;

namespace COSXML.Model.Object
{
    /// <summary>
    /// 实现初始化分片上传返回的结果
    /// <see cref="https://cloud.tencent.com/document/product/436/7746"/>
    /// </summary>
    public sealed class InitMultipartUploadResult : CosResult
    {
        /// <summary>
        /// 返回信息
        /// <see cref="Model.Tag.InitiateMultipartUpload"/>
        /// </summary>
        public InitiateMultipartUpload initMultipartUpload;

        internal override void ParseResponseBody(System.IO.Stream inputStream, string contentType, long contentLength)
        {
            initMultipartUpload = new InitiateMultipartUpload();
            XmlParse.ParseInitiateMultipartUpload(inputStream, initMultipartUpload);
        }

        public override string GetResultInfo()
        {
            return base.GetResultInfo() + (initMultipartUpload == null ? "" : "\n" + initMultipartUpload.GetInfo());
        }
    }
}

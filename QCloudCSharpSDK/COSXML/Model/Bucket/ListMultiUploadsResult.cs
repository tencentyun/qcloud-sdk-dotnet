using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Transfer;
using COSXML.Model.Tag;

namespace COSXML.Model.Bucket
{
    /// <summary>
    /// 查询 Bucket 正在进行中的分块上传返回的结果
    /// <see cref="https://cloud.tencent.com/document/product/436/7736"/>
    /// </summary>
    public sealed class ListMultiUploadsResult : CosResult
    {
        /// <summary>
        /// 所有分块上传的信息
        /// <see cref="COSXML.Model.Tag.ListMultipartUploads"/>
        /// </summary>
        public ListMultipartUploads listMultipartUploads;

        internal override void ParseResponseBody(System.IO.Stream inputStream, string contentType, long contentLength)
        {
            listMultipartUploads = new ListMultipartUploads();
            XmlParse.ParseListMultipartUploads(inputStream, listMultipartUploads);
        }

        public override string GetResultInfo()
        {
            return base.GetResultInfo() + (listMultipartUploads == null ? "" : "\n" + listMultipartUploads.GetInfo());
        }
    }
}

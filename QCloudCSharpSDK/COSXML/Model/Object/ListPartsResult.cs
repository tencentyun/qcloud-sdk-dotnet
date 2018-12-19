using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Model.Tag;
using COSXML.Transfer;

namespace COSXML.Model.Object
{
    /// <summary>
    /// 查询特定分块上传中的已上传的块返回的结果
    /// <see cref="https://cloud.tencent.com/document/product/436/7747"/>
    /// </summary>
    public sealed class ListPartsResult : CosResult
    {
        /// <summary>
        /// 已上传块的所有信息
        /// <see cref="Model.Tag.ListParts"/>
        /// </summary>
        public ListParts listParts;

        internal override void ParseResponseBody(System.IO.Stream inputStream, string contentType, long contentLength)
        {
            listParts = new ListParts();
            XmlParse.ParseListParts(inputStream, listParts);
        }

        public override string GetResultInfo()
        {
            return base.GetResultInfo() + (listParts == null ? "" : "\n" + listParts.GetInfo());
        }

    }
}

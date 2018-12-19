using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Model.Tag;
using COSXML.Transfer;

namespace COSXML.Model.Object
{
    /// <summary>
    /// 批量删除 Object返回的结果
    /// <see cref="https://cloud.tencent.com/document/product/436/8289"/>
    /// </summary>
    public sealed class DeleteMultiObjectResult : CosResult
    {
        /// <summary>
        /// 本次删除返回结果的方式和目标 Object
        /// <see cref="Model.Tag.DeleteResult"/>
        /// </summary>
        public DeleteResult deleteResult;

        internal override void ParseResponseBody(System.IO.Stream inputStream, string contentType, long contentLength)
        {
            deleteResult = new DeleteResult();
            XmlParse.ParseDeleteResult(inputStream, deleteResult);
        }

        public override string GetResultInfo()
        {
            return base.GetResultInfo() + (deleteResult == null ? "" : deleteResult.GetInfo());
        }
    }
}

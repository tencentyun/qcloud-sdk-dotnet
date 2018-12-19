using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Model.Tag;
using COSXML.Transfer;

namespace COSXML.Model.Object
{
    /// <summary>
    /// 复制对象返回的结果
    /// <see cref="https://cloud.tencent.com/document/product/436/10881"/>
    /// </summary>
    public sealed class CopyObjectResult : CosResult
    {
        /// <summary>
        /// 复制结果信息
        /// <see cref="Model.Tag.CopyObject"/>
        /// </summary>
        public CopyObject copyObject;

        internal override void ParseResponseBody(System.IO.Stream inputStream, string contentType, long contentLength)
        {
            copyObject = new CopyObject();
            XmlParse.ParseCopyObjectResult(inputStream, copyObject);
        }

        public override string GetResultInfo()
        {
            return base.GetResultInfo() + (copyObject == null ? "" : "\n" + copyObject.GetInfo());
        }

    }
}

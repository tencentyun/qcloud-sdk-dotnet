using System;
using System.Collections.Generic;

using System.Text;
/**
* Copyright (c) 2018 Tencent Cloud. All rights reserved.
* 11/5/2018 12:13:21 PM
* bradyxiao
*/
namespace COSXML.Model.Tag
{
    /// <summary>
    /// cos server 返回的错误信息
    /// <see cref="https://cloud.tencent.com/document/product/436/7730"/>
    /// </summary>
    public sealed class CosServerError
    {
        /// <summary>
        /// 错误码
        /// </summary>
        public string code;
        /// <summary>
        /// 错误信息
        /// </summary>
        public string message;
        /// <summary>
        /// 资源地址
        /// </summary>
        public string resource;
        /// <summary>
        /// 请求ID
        /// </summary>
        public string requestId;
        /// <summary>
        /// 错误ID
        /// </summary>
        public string traceId;

        public string GetInfo()
        {
            StringBuilder stringBuilder = new StringBuilder("{Error:\n");
            stringBuilder.Append("Code:").Append(code).Append("\n");
            stringBuilder.Append("Message:").Append(message).Append("\n");
            stringBuilder.Append("Rresource:").Append(resource).Append("\n");
            stringBuilder.Append("RequestId:").Append(requestId).Append("\n");
            stringBuilder.Append("TraceId:").Append(traceId).Append("\n");
            stringBuilder.Append("}");
            return stringBuilder.ToString();
        }
    }
}

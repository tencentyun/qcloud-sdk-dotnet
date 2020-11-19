using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace COSXML.CosException
{
    /// <summary>
    /// 客户端异常，通常是指没有收到服务器响应的异常，比如网络异常、本地 IO 异常等。
    /// </summary>
    [Serializable]
    public class CosClientException : System.ApplicationException
    {
        /// <summary>
        /// 错误码
        /// <see cref="Common.CosClientError"/>
        /// </summary>
        public int errorCode;

        public CosClientException(int errorCode, string message)
            : base(message)
        {
            this.errorCode = errorCode;
        }

        public CosClientException(int errorCode, string message, Exception cause)
            : base(message, cause)
        {
            this.errorCode = errorCode;
        }
    }
}

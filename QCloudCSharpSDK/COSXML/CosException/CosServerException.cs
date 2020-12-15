using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using COSXML.Model.Tag;

namespace COSXML.CosException
{
    /// <summary>
    /// 服务器异常，通常是一个非正确的服务器响应，例如权限错误、服务不可用等。
    /// <see cref="Model.Tag.CosServerError"/>
    /// </summary>
    [Serializable]
    public class CosServerException : System.ApplicationException
    {
        /// <summary>
        /// http status code
        /// </summary>
        public int statusCode;

        /// <summary>
        /// http status message
        /// </summary>
        public string statusMessage;

        /// <summary>
        /// cos server error code
        /// </summary>
        public string errorCode;

        /// <summary>
        /// cos server error message
        /// </summary>
        public string errorMessage;

        /// <summary>
        /// cos server requestId for tracking error
        /// </summary>
        public string requestId;

        /// <summary>
        /// cos server trace id
        /// </summary>
        public string traceId;

        /// <summary>
        /// cos server resuorce
        /// </summary>
        public string resource;

        public CosServerException(int statusCode, string statusMessage, CosServerError serverError)
            : this(statusCode, statusMessage)
        {

            if (serverError != null)
            {
                this.resource = serverError.resource;
                this.errorCode = serverError.code;
                this.errorMessage = serverError.message;
                this.requestId = serverError.requestId;
                this.traceId = serverError.traceId;
            }
        }

        public CosServerException(int statusCode, string statusMessage)
            : base($"CosServerException: {statusCode} - {statusMessage}")
        {
            this.statusCode = statusCode;
            this.statusMessage = statusMessage;
        }

        public void SetCosServerError(CosServerError serverError)
        {

            if (serverError != null)
            {
                this.resource = serverError.resource;
                this.errorCode = serverError.code;
                this.errorMessage = serverError.message;
                this.requestId = serverError.requestId;
                this.traceId = serverError.traceId;
            }
        }

        public string GetInfo()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("(")
                .Append("statusCode = ").Append(statusCode).Append(", statusMessage = ").Append(statusMessage)
                .Append(", errorCode = ").Append(errorCode).Append(", errorMessage = ").Append(errorMessage)
                .Append(", requestId = ").Append(requestId).Append(", traceId = ").Append(traceId)
                .Append(", resouce = ").Append(resource)
                .Append(")");

            return builder.ToString();
        }

    }
}

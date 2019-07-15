using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Network;
using System.IO;
/**
* Copyright (c) 2018 Tencent Cloud. All rights reserved.
* 11/2/2018 1:05:46 PM
* bradyxiao
*/
namespace COSXML.Model
{
    /**
     * this class for cos result.
     * 
     */
    public abstract class CosResult
    {
        /// <summary>
        /// http code
        /// </summary>
        public int httpCode;

        /// <summary>
        /// http message
        /// </summary>
        public string httpMessage;

        /// <summary>
        /// http response headers
        /// </summary>
        public Dictionary<string, List<string>> responseHeaders;

        /// <summary>
        /// exchange infor between request and result
        /// </summary>
        /// <param name="cosRequest"></param>
        internal virtual void ExternInfo(CosRequest cosRequest) { }

        /// <summary>
        /// parse status line and headers
        /// </summary>
        /// <param name="response"> <see cref="COSXML.Network.Response"/></param>
        internal virtual void InternalParseResponseHeaders() { }

        /// <summary>
        /// parse response body, such as download files.
        /// </summary>
        /// <param name="inputStream"> input stream </param>
        /// <param name="contentType"> body mime type</param>
        /// <param name="contentLength">body length</param>
        internal virtual void ParseResponseBody(Stream inputStream, string contentType, long contentLength) { }

        /// <summary>
        /// get result message
        /// </summary>
        /// <returns></returns>
        public virtual string GetResultInfo()
        {
            StringBuilder resultBuilder = new StringBuilder();
            resultBuilder.Append(httpCode).Append(" ").Append(httpMessage).Append("\n");
            if (responseHeaders != null)
            {
                foreach(KeyValuePair<string, List<string>> element in responseHeaders)
                {
                    resultBuilder.Append(element.Key).Append(": ").Append(element.Value[0]).Append("\n");
                }
            }

            return resultBuilder.ToString();
        }
    }
}

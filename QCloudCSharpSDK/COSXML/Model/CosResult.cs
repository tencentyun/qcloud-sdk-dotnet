using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Network;
using System.IO;
using COSXML.Transfer;

namespace COSXML.Model
{
    public class CosResult
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
        /// raw http response body
        /// </summary>
        public string RawContentBodyString { set; get; }

        /// <summary>
        /// check successful
        /// </summary>
        /// <returns></returns>
        public bool IsSuccessful()
        {
            return httpCode >= 200 && httpCode < 300;
        }

        /// <summary>
        /// exchange infor between request and result
        /// </summary>
        /// <param name="cosRequest"></param>
        internal virtual void ExternInfo(CosRequest cosRequest) 
        { 

        }

        /// <summary>
        /// parse status line and headers
        /// </summary>
        /// <param name="response"> <see cref="COSXML.Network.Response"/></param>
        internal virtual void InternalParseResponseHeaders() 
        { 

        }

        /// <summary>
        /// parse response body, such as download files.
        /// </summary>
        /// <param name="inputStream"> input stream </param>
        /// <param name="contentType"> body mime type</param>
        /// <param name="contentLength">body length</param>
        internal virtual void ParseResponseBody(Stream inputStream, string contentType, long contentLength) 
        {

        }

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

                foreach (KeyValuePair<string, List<string>> element in responseHeaders)
                {
                    resultBuilder.Append(element.Key).Append(": ").Append(element.Value[0]).Append("\n");
                }
            }

            return resultBuilder.ToString();
        }
    }

    public class CosDataResult<T> : CosResult
    {
        /// <summary>
        /// body数据
        /// </summary>
        protected T _data;

        internal override void ParseResponseBody(Stream inputStream, string contentType, long contentLength)
        {
            if (contentLength != 0)
            {
                _data = XmlParse.Deserialize<T>(inputStream);
            }
        }

        public override string GetResultInfo()
        {
            var info = base.GetResultInfo();
            var methodInfo = typeof(T).GetMethod("GetInfo");
            if (methodInfo != null && _data != null) 
            {
                info = info + "\n" + methodInfo.Invoke(_data, null);
            }
            
            return info;
        }
    }
}

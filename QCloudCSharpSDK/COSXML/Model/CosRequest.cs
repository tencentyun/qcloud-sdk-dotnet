using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;
using COSXML.Network;
using COSXML.Log;
using COSXML.Auth;
using COSXML.Utils;

namespace COSXML.Model
{
    public abstract class CosRequest
    {
        private static string TAG = typeof(CosRequest).FullName;

        protected Request realRequest;

        /// <summary>
        /// isHttps = true, https 请求; isHttps = false, http 请求； default isHttps = false.
        /// </summary>
        protected bool? isHttps = null;

        /// <summary>
        /// cos api 请求对应的 http method.
        /// </summary>
        protected string method = CosRequestMethod.GET;

        /// <summary>
        /// http 请求url中 path 部分.
        /// </summary>
        protected string path;

        /// <summary>
        /// http 请求url中 query 部分.
        /// </summary>
        protected Dictionary<string, string> queryParameters = new Dictionary<string, string>();

        /// <summary>
        /// http 请求 header 部分.
        /// </summary>
        protected Dictionary<string, string> headers = new Dictionary<string, string>();

        /// <summary>
        /// cos 服务的 appid.
        /// </summary>
        protected string appid;

        /// <summary>
        /// cos 服务签名的签名源部分.
        /// </summary>
        protected CosXmlSignSourceProvider cosXmlSignSourceProvider = new CosXmlSignSourceProvider();

        /// <summary>
        /// needMD5 = true, 请求中带上 Content-Md5; needMd5 = false, 请求中不带 Content-Md5; defalut needMd5 = false.
        /// </summary>
        protected bool needMD5 = true;

        /// <summary>
        /// 请求预签名URL
        /// </summary>
        protected string requestUrlWithSign = null;

        public CosXmlConfig serviceConfig { get; set; }

        /// <summary>
        /// http or https for cos request.
        /// </summary>
        public bool? IsHttps
        {
            get
            {
                return isHttps;
            }
            
            set { isHttps = value; }
        }

        /// <summary>
        /// http method
        /// </summary>
        public string Method
        {
            get
            {
                return method;
            }
            set { this.method = value; }
        }

        /// <summary>
        /// path of http url.
        /// </summary>
        public string RequestPath
        {
            get
            {
                return path;
            }
            
            private set { }
        }

        /// <summary>
        /// query of http url.
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<string, string> GetRequestParamters()
        {

            return queryParameters;
        }

        /// <summary>
        /// http request header
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<string, string> GetRequestHeaders()
        {

            return headers;
        }

        /// <summary>
        /// add query parameter for cos request, and cover the current value if it exists with the key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetQueryParameter(string key, string value)
        {
            SetQueryParameter(key, value, true);
        }

        /// <summary>
        /// url 部分都统一 url encode
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="isNeedUrlEncode"></param>
        public void SetQueryParameter(string key, string value, bool isNeedUrlEncode)
        {

            try
            {

                if (value == null)
                {
                    value = "";
                }

                if (isNeedUrlEncode)
                {
                    value = URLEncodeUtils.Encode(value);
                }

                queryParameters.Add(key, value);
            }
            catch (ArgumentException)
            {
                // cover the current value
                // cover the current value
                queryParameters[key] = value;
            }
        }

        /// <summary>
        /// add header for cos request, and cover the current value, if it exists with the key.
        /// </summary>
        /// <param name="key"> header: key </param>
        /// <param name="value"> header: value</param>
        public void SetRequestHeader(string key, string value)
        {
            SetRequestHeader(key, value, false);
        }

        /// <summary>
        /// add headers for cos request, and cover the current value, if it exists with the key.
        /// </summary>
        /// <param name="headers"></param>
        public void SetRequestHeaders(Dictionary<string, string> headers)
        {

            foreach (KeyValuePair<string, string> entry in headers)
            {
                SetRequestHeader(entry.Key, entry.Value);
            }
        }

        /// <summary>
        /// header 默认不 encode
        /// </summary>
        /// <param name="key">不能为null 即不包含空格,即 位于(\u0020, \u007F)，超过这个范围，urlencode</param>
        /// <param name="value">可以为null，为空，且位于(\u001f，\u007F) 和 '\t',超过这个范围，urlencode</param>
        /// <param name="isNeedUrlEncode"></param>
        public void SetRequestHeader(string key, string value, bool isNeedUrlEncode)
        {

            try
            {

                if (value == null)
                {
                    value = "";
                }

                if (isNeedUrlEncode)
                {
                    value = URLEncodeUtils.Encode(value);
                }

                headers.Add(key, value);
            }
            catch (ArgumentException)
            {
                // cover the current value
                // cover the current value
                headers[key] = value;
            }
        }

        /// <summary>
        /// set appid for cos.
        /// </summary>
        /// <param name="appid"> cos appid </param>
        public string APPID
        {
            get
            {
                return this.appid;
            }
            set { this.appid = value; }
        }

        /// <summary>
        /// 是否带上content-md5
        /// </summary>
        public bool IsNeedMD5
        {
            get
            {
                return needMD5;
            }
            set { needMD5 = value; }
        }

        /// <summary>
        /// return the host for cos request
        /// </summary>
        /// <returns>host(string)</returns>
        public abstract string GetHost();

        /// <summary>
        /// return the body for cos request. such as upload file.
        /// </summary>
        /// <returns> <see cref="COSXML.Network.RequestBody"/></returns>
        public abstract RequestBody GetRequestBody();

        /// <summary>
        /// 返回 xml 格式的 requestBody
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        protected Network.RequestBody GetXmlRequestBody(object d)
        {
            string content = Transfer.XmlBuilder.Serialize(d);
            byte[] data = Encoding.UTF8.GetBytes(content);
            ByteRequestBody body = new ByteRequestBody(data);
            return body;
        }


        /// <summary>    
        ///   check parameter for cos.
        /// </summary>
        /// <exception cref="COSXML.CosException.CosClientException"></exception>
        public abstract void CheckParameters();

        /// <summary>
        /// 设置签名的有效期： [signStartTimeSecond, signStartTimeSecond + durationSecond]
        /// </summary>
        /// <param name="signStartTimeSecond"></param>
        /// <param name="durationSecond"></param>
        public virtual void SetSign(long signStartTimeSecond, long durationSecond)
        {
            cosXmlSignSourceProvider.SetSignTime(signStartTimeSecond, durationSecond);
        }

        /// <summary>
        /// 计算签名时，带上头部header 和查询参数 query验证.
        /// 设置签名的有效期： [signStartTimeSecond, signStartTimeSecond + durationSecond]
        /// </summary>
        /// <param name="signStartTimeSecond"></param>
        /// <param name="durationSecond"></param>
        /// <param name="headerKeys"></param>
        /// <param name="queryParameterKeys"></param>
        public virtual void SetSign(long signStartTimeSecond, long durationSecond, List<string> headerKeys, List<string> queryParameterKeys)
        {
            cosXmlSignSourceProvider.SetSignTime(signStartTimeSecond, durationSecond);
            cosXmlSignSourceProvider.AddHeaderKeys(headerKeys);
            cosXmlSignSourceProvider.AddParameterKeys(queryParameterKeys);
        }

        /// <summary>
        /// 直接设置签名串.
        /// </summary>
        /// <param name="sign"></param>
        public virtual void SetSign(string sign)
        {
            SetRequestHeader(CosRequestHeaderKey.AUTHORIZAIION, sign);
        }

        /// <summary>
        /// 返回签名数据源
        /// </summary>
        /// <returns></returns>
        public virtual CosXmlSignSourceProvider GetSignSourceProvider()
        {
            // 默认签署的头部跟参数
            cosXmlSignSourceProvider.AddHeaderKeys(new List<string>() 
            {
                "content-type",
                "content-length",
                "content-md5",
                "host"
            });

            foreach (KeyValuePair<string, string> pair in headers)
            {
                if (pair.Key.StartsWith("x-cos-"))
                {
                    cosXmlSignSourceProvider.AddHeaderKey(pair.Key.ToLower());
                }
            }

            foreach (KeyValuePair<string, string> pair in queryParameters)
            {
                cosXmlSignSourceProvider.AddParameterKey(pair.Key.ToLower());
            }

            return cosXmlSignSourceProvider;
        }

        /// <summary>
        /// 设置预签名URL
        /// </summary>
        /// <param name="requestSignURL"></param>
        public string RequestURLWithSign
        {
            get
            {
                return requestUrlWithSign;
            }
            set { requestUrlWithSign = value; }
        }

        public void BindRequest(Request request)
        {
            this.realRequest = request;
        }

        public void Cancel()
        {

            if (realRequest != null)
            {
                realRequest.Cancel();
            }
        }
    }
}

using System;
using System.Collections.Generic;

using System.Text;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using COSXML.Log;
using System.Reflection;
using System.IO;
using System.Net.Cache;


namespace COSXML.Network
{
    /// <summary>
    /// network request and response.
    /// <list type="bullet">
    /// <item><term>type1: command request</term></item>
    /// <item><term>type2: upload file</term></item>
    /// <item><term>type3: download file</term></item>
    /// </list>
    /// </summary>
    public sealed class CommandTask
    {
        public const string TAG = "CommandTask";

        /// <summary>
        /// init connectionLimit and statueCode = 100 action
        /// </summary>
        /// <param name="config"></param>
        public static void Init(HttpClientConfig config)
        {
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.DefaultConnectionLimit = config.ConnectionLimit;
        }

        /// <summary>
        /// sync excute
        /// </summary>
        /// <param name="request"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static void Excute(Request request, Response response, HttpClientConfig config)
        {
            HttpWebRequest httpWebRequest = null;
            HttpWebResponse httpWebResponse = null;

            try
            {
                //step1: create HttpWebRequest by request.url
                httpWebRequest = HttpWebRequest.Create(request.RequestUrlString) as HttpWebRequest;

                httpWebRequest.AllowWriteStreamBuffering = false;

                //bind webRequest
                request.BindHttpWebRequest(httpWebRequest);

                // handler request
                HandleHttpWebRequest(httpWebRequest, request, config);

                //final: get response
                httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;

                //notify has been got response
                request.onNotifyGetResponse();

                //handle response for [100, 300)
                HandleHttpWebResponse(httpWebResponse, response);
            }
            catch (WebException webEx)
            {

                if (webEx.Response != null && webEx.Response is HttpWebResponse)
                {
                    //notify has been got response

                    request.onNotifyGetResponse();

                    httpWebResponse = (HttpWebResponse)webEx.Response;
                    //handle response for [400, 500]
                    HandleHttpWebResponse(httpWebResponse, response);
                }
                else
                {
                    //QLog.E(TAG, webEx.Message, webEx);
                    throw;
                }

            }
            catch (Exception ex)
            {
                //QLog.E(TAG, ex.Message, ex);
                throw;
            }
            
            finally
            {

                if (httpWebResponse != null)
                {
                    // print log
                    PrintResponseInfo(httpWebResponse);
                    httpWebResponse.Close();
                    //QLog.D("XIAO", "response close");
                }

                if (httpWebRequest != null)
                {
                    httpWebRequest.Abort();
                    //QLog.D("XIAO", "request close");
                }

                QLog.Debug(TAG, "close");
            }

        }

        /// <summary>
        /// handle request
        /// </summary>
        /// <param name="httpWebRequest"></param>
        /// <param name="request"></param>
        /// <param name="config"></param>
        private static void HandleHttpWebRequest(HttpWebRequest httpWebRequest, Request request, HttpClientConfig config)
        {
            HandleHttpWebRequestHeaders(request, httpWebRequest, config);
            //setp5: send request content: body
            if (request.Body != null)
            {
                httpWebRequest.ContentLength = request.Body.ContentLength;
                request.Body.OnWrite(httpWebRequest.GetRequestStream());
            }
            //print request start log
            PrintReqeustInfo(httpWebRequest);
        }

        /// <summary>
        /// handle response
        /// </summary>
        /// <param name="httpWebResponse"></param>
        /// <param name="response"></param>
        private static void HandleHttpWebResponse(HttpWebResponse httpWebResponse, Response response)
        {
            HandleHttpWebResponseHeaders(response, httpWebResponse);

            //handle body
            response.Body.HandleResponseBody(httpWebResponse.GetResponseStream());

            response.OnFinish(response.Code >= 200 && response.Code < 300, null);

            // close 
            //httpWebResponse.Close();
        }


        /// <summary>
        /// async to excute 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <param name="config"></param>
        public static void Schedue(Request request, Response response, HttpClientConfig config)
        {
            HttpWebRequest httpWebRequest = null;
            RequestState requestState = new RequestState();

            try
            {
                requestState.request = request;

                requestState.response = response;

                httpWebRequest = WebRequest.Create(request.RequestUrlString) as HttpWebRequest;

                httpWebRequest.AllowWriteStreamBuffering = false;

                //bind webRequest
                request.BindHttpWebRequest(httpWebRequest);

                //handle request header
                HandleHttpWebRequestHeaders(request, httpWebRequest, config);

                requestState.httpWebRequest = httpWebRequest;

                //handle request body
                if (request.Body != null)
                {
                    httpWebRequest.ContentLength = request.Body.ContentLength;
                    httpWebRequest.BeginGetRequestStream(new AsyncCallback(AsyncRequestCallback), requestState);
                }
                else
                {
                    //wait for response
                    httpWebRequest.BeginGetResponse(new AsyncCallback(AsyncResponseCallback), requestState);
                }

                //print log
                PrintReqeustInfo(httpWebRequest);
            }
            catch (WebException webEx)
            {
                response.OnFinish(false, webEx);
                //abort
                requestState.Clear();
                QLog.Debug(TAG, webEx.Message, webEx);
            }
            catch (Exception ex)
            {
                response.OnFinish(false, ex);
                //abort
                requestState.Clear();
                QLog.Error(TAG, ex.Message, ex);
            }

        }

        public static void AsyncRequestCallback(IAsyncResult ar)
        {
            RequestState requestState = ar.AsyncState as RequestState;
            Stream requestStream = null;

            try
            {
                HttpWebRequest httpWebRequest = requestState.httpWebRequest;

                requestStream = httpWebRequest.EndGetRequestStream(ar);

                ////开始写入数据
                //requestState.request.Body.OnWrite(requestStream);

                ////wait for response
                //httpWebRequest.BeginGetResponse(AsyncResponseCallback, requestState);

                requestState.request.Body.StartHandleRequestBody(requestStream, delegate (Exception exception)
                {

                    if (exception != null)
                    {
                        // handle request body throw exception
                        requestState.response.OnFinish(false, exception);
                        //abort
                        requestState.Clear();
                        QLog.Error(TAG, exception.Message, exception);
                    }
                    else
                    {
                        //wait for response
                        httpWebRequest.BeginGetResponse(new AsyncCallback(AsyncResponseCallback), requestState);
                    }
                });

            }
            catch (Exception ex)
            {
                requestState.response.OnFinish(false, ex);
                //abort
                requestState.Clear();
                QLog.Error(TAG, ex.Message, ex);
            }

        }

        public static void AsyncResponseCallback(IAsyncResult ar)
        {
            RequestState requestState = ar.AsyncState as RequestState;
            HttpWebResponse httpWebResponse = null;

            try
            {
                HttpWebRequest httpWebRequest = requestState.httpWebRequest;

                httpWebResponse = (HttpWebResponse)httpWebRequest.EndGetResponse(ar);

                //nofity get response
                requestState.request.onNotifyGetResponse();

                requestState.httpWebResponse = httpWebResponse;
                //handle response headers
                HandleHttpWebResponseHeaders(requestState.response, httpWebResponse);

                Stream responseStream = httpWebResponse.GetResponseStream();


                requestState.response.Body.StartHandleResponseBody(responseStream, delegate (bool isSuccess, Exception ex)
                    {
                        PrintResponseInfo(httpWebResponse);
                        requestState.response.OnFinish(isSuccess, ex);
                        requestState.Clear();
                    });
            }
            catch (WebException webEx)
            {

                if (webEx.Response != null && webEx.Response is HttpWebResponse)
                {
                    //nofity get response
                    requestState.request.onNotifyGetResponse();

                    //handle response for [400, 500]
                    httpWebResponse = (HttpWebResponse)webEx.Response;

                    requestState.httpWebResponse = httpWebResponse;

                    //handle response headers
                    HandleHttpWebResponseHeaders(requestState.response, httpWebResponse);

                    Stream responseStream = httpWebResponse.GetResponseStream();


                    requestState.response.Body.StartHandleResponseBody(responseStream, delegate (bool isSuccess, Exception ex)
                    {
                        PrintResponseInfo(httpWebResponse);
                        requestState.response.OnFinish(isSuccess, ex);
                        requestState.Clear();
                    });
                }
                else
                {
                    requestState.response.OnFinish(false, webEx);
                    //abort
                    requestState.Clear();
                    QLog.Error(TAG, webEx.Message, webEx);
                }

            }
            catch (Exception ex)
            {
                requestState.response.OnFinish(false, ex);
                //abort
                requestState.Clear();
                QLog.Error(TAG, ex.Message, ex);
            }
        }

        /// <summary>
        /// handle request headers
        /// </summary>
        /// <param name="request"></param>
        /// <param name="httpWebRequest"></param>
        /// <param name="config"></param>
        private static void HandleHttpWebRequestHeaders(Request request, HttpWebRequest httpWebRequest, HttpClientConfig config)
        {
            // set connect timeout
            httpWebRequest.Timeout = config.ConnectionTimeoutMs;
            //set read write timeout
            httpWebRequest.ReadWriteTimeout = config.ReadWriteTimeoutMs;

            // set request method
            httpWebRequest.Method = request.Method.ToUpperInvariant();
            // set user-agent
            httpWebRequest.UserAgent = request.UserAgent;
            //set host, net2.0 cannot set;

            // set allow auto redirect
            httpWebRequest.AllowAutoRedirect = config.AllowAutoRedirect;

            // notice: it is not allowed to set common headers with the WebHeaderCollection.Accept
            // such as: Connection,Content-Length,Content-Type,Date,Expect. Host,If-Modified-Since,Range, Referer,Transfer-Encoding,User-Agent,Proxy-Connection
            //step2: set header and connection properity by request.heders
            foreach (KeyValuePair<string, string> pair in request.Headers)
            {
                HttpHeaderHandle.AddHeader(httpWebRequest.Headers, pair.Key, pair.Value);
            }

            //step3: set proxy, default proxy = null, improte performation
            SetRequestProxy(httpWebRequest, config);

            //step4: https, default all true for "*.myqcloud.com"
            if (request.IsHttps)
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationCertificate);
            }
            //初始化长度
            httpWebRequest.ContentLength = 0L;
        }

        /// <summary>
        /// headle response headers
        /// </summary>
        /// <param name="response"></param>
        /// <param name="httpWebResponse"></param>
        private static void HandleHttpWebResponseHeaders(Response response, HttpWebResponse httpWebResponse)
        {
            response.Code = (int)httpWebResponse.StatusCode;
            response.Message = httpWebResponse.StatusDescription;

            WebHeaderCollection headers = httpWebResponse.Headers;

            // Transfer-Encoding: chunked
            bool isChunked = false; 
            if (headers != null)
            {
                Dictionary<string, List<string>> result = new Dictionary<string, List<string>>(headers.Count);

                for (int i = 0; i < headers.Count; i++)
                {
                    List<string> values = null;
                    string key = headers.GetKey(i);

                    if (headers.GetValues(i) != null)
                    {
                        values = new List<string>();

                        foreach (string value in headers.GetValues(i))
                        {
                            values.Add(value);
                        }
                    }

                    result.Add(key, values);

                    if ("Transfer-Encoding".EndsWith(key, StringComparison.OrdinalIgnoreCase) && values.Contains("chunked")) 
                    {
                        isChunked = true;
                    }
                }

                response.Headers = result;
            }

            if (!isChunked) 
            {
                response.ContentLength = httpWebResponse.ContentLength;
            }
            response.ContentType = httpWebResponse.ContentType;

            if (response.Body != null)
            {
                if (!isChunked)
                {
                    response.Body.ContentLength = httpWebResponse.ContentLength;
                }
                response.Body.ContentType = httpWebResponse.ContentType;
            }

            //handle header
            response.HandleResponseHeader();
        }

        /// <summary>
        /// set proxy
        /// </summary>
        /// <param name="httpWebRequest"></param>
        /// <param name="config"></param>
        private static void SetRequestProxy(HttpWebRequest httpWebRequest, HttpClientConfig config)
        {
            httpWebRequest.Proxy = null;

            if (!String.IsNullOrEmpty(config.ProxyHost))
            {

                if (config.ProxyPort < 0)
                {
                    httpWebRequest.Proxy = new WebProxy(config.ProxyHost);
                }
                else
                {
                    httpWebRequest.Proxy = new WebProxy(config.ProxyHost, config.ProxyPort);
                }

                if (!String.IsNullOrEmpty(config.ProxyUserName))
                {
                    httpWebRequest.Proxy.Credentials = String.IsNullOrEmpty(config.ProxyDomain) ?
                        new NetworkCredential(config.ProxyUserName, config.ProxyUserPassword ?? String.Empty) :
                        new NetworkCredential(config.ProxyUserName, config.ProxyUserPassword ?? String.Empty,
                            config.ProxyDomain);
                }
                // 代理验证
                // 代理验证
                httpWebRequest.PreAuthenticate = true;
            }
        }

        /// <summary>
        /// check certificate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        private static bool CheckValidationCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {

            return true;
        }

        /// <summary>
        /// print request info
        /// </summary>
        /// <param name="httpWebRequest"></param>
        private static void PrintReqeustInfo(HttpWebRequest httpWebRequest)
        {
            StringBuilder requestLog = new StringBuilder("--->");

            requestLog.Append(httpWebRequest.Method).Append(' ').Append(httpWebRequest.Address.AbsoluteUri).Append('\n');
            int count = httpWebRequest.Headers.Count;

            for (int i = 0; i < count; i++)
            {
                requestLog.Append(httpWebRequest.Headers.GetKey(i)).Append(":").Append(httpWebRequest.Headers.GetValues(i)[0]).Append('\n');
            }

            requestLog.Append("allow auto redirect: " + httpWebRequest.AllowAutoRedirect).Append('\n');
            requestLog.Append("connect timeout: " + httpWebRequest.Timeout).Append('\n');
            requestLog.Append("read write timeout: " + httpWebRequest.ReadWriteTimeout).Append('\n');
            requestLog.Append("AllowWriteStreamBuffering: " + httpWebRequest.AllowWriteStreamBuffering).Append('\n');
            //requestLog.Append("proxy: " + (httpWebRequest.Proxy == null ? "null" : ((WebProxy)httpWebRequest.Proxy).Address.ToString()));
            requestLog.Append("<---");
            QLog.Debug(TAG, requestLog.ToString());
        }

        /// <summary>
        /// print response info
        /// </summary>
        /// <param name="httpWebResponse"></param>
        private static void PrintResponseInfo(HttpWebResponse httpWebResponse)
        {
            StringBuilder responseLog = new StringBuilder("<---");

            responseLog.Append(httpWebResponse.Method).Append(' ').Append(httpWebResponse.ResponseUri.AbsoluteUri).Append('\n');
            responseLog.Append((int)httpWebResponse.StatusCode).Append(' ').Append(httpWebResponse.StatusDescription).Append('\n');
            int count = httpWebResponse.Headers.Count;

            for (int i = 0; i < count; i++)
            {
                responseLog.Append(httpWebResponse.Headers.GetKey(i)).Append(":").Append(httpWebResponse.Headers.GetValues(i)[0]).Append('\n');
            }

            responseLog.Append("<---");
            QLog.Debug(TAG, responseLog.ToString());
        }

        internal static class HttpHeaderHandle
        {
            private static MethodInfo addHeaderMethod;

            private static readonly ICollection<PlatformID> monoPlatforms = new List<PlatformID> 
            {
                 PlatformID.MacOSX, PlatformID.Unix 
            };

            private static bool? isMonoPlatform;

            internal static void AddHeader(WebHeaderCollection webHeaderCollection, string key, string value)
            {

                if (isMonoPlatform == null)
                {
                    isMonoPlatform = monoPlatforms.Contains(Environment.OSVersion.Platform);
                }
                // HTTP headers should be encoded to iso-8859-1,
                // however it will be encoded automatically by HttpWebRequest in mono.
                if (false == isMonoPlatform)
                {
                    // Encode headers for win platforms.

                }

                if (addHeaderMethod == null)
                {
                    // Specify the internal method name for adding headers
                    // mono: AddWithoutValidate
                    // win: AddInternal
                    //var internalMethodName = (isMonoPlatform == false) ? "AddWithoutValidate" : "AddInternal";
                    var internalMethodName = "AddWithoutValidate";

                    QLog.Debug(TAG, internalMethodName.ToString());
                    var method = typeof(WebHeaderCollection).GetMethod(
                        internalMethodName,
                        BindingFlags.NonPublic | BindingFlags.Instance,
                        null,
                        new Type[] { typeof(string), typeof(string) },
                        null);

                    addHeaderMethod = method;
                }

                addHeaderMethod.Invoke(webHeaderCollection, new Object[] { key, value });
            }

        }

        internal class RequestState
        {
            public HttpWebRequest httpWebRequest;

            public HttpWebResponse httpWebResponse;

            public Response response;

            public Request request;

            public RequestState()
            {
                httpWebRequest = null;
                httpWebResponse = null;
                response = null;
                request = null;
            }

            public void Clear()
            {

                if (httpWebRequest != null)
                {
                    httpWebRequest.Abort();
                }

                if (httpWebResponse != null)
                {
                    httpWebResponse.Close();
                }

                QLog.Debug(TAG, "Close");
            }
        }
    }
}

using System;
using System.Collections.Generic;

using System.Text;
using System.Net;
using COSXML.Network;
using COSXML.Model;
using COSXML.Common;
using COSXML.Auth;
using COSXML.CosException;
using COSXML.Transfer;
using COSXML.Model.Tag;
using COSXML.Log;
using System.IO;
using COSXML.Model.Object;
using COSXML.Utils;

namespace COSXML.Network
{
    /// <summary>
    /// input: CosRequest; output: CosResponse
    /// </summary>
    public sealed class HttpClient
    {
        private static string TAG = "HttpClient";

        private HttpClientConfig config;

        private static HttpClient instance;

        private Object sync = new Object();

        private static Object syncInstance = new Object();

        private const int MAX_ACTIVIE_TASKS = 5;

        private volatile int activieTasks = 0;

        public static HttpClient GetInstance()
        {
            lock (syncInstance)
            {

                if (instance == null)
                {
                    instance = new HttpClient();
                }
            }

            return instance;
        }

        public void Init(HttpClientConfig config)
        {
            if (this.config == null)
            {
                lock (sync)
                {
                    if (this.config == null)
                    {
                        this.config = config;
                        // init grobal httpwebreqeust
                        CommandTask.Init(this.config);
                    }
                }
            }

        }

        private HttpClient()
        {
        }

        /// <summary>
        /// excute request
        /// </summary>
        /// <param name="cosRequest"></param>
        /// <param name="cosResult"></param>
        /// <param name="credentialProvider"></param>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        public void Excute(CosRequest cosRequest, CosResult cosResult, QCloudCredentialProvider credentialProvider)
        {
            //HttpTask httpTask = new HttpTask();
            //httpTask.cosRequest = cosRequest;
            //httpTask.cosResult = cosResult;
            //httpTask.isSchedue = false;
            InternalExcute(cosRequest, cosResult, credentialProvider);
        }


        public void Schedue(CosRequest cosRequest, CosResult cosResult, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback, QCloudCredentialProvider credentialProvider)
        {
            //HttpTask httpTask = new HttpTask();
            //httpTask.cosRequest = cosRequest;
            //httpTask.cosResult = cosResult;
            //httpTask.isSchedue = true;
            //httpTask.successCallback = successCallback;
            //httpTask.failCallback = failCallback;
            InternalSchedue(cosRequest, cosResult, successCallback, failCallback, credentialProvider);
        }


        /// <summary>
        /// excute request
        /// </summary>
        /// <param name="cosRequest"></param>
        /// <param name="cosResult"></param>
        /// <param name="credentialProvider"></param>
        /// <exception cref="COSXML.CosException.CosClientException">CosClientException</exception>
        /// <exception cref="COSXML.CosException.CosServerException">CosServerException</exception>
        public void InternalExcute(CosRequest cosRequest, CosResult cosResult, QCloudCredentialProvider credentialProvider)
        {

            try
            {
                Request request = CreateRequest(cosRequest, credentialProvider);
                //extern informations exchange
                cosResult.ExternInfo(cosRequest);

                Response response;

                if (cosRequest is GetObjectRequest)
                {
                    GetObjectRequest getObjectRequest = cosRequest as GetObjectRequest;

                    response = new CosResponse(cosResult, getObjectRequest.GetSaveFilePath(), getObjectRequest.GetLocalFileOffset(),
                        getObjectRequest.GetCosProgressCallback());
                }
                else
                {
                    response = new CosResponse(cosResult, null, -1L, null);
                }

                cosRequest.BindRequest(request);
                CommandTask.Excute(request, response, config);
            }
            catch (CosServerException)
            {
                throw;
            }
            catch (CosClientException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new CosClientException((int)CosClientError.BadRequest, ex.Message, ex);
            }

        }

        // public void Execute(Request request, Response response)
        // {

        //     try
        //     {
        //         CommandTask.Excute(request, response, config);
        //     }
        //     catch (CosServerException)
        //     {
        //         throw;
        //     }
        //     catch (CosClientException)
        //     {
        //         throw;
        //     }
        //     catch (Exception ex)
        //     {
        //         throw new CosClientException((int)CosClientError.BadRequest, ex.Message, ex);
        //     }
        // }

        public void InternalSchedue(CosRequest cosRequest, CosResult cosResult, COSXML.Callback.OnSuccessCallback<CosResult> successCallback, COSXML.Callback.OnFailedCallback failCallback, QCloudCredentialProvider credentialProvider)
        {

            try
            {
                Request request = CreateRequest(cosRequest, credentialProvider);
                Response response;

                if (cosRequest is GetObjectRequest)
                {
                    GetObjectRequest getObjectRequest = cosRequest as GetObjectRequest;

                    response = new CosResponse(cosResult, getObjectRequest.GetSaveFilePath(), getObjectRequest.GetLocalFileOffset(),
                        getObjectRequest.GetCosProgressCallback(), successCallback, failCallback);
                }
                else
                {
                    response = new CosResponse(cosResult, null, -1L, null, successCallback, failCallback);
                }

                cosRequest.BindRequest(request);
                CommandTask.Schedue(request, response, config);
            }
            catch (CosServerException serverException)
            {
                //throw serverException;
                failCallback(null, serverException);
            }
            catch (CosClientException clientException)
            {
                //throw clientException;
                failCallback(clientException, null);
            }
            catch (Exception ex)
            {
                //throw new CosClientException((int)CosClientError.BAD_REQUEST, ex.Message, ex);
                failCallback(new CosClientException((int)CosClientError.BadRequest, ex.Message, ex), null);
            }
        }

        private Request CreateRequest(CosRequest cosRequest, QCloudCredentialProvider credentialProvider)
        {
            cosRequest.CheckParameters();
            string requestUrlWithSign = cosRequest.RequestURLWithSign;
            Request request = new Request();

            request.Method = cosRequest.Method;

            if (requestUrlWithSign != null)
            {

                if (requestUrlWithSign.StartsWith("https"))
                {
                    request.IsHttps = true;
                }
                else
                {
                    request.IsHttps = false;
                }

                request.RequestUrlString = requestUrlWithSign;
            }
            else
            {
                request.IsHttps = (bool)cosRequest.IsHttps;
                request.Url = CreateUrl(cosRequest);
                request.Host = cosRequest.GetHost();
            }

            request.UserAgent = config.UserAgnet;
            Dictionary<string, string> headers = cosRequest.GetRequestHeaders();

            if (headers != null)
            {

                foreach (KeyValuePair<string, string> pair in headers)
                {
                    request.AddHeader(pair.Key, pair.Value);
                }
            }

            request.Body = cosRequest.GetRequestBody();

            // cacluate md5
            if (CheckNeedMd5(request, cosRequest.IsNeedMD5) && request.Body != null)
            {
                request.AddHeader(CosRequestHeaderKey.CONTENT_MD5, request.Body.GetMD5());
            }
            // content type header
            if (request.Body != null && request.Body.ContentType != null &&
                    !request.Headers.ContainsKey(CosRequestHeaderKey.CONTENT_TYPE))
            {
                request.AddHeader(CosRequestHeaderKey.CONTENT_TYPE, request.Body.ContentType);
            }

            //cacluate sign, and add it.
            if (requestUrlWithSign == null)
            {
                CheckSign(cosRequest.GetSignSourceProvider(), request, credentialProvider);
            }

            return request;
        }

        private HttpUrl CreateUrl(CosRequest cosRequest)
        {
            HttpUrl httpUrl = new HttpUrl();

            httpUrl.Scheme = (bool)cosRequest.IsHttps ? "https" : "http";
            httpUrl.Host = cosRequest.GetHost();
            httpUrl.Path = URLEncodeUtils.EncodePathOfURL(cosRequest.RequestPath);
            httpUrl.SetQueryParameters(cosRequest.GetRequestParamters());

            return httpUrl;
        }

        /// <summary>
        /// add authorization
        /// </summary>
        /// <param name="qcloudSignSource">QCloudSignSource</param>
        /// <param name="request"></param>
        /// <param name="credentialProvider"></param>
        private void CheckSign(IQCloudSignSource qcloudSignSource, Request request, QCloudCredentialProvider credentialProvider)
        {
            // has authorizaiton, notice: using request.Headers， otherwise, error
            if (request.Headers.ContainsKey(CosRequestHeaderKey.AUTHORIZAIION))
            {
                QLog.Debug(TAG, "has add authorizaiton in headers");

                return;
            }

            //has no authorization, but signSourceProvider == null
            if (qcloudSignSource == null)
            {
                QLog.Debug(TAG, "signSourceProvider == null");

                return;
            }

            if (credentialProvider == null)
            {
                throw new ArgumentNullException("credentialsProvider == null");
            }

            CosXmlSigner signer = new CosXmlSigner();

            signer.Sign(request, qcloudSignSource, credentialProvider.GetQCloudCredentialsCompat(request));
        }

        private bool CheckNeedMd5(Request request, bool isNeedMd5)
        {
            bool result = isNeedMd5;

            if (request.Headers.ContainsKey(CosRequestHeaderKey.CONTENT_MD5))
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// cos response
        /// 分为两类：
        /// 一类下载文件
        /// 一类直接读取数据
        /// </summary>
        private class CosResponse : Response
        {
            private CosResult cosResult;

            private COSXML.Callback.OnSuccessCallback<CosResult> successCallback;

            private COSXML.Callback.OnFailedCallback faileCallback;

            private const int MAX_BUFFER_SIZE = 4096;

            public CosResponse(CosResult cosResult, string saveFilePath, long saveFileOffset, COSXML.Callback.OnProgressCallback downloadProgressCallback)
            {
                this.cosResult = cosResult;

                if (saveFilePath != null)
                {
                    this.Body = new ResponseBody(saveFilePath, saveFileOffset);
                    this.Body.ProgressCallback = downloadProgressCallback;
                }
                else
                {
                    this.Body = new ResponseBody();
                }

            }

            public CosResponse(CosResult cosResult, string saveFilePath, long saveFileOffset, COSXML.Callback.OnProgressCallback downloadProgressCallback,
                COSXML.Callback.OnSuccessCallback<CosResult> successCallback,
                COSXML.Callback.OnFailedCallback failCallback) : this(cosResult, saveFilePath, saveFileOffset, downloadProgressCallback)
            {
                this.successCallback = successCallback;
                this.faileCallback = failCallback;
            }

            /// <summary>
            /// response has been obtain, and parse headers from response
            /// </summary>
            public override void HandleResponseHeader()
            {
                cosResult.httpCode = Code;
                cosResult.httpMessage = Message;
                cosResult.responseHeaders = Headers;
                cosResult.InternalParseResponseHeaders();

                if (Code >= 300)
                {
                    this.Body.ParseStream = PaserServerError;
                }
                else
                {
                    this.Body.ParseStream = cosResult.ParseResponseBody;
                }
            }

            public void PaserServerError(Stream inputStream, string contentType, long contentLength)
            {
                CosServerException cosServerException = new CosServerException(cosResult.httpCode, cosResult.httpMessage);
                List<string> values;

                Headers.TryGetValue("x-cos-request-id", out values);
                cosServerException.requestId = (values != null && values.Count > 0) ? values[0] : null;
                Headers.TryGetValue("x-cos-trace-id", out values);
                cosServerException.traceId = (values != null && values.Count > 0) ? values[0] : null;

                if (inputStream != null)
                {

                    try
                    {
                        CosServerError cosServerError = XmlParse.Deserialize<CosServerError>(inputStream);
                        
                        cosServerException.SetCosServerError(cosServerError);
                    }
                    catch (Exception ex)
                    {
                        QLog.Debug(TAG, ex.Message);

                    }
                }

                throw cosServerException;
            }

            /// <summary>
            /// error
            /// </summary>
            /// <param name="ex"></param>
            public override void OnFinish(bool isSuccess, Exception ex)
            {
                cosResult.RawContentBodyString = Body.rawContentBodyString;

                if (isSuccess && successCallback != null)
                {
                    successCallback(cosResult);
                }
                else
if (faileCallback != null)
                {

                    if (ex is CosClientException)
                    {
                        faileCallback(ex as CosClientException, null);
                    }
                    else
if (ex is CosServerException)
                    {
                        faileCallback(null, ex as CosServerException);
                    }
                    else
                    {
                        faileCallback(new CosClientException((int)CosClientError.InternalError, ex.Message, ex), null);
                    }
                }

            }
        }

    }

}

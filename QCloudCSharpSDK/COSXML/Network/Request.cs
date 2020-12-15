using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Auth;
using COSXML.Log;
using COSXML.Common;
using System.Net;

namespace COSXML.Network
{
    public sealed class Request
    {
        private static string TAG = "Request";
        // post put get delete head, etc.
        private string method;
        // shceme://host:port/path?query# etc.
        private HttpUrl url;
        // key : value
        private Dictionary<string, string> headers;
        // file or byte, etc.
        private RequestBody body;
        // package tag for request
        private Object tag;

        private bool isHttps;

        private string userAgent;

        private string host;

        private string urlString;

        private HttpWebRequest realeHttpRequest;

        public Request()
        {
            headers = new Dictionary<string, string>();
            this.onNotifyGetResponse = this.HandleGetResponse;
        }

        public string Method
        {
            get
            {
                return method;
            }
            set { method = value; }
        }

        public bool IsHttps
        {
            get
            {
                return isHttps;
            }
            set { isHttps = value; }
        }

        public string UserAgent
        {
            get
            {
                return userAgent == null ? CosVersion.GetUserAgent() : userAgent;
            }
            set { userAgent = value; }
        }

        public string Host
        {
            get
            {
                return host == null ? url.Host : host;
            }
            set { host = value; }
        }

        public HttpUrl Url
        {
            get
            {
                //if (url == null) throw new ArgumentNullException("httpUrl == null");
                return url;
            }
            set
            {

                if (value == null)
                {
                    throw new ArgumentNullException("httpUrl == null");
                }

                url = value;
            }
        }

        public string RequestUrlString
        {
            get
            {

                if (urlString == null)
                {
                    urlString = url.ToString();
                }

                return urlString;
            }
            set { urlString = value; }
        }

        public Dictionary<string, string> Headers
        {
            get
            {
                return headers;
            }
            
            private set { }
        }

        public void AddHeader(string name, string value)
        {

            try
            {
                headers.Add(name, value);
            }
            catch (ArgumentNullException)
            {
                QLog.Debug(TAG, "AddHeader: name is null");
            }
            catch (ArgumentException)
            {

                if (String.IsNullOrEmpty(value))
                {
                    return;
                }

                if (!String.IsNullOrEmpty(headers[name]))
                {
                    headers[name] = headers[name] + ',' + value;
                }
                else
                {
                    headers[name] = value;
                }
            }

        }

        public RequestBody Body
        {
            get
            {
                return body;
            }
            set { body = value; }
        }

        public COSXML.Callback.OnNotifyGetResponse onNotifyGetResponse;

        private void HandleGetResponse()
        {

            if (body != null)
            {
                body.OnNotifyGetResponse();
            }
        }

        public void BindHttpWebRequest(HttpWebRequest httpWebRequest)
        {
            this.realeHttpRequest = httpWebRequest;
        }

        public void Cancel()
        {

            if (realeHttpRequest != null)
            {
                realeHttpRequest.Abort();
            }
        }

    }
}

using System;
using System.Collections.Generic;

using System.Text;

namespace COSXML.Network
{
    
    public sealed class HttpUrl
    {

        private string scheme;

        private string userName = "";

        private string userPwd = "";

        private string host;

        private int port = -1;

        private string path = "/";

        private Dictionary<string, string> queryParameters;

        private string fragment;



        public HttpUrl()
        {
            this.queryParameters = new Dictionary<string, string>();
        }

        public string Scheme
        {
            set
            {

                if (value == null)
                {
                    throw new ArgumentNullException("scheme == null");
                }

                if (value.Equals("http", StringComparison.OrdinalIgnoreCase))
                {
                    this.scheme = "http";
                }
                else
if (value.Equals("https", StringComparison.OrdinalIgnoreCase))
                {
                    this.scheme = "https";
                }
                else
                {
                    throw new ArgumentException("unexpected scheme: " + scheme);
                }
            }
            get
            {
                return this.scheme;
            }

        }

        public string UserName
        {
            set
            {

                if (value == null)
                {
                    throw new ArgumentNullException("userName == null");
                }

                this.userName = value;
            }
            get
            {
                return this.userName;
            }
        }

        public string UserPassword
        {
            set
            {

                if (value == null)
                {
                    throw new ArgumentNullException("userPwd == null");
                }

                this.userPwd = value;
            }
            get
            {
                return this.userPwd;
            }
        }

        public string Host
        {
            set
            {

                if (value == null)
                {
                    throw new ArgumentNullException("host == null");
                }

                this.host = value;
            }
            get
            {
                return this.host;
            }
        }

        public int Port
        {
            set
            {

                if (value <= 0 || value >= 65535)
                {
                    throw new ArgumentException("unexpected port: " + port);
                }

                this.port = value;
            }
            get
            {
                return this.port;
            }
        }

        public string Path
        {
            set
            {

                if (value != null)
                {
                    // need url encode
                    // need url encode
                    this.path = value;
                }
            }
            get
            {
                return path;
            }
        }

        public void SetQueryParameters(Dictionary<string, string> queryParameters)
        {

            if (queryParameters != null)
            {

                foreach (KeyValuePair<string, string> pair in queryParameters)
                {
                    this.queryParameters.Add(pair.Key, pair.Value);
                }

            }
        }

        public Dictionary<string, string> GetQueryParameters()
        {

            return queryParameters;
        }

        public string Fragment
        {
            set
            {
                this.fragment = value;
            }
            get
            {
                return this.fragment;
            }

        }

        public override string ToString()
        {

            //if (scheme == null) throw new ArgumentNullException("scheme == null");
            //if (host == null) throw new ArgumentNullException("host == null");

            StringBuilder url = new StringBuilder();


            url.Append(scheme)
                .Append("://");

            if (userName != String.Empty || userPwd != String.Empty)
            {
                url.Append(userName);

                if (userPwd != String.Empty)
                {
                    url.Append(':')
                        .Append(userPwd);
                }

                url.Append('@');
            }

            if (host.IndexOf(':') != -1)
            {
                url.Append('[')
                    .Append(host)
                    .Append(']');
            }
            else
            {
                url.Append(host);
            }

            int effectivePort = EffecivePort();

            if (effectivePort != DefaultPort(scheme))
            {
                url.Append(':')
                    .Append(effectivePort);
            }

            url.Append(path);

            StringBuilder query = new StringBuilder();

            foreach (KeyValuePair<string, string> pair in queryParameters)
            {
                query.Append(pair.Key);

                if (!String.IsNullOrEmpty(pair.Value))
                {
                    query.Append('=').Append(pair.Value);
                }

                query.Append('&');
            }

            string queryString = query.ToString();

            if (queryString.EndsWith("&"))
            {
                queryString = queryString.Remove(queryString.Length - 1);
                url.Append('?');
                url.Append(queryString);
            }

            if (fragment != null)
            {
                url.Append('#')
                    .Append(fragment);
            }

            return url.ToString();

        }

        public int EffecivePort()
        {

            return port != -1 ? port : DefaultPort(scheme);
        }

        private int DefaultPort(string scheme)
        {

            if (scheme.Equals("http", StringComparison.OrdinalIgnoreCase))
            {

                return 80;
            }
            else if (scheme.Equals("https", StringComparison.OrdinalIgnoreCase))
            {

                return 443;
            }
            else
            {

                return -1;
            }
        }
    }

}

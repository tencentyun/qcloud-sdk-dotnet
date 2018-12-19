using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;
/**
* Copyright (c) 2018 Tencent Cloud. All rights reserved.
* 11/6/2018 8:58:18 PM
* bradyxiao
*/
namespace COSXML.Network
{
    public class HttpClientConfig
    {
        private string userAgent;

        private bool allowAutoRedirect;

        private int connectionTimeoutMs;

        private int readWriteTimeoutMs;

        private int maxRetry;

        private int connectionLimit;

        private string proxyHost;

        private int proxyPort;

        private string proxyUserName;

        private string proxyUserPassword;

        private string proxyDomain;

        private HttpClientConfig(Builder builder)
        {
            this.userAgent = builder.userAgent;
            this.allowAutoRedirect = builder.allowAutoRedirect;
            this.connectionTimeoutMs = builder.connectionTimeoutMs;
            this.readWriteTimeoutMs = builder.readWriteTimeoutMs;
            this.maxRetry = builder.maxRetry;
            this.connectionLimit = builder.connectionLimit;
            this.proxyHost = builder.proxyHost;
            this.proxyPort = builder.proxyPort;
            this.proxyUserName = builder.proxyUserName;
            this.proxyUserPassword = builder.proxyUserPassword;
            this.proxyDomain = builder.proxyDomain;
        }

        public string UserAgnet 
        {
            get
            {
                return userAgent;
            }
            private set { } 
        }

        public bool AllowAutoRedirect
        {
            get { return allowAutoRedirect; }
            private set { }
        }

        public int ConnectionTimeoutMs
        {
            get
            {
                return connectionTimeoutMs;
            }

            private set { }
        }

        public int ReadWriteTimeoutMs
        {
            get
            {
                return readWriteTimeoutMs;
            }

            private set { }
        }

        public int MaxRery
        {
            get
            {
                return maxRetry;
            }
            private set { }
        }

        public int ConnectionLimit
        {
            get
            {
                return connectionLimit;
            }
            private set { }
        }

        public string ProxyHost
        {
            get
            {
                return proxyHost;
            }
            private set { }
        }

        public int ProxyPort
        {
            get
            {
                return proxyPort;
            }
            private set { }
        }

        public string ProxyUserName
        {
            get
            {
                return proxyUserName;
            }
            private set { }
        }


        public string ProxyUserPassword
        {
            get
            {
                return proxyUserPassword;
            }
            private set { }
        }

        public string ProxyDomain
        {
            get
            {
                return proxyDomain;
            }
            private set { }
        }

        public class Builder
        {
            internal string userAgent = CosVersion.GetUserAgent();

            internal bool allowAutoRedirect = true;

            internal int connectionTimeoutMs = 45000;

            internal int readWriteTimeoutMs = 45000;

            internal int maxRetry = 3;

            internal int connectionLimit = 512;

            internal int proxyPort = -1;

            internal string proxyHost = null;

            internal string proxyUserName;

            internal string proxyUserPassword;

            internal string proxyDomain;

            public Builder() { }

            public Builder AllowAutoRedirect(bool allow)
            {
                this.allowAutoRedirect = allow;
                return this;
            }

            public Builder SetConnectionLimit(int connectionLimit)
            {
                if (connectionLimit > 2) 
                {
                    this.connectionLimit = connectionLimit;
                }
                return this;
            }

            public Builder SetMaxRetry(int maxRetry)
            {
                if (maxRetry > 0)
                {
                    this.maxRetry = maxRetry;
                }
                return this;
            }

            public Builder SetConnectionTimeoutMs(int connectionTimeoutMs)
            {
                if (connectionTimeoutMs > 10000)
                {
                    this.connectionTimeoutMs = connectionTimeoutMs;
                }
                return this;
            }

            public Builder SetReadWriteTimeoutMs(int readWriteTimeoutMs)
            {
                if (readWriteTimeoutMs > 10000)
                {
                    this.readWriteTimeoutMs = readWriteTimeoutMs;
                }
                return this;
            }

            public Builder SetProxyHost(string host)
            {
                this.proxyHost = host;
                return this;
            }

            public Builder SetProxyPort(int port)
            {
                this.proxyPort = port;
                return this;
            }

            public Builder SetProxyUserName(string userName)
            {
                this.proxyUserName = userName;
                return this;
            }

            public Builder SetProxyUserPassword(string password)
            {
                this.proxyUserPassword = password;
                return this;
            }

            public Builder SetProxyDomain(string domain)
            {
                this.proxyDomain = domain;
                return this;
            }

            public HttpClientConfig Build()
            {
                return new HttpClientConfig(this);
            }
 
        }
    }
}

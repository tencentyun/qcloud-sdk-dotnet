using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;

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

        private bool keepAlive;

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
            this.keepAlive = builder.keepAlive;
        }

        public string UserAgnet
        {
            get
            {

                return userAgent;
            }

        }

        public bool AllowAutoRedirect
        {
            get
            {
                return allowAutoRedirect;
            }

        }

        public int ConnectionTimeoutMs
        {
            get
            {

                return connectionTimeoutMs;
            }


        }

        public int ReadWriteTimeoutMs
        {
            get
            {

                return readWriteTimeoutMs;
            }


        }

        public int ConnectionLimit
        {
            get
            {

                return connectionLimit;
            }

        }

        public string ProxyHost
        {
            get
            {

                return proxyHost;
            }

        }

        public int ProxyPort
        {
            get
            {

                return proxyPort;
            }

        }

        public string ProxyUserName
        {
            get
            {

                return proxyUserName;
            }

        }


        public string ProxyUserPassword
        {
            get
            {

                return proxyUserPassword;
            }

        }

        public string ProxyDomain
        {
            get
            {

                return proxyDomain;
            }

        }

        public bool KeepAlive
        {
            get
            {

                return keepAlive;
            }
        }

        public int MaxRetry
        {
            get
            {

                return maxRetry;
            }
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

            internal bool keepAlive = true;

            public Builder() 
            { 
                
            }

            public Builder AllowAutoRedirect(bool allow)
            {
                this.allowAutoRedirect = allow;

                return this;
            }

            public Builder SetConnectionLimit(int connectionLimit)
            {

                if (connectionLimit > 0)
                {
                    this.connectionLimit = connectionLimit;
                }

                return this;
            }

            public Builder SetConnectionTimeoutMs(int connectionTimeoutMs)
            {

                if (connectionTimeoutMs > 0)
                {
                    this.connectionTimeoutMs = connectionTimeoutMs;
                }

                return this;
            }

            public Builder SetReadWriteTimeoutMs(int readWriteTimeoutMs)
            {

                if (readWriteTimeoutMs > 0)
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

            public Builder SetHttpKeepAlive(bool keepAlive)
            {
                this.keepAlive = keepAlive;

                return this;
            }

            public Builder SetMaxRetry(int maxRetry)
            {
                this.maxRetry = maxRetry;

                return this;
            }

            public HttpClientConfig Build()
            {

                return new HttpClientConfig(this);
            }

        }
    }
}

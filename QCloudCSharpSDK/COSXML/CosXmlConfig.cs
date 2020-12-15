using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Network;

namespace COSXML
{
    /// <summary>
    /// COSXML 服务配置类
    /// </summary>
    public sealed class CosXmlConfig
    {
        private HttpClientConfig httpConfig;

        private string appid;

        private string region;

        private bool isHttps = true;

        private bool isDebug;

        /// <summary>
        /// 读取 Endpoint 后缀
        /// </summary>
        /// <value></value>
        public string endpointSuffix { get; private set; }

        /// <summary>
        /// 获取完整请求域名
        /// </summary>
        /// <value></value>
        public string host { get; }

        private CosXmlConfig(Builder builder)
        {
            this.appid = builder.appid;
            this.region = builder.region;
            this.isHttps = builder.isHttps;
            this.httpConfig = builder.httpClientConfigBuilder.Build();
            this.isDebug = builder.isDebug;
            this.endpointSuffix = builder.endpointSuffix;
            this.host = builder.host;
        }

        /// <summary>
        /// 获取 AppID
        /// </summary>
        /// <value></value>
        public string Appid
        {
            get
            {
                return appid;
            }
        }

        /// <summary>
        /// 获取 Region
        /// </summary>
        /// <value></value>
        public string Region
        {
            get
            {
                return region;
            }
        }

        /// <summary>
        /// 获取是否开启 Https
        /// </summary>
        /// <value></value>
        public bool IsHttps
        {
            get
            {
                return isHttps;
            }
        }

        /// <summary>
        /// 获取 HttpClient 配置
        /// </summary>
        /// <value></value>
        public HttpClientConfig HttpConfig
        {
            get
            {
                return httpConfig;
            }
        }

        /// <summary>
        /// 获取是否开启 DEBUG 日志
        /// </summary>
        /// <value></value>
        public bool IsDebugLog
        {
            get
            {
                return isDebug;
            }
        }

        /// <summary>
        /// Config 构造器
        /// </summary>
        public sealed class Builder
        {
            internal string appid;

            internal string region;

            internal bool isHttps = true;

            internal HttpClientConfig.Builder httpClientConfigBuilder;

            internal bool isDebug = false;

            internal string endpointSuffix;

            internal string host;

            /// <summary>
            /// 初始化一个构造器
            /// </summary>
            public Builder()
            {
                httpClientConfigBuilder = new HttpClientConfig.Builder();
            }

            /// <summary>
            /// cos 服务的Appid
            /// </summary>
            /// <param name="appid"></param>
            /// <returns></returns>
            public Builder SetAppid(string appid)
            {
                this.appid = appid;

                return this;
            }

            /// <summary>
            /// 存储桶所属地域
            /// </summary>
            /// <param name="region"></param>
            /// <returns></returns>
            public Builder SetRegion(string region)
            {
                this.region = region;

                return this;
            }

            /// <summary>
            /// true：https请求
            /// </summary>
            /// <param name="isHttps"></param>
            /// <returns></returns>
            public Builder IsHttps(bool isHttps)
            {
                this.isHttps = isHttps;

                return this;
            }

            /// <summary>
            /// 设置最大连接数，默认值 512
            /// </summary>
            /// <param name="connectionLimit"></param>
            /// <returns></returns>
            public Builder SetConnectionLimit(int connectionLimit)
            {
                this.httpClientConfigBuilder.SetConnectionLimit(connectionLimit);

                return this;
            }

            /// <summary>
            /// 设置 TCP 连接超时时间，单位是毫秒，默认 45 秒
            /// </summary>
            /// <param name="connectionTimeoutMs"></param>
            /// <returns></returns>
            public Builder SetConnectionTimeoutMs(int connectionTimeoutMs)
            {
                this.httpClientConfigBuilder.SetConnectionTimeoutMs(connectionTimeoutMs);

                return this;
            }

            /// <summary>
            /// 设置 TCP 连接读写时间，单位是毫秒，默认 45 秒
            /// </summary>
            /// <param name="readWriteTimeoutMs"></param>
            /// <returns></returns>
            public Builder SetReadWriteTimeoutMs(int readWriteTimeoutMs)
            {
                this.httpClientConfigBuilder.SetReadWriteTimeoutMs(readWriteTimeoutMs);

                return this;
            }

            /// <summary>
            /// 设置 HTTP 代理主机
            /// </summary>
            /// <param name="host"></param>
            /// <returns></returns>
            public Builder SetProxyHost(string host)
            {
                this.httpClientConfigBuilder.SetProxyHost(host);

                return this;
            }

            /// <summary>
            /// 设置 HTTP 代理端口
            /// </summary>
            /// <param name="port"></param>
            /// <returns></returns>
            public Builder SetProxyPort(int port)
            {
                this.httpClientConfigBuilder.SetProxyPort(port);

                return this;
            }

            /// <summary>
            /// 设置 HTTP 代理用户名
            /// </summary>
            /// <param name="userName"></param>
            /// <returns></returns>
            public Builder SetProxyUserName(string userName)
            {
                this.httpClientConfigBuilder.SetProxyUserName(userName);

                return this;
            }

            /// <summary>
            /// 设置 HTTP 代理用户密码
            /// </summary>
            /// <param name="password"></param>
            /// <returns></returns>
            public Builder SetProxyUserPassword(string password)
            {
                this.httpClientConfigBuilder.SetProxyUserPassword(password);

                return this;
            }

            /// <summary>
            /// 设置 HTTP 代理 Domain
            /// </summary>
            /// <param name="domain"></param>
            /// <returns></returns>
            public Builder SetProxyDomain(string domain)
            {
                this.httpClientConfigBuilder.SetProxyDomain(domain);

                return this;
            }

            /// <summary>
            /// 设置是否允许请求重定向
            /// </summary>
            /// <param name="isAllow"></param>
            /// <returns></returns>
            public Builder SetAllowAutoRedirect(bool isAllow)
            {
                this.httpClientConfigBuilder.AllowAutoRedirect(isAllow);

                return this;
            }

            /// <summary>
            /// 设置是否开启 DEBUG 日志
            /// </summary>
            /// <param name="isDebug"></param>
            /// <returns></returns>
            public Builder SetDebugLog(bool isDebug)
            {
                this.isDebug = isDebug;

                return this;
            }

            /// <summary>
            /// 设置 Endpoint 后缀，最终请求域名为 $Bucket.$EndpointSuffix
            /// </summary>
            /// <param name="suffix"></param>
            /// <returns></returns>
            public Builder SetEndpointSuffix(string suffix)
            {
                this.endpointSuffix = suffix;

                return this;
            }

            /// <summary>
            /// 设置完整请求域名
            /// </summary>
            /// <param name="host"></param>
            /// <returns></returns>
            public Builder SetHost(string host)
            {
                this.host = host;

                return this;
            }

            /// <summary>
            /// 构建 Config
            /// </summary>
            /// <returns></returns>
            public CosXmlConfig Build()
            {

                return new CosXmlConfig(this);
            }

        }
    }
}

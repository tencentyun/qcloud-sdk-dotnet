using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;
using COSXML.CosException;

namespace COSXML.Model.Object
{
    /// <summary>
    /// 实现 Object 跨域访问配置的预请求
    /// <see cref="https://cloud.tencent.com/document/product/436/8288"/>
    /// </summary>
    public sealed class OptionObjectRequest : ObjectRequest
    {
        /// <summary>
        /// 模拟跨域访问的请求来源域名
        /// </summary>
        private string origin;

        /// <summary>
        /// 模拟跨域访问的请求 HTTP 方法
        /// </summary>
        private string accessControlMethod;

        public OptionObjectRequest(string bucket, string key, string origin, string accessControlMethod)
            : base(bucket, key)
        {
            this.method = CosRequestMethod.OPTIONS;
            this.origin = origin;

            if (accessControlMethod != null)
            {
                this.accessControlMethod = accessControlMethod.ToUpper();
            }
        }

        /// <summary>
        /// 模拟跨域访问的请求头部
        /// </summary>
        /// <param name="accessControlHeaders"></param>
        public void SetAccessControlHeaders(List<string> accessControlHeaders)
        {

            if (accessControlHeaders != null)
            {
                StringBuilder headers = new StringBuilder();

                foreach (string accessControlHeader in accessControlHeaders)
                {

                    if (accessControlHeader != null)
                    {
                        headers.Append(accessControlHeader).Append(",");
                    }
                }

                string result = headers.ToString();

                if (result.EndsWith(","))
                {
                    result = result.Substring(0, result.Length - 1);
                    SetRequestHeader(CosRequestHeaderKey.ACCESS_CONTROL_REQUEST_HEADERS, result);
                }
            }
        }

        public override void CheckParameters()
        {

            if (origin == null)
            {
                throw new CosClientException((int)CosClientError.InvalidArgument, "origin = null");
            }

            if (accessControlMethod == null)
            {
                throw new CosClientException((int)CosClientError.InvalidArgument, "accessControlMethod = null");
            }

            base.CheckParameters();
        }

        protected override void InteranlUpdateHeaders()
        {

            try
            {
                this.headers.Add(CosRequestHeaderKey.ORIGIN, origin);
            }
            catch (ArgumentException)
            {
                this.headers[CosRequestHeaderKey.ORIGIN] = origin;
            }

            try
            {
                this.headers.Add(CosRequestHeaderKey.ACCESS_CONTROL_REQUEST_METHOD, accessControlMethod);
            }
            catch (ArgumentException)
            {
                this.headers[CosRequestHeaderKey.ACCESS_CONTROL_REQUEST_METHOD] = accessControlMethod;
            }
        }
    }
}

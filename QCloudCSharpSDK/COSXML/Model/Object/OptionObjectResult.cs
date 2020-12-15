using System;
using System.Collections.Generic;

using System.Text;

namespace COSXML.Model.Object
{
    /// <summary>
    /// 实现 Object 跨域访问配置的预请求
    /// <see cref="https://cloud.tencent.com/document/product/436/8288"/>
    /// </summary>
    public sealed class OptionObjectResult : CosResult
    {
        /// <summary>
        /// 跨域访问的请求来源域名
        /// </summary>
        public string accessControlAllowOrigin;

        /// <summary>
        /// OPTIONS 请求得到结果的有效期
        /// </summary>
        public long accessControlMaxAge;

        /// <summary>
        /// 跨域访问的允许请求头部
        /// </summary>
        public List<string> accessControlAllowHeaders;

        /// <summary>
        /// 跨域访问的允许请求 HTTP 方法
        /// </summary>
        public List<string> accessControlAllowMethods;

        /// <summary>
        /// 跨域访问的允许请求自定义头部
        /// </summary>
        public List<string> accessControlAllowExposeHeaders;

        internal override void InternalParseResponseHeaders()
        {
            List<string> values;

            this.responseHeaders.TryGetValue("Access-Control-Allow-Origin", out values);

            if (values != null && values.Count > 0)
            {
                accessControlAllowOrigin = values[0];
            }

            this.responseHeaders.TryGetValue("Access-Control-Max-Age", out values);

            if (values != null && values.Count > 0)
            {
                long.TryParse(values[0], out accessControlMaxAge);
            }

            this.responseHeaders.TryGetValue("Access-Control-Allow-Methods", out values);

            if (values != null && values.Count > 0)
            {
                accessControlAllowMethods = new List<string>(values[0].Split(','));
            }

            this.responseHeaders.TryGetValue("Access-Control-Allow-Headers", out values);

            if (values != null && values.Count > 0)
            {
                accessControlAllowHeaders = new List<string>(values[0].Split(','));
            }

            this.responseHeaders.TryGetValue("Access-Control-Expose-Headers", out values);

            if (values != null && values.Count > 0)
            {
                accessControlAllowExposeHeaders = new List<string>(values[0].Split(','));
            }
        }

        public override string GetResultInfo()
        {

            return base.GetResultInfo() + "\n" + accessControlAllowOrigin;
        }
    }
}

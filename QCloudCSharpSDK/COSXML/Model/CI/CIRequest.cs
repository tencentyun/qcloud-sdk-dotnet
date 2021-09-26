using System;
using System.Collections.Generic;

using System.Text;
using COSXML.CosException;
using COSXML.Common;
using COSXML.Utils;
using COSXML.Network;

namespace COSXML.Model.Object
{
    public class CIRequest : CosRequest
    {
        /// <summary>
        /// 存储桶名称(Bucket)
        /// <see href="https://cloud.tencent.com/document/product/436/7751"/>
        /// </summary>
        protected string bucket;

        public CIRequest()
        {

        }

        public CIRequest(string bucket)
        {
            this.bucket = bucket;
        }

        /// <summary>
        /// Bucket 所在的地域
        /// </summary>
        protected string region;

        public void SetRequestPath(string path)
        {
            this.path = path;
        }

        /// <summary>
        /// Object 所属的 Bucket
        /// </summary>
        public string Bucket
        {
            get
            {
                return this.bucket;
            }
            set { this.bucket = value; }
        }

        /// <summary>
        /// Bucket 所在的地域
        /// </summary>
        public string Region
        {
            get
            {
                return this.region;
            }
            set { this.region = value; }
        }


        public override Network.RequestBody GetRequestBody()
        {

            return null;
        }

        public override string GetHost()
        {
            StringBuilder hostBuilder = new StringBuilder();

            if (!String.IsNullOrEmpty(serviceConfig.host))
            {
                hostBuilder.Append(serviceConfig.host);
            }
            else if (String.IsNullOrEmpty(bucket))
            {
                hostBuilder.Append("ci.").Append(region).Append(".myqcloud.com");
            }
            else
            {
                hostBuilder.Append(bucket);

                if (!String.IsNullOrEmpty(appid) && !bucket.EndsWith("-" + appid))
                {
                    hostBuilder.Append("-")
                        .Append(appid);
                }

                hostBuilder.Append(".ci.")
                    .Append(region)
                    .Append(".myqcloud.com");
            }

            return hostBuilder.ToString();
        }

        public override void CheckParameters()
        {

            if (requestUrlWithSign != null)
            {

                return;
            }
            //if (appid == null)
            //{
            //    throw new CosClientException((int)CosClientError.INVALID_ARGUMENT, "appid is null");
            //}
            // if (region == null || region.Length < 1)
            // {
            //     throw new CosClientException((int)CosClientError.INVALID_ARGUMENT, "region is null");
            // }
            if (path == null || path.Length < 1)
            {
                throw new CosClientException((int)CosClientError.InvalidArgument, "cosPath(null or empty)is invalid");
            }
        }

        protected virtual void InternalUpdateQueryParameters()
        {
        }

        protected virtual void InteranlUpdateHeaders() 
        { 
            
        }

        public override Dictionary<string, string> GetRequestParamters()
        {
            InternalUpdateQueryParameters();

            return base.GetRequestParamters();
        }

    }
}

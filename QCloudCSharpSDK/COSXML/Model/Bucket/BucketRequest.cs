using System;
using System.Collections.Generic;

using System.Text;
using COSXML.CosException;
using COSXML.Common;

namespace COSXML.Model.Bucket
{

    public abstract class BucketRequest : CosRequest
    {
        /// <summary>
        /// cos 存储桶,即 Bucket
        /// </summary>
        protected string bucket;

        /// <summary>
        /// Bucket 所在的地域
        /// </summary>
        protected string region;

        public BucketRequest(string bucket)
        {
            this.bucket = bucket;
            this.path = "/";
        }

        /// <summary>
        /// Bucket 名称， "BucketName-APPID"
        /// </summary>
        public string Bucket
        {
            get
            {
                return this.bucket;
            }
            set { this.bucket = value; }
        }

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

            else
            {
                hostBuilder.Append(bucket);

                if (!String.IsNullOrEmpty(appid) && !bucket.EndsWith("-" + appid))
                {
                    hostBuilder.Append("-")
                        .Append(appid);
                }

                if (serviceConfig.endpointSuffix != null)
                {
                    hostBuilder.Append(".")
                        .Append(serviceConfig.endpointSuffix);
                }

                else
                {
                    hostBuilder.Append(".cos.")
                        .Append(region)
                        .Append(".myqcloud.com");
                }
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
            if (bucket == null)
            {
                throw new CosClientException((int)CosClientError.InvalidArgument, "bucket is null");
            }
            // if (region == null)
            // {
            //     throw new CosClientException((int)CosClientError.INVALID_ARGUMENT, "region is null");
            // }
        }

    }
}

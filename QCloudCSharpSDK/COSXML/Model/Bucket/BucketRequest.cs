using System;
using System.Collections.Generic;

using System.Text;
using COSXML.CosException;
using COSXML.Common;
/**
* Copyright (c) 2018 Tencent Cloud. All rights reserved.
* 11/2/2018 8:03:59 PM
* bradyxiao
*/
namespace COSXML.Model.Bucket
{
    /**
     * Buceket request for cos
     * base class
     * provider bucket,region property
     */
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
            get { return this.bucket; }
            set { this.bucket = value; }
        }

        /// <summary>
        /// Bucket 所在地域
        /// <see cref="COSXML.Common.CosRegion"/>
        /// </summary>
        public string Region
        {
            get { return this.region; }
            set { this.region = value; }
        }

        public override Network.RequestBody GetRequestBody()
        {
            return null;
        }

        public override string GetCOSHost() {
            StringBuilder hostBuilder = new StringBuilder();
            hostBuilder.Append(bucket);
            if (!String.IsNullOrEmpty(appid) && !bucket.EndsWith("-" + appid))
            {
                hostBuilder.Append("-")
                    .Append(appid);
            }
            hostBuilder.Append(".cos.")
                    .Append(region)
                    .Append(".myqcloud.com");
            return hostBuilder.ToString();
        }

        public override string GetHost()
        {
            StringBuilder hostBuilder = new StringBuilder();
            if (!String.IsNullOrEmpty(serviceConfig.host)) {
                hostBuilder.Append(serviceConfig.host);
            } else {
                hostBuilder.Append(bucket);
                if (!String.IsNullOrEmpty(appid) && !bucket.EndsWith("-" + appid))
                {
                    hostBuilder.Append("-")
                        .Append(appid);
                }
                if (serviceConfig.endpointSuffix != null) {
                    hostBuilder.Append(".")
                        .Append(serviceConfig.endpointSuffix);
                } else {
                    hostBuilder.Append(".cos.")
                        .Append(region)
                        .Append(".myqcloud.com");
                }
            }
            return hostBuilder.ToString();
        }

        public override void CheckParameters()
        {
            if (requestUrlWithSign != null) return;
            //if (appid == null)
            //{
            //    throw new CosClientException((int)CosClientError.INVALID_ARGUMENT, "appid is null");
            //}
            if (bucket == null)
            {
                throw new CosClientException((int)CosClientError.INVALID_ARGUMENT, "bucket is null");
            }
            // if (region == null)
            // {
            //     throw new CosClientException((int)CosClientError.INVALID_ARGUMENT, "region is null");
            // }
        }

    }
}

using System;
using System.Collections.Generic;

using System.Text;
using COSXML.CosException;
using COSXML.Common;

namespace COSXML.Model.Object
{
    public abstract class ObjectRequest : CosRequest
    {
        /// <summary>
        /// 存储桶名称(Bucket)
        /// <see cref="https://cloud.tencent.com/document/product/436/7751"/>
        /// </summary>
        protected string bucket;

        /// <summary>
        /// Bucket 所在的地域
        /// <see cref="Common.CosRegion"/>
        /// </summary>
        protected string region;

        public ObjectRequest(string bucket, string key)
        {
            this.bucket = bucket;
            if (key != null && !key.StartsWith("/"))
            {
                this.path = "/" + key;
            }
            else 
            {
                this.path = key;
            }
        }

        /// <summary>
        /// Object 所属的 Bucket
        /// </summary>
        public string Bucket
        {
            get { return this.bucket; }
            set { this.bucket = value; }
        }

        /// <summary>
        /// Bucket 所在的地域
        /// <see cref="Common.CosRegion"/>
        /// </summary>
        public string Region
        {
            get { return this.region; }
            set { this.region = value; }
        }

        /// <summary>
        /// object 名称，对象键
        /// </summary>
        /// <param name="key"></param>
        public virtual void SetCosPath(string key)
        {
            if (key != null && !key.StartsWith("/"))
            {
                this.path = "/" + key;
            }
            else
            {
                this.path = key;
            }
        }

        public override Network.RequestBody GetRequestBody()
        {
            return null;
        }

        public override string GetHost()
        {
            StringBuilder hostBuilder = new StringBuilder();
            if (bucket.EndsWith("-" + appid))
            {
                hostBuilder.Append(bucket);
            }
            else
            {
                hostBuilder.Append(bucket).Append("-")
                    .Append(appid);
            }
            hostBuilder.Append(".cos.")
                .Append(region)
                .Append(".myqcloud.com");
            return hostBuilder.ToString();
        }

        public override void CheckParameters()
        {
            if (bucket == null)
            {
                throw new CosClientException((int)CosClientError.INVALID_ARGUMENT, "bucket is null");
            }
            if (region == null)
            {
                throw new CosClientException((int)CosClientError.INVALID_ARGUMENT, "region is null");
            }
            if (path == null)
            {
                throw new CosClientException((int)CosClientError.INVALID_ARGUMENT, "cosPath is null");
            }

        }

        protected virtual void InternalUpdateQueryParameters() 
        { 
        }

        protected virtual void InteranlUpdateHeaders() { }

        public override Dictionary<string, string> GetRequestParamters()
        {
            InternalUpdateQueryParameters();
            return base.GetRequestParamters();
        }

        public override Dictionary<string, string> GetRequestHeaders()
        {
            InteranlUpdateHeaders();
            return base.GetRequestHeaders();
        }


    }
}

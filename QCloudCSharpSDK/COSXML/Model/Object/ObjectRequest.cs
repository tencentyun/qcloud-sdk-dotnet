using System;
using System.Collections.Generic;

using System.Text;
using COSXML.CosException;
using COSXML.Common;
using COSXML.Utils;

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
            if (!String.IsNullOrEmpty(key) && !key.StartsWith("/"))
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
            if (requestUrlWithSign != null) return;
            //if (appid == null)
            //{
            //    throw new CosClientException((int)CosClientError.INVALID_ARGUMENT, "appid is null");
            //}
            if (bucket == null || bucket.Length < 1)
            {
                throw new CosClientException((int)CosClientError.INVALID_ARGUMENT, "bucket is null");
            }
            if (region == null || region.Length < 1)
            {
                throw new CosClientException((int)CosClientError.INVALID_ARGUMENT, "region is null");
            }
            if (path == null || path.Length < 1)
            {
                throw new CosClientException((int)CosClientError.INVALID_ARGUMENT, "cosPath(null or empty)is invalid");
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

        /// <summary>
        /// cos 服务端加密
        /// </summary>
        public void SetCosServerSideEncryption()
        {
            SetRequestHeader("x-cos-server-side-encryption", "AES256");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerKey"></param>
        public void SetCosServerSideEncryptionWithCustomerKey(string customerKey)
        {
            if (customerKey != null)
            {
                SetRequestHeader("x-cos-server-side-encryption-customer-algorithm", "AES256");
                SetRequestHeader("x-cos-server-side-encryption-customer-key", DigestUtils.GetBase64(customerKey, Encoding.UTF8));
                SetRequestHeader("x-cos-server-side-encryption-customer-key-MD5", DigestUtils.GetMd5ToBase64(customerKey, Encoding.UTF8));
            } 
        }

        public void SetCosServerSideEncryptionWithKMS(string customerKeyID, string json)
        {
            SetRequestHeader("x-cos-server-side-encryption", "cos/kms");
            if (customerKeyID != null)
            {
                SetRequestHeader("x-cos-server-side-encryption-cos-kms-key-id", customerKeyID);
            }
            if (json != null)
            {
                SetRequestHeader("x-cos-server-side-encryption-context", DigestUtils.GetBase64(json, Encoding.UTF8));
            }
        }

    }
}

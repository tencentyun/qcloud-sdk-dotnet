using System;
using System.Collections.Generic;

using System.Text;
using COSXML.CosException;
using COSXML.Common;
using COSXML.Utils;

namespace COSXML.Model.Tag
{
    public sealed class CopySourceStruct
    {
        /// <summary>
        /// cos 服务的appid
        /// </summary>
        public string appid;

        /// <summary>
        /// 存储桶名称
        /// </summary>
        public string bucket;

        /// <summary>
        /// Bucket所属地域
        /// </summary>
        public string region;

        /// <summary>
        /// 对象键
        /// </summary>
        public string key;

        /// <summary>
        /// 对象的版本ID
        /// </summary>
        public string versionId;

        /// <summary>
        /// copy source with versionId
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="bucket"></param>
        /// <param name="region"></param>
        /// <param name="key"></param>
        /// <param name="versionId"></param>
        public CopySourceStruct(string appid, string bucket, string region, string key, string versionId)
        {
            this.appid = appid;
            this.bucket = bucket;
            this.region = region;
            this.key = key;
            this.versionId = versionId;
        }

        /// <summary>
        /// copy source
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="bucket"></param>
        /// <param name="region"></param>
        /// <param name="key"></param>
        public CopySourceStruct(string appid, string bucket, string region, string key)
            : this(appid, bucket, region, key, null) 
            { 
                
            }

        /// <summary>
        /// check parameter
        /// </summary>
        public void CheckParameters()
        {

            if (bucket == null)
            {
                throw new CosClientException((int)CosClientError.InvalidArgument, "copy source bucket = null");
            }

            if (key == null)
            {
                throw new CosClientException((int)CosClientError.InvalidArgument, "copy source cosPath = null");
            }
            // if (appid == null)
            // {
            //     throw new CosClientException((int)CosClientError.INVALID_ARGUMENT, "copy source appid = null");
            // }
            if (region == null)
            {
                throw new CosClientException((int)CosClientError.InvalidArgument, "copy source region = null");
            }
        }

        /// <summary>
        /// get source with urlEncode
        /// </summary>
        /// <returns></returns>
        public string GetCopySouce()
        {

            if (!key.StartsWith("/"))
            {
                key = "/" + key;
            }

            StringBuilder copySource = new StringBuilder();


            copySource.Append(bucket);

            if (!String.IsNullOrEmpty(appid) && !bucket.EndsWith("-" + appid))
            {
                copySource.Append("-")
                        .Append(appid);
            }

            copySource.Append(".").Append("cos").Append(".")
                .Append(region).Append(".")
                .Append("myqcloud.com")
                .Append(URLEncodeUtils.EncodePathOfURL(key));

            if (versionId != null)
            {
                copySource.Append("?versionId=").Append(versionId);
            }

            return copySource.ToString();
        }
    }
}

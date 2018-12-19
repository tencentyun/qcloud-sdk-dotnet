using System;
using System.Collections.Generic;

using System.Text;
/**
* Copyright (c) 2018 Tencent Cloud. All rights reserved.
* 11/5/2018 11:55:28 AM
* bradyxiao
*/
namespace COSXML.Model.Tag
{
    /// <summary>
    /// <see cref="https://cloud.tencent.com/document/product/436/7742"/>
    /// </summary>
    public sealed class CompleteMultipartUploadResult
    {
        /// <summary>
        /// 创建的Object的外网访问域名
        /// </summary>
        public string location;
        /// <summary>
        /// 分块上传的目标Bucket
        /// </summary>
        public string bucket;
        /// <summary>
        /// Object的名称
        /// </summary>
        public string key;
        /// <summary>
        /// 合并后对象的唯一标签值，该值不是对象内容的 MD5 校验值，仅能用于检查对象唯一性
        /// </summary>
        public string eTag;

        
        public string GetInfo()
        {
            StringBuilder stringBuilder = new StringBuilder("{CompleteMultipartUploadResult:\n");
            stringBuilder.Append("Location:").Append(location).Append("\n");
            stringBuilder.Append("Bucket:").Append(bucket).Append("\n");
            stringBuilder.Append("Key:").Append(key).Append("\n");
            stringBuilder.Append("ETag:").Append(eTag).Append("\n");
            stringBuilder.Append("}");
            return stringBuilder.ToString();
        }
    }
}

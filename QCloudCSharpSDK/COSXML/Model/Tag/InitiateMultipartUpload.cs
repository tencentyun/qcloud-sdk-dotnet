using System;
using System.Collections.Generic;

using System.Text;
/**
* Copyright (c) 2018 Tencent Cloud. All rights reserved.
* 11/5/2018 12:01:43 PM
* bradyxiao
*/
namespace COSXML.Model.Tag
{
    /// <summary>
    /// 初始化上传返回的信息
    /// <see cref="https://cloud.tencent.com/document/product/436/7746"/>
    /// </summary>
    public sealed class InitiateMultipartUpload
    {
        /// <summary>
        /// 分片上传的目标 Bucket，由用户自定义字符串和系统生成appid数字串由中划线连接而成，如：mybucket-1250000000.
        /// </summary>
        public string bucket;
        /// <summary>
        /// Object 的名称
        /// </summary>
        public string key;
        /// <summary>
        /// 在后续上传中使用的 ID
        /// </summary>
        public string uploadId;

        public string GetInfo()
        {
            StringBuilder stringBuilder = new StringBuilder("{InitiateMultipartUpload:\n");
            stringBuilder.Append("Bucket:").Append(bucket).Append("\n");
            stringBuilder.Append("Key:").Append(key).Append("\n");
            stringBuilder.Append("UploadId:").Append(uploadId).Append("\n");
            stringBuilder.Append("}");
            return stringBuilder.ToString();
        }
    }
}

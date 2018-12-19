using System;
using System.Collections.Generic;

using System.Text;
/**
* Copyright (c) 2018 Tencent Cloud. All rights reserved.
* 11/5/2018 12:06:59 PM
* bradyxiao
*/
namespace COSXML.Model.Tag
{
    public sealed class PostResponse
    {
        /// <summary>
        /// 对象的完整路径。
        /// </summary>
        public string location;
        /// <summary>
        /// 对象所在的存储桶
        /// </summary>
        public string bucket;
        /// <summary>
        /// 对象 key 名
        /// </summary>
        public string key;
        /// <summary>
        /// 对象Etag 内容
        /// </summary>
        public string eTag;

        
        public string GetInfo() 
        {
            StringBuilder stringBuilder = new StringBuilder("{PostResponse:\n");
            stringBuilder.Append("Location:").Append(location).Append("\n");
            stringBuilder.Append("Bucket:").Append(bucket).Append("\n");
            stringBuilder.Append("Key:").Append(key).Append("\n");
            stringBuilder.Append("ETag:").Append(eTag).Append("\n");
            stringBuilder.Append("}");
            return stringBuilder.ToString();
        }
    }
}

using System;
using System.Collections.Generic;

using System.Text;
/**
* Copyright (c) 2018 Tencent Cloud. All rights reserved.
* 11/5/2018 11:59:32 AM
* bradyxiao
*/
namespace COSXML.Model.Tag
{
    /// <summary>
    /// 复制结果返回信息
    /// <see cref="https://cloud.tencent.com/document/product/436/10881"/>
    /// </summary>
    public sealed class CopyObject
    {
        /// <summary>
        /// 返回文件的 MD5 算法校验值。ETag 的值可以用于检查 Object 的内容是否发生变化
        /// </summary>
        public string eTag;
        /// <summary>
        /// 返回文件最后修改时间，GMT 格式
        /// </summary>
        public string lastModified;
        /// <summary>
        /// key 的 versionId
        /// </summary>
        public string versionId;
    
        public string GetInfo()
        {
            StringBuilder stringBuilder = new StringBuilder("{CopyObject:\n");
            stringBuilder.Append("ETag:").Append(eTag).Append("\n");
            stringBuilder.Append("LastModified:").Append(lastModified).Append("\n");
            stringBuilder.Append("VersionId:").Append(versionId).Append("\n");
            stringBuilder.Append("}");
            return stringBuilder.ToString();
        }

    }
}

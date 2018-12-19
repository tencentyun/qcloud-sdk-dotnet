using System;
using System.Collections.Generic;

using System.Text;
/**
* Copyright (c) 2018 Tencent Cloud. All rights reserved.
* 11/2/2018 5:52:22 PM
* bradyxiao
*/
namespace COSXML.Model.Tag
{
    /// <summary>
    /// <see cref="https://cloud.tencent.com/document/product/436/8291"/>
    /// </summary>
    public sealed class ListAllMyBuckets
    {
        /// <summary>
        /// Bucket 持有者的信息
        /// <see cref="Owner"/>
        /// </summary>
        public Owner owner;
        /// <summary>
        /// 本次响应的所有 Bucket 列表信息
        /// <see cref="Bucket"/>
        /// </summary>
        public List<Bucket> buckets;

        public string GetInfo()
        {
            StringBuilder stringBuilder = new StringBuilder("{ListAllMyBuckets:\n");
            if (owner != null) stringBuilder.Append(owner.GetInfo()).Append("\n");
            stringBuilder.Append("Buckets:\n");
            if (buckets != null)
            {
                foreach (Bucket bucket in buckets)
                {
                    if (bucket != null) stringBuilder.Append(bucket.GetInfo()).Append("\n");
                }
            }
            stringBuilder.Append("}").Append("\n");
            stringBuilder.Append("}");
            return stringBuilder.ToString();
         }

        public sealed class Owner
        {
            /// <summary>
            /// Bucket 所有者的 ID
            /// </summary>
            public string id;
            /// <summary>
            /// Bucket 所有者的名字信息
            /// </summary>
            public string disPlayName;

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{Owner:\n");
                stringBuilder.Append("ID:").Append(id).Append("\n");
                stringBuilder.Append("DisPlayName:").Append(disPlayName).Append("\n");
                stringBuilder.Append("}");
                return stringBuilder.ToString();
            }
        }

        public sealed class Bucket
        {
            /// <summary>
            /// Bucket 的名称
            /// </summary>
            public string name;
            /// <summary>
            /// Bucket 所在地域
            /// </summary>
            public string location;
            /// <summary>
            /// Bucket 创建时间。ISO8601 格式
            /// </summary>
            public string createDate;
            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{Bucket:\n");
                stringBuilder.Append("Name:").Append(name).Append("\n");
                stringBuilder.Append("Location:").Append(location).Append("\n");
                stringBuilder.Append("CreateDate:").Append(createDate).Append("\n");
                stringBuilder.Append("}");
                return stringBuilder.ToString();
            }
        }
    }

    
}

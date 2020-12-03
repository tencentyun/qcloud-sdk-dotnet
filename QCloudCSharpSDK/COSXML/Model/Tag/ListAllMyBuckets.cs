using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;

namespace COSXML.Model.Tag
{
    /// <summary>
    /// 列出存储桶结果
    /// <see cref="https://cloud.tencent.com/document/product/436/8291"/>
    /// </summary>
    [XmlRoot("ListAllMyBucketsResult")]
    public sealed class ListAllMyBuckets
    {
        /// <summary>
        /// Bucket 持有者的信息
        /// <see cref="Owner"/>
        /// </summary>
        [XmlElement("Owner")]
        public Owner owner;

        /// <summary>
        /// 本次响应的所有 Bucket 列表信息
        /// <see cref="Bucket"/>
        /// </summary>
        [XmlArray("Buckets")]
        public List<Bucket> buckets;

        public string GetInfo()
        {
            StringBuilder stringBuilder = new StringBuilder("{ListAllMyBuckets:\n");

            if (owner != null)
            {
                stringBuilder.Append(owner.GetInfo()).Append("\n");
            }

            stringBuilder.Append("Buckets:\n");

            if (buckets != null)
            {

                foreach (Bucket bucket in buckets)
                {

                    if (bucket != null)
                    {
                        stringBuilder.Append(bucket.GetInfo()).Append("\n");
                    }
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
            [XmlElement("ID")]
            public string id;

            /// <summary>
            /// Bucket 所有者的名字信息
            /// </summary>
            [XmlElement("DisplayName")]
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
            [XmlElement("Name")]
            public string name;

            /// <summary>
            /// Bucket 所在地域
            /// </summary>
            [XmlElement("Location")]
            public string location;

            /// <summary>
            /// Bucket 创建时间。ISO8601 格式
            /// </summary>
            [XmlElement("CreationDate")]
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

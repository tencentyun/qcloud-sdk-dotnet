using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;

namespace COSXML.Model.Tag
{
    /// <summary>
    /// 保存 Get Bucket 请求结果的所有信息
    /// <see cref="https://cloud.tencent.com/document/product/436/7734#.E8.AF.B7.E6.B1.82.E7.A4.BA.E4.BE.8B"/>
    /// </summary>
    [XmlRoot("ListBucketResult")]
    public sealed class ListBucket
    {
        /// <summary>
        /// Bucket 的名称
        /// </summary>
        [XmlElement("Name")]
        public string name;

        /// <summary>
        /// 编码格式
        /// </summary>
        [XmlElement("EncodingType")]
        public string encodingType;

        /// <summary>
        /// 前缀匹配，用来规定响应请求返回的文件前缀地址
        /// </summary>
        [XmlElement("Prefix")]
        public string prefix;

        /// <summary>
        /// 默认以 UTF-8 二进制顺序列出条目，所有列出条目从 marker 开始
        /// </summary>
        [XmlElement("Marker")]
        public string marker;

        /// <summary>
        /// 单次响应请求内返回结果的最大的条目数量
        /// </summary>
        [XmlElement("MaxKeys")]
        public int maxKeys;

        /// <summary>
        /// 响应请求条目是否被截断，布尔值：true，false
        /// </summary>
        [XmlElement("IsTruncated")]
        public bool isTruncated;

        /// <summary>
        /// 假如返回条目被截断，则返回 NextMarker 就是下一个条目的起点
        /// </summary>
        [XmlElement("NextMarker")]
        public string nextMarker;

        [XmlElement("Contents")]
        /// <summary>
        /// 对象元数据信息列表
        /// <see cref="Contents"/>
        /// </summary>
        public List<Contents> contentsList;

        /// <summary>
        /// 将 Prefix 到 delimiter 之间的相同路径归为一类，定义为 Common Prefix
        /// <see cref="CommonPrefixes"/>
        /// </summary>
        [XmlElement("CommonPrefixes")]
        public List<CommonPrefixes> commonPrefixesList;

        [XmlElement("Delimiter")]
        public string delimiter;


        public string GetInfo()
        {
            StringBuilder stringBuilder = new StringBuilder("{ListBucket:\n");

            stringBuilder.Append("Name:").Append(name).Append("\n");
            stringBuilder.Append("Encoding-Type:").Append(encodingType).Append("\n");
            stringBuilder.Append("Prefix:").Append(prefix).Append("\n");
            stringBuilder.Append("Marker:").Append(marker).Append("\n");
            stringBuilder.Append("MaxKeys:").Append(maxKeys).Append("\n");
            stringBuilder.Append("IsTruncated:").Append(isTruncated).Append("\n");
            stringBuilder.Append("NextMarker:").Append(nextMarker).Append("\n");

            if (contentsList != null)
            {

                foreach (Contents contents in contentsList)
                {

                    if (contents != null)
                    {
                        stringBuilder.Append(contents.GetInfo()).Append("\n");
                    }
                }
            }

            if (commonPrefixesList != null)
            {

                foreach (CommonPrefixes commonPrefixes in commonPrefixesList)
                {

                    if (commonPrefixes != null)
                    {
                        stringBuilder.Append(commonPrefixes.GetInfo()).Append("\n");
                    }
                }
            }

            stringBuilder.Append("Delimiter:").Append(delimiter).Append("\n");
            stringBuilder.Append("}");

            return stringBuilder.ToString();
        }

        public sealed class Contents
        {
            /// <summary>
            /// Object 的 Key
            /// </summary>
            [XmlElement("Key")]
            public string key;

            /// <summary>
            /// Object 最后被修改时间
            /// </summary>
            [XmlElement("LastModified")]
            public string lastModified;

            /// <summary>
            /// 文件的 eTag
            /// </summary>
            [XmlElement("ETag")]
            public string eTag;

            /// <summary>
            /// 文件大小，单位是 Byte
            /// </summary>
            [XmlElement("Size")]
            public long size;

            /// <summary>
            /// Bucket 持有者信息
            /// <see cref="Owner"/>
            /// </summary>
            [XmlElement("Owner")]
            public Owner owner;

            /// <summary>
            /// Object 的存储级别，枚举值：STANDARD，STANDARD_IA，ARCHIVE
            /// <see cref="COSXML.Common.CosStorageClass"/>
            /// </summary>
            [XmlElement("StorageClass")]
            public string storageClass;

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{Contents:\n");

                stringBuilder.Append("Key:").Append(key).Append("\n");
                stringBuilder.Append("LastModified:").Append(lastModified).Append("\n");
                stringBuilder.Append("ETag:").Append(eTag).Append("\n");
                stringBuilder.Append("Size:").Append(size).Append("\n");

                if (owner != null)
                {
                    stringBuilder.Append(owner.GetInfo()).Append("\n");
                }

                stringBuilder.Append("StorageClass:").Append(storageClass).Append("\n");
                stringBuilder.Append("}");

                return stringBuilder.ToString();
            }
        }

        public sealed class CommonPrefixes
        {
            /// <summary>
            /// 单条 Common 的前缀
            /// </summary>
            [XmlElement("Prefix")]
            public string prefix;

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{CommonPrefixes:\n");

                stringBuilder.Append("Prefix:").Append(prefix).Append("\n");
                stringBuilder.Append("}");

                return stringBuilder.ToString();
            }
        }

        public sealed class Owner
        {
            /// <summary>
            /// Bucket 的 AppID
            /// </summary>
            [XmlElement("ID")]
            public string id;

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{Owner:\n");

                stringBuilder.Append("Id:").Append(id).Append("\n");
                stringBuilder.Append("}");

                return stringBuilder.ToString();
            }
        }
    }
}

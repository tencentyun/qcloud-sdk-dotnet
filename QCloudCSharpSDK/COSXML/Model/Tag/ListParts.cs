using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace COSXML.Model.Tag
{
    [XmlRoot("ListPartsResult")]
    public sealed class ListParts
    {
        /// <summary>
        /// 分块上传的目标 Bucket
        /// </summary>
        [XmlElement("Bucket")]
        public string bucket;

        /// <summary>
        /// 规定返回值的编码方式
        /// </summary>
        [XmlElement("Encoding-type")]
        public string encodingType;

        /// <summary>
        /// Object 的名称
        /// </summary>
        [XmlElement("Key")]
        public string key;

        /// <summary>
        /// 本次分块上传的 uploadID
        /// </summary>
        [XmlElement("UploadId")]
        public string uploadId;

        /// <summary>
        /// 表示这些分块所有者的信息
        /// </summary>
        [XmlElement("Owner")]
        public Owner owner;

        /// <summary>
        /// 默认以 UTF-8 二进制顺序列出条目，所有列出条目从 marker 开始
        /// </summary>
        [XmlElement("PartNumberMarker")]
        public string partNumberMarker;

        /// <summary>
        /// 表示本次上传发起者的信息
        /// <see cref="Initiator"/>
        /// </summary>
        [XmlElement("Initiator")]
        public Initiator initiator;

        /// <summary>
        /// 表示这些分块的存储级别
        /// </summary>
        [XmlElement("StorageClass")]
        public string storageClass;

        /// <summary>
        /// 假如返回条目被截断，则返回 nextPartNumberMarker 就是下一个条目的起点
        /// </summary>
        [XmlElement("NextPartNumberMarker")]
        public string nextPartNumberMarker;

        /// <summary>
        /// 单次返回最大的条目数量
        /// </summary>
        [XmlElement("MaxParts")]
        public string maxParts;

        /// <summary>
        /// 返回条目是否被截断，布尔值：TRUE，FALSE
        /// </summary>
        [XmlElement("IsTruncated")]
        public bool isTruncated;

        /// <summary>
        /// 表示每一个块的信息
        /// <see cref="Part"/>
        /// </summary>
        [XmlElement("Part")]
        public List<Part> parts;

        public sealed class Owner
        {
            
            [XmlElement("ID")]
            public string id;
            
            [XmlElement("DisplayName")]
            public string disPlayName;
        }

        public sealed class Initiator
        {
            /// <summary>
            /// 创建者的一个唯一标识
            /// </summary>
            [XmlElement("ID")]
            public string id;

            /// <summary>
            /// 创建者的用户名描述
            /// </summary>
            [XmlElement("DisplayName")]
            public string disPlayName;
        }

        public sealed class Part
        {
            /// <summary>
            /// 块的编号
            /// </summary>
            [XmlElement("PartNumber")]
            public string partNumber;

            /// <summary>
            /// 块最后修改时间
            /// </summary>
            [XmlElement("LastModified")]
            public string lastModified;

            /// <summary>
            /// Object 块的 MD5 算法校验值
            /// </summary>
            [XmlElement("ETag")]
            public string eTag;

            /// <summary>
            /// 块大小，单位 Byte
            /// </summary>
            [XmlElement("Size")]
            public string size;
        }
    }
}

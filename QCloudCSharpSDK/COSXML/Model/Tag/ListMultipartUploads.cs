using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace COSXML.Model.Tag
{
    /// <summary>
    /// 所有分块上传的信息
    /// <see cref="https://cloud.tencent.com/document/product/436/7736"/>
    /// </summary>
    [XmlRoot("ListMultipartUploadsResult")]
    public sealed class ListMultipartUploads
    {
        /// <summary>
        /// 分块上传的目标 Bucket
        /// </summary>
        [XmlElement("Bucket")]
        public string bucket;

        /// <summary>
        /// 规定返回值的编码格式，合法值：url
        /// </summary>
        [XmlElement("Encoding-type")]
        public string encodingType;

        /// <summary>
        /// 列出条目从该 key 值开始
        /// </summary>
        [XmlElement("KeyMarker")]
        public string keyMarker;

        /// <summary>
        /// 列出条目从该 UploadId 值开始
        /// </summary>
        [XmlElement("UploadIdMarker")]
        public string uploadIdMarker;

        /// <summary>
        /// 假如返回条目被截断，则返回 NextKeyMarker 就是下一个条目的起点
        /// </summary>
        [XmlElement("NextKeyMarker")]
        public string nextKeyMarker;

        /// <summary>
        /// 假如返回条目被截断，则返回 UploadId 就是下一个条目的起点
        /// </summary>
        [XmlElement("NextUploadIdMarker")]
        public string nextUploadIdMarker;

        /// <summary>
        /// 设置最大返回的 multipart 数量，合法取值从 0 到 1000
        /// </summary>
        [XmlElement("MaxUploads")]
        public string maxUploads;

        /// <summary>
        /// 返回条目是否被截断，布尔值：TRUE，FALSE
        /// </summary>
        [XmlElement("IsTruncated")]
        public bool isTruncated;

        /// <summary>
        /// 限定返回的 Object key 必须以 Prefix 作为前缀。
        /// 注意使用 prefix 查询时，返回的 key 中仍会包含 Prefix
        /// </summary>
        [XmlElement("Prefix")]
        public string prefix;

        /// <summary>
        /// 定界符为一个符号，
        /// 对 object 名字包含指定前缀且第一次出现 delimiter 字符之间的 object 作为一组元素：common prefix。
        /// 如果没有 prefix，则从路径起点开始
        /// </summary>
        [XmlElement("Delimiter")]
        public string delimiter;

        /// <summary>
        /// 每个 Upload 的信息
        /// <see cref="Upload"/>
        /// </summary>
        [XmlElement("Upload")]
        public List<Upload> uploads;

        /// <summary>
        /// 将 prefix 到 delimiter 之间的相同路径归为一类，定义为 Common Prefix
        /// <see cref="CommonPrefixes"/>
        /// </summary>
        [XmlElement("CommonPrefixes")]
        public List<CommonPrefixes> commonPrefixesList;


        public string GetInfo()
        {
            StringBuilder stringBuilder = new StringBuilder("{ListMultipartUploads:\n");

            stringBuilder.Append("Bucket:").Append(bucket).Append("\n");
            stringBuilder.Append("EncodingType:").Append(encodingType).Append("\n");
            stringBuilder.Append("KeyMarker:").Append(keyMarker).Append("\n");
            stringBuilder.Append("UploadIdMarker:").Append(uploadIdMarker).Append("\n");
            stringBuilder.Append("NextKeyMarker:").Append(nextKeyMarker).Append("\n");
            stringBuilder.Append("NextUploadIdMarker:").Append(nextUploadIdMarker).Append("\n");
            stringBuilder.Append("MaxUploads:").Append(maxUploads).Append("\n");
            stringBuilder.Append("IsTruncated:").Append(isTruncated).Append("\n");
            stringBuilder.Append("Prefix:").Append(prefix).Append("\n");
            stringBuilder.Append("Delimiter:").Append(delimiter).Append("\n");

            if (uploads != null)
            {

                foreach (Upload upload in uploads)
                {

                    if (upload != null)
                    {
                        stringBuilder.Append(upload.GetInfo()).Append("\n");
                    }
                }
            }

            if (commonPrefixesList != null)
            {

                foreach (CommonPrefixes commonPrefix in commonPrefixesList)
                {

                    if (commonPrefix != null)
                    {
                        stringBuilder.Append(commonPrefix.GetInfo()).Append("\n");
                    }
                }
            }

            stringBuilder.Append("}");

            return stringBuilder.ToString();
        }

        public sealed class Upload
        {
            /// <summary>
            /// Object 的名称
            /// </summary>
            [XmlElement("Key")]
            public string key;

            /// <summary>
            /// 示本次分块上传的 ID
            /// </summary>
            [XmlElement("UploadId")]
            public string uploadID;

            /// <summary>
            /// 用来表示分块的存储级别，枚举值：STANDARD，STANDARD_IA，ARCHIVE
            /// </summary>
            [XmlElement("StorageClass")]
            public string storageClass;

            /// <summary>
            /// 用来表示本次上传发起者的信息
            /// <see cref="initiator"/>
            /// </summary>
            [XmlElement("Initiator")]
            public Initiator initiator;

            /// <summary>
            /// 用来表示这些分块所有者的信息
            /// <see cref="Owner"/>
            /// </summary>
            [XmlElement("Owner")]
            public Owner owner;

            /// <summary>
            /// 分块上传的起始时间
            /// </summary>
            [XmlElement("Initiated")]
            public string initiated;


            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{Upload:\n");

                stringBuilder.Append("Key:").Append(key).Append("\n");
                stringBuilder.Append("UploadID:").Append(uploadID).Append("\n");
                stringBuilder.Append("StorageClass:").Append(storageClass).Append("\n");

                if (initiator != null)
                {
                    stringBuilder.Append(initiator.GetInfo()).Append("\n");
                }

                if (owner != null)
                {
                    stringBuilder.Append(owner.GetInfo()).Append("\n");
                }

                stringBuilder.Append("Initiated:").Append(initiated).Append("\n");
                stringBuilder.Append("}");

                return stringBuilder.ToString();
            }
        }

        public sealed class CommonPrefixes
        {
            /// <summary>
            /// 显示具体的 CommonPrefixes
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

        public sealed class Initiator
        {
            /// <summary>
            /// 用户唯一的 CAM 身份 UIN
            /// </summary>
            [XmlElement("UIN")]
            public string uin;

            /// <summary>
            /// 用户唯一的 CAM 身份 ID
            /// </summary>
            [XmlElement("ID")]
            public string id;

            /// <summary>
            /// 用户身份 ID 的简称（UIN）
            /// </summary>
            [XmlElement("DisplayName")]
            public string displayName;

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{Initiator:\n");

                stringBuilder.Append("Uin:").Append(uin).Append("\n");
                stringBuilder.Append("Id:").Append(id).Append("\n");
                stringBuilder.Append("DisplayName:").Append(displayName).Append("\n");
                stringBuilder.Append("}");

                return stringBuilder.ToString();
            }
        }

        public sealed class Owner
        {
            /// <summary>
            /// 用户唯一的 CAM 身份 UID
            /// </summary>
            public string uid;

            /// <summary>
            /// 用户唯一的 CAM 身份 ID
            /// </summary>
            [XmlElement("ID")]
            public string id;

            /// <summary>
            /// 用户身份 ID 的简称（UIN）
            /// </summary>
            [XmlElement("DisplayName")]
            public string displayName;

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{Owner:\n");

                stringBuilder.Append("Uid:").Append(uid).Append("\n");
                stringBuilder.Append("Id:").Append(id).Append("\n");
                stringBuilder.Append("DisplayName:").Append(displayName).Append("\n");
                stringBuilder.Append("}");

                return stringBuilder.ToString();
            }
        }
    }
}

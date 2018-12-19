using System;
using System.Collections.Generic;

using System.Text;
/**
* Copyright (c) 2018 Tencent Cloud. All rights reserved.
* 11/5/2018 12:03:35 PM
* bradyxiao
*/
namespace COSXML.Model.Tag
{
    public sealed class ListParts
    {
        /// <summary>
        /// 分块上传的目标 Bucke
        /// </summary>
        public string bucket;
        /// <summary>
        /// 规定返回值的编码方式
        /// </summary>
        public string encodingType;
        /// <summary>
        /// Object 的名称
        /// </summary>
        public string key;
        /// <summary>
        /// 本次分块上传的 uploadID
        /// </summary>
        public string uploadId;
        /// <summary>
        /// 表示这些分块所有者的信息
        /// </summary>
        public Owner owner;
        /// <summary>
        /// 默认以 UTF-8 二进制顺序列出条目，所有列出条目从 marker 开始
        /// </summary>
        public string partNumberMarker;
        /// <summary>
        /// 表示本次上传发起者的信息
        /// <see cref="Initiator"/>
        /// </summary>
        public Initiator initiator;
        /// <summary>
        /// 表示这些分块的存储级别
        /// </summary>
        public string storageClass;
        /// <summary>
        /// 假如返回条目被截断，则返回 nextPartNumberMarker 就是下一个条目的起点
        /// </summary>
        public string nextPartNumberMarker;
        /// <summary>
        /// 单次返回最大的条目数量
        /// </summary>
        public string maxParts;
        /// <summary>
        /// 返回条目是否被截断，布尔值：TRUE，FALSE
        /// </summary>
        public bool isTruncated;
        /// <summary>
        /// 表示每一个块的信息
        /// <see cref="Part"/>
        /// </summary>
        public List<Part> parts;

        
        public string GetInfo()
        {
            StringBuilder stringBuilder = new StringBuilder("{ListParts:\n");
            stringBuilder.Append("Bucket:").Append(bucket).Append("\n");
            stringBuilder.Append("Encoding-Type:").Append(encodingType).Append("\n");
            stringBuilder.Append("Key:").Append(key).Append("\n");
            stringBuilder.Append("UploadId:").Append(uploadId).Append("\n");
            if(owner != null)stringBuilder.Append(owner.GetInfo()).Append("\n");
            stringBuilder.Append("PartNumberMarker:").Append(partNumberMarker).Append("\n");
            if(initiator != null) stringBuilder.Append(initiator.GetInfo()).Append("\n");
            stringBuilder.Append("StorageClass:").Append(storageClass).Append("\n");
            stringBuilder.Append("NextPartNumberMarker:").Append(nextPartNumberMarker).Append("\n");
            stringBuilder.Append("MaxParts:").Append(maxParts).Append("\n");
            stringBuilder.Append("IsTruncated:").Append(isTruncated).Append("\n");
            if(parts != null)
            {
                foreach(Part part in parts)
                {
                    if(part != null)stringBuilder.Append(part.GetInfo()).Append("\n");
                }
            }
            stringBuilder.Append("}");
            return stringBuilder.ToString();
        }

        public sealed class Owner
        {
            /**
             * 创建者的一个唯一标识
             */
            public string id;
            /**
             * 创建者的用户名描述
             */
            public string disPlayName;

            
            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{Owner:\n");
                stringBuilder.Append("Id:").Append(id).Append("\n");
                stringBuilder.Append("DisPlayName:").Append(disPlayName).Append("\n");
                stringBuilder.Append("}");
                return stringBuilder.ToString();
            }
        }

        public sealed class Initiator
        {
            /// <summary>
            /// 创建者的一个唯一标识
            /// </summary>
            public string id;
            /// <summary>
            /// 创建者的用户名描述
            /// </summary>
            public string disPlayName;

            
            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{Initiator:\n");
                stringBuilder.Append("Id:").Append(id).Append("\n");
                stringBuilder.Append("DisPlayName:").Append(disPlayName).Append("\n");
                stringBuilder.Append("}");
                return stringBuilder.ToString();
            }
        }

        public sealed class Part
        {
            /// <summary>
            /// 块的编号
            /// </summary>
            public string partNumber;
            /// <summary>
            /// 块最后修改时间
            /// </summary>
            public string lastModified;
            /// <summary>
            /// Object 块的 MD5 算法校验值
            /// </summary>
            public string eTag;
            /// <summary>
            /// 块大小，单位 Byte
            /// </summary>
            public string size;

            
            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{Part:\n");
                stringBuilder.Append("PartNumber:").Append(partNumber).Append("\n");
                stringBuilder.Append("LastModified:").Append(lastModified).Append("\n");
                stringBuilder.Append("ETag:").Append(eTag).Append("\n");
                stringBuilder.Append("Size:").Append(size).Append("\n");
                stringBuilder.Append("}");
                return stringBuilder.ToString();
            }
        }
    }
}

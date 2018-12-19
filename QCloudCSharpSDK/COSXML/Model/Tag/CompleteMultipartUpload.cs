using System;
using System.Collections.Generic;

using System.Text;
/**
* Copyright (c) 2018 Tencent Cloud. All rights reserved.
* 11/5/2018 11:51:33 AM
* bradyxiao
*/
namespace COSXML.Model.Tag
{
    /// <summary>
    /// 本次分块上传的所有信息
    /// <see cref="https://cloud.tencent.com/document/product/436/7742"/>
    /// </summary>
    public sealed class CompleteMultipartUpload
    {
        /// <summary>
        /// 本次分块上传中每个块的信息
        /// <see cref="Part"/>
        /// </summary>
        public List<Part> parts;

        public string GetInfo()
        {
            StringBuilder stringBuilder = new StringBuilder("{CompleteMultipartUpload:\n");
            if (parts != null)
            {
                foreach (Part part in parts)
                {
                    stringBuilder.Append(part.GetInfo());
                }
                
            }
            stringBuilder.Append("}");
            return stringBuilder.ToString();
        }

        /**
         * 本块编号 和 eTag值
         */
        public sealed class Part
        {
            /// <summary>
            /// 块编号
            /// </summary>
            public int partNumber;
            /// <summary>
            /// 每个块文件的 eTag 值
            /// </summary>
            public string eTag;

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{Part:\n");
                stringBuilder.Append("PartNumber:").Append(partNumber).Append("\n");
                stringBuilder.Append("ETag:").Append(eTag).Append("\n");
                stringBuilder.Append("}");
                return stringBuilder.ToString();
            }
        }
    }
}

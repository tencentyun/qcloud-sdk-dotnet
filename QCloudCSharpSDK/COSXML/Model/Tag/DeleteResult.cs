using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;

namespace COSXML.Model.Tag
{
    /// <summary>
    /// 本次删除返回结果的方式和目标 Object
    /// <see cref="https://cloud.tencent.com/document/product/436/8289"/>
    /// </summary>
    [XmlRoot("DeleteResult")]
    public sealed class DeleteResult
    {
        /// <summary>
        /// 本次删除的成功 Object 信息
        /// <see cref="Deleted"/>
        /// </summary>
        [XmlElement("Deleted")]
        public List<Deleted> deletedList;

        /// <summary>
        /// 本次删除的失败 Object 信息
        /// <see cref="Error"/>
        /// </summary>
        [XmlElement("Error")]
        public List<Error> errorList;


        public string GetInfo()
        {
            StringBuilder stringBuilder = new StringBuilder("{DeleteResult:\n");

            if (deletedList != null)
            {

                foreach (Deleted deleted in deletedList)
                {

                    if (deleted != null)
                    {
                        stringBuilder.Append(deleted.GetInfo()).Append("\n");
                    }
                }
            }

            if (errorList != null)
            {

                foreach (Error error in errorList)
                {

                    if (error != null)
                    {
                        stringBuilder.Append(error.GetInfo()).Append("\n");
                    }
                }
            }

            stringBuilder.Append("}");

            return stringBuilder.ToString();
        }

        public sealed class Deleted
        {
            /// <summary>
            /// Object 的名称
            /// </summary>
            [XmlElement("Key")]
            public string key;

            /// <summary>
            /// Object 的版本Id
            /// </summary>
            [XmlElement("VersionId")]
            public string versionId;

            /// <summary>
            /// deleete marker
            /// </summary>
            [XmlElement("DeleteMarker")]
            public string deleteMarker;

            /// <summary>
            /// delete marker versionId
            /// </summary>
            [XmlElement("DeleteMarkerVersionId")]
            public string deleteMarkerVersionId;

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{Deleted:\n");

                stringBuilder.Append("Key:").Append(key).Append("\n");
                stringBuilder.Append("VersionId:").Append(versionId).Append("\n");
                stringBuilder.Append("DeleteMarker:").Append(deleteMarker).Append("\n");
                stringBuilder.Append("DeleteMarkerVersionId:").Append(deleteMarkerVersionId).Append("\n");
                stringBuilder.Append("}");

                return stringBuilder.ToString();
            }
        }

        public sealed class Error
        {
            /// <summary>
            /// 删除失败的 Object 的名称
            /// </summary>
            [XmlElement("Key")]
            public string key;

            /// <summary>
            /// 删除失败的错误代码
            /// </summary>
            [XmlElement("Code")]
            public string code;

            /// <summary>
            /// 删除失败的错误信息
            /// </summary>
            [XmlElement("Message")]
            public string message;

            /// <summary>
            /// 删除失败的 Object 的 版本Id
            /// </summary>
            [XmlElement("VersionId")]
            public string versionId;


            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{Error:\n");

                stringBuilder.Append("Key:").Append(key).Append("\n");
                stringBuilder.Append("Code:").Append(code).Append("\n");
                stringBuilder.Append("Message:").Append(message).Append("\n");
                stringBuilder.Append("VersionId:").Append(versionId).Append("\n");
                stringBuilder.Append("}");

                return stringBuilder.ToString();
            }
        }
    }
}

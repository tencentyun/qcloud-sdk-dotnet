using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;

namespace COSXML.Model.Tag
{
    /// <summary>
    /// ACL权限
    /// <see cref="https://cloud.tencent.com/document/product/436/7733"/>
    /// </summary>
    [XmlRoot]
    public sealed class AccessControlPolicy
    {
        /// <summary>
        /// Bucket 持有者信息
        /// <see cref="Owner"/>
        /// </summary>
        [XmlElement("Owner")]
        public Owner owner;

        /// <summary>
        /// 被授权者信息与权限信息
        /// <see cref="AccessControlList"/>
        /// </summary>
        [XmlElement("AccessControlList")]
        public AccessControlList accessControlList;

        public string GetInfo()
        {
            StringBuilder stringBuilder = new StringBuilder("{AccessControlPolicy:\n");

            if (owner != null)
            {
                stringBuilder.Append(owner.GetInfo()).Append("\n");
            }

            if (accessControlList != null)
            {
                stringBuilder.Append(accessControlList.GetInfo()).Append("\n");
            }

            stringBuilder.Append("}");

            return stringBuilder.ToString();
        }

        public sealed class Owner
        {
            /// <summary>
            /// Bucket 持有者 ID
            /// 格式：qcs::cam::uin/<OwnerUin>:uin/<SubUin> 如果是根帐号，<OwnerUin> 和 <SubUin> 是同一个值
            /// </summary>
            [XmlElement("ID")]
            public string id;

            /// <summary>
            /// Bucket 持有者
            /// </summary>
            [XmlElement("DisplayName")]
            public string displayName;

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{Owner:\n");

                stringBuilder.Append("Id:").Append(id).Append("\n");
                stringBuilder.Append("}");

                return stringBuilder.ToString();
            }
        }


        public sealed class AccessControlList
        {
            /// <summary>
            /// 单个 Bucket 的授权信息。一个 AccessControlList 可以拥有 100 条 Grant
            /// <see cref="Grant"/>
            /// </summary>
            [XmlElement("Grant")]
            public List<Grant> grants;

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{AccessControlList:\n");

                if (grants != null)
                {

                    foreach (Grant grant in grants)
                    {

                        if (grant != null)
                        {
                            stringBuilder.Append(grant.GetInfo()).Append("\n");
                        }
                    }
                }

                stringBuilder.Append("}");

                return stringBuilder.ToString();
            }
        }

        public sealed class Grant
        {
            /// <summary>
            /// 说明被授权者的信息,
            /// <see cref="Grantee"/>
            /// </summary>
            [XmlElement("Grantee")]
            public Grantee grantee;

            /// <summary>
            /// 指明授予被授权者的权限信息
            /// <see cref="COSXML.Common.CosGrantPermission"/>
            /// </summary>
            [XmlElement("Permission")]
            public string permission;

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{Grant:\n");

                if (grantee != null)
                {
                    stringBuilder.Append(grantee.GetInfo()).Append("\n");
                }

                stringBuilder.Append("Permission:").Append(permission).Append("\n");
                stringBuilder.Append("}");

                return stringBuilder.ToString();
            }
        }

        public sealed class Grantee
        {
            /// <summary>
            /// 用户的 ID，如果是根帐号，格式为：qcs::cam::uin/<OwnerUin>:uin/<OwnerUin>  
            /// 如果是子帐号，格式为： qcs::cam::uin/<OwnerUin>:uin/<SubUin>
            /// </summary>
            [XmlElement("ID")]
            public string id;

            /// <summary>
            /// 或 http://cam.qcloud.com/groups/global/AllUsers （指代所有用户）.
            /// </summary>
            [XmlElement("URI")]
            public string uri;

            /// <summary>
            /// 或 qcs::cam::anyone:anyone （指代所有用户）.
            /// </summary>
            [XmlElement("DisplayName")]
            public string displayName;

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{Grantee:\n");

                if (uri != null)
                {
                    stringBuilder.Append("URI:").Append(uri).Append("\n");
                }

                if (id != null)
                {
                    stringBuilder.Append("Id:").Append(id).Append("\n");
                }

                stringBuilder.Append("}");

                return stringBuilder.ToString();
            }
        }
    }


}

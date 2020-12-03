using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;

namespace COSXML.Model.Tag
{
    [XmlRoot("LifecycleConfiguration")]
    public sealed class LifecycleConfiguration
    {
        /// <summary>
        /// 规则描述
        /// <see cref="Rule"/>
        /// </summary>
        [XmlElement("Rule")]
        public List<Rule> rules;

        public string GetInfo()
        {
            StringBuilder stringBuilder = new StringBuilder("{LifecycleConfiguration:\n");

            if (rules != null)
            {

                foreach (Rule rule in rules)
                {

                    if (rule != null)
                    {
                        stringBuilder.Append(rule.GetInfo()).Append("\n");
                    }
                }
            }

            stringBuilder.Append("}");

            return stringBuilder.ToString();
        }

        public sealed class Rule
        {
            /// <summary>
            /// 用于唯一标识规则，长度不能超过 255 个字符
            /// </summary>
            [XmlElement("ID")]
            public string id;

            /// <summary>
            /// Filter 用于描述规则影响的 Object 集合
            /// <see cref="Filter"/>
            /// </summary>
            [XmlElement("Filter")]
            public Filter filter;

            /// <summary>
            /// 指明规则是否启用，枚举值：Enabled，Disabled
            /// </summary>
            [XmlElement("Status")]
            public string status;

            /// <summary>
            /// 规则转换属性，对象何时转换为 Standard_IA 或 Archive
            /// <see cref="Transition"/>
            /// </summary>
            [XmlElement("Transition")]
            public Transition transition;

            /// <summary>
            /// 规则过期属性
            /// <see cref="Expiration"/>
            /// </summary>
            [XmlElement("Expiration")]
            public Expiration expiration;

            /// <summary>
            /// 指明非当前版本对象何时过期
            /// <see cref="NoncurrentVersionExpiration"/>
            /// </summary>
            [XmlElement("NoncurrentVersionExpiration")]
            public NoncurrentVersionExpiration noncurrentVersionExpiration;

            /// <summary>
            /// 指明非当前版本对象何时转换为 STANDARD_IA 或 ARCHIVE
            /// <see cref="NoncurrentVersionTransition"/>
            /// </summary>
            [XmlElement("NoncurrentVersionTransition")]
            public NoncurrentVersionTransition noncurrentVersionTransition;

            /// <summary>
            /// 设置允许分片上传保持运行的最长时间
            /// <see cref="AbortIncompleteMultiUpload"/>
            /// </summary>
            [XmlElement("AbortIncompleteMultipartUpload")]
            public AbortIncompleteMultiUpload abortIncompleteMultiUpload;

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{Rule:\n");

                stringBuilder.Append("Id:").Append(id).Append("\n");

                if (filter != null)
                {
                    stringBuilder.Append(filter.GetInfo()).Append("\n");
                }

                stringBuilder.Append("Status:").Append(status).Append("\n");

                if (transition != null)
                {
                    stringBuilder.Append(transition.GetInfo()).Append("\n");
                }

                if (expiration != null)
                {
                    stringBuilder.Append(expiration.GetInfo()).Append("\n");
                }

                if (noncurrentVersionExpiration != null)
                {
                    stringBuilder.Append(noncurrentVersionExpiration.GetInfo()).Append("\n");
                }

                if (noncurrentVersionTransition != null)
                {
                    stringBuilder.Append(noncurrentVersionTransition.GetInfo()).Append("\n");
                }

                if (abortIncompleteMultiUpload != null)
                {
                    stringBuilder.Append(abortIncompleteMultiUpload.GetInfo()).Append("\n");
                }

                stringBuilder.Append("}");

                return stringBuilder.ToString();
            }

        }

        public sealed class Filter
        {
            /// <summary>
            /// 指定规则所适用的前缀。匹配前缀的对象受该规则影响，Prefix 最多只能有一个
            /// </summary>
            [XmlElement("Prefix")]
            public string prefix;

            [XmlElement("And")]
            public FilterAnd filterAnd;

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{Filter:\n");

                stringBuilder.Append("Prefix:").Append(prefix).Append("\n");

                if (filterAnd != null)
                {
                    stringBuilder.Append(filterAnd.GetInfo()).Append("\n");
                }

                stringBuilder.Append("}");

                return stringBuilder.ToString();
            }
        }

        public sealed class FilterAnd
        {
            [XmlElement("Prefix")]
            public string prefix;

            [XmlElement("Tag")]
            public List<Tagging.Tag> tags;

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{And:\n");

                stringBuilder.Append("Prefix:").Append(prefix).Append("\n");
                stringBuilder.Append("}");

                return stringBuilder.ToString();
            }
        }

        public sealed class Transition
        {
            /// <summary>
            /// 指明规则对应的动作在对象最后的修改日期过后多少天操作
            /// </summary>
            [XmlElement("Days")]
            public int days;

            /// <summary>
            /// 指明规则对应的动作在何时操作
            /// </summary>
            [XmlElement("Date")]
            public string date;

            /// <summary>
            /// 指定 Object 转储到的目标存储类型，枚举值： STANDARD_IA, ARCHIVE
            /// <see cref="COSXML.Common.CosStorageClass"/>
            /// </summary>
            [XmlElement("StorageClass")]
            public string storageClass;

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{Transition:\n");

                stringBuilder.Append("Days:").Append(days).Append("\n");
                stringBuilder.Append("Date:").Append(date).Append("\n");
                stringBuilder.Append("StorageClass:").Append(storageClass).Append("\n");
                stringBuilder.Append("}");

                return stringBuilder.ToString();
            }

            public bool ShouldSerializedays()
            {
                return days > 0;
            }
        }

        public sealed class Expiration
        {
            /// <summary>
            /// 指明规则对应的动作在何时操作
            /// </summary>
            [XmlElement("Date")]
            public string date;

            /// <summary>
            /// 指明规则对应的动作在对象最后的修改日期过后多少天操作
            /// </summary>
            [XmlElement("Days")]
            public int days;

            /// <summary>
            /// 删除过期对象删除标记，取值为 true，false
            /// </summary>
            [XmlElement("ExpiredObjectDeleteMarker")]
            public bool expiredObjectDeleteMarker;

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{Expiration:\n");

                stringBuilder.Append("Days:").Append(days).Append("\n");
                stringBuilder.Append("Date:").Append(date).Append("\n");
                stringBuilder.Append("ExpiredObjectDeleteMarker:").Append(expiredObjectDeleteMarker).Append("\n");
                stringBuilder.Append("}");

                return stringBuilder.ToString();
            }

            public bool ShouldSerializedays()
            {
                return days > 0;
            }

            public bool ShouldSerializeexpiredObjectDeleteMarker()
            {
                return expiredObjectDeleteMarker;
            }
        }

        public sealed class NoncurrentVersionExpiration
        {
            /// <summary>
            /// 指明规则对应的动作在对象变成非当前版本多少天后执行
            /// </summary>
            [XmlElement("NoncurrentDays")]
            public int noncurrentDays;

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{NoncurrentVersionExpiration:\n");

                stringBuilder.Append("NoncurrentDays:").Append(noncurrentDays).Append("\n");
                stringBuilder.Append("}");

                return stringBuilder.ToString();
            }
        }

        public sealed class NoncurrentVersionTransition
        {
            /// <summary>
            /// 指明规则对应的动作在对象变成非当前版本多少天后执行
            /// </summary>
            [XmlElement("NoncurrentDays")]
            public int noncurrentDays;

            /// <summary>
            /// 指定 Object 转储到的目标存储类型
            /// <see cref="COSXML.Common.CosStorageClass"/>
            /// </summary>
            [XmlElement("StorageClass")]
            public string storageClass;

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{NoncurrentVersionTransition:\n");

                stringBuilder.Append("NoncurrentDays:").Append(noncurrentDays).Append("\n");
                stringBuilder.Append("StorageClass:").Append(storageClass).Append("\n");
                stringBuilder.Append("}");

                return stringBuilder.ToString();
            }
        }

        public sealed class AbortIncompleteMultiUpload
        {
            /// <summary>
            /// 指明分片上传开始后多少天内必须完成上传
            /// </summary>
            [XmlElement("DaysAfterInitiation")]
            public int daysAfterInitiation;

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{AbortIncompleteMultiUpload:\n");

                stringBuilder.Append("DaysAfterInitiation:").Append(daysAfterInitiation).Append("\n");
                stringBuilder.Append("}");

                return stringBuilder.ToString();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;

namespace COSXML.Model.Tag
{
    [XmlRoot("ReplicationConfiguration")]
    public sealed class ReplicationConfiguration
    {
        [XmlElement("Role")]
        public string role;

        [XmlElement("Rule")]
        public List<Rule> rules;

        public string GetInfo()
        {
            StringBuilder stringBuilder = new StringBuilder("{ReplicationConfiguration:\n");

            stringBuilder.Append("Role:").Append(role).Append("\n");

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
            [XmlElement("ID")]
            public string id;

            [XmlElement("Status")]
            public string status;

            [XmlElement("Prefix")]
            public string prefix;

            [XmlElement("Destination")]
            public Destination destination;

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{Rule:\n");

                stringBuilder.Append("Id:").Append(id).Append("\n");
                stringBuilder.Append("Status:").Append(status).Append("\n");
                stringBuilder.Append("Prefix:").Append(prefix).Append("\n");

                if (destination != null)
                {
                    stringBuilder.Append(destination.GetInfo()).Append("\n");
                }

                stringBuilder.Append("}");

                return stringBuilder.ToString();
            }
        }

        public sealed class Destination
        {
            [XmlElement("Bucket")]
            public string bucket;

            [XmlElement("StorageClass")]
            public string storageClass;

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{Destination:\n");

                stringBuilder.Append("Bucket:").Append(bucket).Append("\n");
                stringBuilder.Append("StorageClass:").Append(storageClass).Append("\n");
                stringBuilder.Append("}");

                return stringBuilder.ToString();
            }
        }

    }
}

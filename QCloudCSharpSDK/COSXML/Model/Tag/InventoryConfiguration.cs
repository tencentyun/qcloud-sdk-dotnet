using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace COSXML.Model.Tag
{
    [XmlRoot]
    public sealed class InventoryConfiguration
    {
        [XmlElement("Id")]
        public string id;

        [XmlElement("IsEnabled")]
        public bool isEnabled;

        [XmlElement("IncludedObjectVersions")]
        public string includedObjectVersions;

        [XmlElement("Filter")]
        public Filter filter;

        [XmlElement("OptionalFields")]
        public OptionalFields optionalFields;

        [XmlElement("Schedule")]
        public Schedule schedule;

        [XmlElement("Destination")]
        public Destination destination;


        public string GetInfo()
        {
            StringBuilder stringBuilder = new StringBuilder("{InventoryConfiguration:\n");

            stringBuilder.Append("Id").Append(id).Append("\n");
            stringBuilder.Append("IsEnabled:").Append(isEnabled).Append("\n");

            if (destination != null)
            {
                stringBuilder.Append(destination.GetInfo()).Append("\n");
            }

            if (schedule != null)
            {
                stringBuilder.Append(schedule.GetInfo()).Append("\n");
            }

            if (filter != null)
            {
                stringBuilder.Append(filter.GetInfo()).Append("\n");
            }

            stringBuilder.Append("IncludedObjectVersions:").Append(includedObjectVersions).Append("\n");

            if (optionalFields != null)
            {
                stringBuilder.Append(optionalFields.GetInfo()).Append("\n");
            }

            stringBuilder.Append("}");

            return stringBuilder.ToString();
        }

        public sealed class Filter
        {
            [XmlElement("Prefix")]
            public string prefix;

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{Filter:\n");

                stringBuilder.Append("Prefix:").Append(prefix).Append("\n");
                stringBuilder.Append("}");

                return stringBuilder.ToString();
            }
        }

        public sealed class OptionalFields
        {
            [XmlElement("Field")]
            public List<string> fields;

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{OptionalFields:\n");

                if (fields != null)
                {

                    foreach (string field in fields)
                    {
                        stringBuilder.Append("Field:").Append(field).Append("\n");
                    }
                }

                stringBuilder.Append("}");

                return stringBuilder.ToString();
            }
        }

        public sealed class Schedule
        {
            [XmlElement("Frequency")]
            public string frequency;

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{Schedule:\n");

                stringBuilder.Append("Frequency:").Append(frequency).Append("\n");
                stringBuilder.Append("}");

                return stringBuilder.ToString();
            }
        }

        public sealed class Destination
        {
            [XmlElement("COSBucketDestination")]
            public COSBucketDestination cosBucketDestination;

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{Destination:\n");

                if (cosBucketDestination != null)
                {
                    stringBuilder.Append(cosBucketDestination.GetInfo()).Append("\n");
                }

                stringBuilder.Append("}");

                return stringBuilder.ToString();
            }
        }

        public sealed class COSBucketDestination
        {
            [XmlElement("Format")]
            public string format;

            [XmlElement("AccountId")]
            public string accountId;

            [XmlElement("Bucket")]
            public string bucket;

            [XmlElement("Prefix")]
            public string prefix;

            [XmlElement("Encryption")]
            public Encryption encryption;

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{COSBucketDestination:\n");

                stringBuilder.Append("Format:").Append(format).Append("\n");
                stringBuilder.Append("AccountId:").Append(accountId).Append("\n");
                stringBuilder.Append("Bucket:").Append(bucket).Append("\n");
                stringBuilder.Append("Prefix:").Append(prefix).Append("\n");

                if (encryption != null)
                {
                    stringBuilder.Append(encryption.GetInfo()).Append("\n");
                }

                stringBuilder.Append("}");

                return stringBuilder.ToString();
            }
        }

        public sealed class Encryption
        {
            [XmlElement("SSE-COS")]
            public string sSECOS;

            public String GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{Encryption:\n");

                stringBuilder.Append("SSE-COS:").Append(sSECOS).Append("\n");
                stringBuilder.Append("}");

                return stringBuilder.ToString();
            }
        }


    }
}
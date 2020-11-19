using System;
using System.Collections.Generic;
using System.Text;

namespace COSXML.Model.Tag
{
    public sealed class InventoryConfiguration
    {
        public string id;

        public bool isEnabled;

        public string includedObjectVersions;

        public Filter filter;

        public OptionalFields optionalFields;

        public Schedule schedule;

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
            public string format;

            public string accountId;

            public string bucket;

            public string prefix;

            public Encryption encryption;

            public void setBucket(String region, String bucket)
            {

                if (region != null && bucket != null)
                {
                    this.bucket = String.Format("qcs::cos:%s::%s", region, bucket);
                }
            }

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{COSBucketDestination:\n");

                stringBuilder.Append("Format:").Append(format).Append("\n"); ;
                stringBuilder.Append("AccountId:").Append(accountId).Append("\n"); ;
                stringBuilder.Append("Bucket:").Append(bucket).Append("\n"); ;
                stringBuilder.Append("Prefix:").Append(prefix).Append("\n"); ;

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
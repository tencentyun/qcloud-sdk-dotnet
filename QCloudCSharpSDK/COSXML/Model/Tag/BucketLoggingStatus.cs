using System;
using System.Xml.Serialization;
using System.Text;

namespace COSXML.Model.Tag
{
    [XmlRoot("BucketLoggingStatus")]
    public sealed class BucketLoggingStatus
    {
        [XmlElement("LoggingEnabled")]
        public LoggingEnabled loggingEnabled;

        public string GetInfo()
        {
            StringBuilder stringBuilder = new StringBuilder("{BucketLoggingStatus:\n");

            if (loggingEnabled != null)
            {
                stringBuilder.Append(loggingEnabled.ToString()).Append("\n");
            }

            stringBuilder.Append("}");

            return stringBuilder.ToString();
        }

        public sealed class LoggingEnabled
        {
            [XmlElement("TargetBucket")]
            public string targetBucket;

            [XmlElement("TargetPrefix")]
            public string targetPrefix;

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{LoggingEnabled:\n");

                stringBuilder.Append("TargetBucket:").Append(targetBucket).Append("\n");
                stringBuilder.Append("TargetPrefix:").Append(targetPrefix).Append("\n");
                stringBuilder.Append("}");

                return stringBuilder.ToString();
            }
        }
    }
}

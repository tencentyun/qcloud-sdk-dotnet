using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSXML.Model.Tag
{
    public sealed class BucketLoggingStatus
    {
        public LoggingEnabled loggingEnabled;

        public string GetInfo()
        {
            StringBuilder stringBuilder = new StringBuilder("{BucketLoggingStatus:\n");
            if (loggingEnabled != null) stringBuilder.Append(loggingEnabled.ToString()).Append("\n");
            stringBuilder.Append("}");
            return stringBuilder.ToString();
        }

        public sealed class LoggingEnabled
        {
            public string targetBucket;
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

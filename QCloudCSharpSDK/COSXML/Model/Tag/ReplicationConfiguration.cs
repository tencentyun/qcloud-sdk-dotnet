using System;
using System.Collections.Generic;

using System.Text;

namespace COSXML.Model.Tag
{
    public sealed class ReplicationConfiguration
    {
        public string role;
        public List<Rule> rules;

        public string GetInfo()
        {
            StringBuilder stringBuilder = new StringBuilder("{ReplicationConfiguration:\n");
            stringBuilder.Append("Role:").Append(role).Append("\n");
            if(rules != null)
            {
                foreach(Rule rule in rules)
                {
                    if (rule != null) stringBuilder.Append(rule.GetInfo()).Append("\n");
                }
            }
            stringBuilder.Append("}");
            return stringBuilder.ToString();
        }

        public sealed class Rule
        {
            public string id;
            public string status;
            public string prefix;
            public Destination destination;

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{Rule:\n");
                stringBuilder.Append("Id:").Append(id).Append("\n");
                stringBuilder.Append("Status:").Append(status).Append("\n");
                stringBuilder.Append("Prefix:").Append(prefix).Append("\n");
                if (destination != null) stringBuilder.Append(destination.GetInfo()).Append("\n");
                stringBuilder.Append("}");
                return stringBuilder.ToString();
            }
        }

        public sealed class Destination
        {
            public string bucket;
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

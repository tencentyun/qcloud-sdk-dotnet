using System;
using System.Collections.Generic;

using System.Text;

namespace COSXML.Model.Tag
{
    public sealed class VersioningConfiguration
    {
        public string status;

        public string GetInfo()
        {
            StringBuilder stringBuilder = new StringBuilder("{VersioningConfiguration:\n");
            stringBuilder.Append("Status:").Append(status).Append("\n");
            stringBuilder.Append("}");
            return stringBuilder.ToString();
        }
    }
}

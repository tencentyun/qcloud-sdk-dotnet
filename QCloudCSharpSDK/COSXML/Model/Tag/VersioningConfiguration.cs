using System;
using System.Xml.Serialization;

using System.Text;

namespace COSXML.Model.Tag
{
    [XmlRoot]
    public sealed class VersioningConfiguration
    {
        [XmlElement("Status")]
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

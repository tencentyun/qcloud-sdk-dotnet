using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace COSXML.Model.Tag
{
    [XmlRoot("ListInventoryConfigurationResult")]
    public sealed class ListInventoryConfiguration
    {
        [XmlElement("InventoryConfiguration")]
        public List<InventoryConfiguration> inventoryConfigurations;

        [XmlElement("IsTruncated")]
        public bool isTruncated = false;

        [XmlElement("ContinuationToken")]
        public string continuationToken;

        [XmlElement("NextContinuationToken")]
        public string nextContinuationToken;

        public String GetInfo()
        {
            StringBuilder stringBuilder = new StringBuilder("{ListInventoryConfigurationResult\n");

            stringBuilder.Append("IsTruncated:").Append(isTruncated).Append("\n");

            if (continuationToken != null)
            {
                stringBuilder.Append("ContinuationToken:").Append(continuationToken).Append("\n");
            }

            if (nextContinuationToken != null)
            {
                stringBuilder.Append("NextContinuationToken:").Append(nextContinuationToken).Append("\n");
            }

            if (inventoryConfigurations != null)
            {

                foreach (InventoryConfiguration inventoryConfiguration in inventoryConfigurations)
                {

                    if (inventoryConfiguration != null)
                    {
                        stringBuilder.Append(inventoryConfiguration.GetInfo()).Append("\n");
                    }
                }
            }

            stringBuilder.Append("}");

            return stringBuilder.ToString();
        }
    }
}

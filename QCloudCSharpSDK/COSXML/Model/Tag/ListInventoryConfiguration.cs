using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSXML.Model.Tag
{
    public sealed class ListInventoryConfiguration
    {
        public List<InventoryConfiguration> inventoryConfigurations;
        public bool isTruncated = false;
        public string continuationToken;
        public string nextContinuationToken;

        public String GetInfo()
        {
            StringBuilder stringBuilder = new StringBuilder("{ListInventoryConfigurationResult\n");
            stringBuilder.Append("IsTruncated:").Append(isTruncated).Append("\n");
            if (continuationToken != null) stringBuilder.Append("ContinuationToken:").Append(continuationToken).Append("\n");
            if (nextContinuationToken != null) stringBuilder.Append("NextContinuationToken:").Append(nextContinuationToken).Append("\n");
            if (inventoryConfigurations != null)
            {
                foreach (InventoryConfiguration inventoryConfiguration in inventoryConfigurations)
                {
                    if (inventoryConfiguration != null) stringBuilder.Append(inventoryConfiguration.GetInfo()).Append("\n");
                }
            }
            stringBuilder.Append("}");
            return stringBuilder.ToString();
        }
    }
}

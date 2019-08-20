using COSXML.Common;
using COSXML.Model.Tag;
using COSXML.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COSXML.Model.Bucket
{
    public sealed class PutBucketInventoryRequest : BucketRequest
    {
        private InventoryConfiguration inventoryConfiguration;
        public PutBucketInventoryRequest(string bucket) : base(bucket)
        {
            this.method = CosRequestMethod.PUT;
            this.queryParameters.Add("inventory", null);
            this.IsNeedMD5 = true;
            inventoryConfiguration = new InventoryConfiguration();
            inventoryConfiguration.isEnabled = true;
            inventoryConfiguration.id = "None";
            inventoryConfiguration.schedule = new InventoryConfiguration.Schedule();
            inventoryConfiguration.destination = new InventoryConfiguration.Destination();
            inventoryConfiguration.destination.cosBucketDestination = new InventoryConfiguration.COSBucketDestination();
        }

        public void SetInventoryId(string inventoryId)
        {
            inventoryConfiguration.id = inventoryId;
        }

        public void IsEnable(bool isEnabled)
        {
            inventoryConfiguration.isEnabled = isEnabled;
        }

        public void SetFilter(string prefix)
        {
            if (!String.IsNullOrEmpty(prefix))
            {
                inventoryConfiguration.filter = new InventoryConfiguration.Filter();
                inventoryConfiguration.filter.prefix = prefix;
            }
        }

        public void SetDestination(string format, string accountId, string bucket, string region, string prefix)
        {
            if (format != null) inventoryConfiguration.destination.cosBucketDestination.format = format;
            if (accountId != null) inventoryConfiguration.destination.cosBucketDestination.accountId = accountId;
            if (bucket != null && region != null)
            {
                inventoryConfiguration.destination.cosBucketDestination.bucket = "qcs::cos:" + region
                        + "::" + bucket;
            }
            if (prefix != null) inventoryConfiguration.destination.cosBucketDestination.prefix = prefix;
            inventoryConfiguration.destination.cosBucketDestination.encryption = new InventoryConfiguration.Encryption();
            inventoryConfiguration.destination.cosBucketDestination.encryption.sSECOS = ""; //默认不填
        }

        public void SetScheduleFrequency(String frequency)
        {
            if (frequency != null)
            {
                inventoryConfiguration.schedule.frequency = frequency;
            }
        }

        public void SetOptionalFields(string field)
        {
            if (field != null)
            {
                if (inventoryConfiguration.optionalFields == null)
                {
                    inventoryConfiguration.optionalFields = new InventoryConfiguration.OptionalFields();
                    inventoryConfiguration.optionalFields.fields = new List<string>(6);
                }
                inventoryConfiguration.optionalFields.fields.Add(field);
            }
        }

        public void SetIncludedObjectVersions(string includedObjectVersions)
        {
            if (includedObjectVersions != null)
            {
                inventoryConfiguration.includedObjectVersions = includedObjectVersions;
            }
        }

        public override Network.RequestBody GetRequestBody()
        {
            string content = Transfer.XmlBuilder.BuildInventoryConfiguration(inventoryConfiguration);
            byte[] data = Encoding.UTF8.GetBytes(content);
            ByteRequestBody body = new ByteRequestBody(data);
            return body;
        }

    }
}

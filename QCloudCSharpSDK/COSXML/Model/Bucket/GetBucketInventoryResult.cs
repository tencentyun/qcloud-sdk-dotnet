using COSXML.Model.Tag;
using COSXML.Transfer;
namespace COSXML.Model.Bucket
{
    public sealed class GetBucketInventoryResult : CosResult
    {

        public InventoryConfiguration inventoryConfiguration;

        internal override void ParseResponseBody(System.IO.Stream inputStream, string contentType, long contentLength)
        {
            inventoryConfiguration = new InventoryConfiguration();
            XmlParse.ParseInventoryConfiguration(inputStream, inventoryConfiguration);
        }

        public override string GetResultInfo()
        {
            return base.GetResultInfo() + (inventoryConfiguration == null ? "" : "\n" + inventoryConfiguration.GetInfo());
        }
    }
}

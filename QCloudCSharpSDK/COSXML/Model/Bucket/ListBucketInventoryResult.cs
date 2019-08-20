using COSXML.Model.Tag;
using COSXML.Transfer;


namespace COSXML.Model.Bucket
{
    public sealed class ListBucketInventoryResult : CosResult
    {
        public ListInventoryConfiguration listInventoryConfiguration;
        internal override void ParseResponseBody(System.IO.Stream inputStream, string contentType, long contentLength)
        {
            listInventoryConfiguration = new ListInventoryConfiguration();
            XmlParse.ParseListInventoryConfiguration(inputStream, listInventoryConfiguration);
        }

        public override string GetResultInfo()
        {
            return base.GetResultInfo() + (listInventoryConfiguration == null ? "" : "\n" + listInventoryConfiguration.GetInfo());
        }
    }
}

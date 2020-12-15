using COSXML.Model.Tag;
using COSXML.Transfer;
namespace COSXML.Model.Bucket
{
    public sealed class GetBucketInventoryResult : CosDataResult<InventoryConfiguration>
    {

        public InventoryConfiguration inventoryConfiguration {get => _data; }
    }
}

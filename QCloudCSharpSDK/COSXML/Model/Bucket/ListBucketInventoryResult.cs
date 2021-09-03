using COSXML.Model.Tag;
using COSXML.Transfer;


namespace COSXML.Model.Bucket
{
    public sealed class ListBucketInventoryResult : CosDataResult<ListInventoryConfiguration>
    {
        public ListInventoryConfiguration listInventoryConfiguration { 
            get {return _data; } 
        }
    }
}

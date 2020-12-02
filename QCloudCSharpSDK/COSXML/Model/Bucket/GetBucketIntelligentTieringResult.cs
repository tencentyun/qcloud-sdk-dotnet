using COSXML.Model.Tag;
using COSXML.Transfer;

namespace COSXML.Model.Bucket
{
    public sealed class GetBucketIntelligentTieringResult : CosDataResult<IntelligentTieringConfiguration>
    {
        public IntelligentTieringConfiguration configuration {get => _data; }
    }
}

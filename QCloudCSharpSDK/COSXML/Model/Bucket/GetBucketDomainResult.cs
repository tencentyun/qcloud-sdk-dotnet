using COSXML.Model.Tag;
using COSXML.Transfer;

namespace COSXML.Model.Bucket
{
    public sealed class GetBucketDomainResult : CosDataResult<DomainConfiguration>
    {
        public DomainConfiguration domainConfiguration {get => _data; }
    }
}

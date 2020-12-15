using COSXML.Model.Tag;
using COSXML.Transfer;

namespace COSXML.Model.Bucket
{
    public sealed class GetBucketWebsiteResult : CosDataResult<WebsiteConfiguration>
    {
        public WebsiteConfiguration websiteConfiguration {get => _data; }
    }
}

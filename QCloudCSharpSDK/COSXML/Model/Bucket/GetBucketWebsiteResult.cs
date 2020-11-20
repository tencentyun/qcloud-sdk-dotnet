using COSXML.Model.Tag;
using COSXML.Transfer;

namespace COSXML.Model.Bucket
{
    public sealed class GetBucketWebsiteResult : CosResult
    {
        public WebsiteConfiguration websiteConfiguration;

        internal override void ParseResponseBody(System.IO.Stream inputStream, string contentType, long contentLength)
        {
            websiteConfiguration = XmlParse.Deserialize<WebsiteConfiguration>(inputStream);
        }

        public override string GetResultInfo()
        {

            return base.GetResultInfo() + (websiteConfiguration == null ? "" : "\n" + websiteConfiguration.ToString());
        }
    }
}

using COSXML.Model.Tag;
using COSXML.Transfer;

namespace COSXML.Model.Bucket
{
    public sealed class GetBucketWebsiteResult : CosResult
    {
        public WebsiteConfiguration websiteConfiguration;

        internal override void ParseResponseBody(System.IO.Stream inputStream, string contentType, long contentLength)
        {
            websiteConfiguration = new WebsiteConfiguration();
            XmlParse.ParseWebsiteConfig(inputStream, websiteConfiguration);
        }

        public override string GetResultInfo()
        {
            return base.GetResultInfo() + (websiteConfiguration == null ? "" : "\n" + websiteConfiguration.ToString());
        }
    }
}

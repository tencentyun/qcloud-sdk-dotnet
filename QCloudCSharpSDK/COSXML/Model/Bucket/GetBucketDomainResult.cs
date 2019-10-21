using COSXML.Model.Tag;
using COSXML.Transfer;

namespace COSXML.Model.Bucket
{
    public sealed class GetBucketDomainResult : CosResult
    {
        public DomainConfiguration domainConfiguration;

        internal override void ParseResponseBody(System.IO.Stream inputStream, string contentType, long contentLength)
        {
            domainConfiguration = new DomainConfiguration();
            XmlParse.ParseBucketDomain(inputStream, domainConfiguration);
        }

        public override string GetResultInfo()
        {
            return base.GetResultInfo() + (domainConfiguration == null ? "" : "\n" + domainConfiguration.ToString());
        }
    }
}

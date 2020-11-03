using COSXML.Model.Tag;
using COSXML.Transfer;

namespace COSXML.Model.Bucket
{
    public sealed class GetBucketIntelligentTieringResult : CosResult
    {
        public IntelligentTieringConfiguration configuration;

        internal override void ParseResponseBody(System.IO.Stream inputStream, string contentType, long contentLength)
        {
            configuration = XmlParse.ParseBucketIntelligentTiering(inputStream);
        }
    }
}

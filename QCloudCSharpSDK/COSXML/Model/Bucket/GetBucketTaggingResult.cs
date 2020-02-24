using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Model.Tag;
using COSXML.Transfer;

namespace COSXML.Model.Bucket
{
    public sealed class GetBucketTaggingResult : CosResult
    {
        public Tagging tagging;

        internal override void ParseResponseBody(System.IO.Stream inputStream, string contentType, long contentLength)
        {
            tagging = new Tagging();
            XmlParse.ParseTagging(inputStream, tagging);
        }
    }
}

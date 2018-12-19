using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Model.Tag;
using COSXML.Transfer;

namespace COSXML.Model.Bucket
{
    public sealed class GetBucketVersioningResult : CosResult
    {
        public VersioningConfiguration versioningConfiguration;

        internal override void ParseResponseBody(System.IO.Stream inputStream, string contentType, long contentLength)
        {
            versioningConfiguration = new VersioningConfiguration();
            XmlParse.ParseVersioningConfiguration(inputStream, versioningConfiguration);
        }

        public override string GetResultInfo()
        {
            return base.GetResultInfo() + (versioningConfiguration == null ? "" : "\n" + versioningConfiguration.GetInfo());
        }
    }
}

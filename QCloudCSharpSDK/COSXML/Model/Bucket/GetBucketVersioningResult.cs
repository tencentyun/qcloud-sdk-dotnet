using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Model.Tag;
using COSXML.Transfer;

namespace COSXML.Model.Bucket
{
    public sealed class GetBucketVersioningResult : CosDataResult<VersioningConfiguration>
    {
        public VersioningConfiguration versioningConfiguration {get => _data; }
    }
}

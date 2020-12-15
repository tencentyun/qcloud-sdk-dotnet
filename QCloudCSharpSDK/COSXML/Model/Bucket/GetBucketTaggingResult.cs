using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Model.Tag;
using COSXML.Transfer;

namespace COSXML.Model.Bucket
{
    public sealed class GetBucketTaggingResult : CosDataResult<Tagging>
    {
        public Tagging tagging {get => _data; }
    }
}

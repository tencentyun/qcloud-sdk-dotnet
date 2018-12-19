using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Model.Tag;
using COSXML.Transfer;

namespace COSXML.Model.Bucket
{
    public sealed class ListBucketVersionsResult :CosResult
    {
        public ListBucketVersions listBucketVersions;

        internal override void ParseResponseBody(System.IO.Stream inputStream, string contentType, long contentLength)
        {
            listBucketVersions = new ListBucketVersions();
            XmlParse.ParseListBucketVersions(inputStream, listBucketVersions);
        }

        public override string GetResultInfo()
        {
            return base.GetResultInfo() + (listBucketVersions == null ? "" : "\n" + listBucketVersions.GetInfo());
        }
    }
}

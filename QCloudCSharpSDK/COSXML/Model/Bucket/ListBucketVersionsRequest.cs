using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;

namespace COSXML.Model.Bucket
{
    public sealed class ListBucketVersionsRequest : BucketRequest
    {
        public ListBucketVersionsRequest(string bucket)
            : base(bucket)
        {
            this.method = CosRequestMethod.GET;
            this.queryParameters.Add("versions", null);
        }

        public void SetPrefix(string prefix)
        {

            if (prefix != null)
            {
                SetQueryParameter("prefix", prefix);
            }
        }

        public void SetKeyMarker(string keyMarker)
        {

            if (keyMarker != null)
            {
                SetQueryParameter("key-marker", keyMarker);
            }
        }

        public void SetVersionIdMarker(string versionIdMarker)
        {

            if (versionIdMarker != null)
            {
                SetQueryParameter("version-id-marker", versionIdMarker);
            }
        }

        public void SetDelimiter(string delimiter)
        {

            if (delimiter != null)
            {
                SetQueryParameter("delimiter", delimiter);
            }
        }

        public void SetEncodingType(string encodingType)
        {

            if (encodingType != null)
            {
                SetQueryParameter("encoding-type", encodingType);
            }
        }

        public void SetMaxKeys(string maxKeys)
        {

            if (maxKeys != null)
            {
                SetQueryParameter("max-keys", maxKeys);
            }
            else
            {
                SetQueryParameter("max-keys", "1000");
            }
        }
    }
}

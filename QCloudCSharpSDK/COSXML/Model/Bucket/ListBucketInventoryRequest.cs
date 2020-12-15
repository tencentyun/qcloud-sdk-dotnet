using COSXML.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace COSXML.Model.Bucket
{
    public sealed class ListBucketInventoryRequest : BucketRequest
    {
        private String continuationToken;

        public ListBucketInventoryRequest(string bucket) : base(bucket)
        {
            this.method = CosRequestMethod.GET;
            this.queryParameters.Add("inventory", null);
        }

        public void SetContinuationToken(String continuationToken)
        {

            if (continuationToken != null)
            {
                SetQueryParameter("continuation-token", continuationToken);
            }
        }
    }
}

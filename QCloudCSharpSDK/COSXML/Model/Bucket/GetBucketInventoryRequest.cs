using COSXML.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace COSXML.Model.Bucket
{
    public sealed class GetBucketInventoryRequest : BucketRequest
    {
        public GetBucketInventoryRequest(string bucket) : base(bucket)
        {
            this.method = CosRequestMethod.GET;
            this.queryParameters.Add("inventory", null);
        }

        public void SetInventoryId(string inventoryId)
        {

            if (inventoryId != null)
            {
                SetQueryParameter("id", inventoryId);
            }
        }
    }
}

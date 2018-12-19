using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;
/**
* Copyright (c) 2018 Tencent Cloud. All rights reserved.
* 11/2/2018 8:33:02 PM
* bradyxiao
*/
namespace COSXML.Model.Bucket
{
    /// <summary>
    /// 删除 Bucket CORS
    /// <see cref="https://cloud.tencent.com/document/product/436/8283"/>
    /// </summary>
    public sealed class DeleteBucketCORSRequest : BucketRequest
    {
        public DeleteBucketCORSRequest(string bucket) : base(bucket)
        {
            this.method = CosRequestMethod.DELETE;
            this.queryParameters.Add("cors", null);
        }
    }
}

using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;
/**
* Copyright (c) 2018 Tencent Cloud. All rights reserved.
* 11/2/2018 8:39:37 PM
* bradyxiao
*/
namespace COSXML.Model.Bucket
{
    /// <summary>
    /// 删除 Bucket Lifecycle
    /// <see cref="https://cloud.tencent.com/document/product/436/8284"/>
    /// </summary>
    public sealed class DeleteBucketLifecycleRequest : BucketRequest
    {
        public DeleteBucketLifecycleRequest(string bucket)
            : base(bucket)
        {
            this.method = CosRequestMethod.DELETE;
            this.queryParameters.Add("lifecycle", null);
        }
    }
}

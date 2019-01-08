using System;
using System.Collections.Generic;
using System.Text;
using COSXML.Common;
/**
* Copyright (c) 2018 Tencent Cloud. All rights reserved.
* 11/12/2018 9:22:06 PM
* bradyxiao
*/
namespace COSXML.Model.Bucket
{
    /// <summary>
    /// 删除 Bucket 权限策略
    /// <see cref="https://cloud.tencent.com/document/product/436/8285"/>
    /// </summary>
    public sealed class DeleteBucketPolicyRequest : BucketRequest
    {
        public DeleteBucketPolicyRequest(string bucket)
            : base(bucket)
        {
            this.method = CosRequestMethod.DELETE;
            this.queryParameters.Add("policy", null);
        }
    }
}

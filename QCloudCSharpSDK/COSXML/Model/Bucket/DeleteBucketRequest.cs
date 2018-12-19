using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;
/**
* Copyright (c) 2018 Tencent Cloud. All rights reserved.
* 11/2/2018 9:03:15 PM
* bradyxiao
*/
namespace COSXML.Model.Bucket
{
    public sealed class DeleteBucketRequest : BucketRequest
    {
        /// <summary>
        /// 删除 空 Bucket
        /// </summary>
        /// <param name="bucket"></param>
        public DeleteBucketRequest(string bucket)
            : base(bucket)
        {
            this.method = CosRequestMethod.DELETE;
        }
    }
}

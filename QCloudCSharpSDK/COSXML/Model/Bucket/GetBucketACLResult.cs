using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Model.Tag;
using System.IO;
using COSXML.Transfer;
/**
* Copyright (c) 2018 Tencent Cloud. All rights reserved.
* 11/2/2018 9:08:55 PM
* bradyxiao
*/
namespace COSXML.Model.Bucket
{
    /// <summary>
    /// 获取 Bucket 访问权限列表返回的结果
    /// <see cref="https://cloud.tencent.com/document/product/436/7733"/>
    /// </summary>
    public sealed class GetBucketACLResult : CosResult
    {
        /// <summary>
        /// 访问权限列表信息
        /// <see cref="COSXML.Model.Tag.AccessControlPolicy"/>
        /// </summary>
        public AccessControlPolicy accessControlPolicy;

        internal override void ParseResponseBody(Stream inputStream, string contentType, long contentLength)
        {
            accessControlPolicy = new AccessControlPolicy();
            XmlParse.ParseAccessControlPolicy(inputStream, accessControlPolicy);
        }

        public override string GetResultInfo()
        {
            return base.GetResultInfo() + (accessControlPolicy == null ? "" : "\n" + accessControlPolicy.GetInfo());
        }
    }
}

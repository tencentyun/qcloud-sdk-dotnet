using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Model.Tag;
using COSXML.Transfer;
using System.IO;
/**
* Copyright (c) 2018 Tencent Cloud. All rights reserved.
* 11/2/2018 5:50:56 PM
* bradyxiao
*/
namespace COSXML.Model.Service
{
    /// <summary>
    /// 获取所有 Bucket 列表返回的结果
    /// <see cref="https://cloud.tencent.com/document/product/436/8291"/>
    /// </summary>
    public sealed class GetServiceResult : CosResult
    {
        /// <summary>
        /// list all buckets for users
        /// <see cref="COSXML.Model.Tag.ListAllMyBuckets"/>
        /// </summary>
        public ListAllMyBuckets listAllMyBuckets;

        internal override void ParseResponseBody(System.IO.Stream inputStream, string contentType, long contentLength)
        {
            listAllMyBuckets = new ListAllMyBuckets();
            XmlParse.ParseListAllMyBucketsResult(inputStream, listAllMyBuckets);
        }

        public override string GetResultInfo()
        {
            return base.GetResultInfo() + (listAllMyBuckets == null ? "" : "\n" + listAllMyBuckets.GetInfo());
        }
    }
}

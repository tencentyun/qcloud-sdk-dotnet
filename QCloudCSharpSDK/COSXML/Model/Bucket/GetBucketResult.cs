using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Model.Tag;
using COSXML.Transfer;
using System.IO;

namespace COSXML.Model.Bucket
{
    /// <summary>
    /// 获取 Bucket 对象列表返回的jieguo
    /// <see cref="https://cloud.tencent.com/document/product/436/7734"/>
    /// </summary>
    public sealed class GetBucketResult : CosResult
    {
        /// <summary>
        /// 保存 Get Bucket 请求结果的所有信息
        /// <see cref="COSXML.Model.Tag.ListBucket"/>
        /// </summary>
        public ListBucket listBucket;

        internal override void ParseResponseBody(System.IO.Stream inputStream, string contentType, long contentLength)
        {
            listBucket = new ListBucket();
            XmlParse.ParseListBucket(inputStream, listBucket);
        }

        public override string GetResultInfo()
        {
            return base.GetResultInfo() + (listBucket == null ? "" : "\n" + listBucket.GetInfo()); 
        }
    }
}

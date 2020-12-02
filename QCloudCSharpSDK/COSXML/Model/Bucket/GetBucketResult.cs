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
    public sealed class GetBucketResult : CosDataResult<ListBucket>
    {
        /// <summary>
        /// 保存 Get Bucket 请求结果的所有信息
        /// <see cref="COSXML.Model.Tag.ListBucket"/>
        /// </summary>
        public ListBucket listBucket {get => _data; }
    }
}

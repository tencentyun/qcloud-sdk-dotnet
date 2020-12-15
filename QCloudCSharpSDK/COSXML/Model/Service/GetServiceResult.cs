using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Model.Tag;
using COSXML.Transfer;
using System.IO;

namespace COSXML.Model.Service
{
    /// <summary>
    /// 获取所有 Bucket 列表返回的结果
    /// <see cref="https://cloud.tencent.com/document/product/436/8291"/>
    /// </summary>
    public sealed class GetServiceResult : CosDataResult<ListAllMyBuckets>
    {
        /// <summary>
        /// list all buckets for users
        /// <see cref="COSXML.Model.Tag.ListAllMyBuckets"/>
        /// </summary>
        public ListAllMyBuckets listAllMyBuckets {get => _data; }
    }
}

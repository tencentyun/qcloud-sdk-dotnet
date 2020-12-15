using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Transfer;
using COSXML.Model.Tag;

namespace COSXML.Model.Bucket
{
    /// <summary>
    /// 查询 Bucket 正在进行中的分块上传返回的结果
    /// <see cref="https://cloud.tencent.com/document/product/436/7736"/>
    /// </summary>
    public sealed class ListMultiUploadsResult : CosDataResult<ListMultipartUploads>
    {
        /// <summary>
        /// 所有分块上传的信息
        /// <see cref="COSXML.Model.Tag.ListMultipartUploads"/>
        /// </summary>
        public ListMultipartUploads listMultipartUploads {get => _data; }
    }
}

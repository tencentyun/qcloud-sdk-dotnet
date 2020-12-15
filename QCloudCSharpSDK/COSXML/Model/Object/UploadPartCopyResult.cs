using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Model.Tag;
using COSXML.Transfer;

namespace COSXML.Model.Object
{
    /// <summary>
    /// 分片复制返回的结果
    /// <see cref="https://cloud.tencent.com/document/product/436/12633"/>
    /// </summary>
    public sealed class UploadPartCopyResult : CosDataResult<CopyPartResult>
    {
        /// <summary>
        /// 分片复制的结果信息
        /// <see cref="Model.Tag.CopyObject"/>
        /// </summary>
        public CopyPartResult copyPart {get => _data; }
    }
}

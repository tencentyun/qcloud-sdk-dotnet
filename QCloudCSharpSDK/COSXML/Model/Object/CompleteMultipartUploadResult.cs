using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Model.Tag;
using COSXML.Transfer;

namespace COSXML.Model.Object
{
    /// <summary>
    /// 完成整个分块上传返回的结果
    /// <see cref="https://cloud.tencent.com/document/product/436/7742"/>
    /// </summary>
    public sealed class CompleteMultipartUploadResult : CosDataResult<CompleteResult>
    {
        /// <summary>
        /// Complete返回信息
        /// <see cref="Model.Tag.CompleteResult"/>
        /// </summary>
        public CompleteResult completeResult {get => _data; }
    }
}

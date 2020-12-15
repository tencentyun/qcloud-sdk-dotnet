using System;
using System.IO;
using System.Collections.Generic;
using COSXML.Model.Tag;
using COSXML.Transfer;

namespace COSXML.Model.CI
{
    /// <summary>
    /// 图片处理结果
    /// </summary>
    public sealed class ImageProcessResult : CosDataResult<PicOperationUploadResult>
    {

        /// <summary>
        /// 图片处理结果
        /// </summary>
        /// <value></value>
        public PicOperationUploadResult uploadResult { get => _data;  }
    }
}

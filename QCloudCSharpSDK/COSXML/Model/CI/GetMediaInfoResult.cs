using System;
using System.IO;
using System.Collections.Generic;
using COSXML.Model.Tag;
using COSXML.Transfer;

namespace COSXML.Model.CI
{
    /// <summary>
    /// 获取媒体信息结果
    /// </summary>
    public sealed class GetMediaInfoResult : CosDataResult<MediaInfoResult>
    {
        public MediaInfoResult mediaInfoResult {
            get { return _data; }
        }
    }
}

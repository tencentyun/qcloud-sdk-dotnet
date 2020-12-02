using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Model.Tag;
using COSXML.Transfer;

namespace COSXML.Model.Bucket
{
    /// <summary>
    /// 获取 Bucket CORS 返回的结果
    /// <see cref="https://cloud.tencent.com/document/product/436/8274"/>
    /// </summary>
    public class GetBucketCORSResult : CosDataResult<CORSConfiguration>
    {
        /// <summary>
        /// 跨域资源共享配置的所有信息，最多可以包含100条 CORSRule
        /// <see cref="COSXML.Model.Tag.CORSConfiguration"/>
        /// </summary>
        public CORSConfiguration corsConfiguration {get => _data; }

    }
}

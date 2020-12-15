using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Model.Tag;
using COSXML.Transfer;

namespace COSXML.Model.Bucket
{
    /// <summary>
    /// 查询 Bucket 的生命周期配置 返回的结果
    /// <see cref="https://cloud.tencent.com/document/product/436/8278"/>
    /// </summary>
    public sealed class GetBucketLifecycleResult : CosDataResult<LifecycleConfiguration>
    {
        /// <summary>
        /// 生命周期配置信息
        /// <see cref="COSXML.Model.Tag.LifecycleConfiguration"/>
        /// </summary>
        public LifecycleConfiguration lifecycleConfiguration {get => _data; }
    }
}

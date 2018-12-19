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
    public sealed class GetBucketLifecycleResult : CosResult
    {
        /// <summary>
        /// 生命周期配置信息
        /// <see cref="COSXML.Model.Tag.LifecycleConfiguration"/>
        /// </summary>
        public LifecycleConfiguration lifecycleConfiguration;

        internal override void ParseResponseBody(System.IO.Stream inputStream, string contentType, long contentLength)
        {
            lifecycleConfiguration = new LifecycleConfiguration();
            XmlParse.ParseLifecycleConfiguration(inputStream, lifecycleConfiguration);
        }

        public override string GetResultInfo()
        {
            return base.GetResultInfo() + (lifecycleConfiguration == null ? "" : "\n " + lifecycleConfiguration.GetInfo());
        }
    }
}

using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Model.Tag;
using COSXML.Transfer;

namespace COSXML.Model.Bucket
{
    /// <summary>
    /// 查询 Bucket 地域信息返回的结果
    /// <see cref="https://cloud.tencent.com/document/product/436/8275"/>
    /// </summary>
    public sealed class GetBucketLocationResult : CosDataResult<LocationConstraint>
    {
        /// <summary>
        /// 地域信息
        /// <see cref="COSXML.Model.Tag.LocationConstraint"/>
        /// </summary>
        public LocationConstraint locationConstraint {get => _data;}
    }
}

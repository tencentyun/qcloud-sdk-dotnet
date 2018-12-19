using System;
using System.Collections.Generic;

using System.Text;

namespace COSXML.Model.Tag
{
    /// <summary>
    /// 地域信息
    /// <see cref="https://cloud.tencent.com/document/product/436/8275"/>
    /// </summary>
    public sealed class LocationConstraint
    {
        /// <summary>
        ///  Bucket 所在地域
        ///  <see cref="COSXML.Common.CosRegion"/>
        /// </summary>
        public string location;

        public string GetInfo()
        {
            StringBuilder stringBuilder = new StringBuilder("{LocationConstraint:\n");
            stringBuilder.Append("Location:").Append(location).Append("\n");
            stringBuilder.Append("}");
            return stringBuilder.ToString();
        }
    }
}

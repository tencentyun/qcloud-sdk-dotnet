using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Model.Tag;
using COSXML.Transfer;

namespace COSXML.Model.Object
{
    /// <summary>
    /// 获取某个存储桶下的某个对象的访问权限的返回结果
    /// <see cref="https://cloud.tencent.com/document/product/436/7744"/>
    /// </summary>
    public sealed class GetObjectACLResult : CosDataResult<AccessControlPolicy>
    {
        /// <summary>
        /// 访问权限信息
        /// <see cref="Model.Tag.AccessControlPolicy"/>
        /// </summary>
        public AccessControlPolicy accessControlPolicy {get => _data; }
    }
}

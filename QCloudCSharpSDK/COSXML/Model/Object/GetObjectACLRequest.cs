using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;

namespace COSXML.Model.Object
{
    /// <summary>
    /// 获取某个存储桶下的某个对象的访问权限
    /// <see cref="https://cloud.tencent.com/document/product/436/7744"/>
    /// </summary>
    public sealed class GetObjectACLRequest : ObjectRequest
    {
        public GetObjectACLRequest(string bucket, string key)
            : base(bucket, key)
        {
            this.method = CosRequestMethod.GET;
            this.queryParameters.Add("acl", null);
        }
    }
}

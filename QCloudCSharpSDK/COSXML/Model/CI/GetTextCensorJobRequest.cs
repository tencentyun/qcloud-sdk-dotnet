using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;
using COSXML.Model.Object;
using COSXML.Model.Tag;
using COSXML.CosException;
using COSXML.Utils;

namespace COSXML.Model.CI
{
    /// <summary>
    /// 查询文本审核任务
    /// <see href="https://cloud.tencent.com/document/product/436/56288"/>
    /// </summary>
    public sealed class GetTextCensorJobRequest : CIRequest
    {
        public GetTextCensorJobRequest(string bucket, string JobId)
            : base(bucket)
        {
            this.method = CosRequestMethod.GET;
            this.SetRequestPath("/text/auditing/" + JobId);
        }
    }
}

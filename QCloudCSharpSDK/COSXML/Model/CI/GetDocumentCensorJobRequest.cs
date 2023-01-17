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
    /// 查询文档审核任务
    /// <see href="https://cloud.tencent.com/document/product/460/59383"/>
    /// </summary>
    public sealed class GetDocumentCensorJobRequest : CIRequest
    {
        public GetDocumentCensorJobRequest(string bucket, string JobId)
            : base(bucket)
        {
            this.method = CosRequestMethod.GET;
            this.SetRequestPath("/document/auditing/" + JobId);
        }
    }
}

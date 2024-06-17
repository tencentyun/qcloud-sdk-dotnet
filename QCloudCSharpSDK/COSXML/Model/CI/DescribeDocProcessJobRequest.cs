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
    /// 查询指定的文档转码任务
    /// <see href="https://cloud.tencent.com/document/product/460/46943"/>
    /// </summary>
    public sealed class DescribeDocProcessJobRequest : CIRequest
    {
        public DescribeDocProcessJobRequest(string bucket,string jobId)
            : base(bucket)
        {
            this.method = CosRequestMethod.GET;
            this.SetRequestPath("/doc_jobs/" + jobId);
            this.SetRequestHeader("Content-Type", "application/xml");

        }
    }
}

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
    /// 查询音频审核任务
    /// <see href="https://cloud.tencent.com/document/product/436/54064"/>
    /// </summary>
    public sealed class GetAudioCensorJobRequest : CIRequest
    {
        public GetAudioCensorJobRequest(string bucket, string JobId)
            : base(bucket)
        {
            this.method = CosRequestMethod.GET;
            this.SetRequestPath("/audio/auditing/" + JobId);
        }
    }
}

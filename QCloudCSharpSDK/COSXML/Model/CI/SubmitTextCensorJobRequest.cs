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
    /// 提交文本审核任务
    /// <see href="https://cloud.tencent.com/document/product/436/56289"/>
    /// </summary>
    public sealed class SubmitTextCensorJobRequest : CIRequest
    {
        public TextCensorJobInfo textCensorJobInfo;
        public SubmitTextCensorJobRequest(string bucket)
            : base(bucket)
        {
            textCensorJobInfo = new TextCensorJobInfo();
            this.method = CosRequestMethod.POST;
            this.SetRequestPath("/text/auditing");
            this.SetRequestHeader("Content-Type", "application/xml");
            textCensorJobInfo.input = new TextCensorJobInfo.Input();
            textCensorJobInfo.conf = new TextCensorJobInfo.Conf();
        }

        public void SetCensorObject(string key)
        {
            textCensorJobInfo.input.obj = key;
        }

        public void SetCensorContent(string content)
        {
            textCensorJobInfo.input.content = content;
        }

        public void SetDetectType(string detectType)
        {
            textCensorJobInfo.conf.detectType = detectType;
        }

        public void SetCallback(string callbackUrl)
        {
            textCensorJobInfo.conf.callback = callbackUrl;
        }

        public void SetBizType(string BizType)
        {
            textCensorJobInfo.conf.bizType = BizType;
        }

        public override Network.RequestBody GetRequestBody()
        {
            return GetXmlRequestBody(textCensorJobInfo);
        }

    }
}

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
    /// 提交音频审核任务
    /// <see href="https://cloud.tencent.com/document/product/436/54063"/>
    /// </summary>
    public sealed class SubmitAudioCensorJobRequest : CIRequest
    {
        public AudioCencorJobInfo audioCencorJobInfo;
        public SubmitAudioCensorJobRequest(string bucket)
            : base(bucket)
        {
            audioCencorJobInfo = new AudioCencorJobInfo();
            this.method = CosRequestMethod.POST;
            this.SetRequestPath("/audio/auditing");
            this.SetRequestHeader("Content-Type", "application/xml");
            audioCencorJobInfo.input = new AudioCencorJobInfo.Input();
            audioCencorJobInfo.conf = new AudioCencorJobInfo.Conf();
        }

        public void SetCensorObject(string key)
        {
            audioCencorJobInfo.input.obj = key;
        }

        public void SetCensorUrl(string url)
        {
            audioCencorJobInfo.input.url = url;
        }

        public void SetDetectType(string detectType)
        {
            audioCencorJobInfo.conf.detectType = detectType;
        }

        public void SetCallback(string callbackUrl)
        {
            audioCencorJobInfo.conf.callback = callbackUrl;
        }

        public void SetCallbackVersion(string callbackVersion)
        {
            audioCencorJobInfo.conf.callbackVersion = callbackVersion;
        }

        public void SetBizType(string BizType)
        {
            audioCencorJobInfo.conf.bizType = BizType;
        }

        public override Network.RequestBody GetRequestBody()
        {
            return GetXmlRequestBody(audioCencorJobInfo);
        }

    }
}

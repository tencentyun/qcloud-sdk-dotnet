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
    /// 提交视频审核任务
    /// <see href="https://cloud.tencent.com/document/product/436/47316"/>
    /// </summary>
    public sealed class SubmitVideoCensorJobRequest : CIRequest
    {
        public VideoCencorJobInfo videoCencorJobInfo;
        public SubmitVideoCensorJobRequest(string bucket)
            : base(bucket)
        {
            videoCencorJobInfo = new VideoCencorJobInfo();
            this.method = CosRequestMethod.POST;
            this.SetRequestPath("/video/auditing");
            this.SetRequestHeader("Content-Type", "application/xml");
            videoCencorJobInfo.input = new VideoCencorJobInfo.Input();
            videoCencorJobInfo.conf = new VideoCencorJobInfo.Conf();
            videoCencorJobInfo.conf.snapshot = new VideoCencorJobInfo.Snapshot();
        }

        public void SetCensorObject(string key)
        {
            videoCencorJobInfo.input.obj = key;
        }

        public void SetDetectType(string detectType)
        {
            videoCencorJobInfo.conf.detectType = detectType;
        }

        public void SetCallback(string callbackUrl)
        {
            videoCencorJobInfo.conf.callback = callbackUrl;
        }

        public void SetCallbackVersion(string callbackVersion)
        {
            videoCencorJobInfo.conf.callbackVersion = callbackVersion;
        }

        public void SetBizType(string BizType)
        {
            videoCencorJobInfo.conf.bizType = BizType;
        }

        public void SetDetectContent(int detectContent)
        {
            videoCencorJobInfo.conf.detectContent = detectContent.ToString();
        }

        public void SetSnapshotCount(string count)
        {
            videoCencorJobInfo.conf.snapshot.count = count;
        }

        public void SetSnapshotMode(string mode)
        {
            videoCencorJobInfo.conf.snapshot.mode = mode;
        }

        public void SetSnapshotTimeInterval(string timeInterval)
        {
            videoCencorJobInfo.conf.snapshot.timeInterval = timeInterval;
        }

        public override Network.RequestBody GetRequestBody()
        {
            return GetXmlRequestBody(videoCencorJobInfo);
        }

    }
}

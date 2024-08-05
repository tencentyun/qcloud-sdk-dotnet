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
    /// 图片审核
    /// <see href="https://cloud.tencent.com/document/product/436/45434"/>
    /// </summary>
    public sealed class SensitiveContentRecognitionRequest : ObjectRequest
    {
        public SensitiveContentRecognitionRequest(string bucket, string key)
            : base(bucket,key){

            this.method = CosRequestMethod.GET;
            this.queryParameters.Add("ci-process", "sensitive-content-recognition");
        }
        public void SetBizType(string bizType){
            this.queryParameters.Add("biz-type", bizType);
        }

        public void SetDetectUrl(string detectUrl){
            this.queryParameters.Add("detect-url", detectUrl);
        }

        public void SetInterval(string interval){
            this.queryParameters.Add("interval", interval);
        }

        public void SetMaxFrames(string maxFrames){
            this.queryParameters.Add("max-frames", maxFrames);
        }

        public void SetLargeImageDetect(string largeImageDetect){
            this.queryParameters.Add("large-image-detect", largeImageDetect);
        }

        public void SetDataid(string dataid){
            this.queryParameters.Add("dataid", dataid);
        }

        public void SetAsync(string async){
            this.queryParameters.Add("async", async);
        }

        public void SetCallback(string callback){
            this.queryParameters.Add("callback", callback);
        }
    }
}

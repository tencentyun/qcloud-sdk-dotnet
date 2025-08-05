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
            : base(bucket,key)
        {
            this.isCheckPathEmpty = false;
            this.method = CosRequestMethod.GET;
            this.queryParameters.Add("ci-process", "sensitive-content-recognition");
        }
        public void SetBizType(string bizType){
            if(bizType != null){
                SetQueryParameter("biz-type", bizType);
            }
        }

        public void SetDetectUrl(string detectUrl){
            if(detectUrl != null){
                SetQueryParameter("detect-url", detectUrl);
            }
        }

        public void SetInterval(string interval){
            if(interval != null){
                SetQueryParameter("interval", interval);
            }
        }

        public void SetMaxFrames(string maxFrames){
            if(maxFrames != null){
                SetQueryParameter("max-frames", maxFrames);
            }
        }

        public void SetLargeImageDetect(string largeImageDetect){
            if(largeImageDetect != null){
                SetQueryParameter("large-image-detect", largeImageDetect);
            }
        }

        public void SetDataId(string dataId){
            if(dataId != null){
                SetQueryParameter("dataid", dataId);
            } 
        }

        public void SetAsync(string async){
            if(async != null){
                SetQueryParameter("async", async);
            }
        }

        public void SetCallback(string callback){
            if(callback != null){
                SetQueryParameter("callback", callback);
            }
        }
    }
}

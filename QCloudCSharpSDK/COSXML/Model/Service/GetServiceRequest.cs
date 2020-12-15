using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;

namespace COSXML.Model.Service
{
    /// <summary>
    /// 获取所有 Bucket 列表
    /// <see cref="https://cloud.tencent.com/document/product/436/8291"/>
    /// </summary>
    public sealed class GetServiceRequest : CosRequest
    {
        public string host { get; set; }

        public GetServiceRequest()
        {
            method = CosRequestMethod.GET;
            path = "/";
            host = "service.cos.myqcloud.com";
        }

        public override string GetHost()
        {

            return host;
        }

        public override Network.RequestBody GetRequestBody()
        {

            return null;
        }

        public override void CheckParameters()
        {

            return;
        }
    }
}

using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Model.Tag;
using COSXML.Transfer;
/**
* Copyright (c) 2018 Tencent Cloud. All rights reserved.
* 11/2/2018 10:01:37 PM
* bradyxiao
*/
namespace COSXML.Model.Bucket
{
    /// <summary>
    /// 获取 Bucket CORS 返回的结果
    /// <see cref="https://cloud.tencent.com/document/product/436/8274"/>
    /// </summary>
    public class GetBucketCORSResult : CosResult
    {
        /// <summary>
        /// 跨域资源共享配置的所有信息，最多可以包含100条 CORSRule
        /// <see cref="COSXML.Model.Tag.CORSConfiguration"/>
        /// </summary>
        public CORSConfiguration corsConfiguration;

        internal override void ParseResponseBody(System.IO.Stream inputStream, string contentType, long contentLength)
        {
            corsConfiguration = new CORSConfiguration();
            XmlParse.ParseCORSConfiguration(inputStream, corsConfiguration);
        }

        public override string GetResultInfo()
        {
            return base.GetResultInfo() + (corsConfiguration == null ? "" : "\n " + corsConfiguration.GetInfo());
        }

    }
}

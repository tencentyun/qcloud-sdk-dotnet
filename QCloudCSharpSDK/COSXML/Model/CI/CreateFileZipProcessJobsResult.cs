//  Create by COSSDKTOOLS;

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
    /// 多文件打包压缩功能可以将您的多个文件，打包为 zip 等压缩包格式，以提交任务的方式进行多文件打包压缩，异步返回打包后的文件，该接口属于 POST 请求
    /// <see href="https://cloud.tencent.com/document/product/460/83091"/>
    /// </summary>
    public sealed class CreateFileZipProcessJobsResult : CosDataResult<CreateFileZipProcessJobsResponse>
    {

        /// <summary>
        /// 多文件打包压缩功能可以将您的多个文件，打包为 zip 等压缩包格式，以提交任务的方式进行多文件打包压缩，异步返回打包后的文件，该接口属于 POST 请求 结果
        /// </summary>
        /// <value></value>
        public CreateFileZipProcessJobsResponse createFileZipProcessJobsResult
        {
            get { return _data; }
        }
    }

}

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
    /// 本接口用于主动查询指定的多文件打包压缩任务结果
    /// <see href="https://cloud.tencent.com/document/product/460/83092"/>
    /// </summary>
    public sealed class DescribeFileZipProcessJobsResult : CosDataResult<DescribeFileZipProcessJobsResponse>
    {

        /// <summary>
        /// 本接口用于主动查询指定的多文件打包压缩任务结果 结果
        /// </summary>
        /// <value></value>
        public DescribeFileZipProcessJobsResponse describeFileZipProcessJobsResult
        {
            get { return _data; }
        }
    }

}

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
    /// 拉取符合条件的文档转码任务
    /// <see href="https://cloud.tencent.com/document/product/460/46944"/>
    /// </summary>
    public sealed class DescribeDocProcessJobsResult : CosDataResult<ListDocProcessResponse>
    {

        /// <summary>
        /// 文档转码结果
        /// </summary>
        /// <value></value>
        public ListDocProcessResponse listDocProcessResult
        {
            get { return _data; }
        }
    }
}

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
    /// 查询指定的文档转码任务
    /// <see href="https://cloud.tencent.com/document/product/460/46943"/>
    /// </summary>
    public sealed class DescribeDocProcessJobResult : CosDataResult<DocProcessResponse>
    {

        /// <summary>
        /// 文档转码结果
        /// </summary>
        /// <value></value>
        public DocProcessResponse taskDocProcessResult
        {
            get { return _data; }
        }
    }
}

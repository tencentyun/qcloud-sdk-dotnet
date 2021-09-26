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
    public sealed class DescribeMediaBucketsRequest : CIRequest
    {
        public DescribeMediaBucketsRequest() : base()
        {
            this.method = CosRequestMethod.GET;
            this.SetRequestPath("/mediabucket");
        }

        public void SetRegions(string regions)
        {
            this.queryParameters.Add("regions", regions);
        }

        public void SetBucketNames(string bucketNames)
        {
            this.queryParameters.Add("bucketNames", bucketNames);
        }

        public void SetBucketName(string bucketName)
        {
            this.queryParameters.Add("bucketName", bucketName);
        }

        public void SetPageNumber(string pageNumber)
        {
            this.queryParameters.Add("pageNumber", pageNumber);
        }

        public void SetPageSize(string pageSize)
        {
            this.queryParameters.Add("pageSize", pageSize);
        }
    }
}

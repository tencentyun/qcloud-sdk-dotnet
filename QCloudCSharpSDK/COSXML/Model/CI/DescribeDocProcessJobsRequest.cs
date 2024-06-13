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
    public sealed class DescribeDocProcessJobsRequest : CIRequest
    {
        public DescribeDocProcessJobsRequest(string bucket)
            : base(bucket)
        {
            this.method = CosRequestMethod.GET;
            this.SetRequestPath("/doc_jobs");
            this.SetRequestHeader("Content-Type", "application/xml");

        }
        /// <summary>
        /// 任务的 Tag：DocProcess
        /// </summary>
        /// <param name="tag"></param>
        public void SetTag(string tag){
            this.queryParameters.Add("tag", tag);
        }
        /// <summary>
        /// 拉取该队列 ID 下的任务，可在任务响应内容或控制台中获取
        /// </summary>
        /// <param name="queueId"></param>
        public void SetQueueId(string queueId){
            this.queryParameters.Add("queueId", queueId);
        }
        /// <summary>
        /// Desc 或者 Asc。默认为 Desc
        /// </summary>
        /// <param name="orderByTime"></param>
        public void SetOrderByTime(string orderByTime){
            this.queryParameters.Add("orderByTime", orderByTime);
        }
        /// <summary>
        /// 请求的上下文，用于翻页。上次返回的值
        /// </summary>
        /// <param name="nextToken"></param>
        public void SetNextToken(string nextToken){
            this.queryParameters.Add("nextToken", nextToken);
        }

        /// <summary>
        /// 拉取的最大任务数。默认为10。最大为100
        /// </summary>
        /// <param name="size"></param>
        public void SetSize(string size){
            this.queryParameters.Add("size", size);
        }

        /// <summary>
        /// 拉取该状态的任务，以,分割，支持多状态：All、Submitted、Running、Success、Failed、Pause、Cancel。默认为 All
        /// </summary>
        /// <param name="states"></param>
        public void SetStates(string states){
            this.queryParameters.Add("states", states);
        }
        /// <summary>
        /// 拉取创建时间大于等于该时间的任务。格式为：%Y-%m-%dT%H:%m:%S%z
        /// </summary>
        /// <param name="startCreationTime"></param>
        public void SetStartCreationTime(string startCreationTime){
            this.queryParameters.Add("startCreationTime", URLEncodeUtils.Encode(startCreationTime));
        }
        /// <summary>
        /// 拉取创建时间小于等于该时间的任务。格式为：%Y-%m-%dT%H:%m:%S%z
        /// </summary>
        /// <param name="endCreationTime"></param>
        public void SetEndCreationTime(string endCreationTime){
            this.queryParameters.Add("endCreationTime",  URLEncodeUtils.Encode(endCreationTime));
        }
    }
}

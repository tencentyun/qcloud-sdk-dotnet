using System.Collections.Generic;
using System.Xml.Serialization;

namespace COSXML.Model.Tag
{
    /// <summary>
    /// 提交文档转码任务返回https://cloud.tencent.com/document/product/460/46942
    /// </summary>
    [XmlRoot("Response")]
    public sealed class DocProcessResponse
    {
        /// <summary>
        /// 任务的详细信息
        /// </summary>
        [XmlElement("JobsDetail")]
        public Detail JobsDetail;

        public class Detail
        {
            /// <summary>
            /// 错误码，只有 State 为 Failed 时有意义
            /// </summary>
            [XmlElement]
            public string Code;
            /// <summary>
            /// 错误描述，只有 State 为 Failed 时有意义
            /// </summary>
            [XmlElement]
            public string Message;
            /// <summary>
            /// 新创建任务的 ID
            /// </summary>
            [XmlElement]
            public string JobId;

            /// <summary>
            /// 新创建任务的 Tag：Concat
            /// </summary>
            [XmlElement]
            public string Tag;

            /// <summary>
            /// 任务状态
            /// Submitted：已提交，待执行
            /// Running：执行中
            /// Success：执行成功
            /// Failed：执行失败
            /// Pause：任务暂停，当暂停队列时，待执行的任务会变为暂停状态
            /// Cancel：任务被取消执行

            /// </summary>
            [XmlElement]
            public string State;
            /// <summary>
            /// 任务的创建时间
            /// </summary>
            [XmlElement]
            public string CreationTime;
            /// <summary>
            /// 任务的开始时间
            /// </summary>
            [XmlElement]
            public string StartTime;
            /// <summary>
            /// 任务的结束时间
            /// </summary>
            [XmlElement]
            public string EndTime;
            /// <summary>
            /// 任务所属的 队列 ID
            /// </summary>
            [XmlElement]
            public string QueueId;

            [XmlElement] public string QueueType;

            /// <summary>
            /// 该任务的输入资源地址
            /// </summary>
            [XmlElement]
            public InputInfo Input;
            /// <summary>
            /// 该任务的规则
            /// </summary>
            [XmlElement]
            public Operation Operation;
        }
        public class InputInfo
        {
            /// <summary>
            /// 存储桶的地域
            /// </summary>
            [XmlElement]
            public string BucketId;
            /// <summary>
            /// 存储结果的存储桶
            /// </summary>
            [XmlElement]
            public string Object;
            /// <summary>
            /// 输出结果的文件名
            /// </summary>
            [XmlElement]
            public string Region;

        }
        public class Operation
        {
            /// <summary>
            /// 文档预览任务参数
            /// </summary>
            [XmlElement]
            public DocProcess DocProcess;
            [XmlElement]
            public DocProcessResult DocProcessResult;
            /// <summary>
            /// 文件的输出地址
            /// </summary>
            [XmlElement]
            public Output Output;
        }

        public class DocProcess
        {
            [XmlElement]
            public string SrcType;
            [XmlElement]
            public string TgtType;
            [XmlElement]
            public string StartPage;
            [XmlElement]
            public string EndPage;

            [XmlElement] public string ImageDpi;
            [XmlElement]
            public string ImageParams;

            [XmlElement] public string PageRanges;
            [XmlElement]
            public string Comments;

            [XmlElement] public string DocPassword;
            [XmlElement]
            public string PaperDirection;

            [XmlElement] public string PicPagination;
            [XmlElement]
            public string PaperSize;
            [XmlElement]
            public string Quality;
            [XmlElement]
            public string SheetId;
            [XmlElement]
            public string Zoom;
        }

        public class DocProcessResult
        {
            [XmlElement]
            public string FailPageCount;
            [XmlElement]
            public string SuccPageCount;
            [XmlElement]
            public string TaskId;
            [XmlElement]
            public string TgtType;
            [XmlElement]
            public string TotalPageCount;
            [XmlElement]
            public string TotalSheetCount;

            [XmlElement]
            public List<PageInfo> PageInfo;
        }
        public class PageInfo
        {
            [XmlElement] public string PageNo;

            [XmlElement]
            public string PicIndex;

            [XmlElement]
            public string PicNum;

            [XmlElement]
            public string TgtUri;

            [XmlElement("X-SheetPics")]
            public string SheetPics;
        }

        public class Output
        {
            [XmlElement]
            public string Bucket;
            [XmlElement]
            public string Object;
            [XmlElement]
            public string Region;
        }
    }
}

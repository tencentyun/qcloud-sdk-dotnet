
namespace COSXML.Model.Tag
{
    /// <summary>
    /// 图片审核结果
    /// </summary>
    public sealed class SensitiveRecognitionResult
    {

        /// <summary>
        /// 鉴黄审核信息
        /// </summary>
        public RecognitionInfo PornInfo;

        /// <summary>
        /// 鉴暴恐审核信息
        /// </summary>
        public RecognitionInfo TerroristInfo;

        /// <summary>
        /// 鉴政审核信息
        /// </summary>
        public RecognitionInfo PoliticsInfo;

        /// <summary>
        /// 广告审核信息
        /// </summary>
        public RecognitionInfo AdsInfo;

        /// <summary>
        /// 原图信息
        /// </summary>
        public sealed class RecognitionInfo
        {
            /// <summary>
            /// 错误码，0为正确，其他数字对应相应错误
            /// </summary>
            public int Code;
            /// <summary>
            /// 具体错误信息，如正常则为 OK
            /// </summary>
            public string Msg;
            /// <summary>
            /// 是否命中该审核分类，0表示未命中，1表示命中，2表示疑似
            /// </summary>
            public int HitFlag;
            /// <summary>
            /// 审核分值。0 - 60分表示图片正常，60 - 90分表示图片疑似敏感，90 - 100分表示图片确定敏感
            /// </summary>
            public int Score;
            /// <summary>
            /// 识别出的图片标签
            /// </summary>
            public string Label;
            /// <summary>
            /// 视频文件参数，该类型违规的截图张数
            /// </summary>
            public int Count;
        }
    }
}
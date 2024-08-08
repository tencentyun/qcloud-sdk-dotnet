using System.Collections.Generic;
using System.Xml.Serialization;

namespace COSXML.Model.Tag
{
    /// <summary>
    /// 图片审核功能支持同步、异步请求方式，您可以通过本接口对图片文件进行内容审核。该接口属于 GET 请求。该接口支持情况如下： 结果
    /// <see href="https://cloud.tencent.com/document/product/460/37318"/>
    /// </summary>
    [XmlRoot("RecognitionResult")]
    public sealed class SensitiveRecognitionResult
    {
        /// 图片标识，审核结果会返回原始内容，长度限制为512字节。
        [XmlElement("DataId")]
        public string DataId;

        /// 图片审核任务的 ID。
        [XmlElement("JobId")]
        public string JobId;

        /// 审核任务的状态，值为 Success（审核成功）。
        [XmlElement("State")]
        public string State;

        /// 存储在 COS 桶中的图片名称，创建任务使用 ObjectKey 时返回。
        [XmlElement("Object")]
        public string Object;

        /// 图片文件的链接地址，创建任务使用 detect-url 时返回。
        [XmlElement("Url")]
        public string Url;

        /// 图片是否被压缩处理，值为 0（未压缩），1（正常压缩）。
        [XmlElement("CompressionResult")]
        public int CompressionResult;

        /// 该字段表示本次判定的审核结果，您可以根据该结果，进行后续的操作；建议您按照业务所需，对不同的审核结果进行相应处理。有效值：0（审核正常），1 （判定为违规敏感文件），2（疑似敏感，建议人工复核）。
        [XmlElement("Result")]
        public int Result;

        /// 该字段用于返回检测结果中所对应的优先级最高的恶意标签，表示模型推荐的审核结果，建议您按照业务所需，对不同违规类型与建议值进行处理。返回值：Normal 表示正常，Porn 表示色情，Ads 表示广告，Quality 表示低质量，以及其他不安全或不适宜的类型。
        [XmlElement("Label")]
        public string Label;

        /// 该字段为 Label 的子集，表示审核命中的具体审核类别。例如 Sexy，表示色情标签中的性感类别。
        [XmlElement("Category")]
        public string Category;

        /// 该图命中的二级标签结果。
        [XmlElement("SubLabel")]
        public string SubLabel;

        /// 该字段表示审核结果命中审核信息的置信度，取值范围：0（置信度最低）-100（置信度最高 ），越高代表该内容越有可能属于当前返回审核信息例如：色情 99，则表明该内容非常有可能属于色情内容
        [XmlElement("Score")]
        public int Score;

        /// 该图里的文字内容（OCR），当审核策略开启文本内容检测时返回。
        [XmlElement("Text")]
        public string Text;

        /// <summary>
        /// 审核场景为色情性感的审核结果信息。
        /// </summary>
        [XmlElement("PornInfo")]
        public RecognitionInfo PornInfo;


        /// <summary>
        /// 审核场景为图片质量差不清晰的审核结果信息。
        /// </summary>
        [XmlElement("QualityInfo")]
        public RecognitionInfo QualityInfo;

        /// <summary>
        /// 审核场景为广告引导的审核结果信息。
        /// </summary>
        [XmlElement("AdsInfo")]
        public RecognitionInfo AdsInfo;
        [XmlElement("PoliticsInfo")]
        public RecognitionInfo PoliticsInfo;
        [XmlElement("TerrorismInfo")]
        public RecognitionInfo TerrorismInfo;
        /// <summary>
        /// 原图信息
        /// </summary>
        public sealed class RecognitionInfo
        {
            /// <summary>
            /// 单个审核场景的错误码，0为成功，其他为失败。详情请参见 错误码。
            /// <summary>
            [XmlElement]
            public int Code;

            /// <summary>
            /// 具体错误信息，如正常则为 OK。
            /// <summary>
            [XmlElement]
            public string Msg;

            /// <summary>
            /// 用于返回该审核场景的审核结果，返回值：0：正常。1：确认为当前场景的违规内容。2：疑似为当前场景的违规内容。
            /// <summary>
            [XmlElement]
            public int HitFlag;

            /// <summary>
            /// 该字段表示审核结果命中审核信息的置信度，取值范围：0（置信度最低）-100（置信度最高 ），越高代表该内容越有可能属于当前返回审核信息例如：色情 99，则表明该内容非常有可能属于色情内容。
            /// <summary>
            [XmlElement]
            public int Score;

            /// <summary>
            /// 该图的结果标签（为综合标签，可能为 SubLabel，可能为人物名字等）。
            /// <summary>
            [XmlElement]
            public string Label;

            /// <summary>
            /// 该字段为 Label 的子集，表示审核命中的具体审核类别。例如 Sexy，表示色情标签中的性感类别。
            /// <summary>
            [XmlElement]
            public string Category;

            /// <summary>
            /// 该图的二级标签结果。
            /// <summary>
            [XmlElement]
            public string SubLabel;

            /// <summary>
            /// 该字段表示 OCR 文本识别的详细检测结果，包括文本坐标信息、文本识别结果等信息，有相关违规内容时返回。
            /// <summary>
            [XmlElement]
            public List<AuditingJobResponseOcrResults> OcrResults;

            /// <summary>
            /// 该字段用于返回基于风险库识别的结果。注意：未命中风险库中样本时，此字段不返回。
            /// <summary>
            [XmlElement]
            public List<AuditingPictureJobResponseLibResults> LibResults;
        }
        public sealed class AuditingJobResponseOcrResults {

            /// <summary>
            /// 有敏感信息的 Ocr 文本内容。
            /// <summary>
            [XmlElement]
            public string Text;

            /// <summary>
            /// 该段内容中的敏感文字。
            /// <summary>
            [XmlElement]
            public List<string> Keywords;

            /// <summary>
            /// 该段文字在图片中的位置信息。
            /// <summary>
            [XmlElement]
            public AuditingPictureJobResponseLocation Location;

        }

        public sealed class AuditingPictureJobResponseLocation {

            /// <summary>
            /// 相对于原点（图片左上角）的 X 坐标。
            /// <summary>
            [XmlElement]
            public float X;

            /// <summary>
            /// 相对于原点（图片左上角）的 Y 坐标。
            /// <summary>
            [XmlElement]
            public float Y;

            /// <summary>
            /// 敏感区域的高度。
            /// <summary>
            [XmlElement]
            public float Height;

            /// <summary>
            /// 敏感区域的宽度。
            /// <summary>
            [XmlElement]
            public float Width;

            /// <summary>
            /// 敏感区域的旋转角度。
            /// <summary>
            [XmlElement]
            public float Rotate;

        }

        public sealed class AuditingPictureJobResponseLibResults {

            /// <summary>
            /// 该字段表示命中的风险库中的图片样本 ID。
            /// <summary>
            [XmlElement]
            public string ImageId;

            /// <summary>
            /// 该字段用于返回当前标签下的置信度，取值范围：0（置信度最低）-100（置信度最高），越高代表当前的图片越有可能命中库中的样本。例如：色情 99，则表明该数据非常有可能命中库中的色情样本。
            /// <summary>
            [XmlElement]
            public int Score;

        }
    }
}
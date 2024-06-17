using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Text;

namespace COSXML.Model.Tag
{
    /// <summary>
    /// 文档转码请求body
    /// <see href="https://cloud.tencent.com/document/product/460/46942"/>
    /// </summary>
    [XmlRoot("Request")]
    public sealed class DocumentProcessJobInfo
    {
        /// <summary>
        /// 创建任务的 Tag，目前仅支持：DocProcess
        /// </summary>
        [XmlElement("Tag")] public string tag = "DocProcess";

        /// <summary>
        /// 待操作的文件对象
        /// </summary>
        [XmlElement("Input")] public Input input;

        /// <summary>
        /// 操作规则
        /// </summary>
        [XmlElement("Operation")] public Operation operation;

        /// <summary>
        /// 任务所在的队列 ID，开通预览服务后自动生成
        /// </summary>
        [XmlElement("QueueId")] public string queueId;

        public string GetInfo()
        {
            StringBuilder stringBuilder = new StringBuilder("{Request:\n");

            stringBuilder.Append("Tag:" + tag + "\n");
            stringBuilder.Append(input.GetInfo()).Append("\n");
            stringBuilder.Append(operation.GetInfo()).Append("\n");
            if (queueId != null)
            {
                stringBuilder.Append("QueueId:" + queueId + "\n");
            }

            stringBuilder.Append("}");
            return stringBuilder.ToString();
        }

        public sealed class Input
        {

            /// <summary>
            ///  文件在 COS 上的文件路径，Bucket 由 Host 指定
            /// </summary>
            [XmlElement("Object")] public string inputObject;

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{Input:\n");
                stringBuilder.Append("Object:").Append(inputObject).Append("\n");
                stringBuilder.Append("}");
                return stringBuilder.ToString();
            }
        }


        public sealed class Operation
        {
            /// <summary>
            /// 当 Tag 为 DocProcess 时有效，指定该任务的参数
            /// </summary>
            [XmlElement("DocProcess")] public DocProcess docProcess;

            /// <summary>
            /// 结果输出地址
            /// </summary>
            [XmlElement("Output")] public Output output;


            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{Operation:\n");
                if (docProcess != null)
                {
                    stringBuilder.Append(docProcess.GetInfo()).Append("\n");
                }
                stringBuilder.Append(output.GetInfo()).Append("\n");
                stringBuilder.Append("}");

                return stringBuilder.ToString();

            }
        }

        public sealed class DocProcess
        {
            /// <summary>
            /// 源数据的后缀类型，当前文档转换根据 cos 对象的后缀名来确定源数据类型，当 cos 对象没有后缀名时，可以设置该值
            /// </summary>
            [XmlElement("SrcType")] public string srcType;

            /// <summary>
            /// 转换输出目标文件类型：
            /// jpg，转成 jpg 格式的图片文件；如果传入的格式未能识别，默认使用 jpg 格式
            /// png，转成 png 格式的图片文件
            /// pdf，转成 pdf 格式文件（暂不支持指定页数）
            /// </summary>
            [XmlElement("TgtType")] public string tgtType;

            /// <summary>
            /// 从第 X 页开始转换；在表格文件中，一张表可能分割为多页转换，生成多张图片。StartPage 表示从指定 SheetId 的第 X 页开始转换。默认为1
            /// </summary>
            [XmlElement("StartPage")] public string startPage;

            /// <summary>
            /// 转换至第 X 页；在表格文件中，一张表可能分割为多页转换，生成多张图片。EndPage 表示转换至指定 SheetId 的第 X 页。默认为-1，即转换全部页
            /// </summary>
            [XmlElement("EndPage")] public string endPage;

            /// <summary>
            /// 表格文件参数，转换第 X 个表，默认为0；设置 SheetId 为0，即转换文档中全部表
            /// </summary>
            [XmlElement("SheetId")] public string sheetId;


            /// <summary>
            /// 表格文件转换纸张方向，0代表垂直方向，非0代表水平方向，默认为0
            /// </summary>
            [XmlElement("PaperDirection")] public string paperDirection;

            /// <summary>
            /// 设置纸张（画布）大小，对应信息为： 0 → A4 、 1 → A2 、 2 → A0 ，默认 A4 纸张
            /// </summary>
            [XmlElement("PaperSize")] public string paperSize;

            /// <summary>
            ///转换后的图片处理参数，支持 基础图片处理 所有处理参数，多个处理参数可通过 管道操作符 分隔，从而实现在一次访问中按顺序对图片进行不同处理。
            /// </summary>
            [XmlElement("ImageParams")] public string imageParams;


            /// <summary>
            ///生成预览图的图片质量，取值范围 [1-100]，默认值100。 例：值为100，代表生成图片质量为100%。
            /// </summary>
            [XmlElement("Quality")] public string quality;

            /// <summary>
            /// 预览图片的缩放参数，取值范围[10-200]， 默认值100。 例：值为200，代表图片缩放比例为200% 即放大两倍。
            /// </summary>
            [XmlElement("Zoom")] public string zoom;

            /// <summary>
            /// 按指定 dpi 渲染图片，该参数与  Zoom  共同作用，取值范围  96-600 ，默认值为  96 。转码后的图片单边宽度需小于65500像素。
            /// </summary>
            [XmlElement("ImageDpi")] public string imageDpi;

            /// <summary>
            ///是否转换成单张长图，设置为 1 时，最多仅支持将 20 标准页面合成单张长图，超过可能会报错，分页范围可以通过 StartPage、EndPage 控制。默认值为 0 ，按页导出图片，TgtType="png"/"jpg" 时生效。
            /// </summary>
            [XmlElement("PicPagination")] public string picPagination;

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{DocProcess:\n");
                if (srcType != null)
                {
                    stringBuilder.Append("SrcType:").Append(srcType).Append("\n");
                }
                if (tgtType != null)
                {
                    stringBuilder.Append("TgtType:").Append(tgtType).Append("\n");
                }
                if (startPage != null)
                {
                    stringBuilder.Append("StartPage:").Append(startPage).Append("\n");
                }
                if (endPage != null)
                {
                    stringBuilder.Append("EndPage:").Append(endPage).Append("\n");
                }
                if (sheetId != null)
                {
                    stringBuilder.Append("SheetId:").Append(sheetId).Append("\n");
                }
                if (paperDirection != null)
                {
                    stringBuilder.Append("PaperDirection:").Append(paperDirection).Append("\n");
                }

                if (paperSize != null)
                {
                    stringBuilder.Append("PaperSize:").Append(paperSize).Append("\n");
                }
                if (imageParams != null)
                {
                    stringBuilder.Append("ImageParams:").Append(imageParams).Append("\n");
                }
                if (quality != null)
                {
                    stringBuilder.Append("Quality:").Append(quality).Append("\n");
                }
                if (zoom != null)
                {
                    stringBuilder.Append("Zoom:").Append(zoom).Append("\n");
                }
                if (imageDpi != null)
                {
                    stringBuilder.Append("ImageDpi:").Append(imageDpi).Append("\n");
                }

                if (picPagination != null)
                {
                    stringBuilder.Append("PicPagination:").Append(picPagination).Append("\n");
                }
                stringBuilder.Append("}");
                return stringBuilder.ToString();
            }
        }

        public sealed class Output
        {
            /// <summary>
            ///存储桶的地域
            /// </summary>
            [XmlElement("Region")] public string region;

            /// <summary>
            ///存储结果的存储桶
            /// </summary>
            [XmlElement("Bucket")] public string bucket;

            /// <summary>
            ///输出文件路径。
            ///非表格文件输出文件名需包含 ${Number} 或 ${Page} 参数。多个输出文件，${Number} 表示序号从1开始，${Page} 表示序号与预览页码一致。
            ///${Number} 表示多个输出文件，序号从1开始，例如输入 abc_${Number}.jpg，预览某文件5 - 6页，则输出文件名为 abc_1.jpg，abc_2.jpg
            ///${Page} 表示多个输出文件，序号与预览页码一致，例如输入 abc_${Page}.jpg，预览某文件5-6页，则输出文件名为 abc_5.jpg，abc_6.jpg
            ///    表格文件输出路径需包含 ${SheetID} 占位符，输出文件名必须包含 ${Number} 参数。
            ///例如 /${SheetID}/abc_${Number}.jpg，先根据 excel 转换的表格数，生成对应数量的文件夹，再在对应的文件夹下，生成对应数量的图片文件
            /// </summary>
            [XmlElement("Object")] public string outPutObject;

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{Output:\n");
                stringBuilder.Append("Region:").Append(region).Append("\n");
                stringBuilder.Append("Bucket:").Append(bucket).Append("\n");
                stringBuilder.Append("Object:").Append(outPutObject).Append("\n");
                stringBuilder.Append("}");
                return stringBuilder.ToString();
            }
        }

    }

}

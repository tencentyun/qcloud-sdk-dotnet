//  Create by COSSDKTOOLS;

using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;

namespace COSXML.Model.CI
{
    /// <summary>
    /// 多文件打包压缩功能可以将您的多个文件，打包为 zip 等压缩包格式，以提交任务的方式进行多文件打包压缩，异步返回打包后的文件，该接口属于 POST 请求 请求体
    /// <see href="https://cloud.tencent.com/document/product/460/83091"/>
    /// </summary>
    [XmlRoot("Request")]
    public sealed class CreateFileZipProcessJobs {

        /// 表示任务的类型，多文件打包压缩默认为：FileCompress。
        [XmlElement("Tag")]
        public string tag;

        /// 包含文件打包压缩的处理规则。
        [XmlElement("Operation")]
        public CreateFileZipProcessJobsOperation operation;

        /// 任务回调格式，JSON 或 XML，默认 XML，优先级高于队列的回调格式。
        [XmlElement("CallBackFormat")]
        public string callBackFormat;

        /// 任务回调类型，Url 或 TDMQ，默认 Url，优先级高于队列的回调类型。
        [XmlElement("CallBackType")]
        public string callBackType;

        /// 任务回调的地址，优先级高于队列的回调地址。
        [XmlElement("CallBack")]
        public string callBack;

        /// 任务回调 TDMQ 配置，当 CallBackType 为 TDMQ 时必填。详情请参见 CallBackMqConfig。
        [XmlElement("CallBackMqConfig")]
        public CallBackMqConfig callBackMqConfig;

        public string GetInfo()
        {
            StringBuilder stringBuilder = new StringBuilder("{Request:\n");
            if (tag != null) stringBuilder.Append(tag.ToString()).Append("\n");
            if (operation != null) stringBuilder.Append(operation.GetInfo()).Append("\n");
            if (callBackFormat != null) stringBuilder.Append(callBackFormat.ToString()).Append("\n");
            if (callBackType != null) stringBuilder.Append(callBackType.ToString()).Append("\n");
            if (callBack != null) stringBuilder.Append(callBack.ToString()).Append("\n");
            if (callBackMqConfig != null) stringBuilder.Append(callBackMqConfig.GetInfo()).Append("\n");
            stringBuilder.Append("}");
            return stringBuilder.ToString();
        }
        public sealed class CreateFileZipProcessJobsOperation {

            /// 指定文件打包压缩的处理规则。
            [XmlElement("FileCompressConfig")]
            public FileCompressConfig fileCompressConfig;

            /// 透传用户信息，可打印的 ASCII 码，长度不超过1024。
            [XmlElement("UserData")]
            public string userData;

            /// 指定打包压缩后的文件保存的地址信息。
            [XmlElement("Output")]
            public CreateFileZipProcessJobsOutput output;

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{Request:\n");
                if (fileCompressConfig != null) stringBuilder.Append(fileCompressConfig.GetInfo()).Append("\n");
                if (userData != null) stringBuilder.Append(userData.ToString()).Append("\n");
                if (output != null) stringBuilder.Append(output.GetInfo()).Append("\n");
                stringBuilder.Append("}");
                return stringBuilder.ToString();
            }
        }

        public sealed class CallBackMqConfig {

            /// 消息队列所属园区，目前支持园区 sh（上海）、bj（北京）、gz（广州）、cd（成都）、hk（中国香港）
            [XmlElement("MqRegion")]
            public string mqRegion;

            /// 消息队列使用模式，默认 Queue ：主题订阅：Topic队列服务: Queue
            [XmlElement("MqMode")]
            public string mqMode;

            /// TDMQ 主题名称
            [XmlElement("MqName")]
            public string mqName;

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{Request:\n");
                if (mqRegion != null) stringBuilder.Append(mqRegion.ToString()).Append("\n");
                if (mqMode != null) stringBuilder.Append(mqMode.ToString()).Append("\n");
                if (mqName != null) stringBuilder.Append(mqName.ToString()).Append("\n");
                stringBuilder.Append("}");
                return stringBuilder.ToString();
            }
        }

        public sealed class CreateFileZipProcessJobsOutput {

            /// 存储桶的地域。
            [XmlElement("Region")]
            public string region;

            /// 保存压缩后文件的存储桶。
            [XmlElement("Bucket")]
            public string bucket;

            /// 压缩后文件的文件名
            [XmlElement("Object")]
            public string objectInfo;

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{Request:\n");
                if (region != null) stringBuilder.Append(region.ToString()).Append("\n");
                if (bucket != null) stringBuilder.Append(bucket.ToString()).Append("\n");
                if (objectInfo != null) stringBuilder.Append(objectInfo.ToString()).Append("\n");
                stringBuilder.Append("}");
                return stringBuilder.ToString();
            }
        }

        public sealed class FileCompressConfig {

            /// 文件打包时，是否需要去除源文件已有的目录结构，有效值：0：不需要去除目录结构，打包后压缩包中的文件会保留原有的目录结构；1：需要，打包后压缩包内的文件会去除原有的目录结构，所有文件都在同一层级。例如：源文件 URL 为 https://domain/source/test.mp4，则源文件路径为 source/test.mp4，如果为 1，则 ZIP 包中该文件路径为 test.mp4；如果为0， ZIP 包中该文件路径为 source/test.mp4。
            [XmlElement("Flatten")]
            public string flatten;

            /// 打包压缩的类型，有效值：zip、tar、tar.gz。
            [XmlElement("Format")]
            public string format;

            /// 压缩类型，仅在Format为tar.gz或zip时有效。faster：压缩速度较快better：压缩质量较高，体积较小default：适中的压缩方式默认值为default
            [XmlElement("Type")]
            public string type;

            /// 压缩包密钥，传入时需先经过 base64 编码，编码后长度不能超过128。当 Format 为 zip 时生效。
            [XmlElement("CompressKey")]
            public string compressKey;

            /// 支持将需要打包的文件整理成索引文件，后台将根据索引文件内提供的文件 url，打包为一个压缩包文件。索引文件需要保存在当前存储桶中，本字段需要提供索引文件的对象地址，不需要带域名，填写示例：/test/index.csv索引文件格式：仅支持 CSV 文件，一行一条 URL（仅支持本存储桶文件），如有多列字段，默认取第一列作为URL。
            [XmlElement("UrlList")]
            public string urlList;

            /// 支持对存储桶中的某个前缀进行打包，如果需要对某个目录进行打包，需要加/，例如test目录打包，则值为：test/。
            [XmlElement("Prefix")]
            public string prefix;

            /// 打包时如果单个文件出错，是否忽略错误继续打包。有效值为：ture：忽略错误继续打包后续的文件；false：遇到某个文件执行打包报错时，直接终止打包任务，不返回压缩包。默认值为false。
            [XmlElement("IgnoreError ")]
            public string ignoreError ;

            /// 支持对存储桶中的文件进行打包，可填写多个，个数不能超过 1000，如需打包更多文件，请使用 UrlList 或 Prefix 参数。
            [XmlElement("KeyConfig")]
            public List<KeyConfig> keyConfig;

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{Request:\n");
                if (flatten != null) stringBuilder.Append(flatten.ToString()).Append("\n");
                if (format != null) stringBuilder.Append(format.ToString()).Append("\n");
                if (type != null) stringBuilder.Append(type.ToString()).Append("\n");
                if (compressKey != null) stringBuilder.Append(compressKey.ToString()).Append("\n");
                if (urlList != null) stringBuilder.Append(urlList.ToString()).Append("\n");
                if (prefix != null) stringBuilder.Append(prefix.ToString()).Append("\n");
                if (ignoreError  != null) stringBuilder.Append(ignoreError.ToString()).Append("\n");
                if (keyConfig != null)
                {
                    foreach (var config in keyConfig)
                    {
                        stringBuilder.Append(config.GetInfo()).Append("\n");
                    }
                }
                stringBuilder.Append("}");
                return stringBuilder.ToString();
            }
        }

        public sealed class KeyConfig {

            /// 存储桶中的包含路径的完整文件名称，请使用 UrlList 或 Prefix 参数。
            [XmlElement("Key")]
            public string key;

            /// 文件重命名，打包后文件名将以该值为准，不填表示不更改文件名。
            [XmlElement("Rename")]
            public string rename;

            /// 图片处理参数，支持的参数详见 基础图片处理，填写示例：imageMogr2/thumbnail/!50p
            [XmlElement("ImageParams")]
            public string imageParams;

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{Request:\n");
                if (key != null) stringBuilder.Append(key.ToString()).Append("\n");
                if (rename != null) stringBuilder.Append(rename.ToString()).Append("\n");
                if (imageParams != null) stringBuilder.Append(imageParams.ToString()).Append("\n");
                stringBuilder.Append("}");
                return stringBuilder.ToString();
            }
        }
    }
}

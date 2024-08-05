//  Create by COSSDKTOOLS;

using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;

namespace COSXML.Model.CI
{
    /// <summary>
    /// 本接口用于主动查询指定的多文件打包压缩任务结果 结果
    /// <see href="https://cloud.tencent.com/document/product/460/83092"/>
    /// </summary>
    [XmlRoot("Response")]
    public sealed class DescribeFileZipProcessJobsResponse {

        /// 同提交多文件打包任务接口中的 Response.JobsDetail 
        [XmlElement("JobsDetail")]
        public List<CreateFileZipProcessJobsResponseJobsDetail> JobsDetail;

        /// 查询的 ID 中不存在任务，所有任务都存在时不返回。
        [XmlElement("NonExistJobIds")]
        public List<string> NonExistJobIds;

        public sealed class CreateFileZipProcessJobsResponseJobsDetail {

            /// <summary>            
            /// 错误码，只有 State 为 Failed 时有意义。
            /// <summary>            
            [XmlElement]
            public string Code;

            /// <summary>            
            /// 错误描述，只有 State 为 Failed 时有意义。
            /// <summary>            
            [XmlElement]
            public string Message;

            /// <summary>            
            /// 新创建任务的 ID。
            /// <summary>            
            [XmlElement]
            public string JobId;

            /// <summary>            
            /// 表示任务的类型，多文件打包压缩默认为：FileCompress。
            /// <summary>            
            [XmlElement]
            public string Tag;

            /// <summary>            
            /// 任务状态Submitted：已提交，待执行。Running：执行中。Success：执行成功。Failed：执行失败。Pause：任务暂停，当暂停队列时，待执行的任务会变为暂停状态。Cancel：任务被取消执行。
            /// <summary>            
            [XmlElement]
            public string State;

            /// <summary>            
            /// 任务进度百分比，范围为[0, 100]。
            /// <summary>            
            [XmlElement]
            public int Progress;

            /// <summary>            
            /// 任务的创建时间。
            /// <summary>            
            [XmlElement]
            public string CreationTime;

            /// <summary>            
            /// 任务的开始时间。
            /// <summary>            
            [XmlElement]
            public string StartTime;

            /// <summary>            
            /// 任务的结束时间。
            /// <summary>            
            [XmlElement]
            public string EndTime;

            /// <summary>            
            /// 任务所属的 队列 ID。
            /// <summary>            
            [XmlElement]
            public string QueueId;

            /// <summary>            
            /// 多文件打包压缩的处理规则。
            /// <summary>            
            [XmlElement]
            public CreateFileZipProcessJobsResponseOperation Operation;

        }

        public sealed class CreateFileZipProcessJobsResponseOperation {

            /// <summary>            
            /// 透传用户信息。
            /// <summary>            
            [XmlElement]
            public string UserData;

            /// <summary>            
            /// 同请求中的 Request.Operation.Output。
            /// <summary>            
            [XmlElement]
            public CreateFileZipProcessJobsOutput Output;

            /// <summary>            
            /// 同请求中的 Request.Operation.FileCompressConfig。
            /// <summary>            
            [XmlElement]
            public FileCompressConfig FileCompressConfig;

            /// <summary>            
            /// 多文件打包压缩的结果，任务未完成时不返回。
            /// <summary>            
            [XmlElement]
            public FileCompressResult FileCompressResult;

        }

        public sealed class CreateFileZipProcessJobsOutput {

            /// <summary>            
            /// 存储桶的地域。
            /// <summary>            
            [XmlElement]
            public string Region;

            /// <summary>            
            /// 保存压缩后文件的存储桶。
            /// <summary>            
            [XmlElement]
            public string Bucket;

            /// <summary>            
            /// 压缩后文件的文件名
            /// <summary>            
            [XmlElement]
            public string Object;

        }

        public sealed class FileCompressConfig {

            /// <summary>            
            /// 文件打包时，是否需要去除源文件已有的目录结构，有效值：0：不需要去除目录结构，打包后压缩包中的文件会保留原有的目录结构；1：需要，打包后压缩包内的文件会去除原有的目录结构，所有文件都在同一层级。例如：源文件 URL 为 https://domain/source/test.mp4，则源文件路径为 source/test.mp4，如果为 1，则 ZIP 包中该文件路径为 test.mp4；如果为0， ZIP 包中该文件路径为 source/test.mp4。
            /// <summary>            
            [XmlElement]
            public string Flatten;

            /// <summary>            
            /// 打包压缩的类型，有效值：zip、tar、tar.gz。
            /// <summary>            
            [XmlElement]
            public string Format;

            /// <summary>            
            /// 压缩类型，仅在Format为tar.gz或zip时有效。faster：压缩速度较快better：压缩质量较高，体积较小default：适中的压缩方式默认值为default
            /// <summary>            
            [XmlElement]
            public string Type;

            /// <summary>            
            /// 压缩包密钥，传入时需先经过 base64 编码，编码后长度不能超过128。当 Format 为 zip 时生效。
            /// <summary>            
            [XmlElement]
            public string CompressKey;

            /// <summary>            
            /// 支持将需要打包的文件整理成索引文件，后台将根据索引文件内提供的文件 url，打包为一个压缩包文件。索引文件需要保存在当前存储桶中，本字段需要提供索引文件的对象地址，不需要带域名，填写示例：/test/index.csv索引文件格式：仅支持 CSV 文件，一行一条 URL（仅支持本存储桶文件），如有多列字段，默认取第一列作为URL。
            /// <summary>            
            [XmlElement]
            public string UrlList;

            /// <summary>            
            /// 支持对存储桶中的某个前缀进行打包，如果需要对某个目录进行打包，需要加/，例如test目录打包，则值为：test/。
            /// <summary>            
            [XmlElement]
            public string Prefix;

            /// <summary>            
            /// 支持对存储桶中的多个文件进行打包，个数不能超过 1000，如需打包更多文件，请使用UrlList或Prefix参数。
            /// <summary>            
            [XmlElement]
            public List<string> Key;

            /// <summary>            
            /// 打包时如果单个文件出错，是否忽略错误继续打包。有效值为：ture：忽略错误继续打包后续的文件；false：遇到某个文件执行打包报错时，直接终止打包任务，不返回压缩包。默认值为false。
            /// <summary>            
            [XmlElement]
            public string IgnoreError ;

            /// <summary>            
            /// 支持对存储桶中的文件进行打包，可填写多个，个数不能超过 1000，如需打包更多文件，请使用 UrlList 或 Prefix 参数。
            /// <summary>            
            [XmlElement]
            public List<KeyConfig> KeyConfig;

        }

        public sealed class KeyConfig {

            /// <summary>            
            /// 存储桶中的包含路径的完整文件名称，请使用 UrlList 或 Prefix 参数。
            /// <summary>            
            [XmlElement]
            public string Key;

            /// <summary>            
            /// 文件重命名，打包后文件名将以该值为准，不填表示不更改文件名。
            /// <summary>            
            [XmlElement]
            public string Rename;

            /// <summary>            
            /// 图片处理参数，支持的参数详见 基础图片处理，填写示例：imageMogr2/thumbnail/!50p
            /// <summary>            
            [XmlElement]
            public string ImageParams;

        }

        public sealed class FileCompressResult {

            /// <summary>            
            /// 打包压缩后文件保存的存储桶的地域。
            /// <summary>            
            [XmlElement]
            public string Region;

            /// <summary>            
            /// 打包压缩后文件保存的存储桶。
            /// <summary>            
            [XmlElement]
            public string Bucket;

            /// <summary>            
            /// 打包压缩后文件的名称。
            /// <summary>            
            [XmlElement]
            public string Object;

            /// <summary>            
            /// 打包文件的总数
            /// <summary>            
            [XmlElement]
            public string CompressFileCount  ;

            /// <summary>            
            /// 打包时出错的文件数 
            /// <summary>            
            [XmlElement]
            public string ErrorCount;

        }

    }


}

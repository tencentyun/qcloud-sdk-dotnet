using COSXML;
using COSXML.Auth;
using COSXML.Model.CI;


namespace COSXMLDemo
{
    public class CreateFileZipProcessJobModel
    {
        
        public CosXml cosXml;
        
        // 初始化COS服务实例
        private void InitCosXml()
        {
            string region = Environment.GetEnvironmentVariable("COS_REGION");
            CosXmlConfig config = new CosXmlConfig.Builder()
                .SetRegion(region) // 设置默认的地域, COS 地域的简称请参照 https://cloud.tencent.com/document/product/436/6224
                .Build();
            string secretId = Environment.GetEnvironmentVariable("SECRET_ID"); // 云 API 密钥 SecretId, 获取 API 密钥请参照 https://console.cloud.tencent.com/cam/capi
            string secretKey = Environment.GetEnvironmentVariable("SECRET_KEY"); // 云 API 密钥 SecretKey, 获取 API 密钥请参照 https://console.cloud.tencent.com/cam/capi
            long durationSecond = 600; //每次请求签名有效时长，单位为秒
            QCloudCredentialProvider qCloudCredentialProvider = new DefaultQCloudCredentialProvider(secretId, secretKey, durationSecond);
            this.cosXml = new CosXmlServer(config, qCloudCredentialProvider);
        }

        CreateFileZipProcessJobModel()
        {
            InitCosXml();
        }
        
        public void CreateFileZipProcessJob()
        {
            try
            {
                string bucket = "bucketname-APPID";
                string textKey = "";
                CreateFileZipProcessJobsRequest request = new CreateFileZipProcessJobsRequest(bucket);
                // 表示任务的类型，多文件打包压缩默认为：FileCompress。;
                request.SetTag("FileCompress");
                // 文件打包时，是否需要去除源文件已有的目录结构，有效值：0：不需要去除目录结构，打包后压缩包中的文件会保留原有的目录结构；1：需要，打包后压缩包内的文件会去除原有的目录结构，所有文件都在同一层级。例如：源文件 URL 为 https://domain/source/test.mp4，则源文件路径为 source/test.mp4，如果为 1，则 ZIP 包中该文件路径为 test.mp4；如果为0， ZIP 包中该文件路径为 source/test.mp4。;
                request.SetFlatten("0");
                // 打包压缩的类型，有效值：zip、tar、tar.gz。;
                request.SetFormat("zip");
                // 压缩类型，仅在Format为tar.gz或zip时有效。faster：压缩速度较快better：压缩质量较高，体积较小default：适中的压缩方式默认值为default;
                request.SetType("better");
                // 压缩包密钥，传入时需先经过 base64 编码，编码后长度不能超过128。当 Format 为 zip 时生效。;
                request.SetCompressKey("");
                // 支持将需要打包的文件整理成索引文件，后台将根据索引文件内提供的文件 url，打包为一个压缩包文件。索引文件需要保存在当前存储桶中，本字段需要提供索引文件的对象地址，不需要带域名，填写示例：/test/index.csv索引文件格式：仅支持 CSV 文件，一行一条 URL（仅支持本存储桶文件），如有多列字段，默认取第一列作为URL。;
                request.SetUrlList("");
                // 支持对存储桶中的某个前缀进行打包，如果需要对某个目录进行打包，需要加/，例如test目录打包，则值为：test/。;
                request.SetPrefix("");
                // 支持对存储桶中的多个文件进行打包，个数不能超过 1000，如需打包更多文件，请使用UrlList或Prefix参数。;
                COSXML.Model.CI.CreateFileZipProcessJobs.KeyConfig keyConfig = new  CreateFileZipProcessJobs.KeyConfig();
                keyConfig.key = "CITestImage.png";
                keyConfig.rename = "CITestImage.zip";
                keyConfig.imageParams = "";
                request.setKeyConfig(keyConfig);
                // 打包时如果单个文件出错，是否忽略错误继续打包。有效值为：ture：忽略错误继续打包后续的文件；false：遇到某个文件执行打包报错时，直接终止打包任务，不返回压缩包。默认值为false。;
                request.SetIgnoreError ("true");
                // 透传用户信息，可打印的 ASCII 码，长度不超过1024。;
                request.SetUserData("");
                // 存储桶的地域。;
                request.SetRegion("");
                // 保存压缩后文件的存储桶。;
                request.SetBucket(bucket);
                // 压缩后文件的文件名;
                request.SetObjectInfo("");
                // 任务回调格式，JSON 或 XML，默认 XML，优先级高于队列的回调格式。;
                request.SetCallBackFormat("");
                // 任务回调类型，Url 或 TDMQ，默认 Url，优先级高于队列的回调类型。;
                request.SetCallBackType("Url");
                // 任务回调的地址，优先级高于队列的回调地址。;
                request.SetCallBack("");
                // 消息队列所属园区，目前支持园区 sh（上海）、bj（北京）、gz（广州）、cd（成都）、hk（中国香港）;
                request.SetMqRegion("");
                // 消息队列使用模式，默认 Queue ：主题订阅：Topic队列服务: Queue;
                request.SetMqMode("");
                // TDMQ 主题名称;
                request.SetMqName("");
                request.createFileZipProcessJobs.GetInfo();
                CreateFileZipProcessJobsResult result = cosXml.createFileZipProcessJobs(request);

                Console.WriteLine(result.createFileZipProcessJobsResult.JobsDetail.JobId);
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx);
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
            }
        }
        
        public static void CreateFileZipProcessJobModelMain()
        {
            CreateFileZipProcessJobModel m = new CreateFileZipProcessJobModel();
            m.CreateFileZipProcessJob();
        }
    }
}

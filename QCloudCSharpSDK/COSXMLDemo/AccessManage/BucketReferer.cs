using COSXML;
using COSXML.Auth;
using COSXML.Model.Bucket;
using COSXML.Model.Tag;
namespace COSXMLDemo
{
    public class BucketRefererModel
    {
        private CosXml cosXml;

        public string bucket;

        public void InitParams()
        {
            bucket = Environment.GetEnvironmentVariable("BUCKET");
        }

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

        BucketRefererModel()
        {
            InitCosXml();
            InitParams();
        }
        
        // 设置存储桶防盗链
        public void PutBucketReferer()
        {
            try
            {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                string bucket = "examplebucket-1250000000";
                PutBucketRefererRequest request = new PutBucketRefererRequest(bucket);
                // 设置防盗链规则
                RefererConfiguration configuration = new RefererConfiguration();
                configuration.Status = "Enabled"; // 是否开启防盗链，枚举值：Enabled、Disabled
                configuration.RefererType = "White-List"; // 防盗链类型，枚举值：Black-List、White-List
                // 生效域名列表， 支持多个域名且为前缀匹配， 支持带端口的域名和 IP， 支持通配符*，做二级域名或多级域名的通配
                configuration.domainList = new DomainList();
                // 单条生效域名 例如www.qq.com/example，192.168.1.2:8080， *.qq.com
                configuration.domainList.AddDomain("*.domain1.com");
                configuration.domainList.AddDomain("*.domain2.com");
                // 是否允许空 Referer 访问，枚举值：Allow、Deny，默认值为 Deny
                configuration.EmptyReferConfiguration = "Deny";
                request.SetRefererConfiguration(configuration);
                //执行请求
                PutBucketRefererResult result = cosXml.PutBucketReferer(request);
                //请求成功
                Console.WriteLine(result.GetResultInfo());
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

        // 获取存储桶防盗链规则
        public void GetBucketReferer()
        {
            try
            {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                string bucket = "examplebucket-1250000000";
                GetBucketRefererRequest request = new GetBucketRefererRequest(bucket);
                // 执行请求
                GetBucketRefererResult result = cosXml.GetBucketReferer(request);
                // Status参数
                Console.WriteLine(result.GetResultInfo());
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

        public static void BucketRefererMain()
        {
            BucketRefererModel m = new BucketRefererModel();
            m.PutBucketReferer();
            m.GetBucketReferer();
        }
    }
}

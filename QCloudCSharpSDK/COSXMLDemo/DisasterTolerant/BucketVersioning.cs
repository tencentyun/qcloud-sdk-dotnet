using COSXML;
using COSXML.Auth;
using COSXML.Model.Bucket;
using COSXML.Model.Tag;

namespace COSXMLDemo.DisasterTolerant
{
    public class BucketVersioningModel
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

        BucketVersioningModel()
        {
            InitCosXml();
            InitParams();
        }

        // 设置存储桶多版本
        public void PutBucketVersioning()
        {
            // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
            string bucket = "examplebucket-1250000000";
            PutBucketVersioningRequest request = new PutBucketVersioningRequest(bucket);
            request.IsEnableVersionConfig(true); //true: 开启版本控制; false:暂停版本控制

            try
            {
                PutBucketVersioningResult result = cosXml.PutBucketVersioning(request);
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

        // 获取存储桶多版本状态
        public void GetBucketVersioning()
        {
            // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
            string bucket = "examplebucket-1250000000";
            GetBucketVersioningRequest request = new GetBucketVersioningRequest(bucket);
            try
            {
                GetBucketVersioningResult result = cosXml.GetBucketVersioning(request);
                // 存储桶的生命周期配置
                VersioningConfiguration conf = result.versioningConfiguration;
                Console.WriteLine(conf.GetInfo());
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
        
        public static void BucketVersioningMain()
        {
            BucketVersioningModel m = new BucketVersioningModel();
            m.PutBucketVersioning();
            m.GetBucketVersioning();
        }
    }
}
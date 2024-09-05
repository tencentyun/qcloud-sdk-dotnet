using COSXML;
using COSXML.Auth;
using COSXML.Model.Bucket;
using COSXML.Model.Tag;
namespace COSXMLDemo
{
    public class BucketLoggingModel
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

        BucketLoggingModel()
        {
            InitCosXml();
            InitParams();
        }

        // 开启存储桶日志服务
        public void PutBucketLogging()
        {
            try
            {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                string bucket = "examplebucket-1250000000";
                PutBucketLoggingRequest request = new PutBucketLoggingRequest(bucket);
                // 设置保存日志的目标路径
                request.SetTarget("targetbucket-1250000000", "logs/");
                //执行请求
                PutBucketLoggingResult result = cosXml.PutBucketLogging(request);
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

        // 获取存储桶日志服务
        public void GetBucketLogging()
        {
            try
            {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                // string bucket = "examplebucket-1250000000";
                GetBucketLoggingRequest request = new GetBucketLoggingRequest(bucket);
                //执行请求
                GetBucketLoggingResult result = cosXml.GetBucketLogging(request);
                //请求成功
                BucketLoggingStatus status = result.bucketLoggingStatus;
                if (status != null && status.loggingEnabled != null)
                {
                    string targetBucket = status.loggingEnabled.targetBucket;
                    string targetPrefix = status.loggingEnabled.targetPrefix;
                }
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

        public static void BucketLoggingMain()
        {
            BucketLoggingModel e = new BucketLoggingModel();
            e.PutBucketLogging();
            e.GetBucketLogging();
        }

    }
}

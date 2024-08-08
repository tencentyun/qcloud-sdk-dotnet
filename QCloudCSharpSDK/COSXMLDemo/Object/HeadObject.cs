using COSXML;
using COSXML.Auth;
using COSXML.Model.Object;

namespace COSXMLDemo
{
    public class HeadObjectModel
    {
        public CosXml cosXml;
        // 初始化COS服务实例
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

        HeadObjectModel()
        {
            InitCosXml();
            InitParams();
        }
        
        public void HeadObject()
        {
            try
            {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                string bucket = "examplebucket-1250000000";
                string key = "exampleObject"; //对象键
                HeadObjectRequest request = new HeadObjectRequest(bucket, key);
                //执行请求
                HeadObjectResult result = cosXml.HeadObject(request);
                //请求成功
                Console.WriteLine(result.GetResultInfo());
                // 打印对象crc64 和 etag信息
                Console.WriteLine(result.crc64ecma);
                Console.WriteLine(result.eTag);
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
        
        public static void HeadObjectMain()
        {
            HeadObjectModel demo = new HeadObjectModel();
            
            demo.HeadObject();
        }
    }
}

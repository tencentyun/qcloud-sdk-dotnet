using COSXML;
using COSXML.Auth;
using COSXML.Model.Bucket;
using COSXML.Model.Object;
using COSXML.Model.Tag;
namespace COSXMLDemo
{
    public class BucketCORSModel
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

        BucketCORSModel()
        {
            InitCosXml();
            InitParams();
        }

        // 设置存储桶跨域规则
        public void PutBucketCors()
        {
            try
            {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                string bucket = "examplebucket-1250000000";
                PutBucketCORSRequest request = new PutBucketCORSRequest(bucket);
                //设置跨域访问配置 CORS
                COSXML.Model.Tag.CORSConfiguration.CORSRule corsRule =
                    new COSXML.Model.Tag.CORSConfiguration.CORSRule();
                corsRule.id = "corsconfigureId";
                corsRule.maxAgeSeconds = 6000;

                corsRule.allowedOrigins = new List<string>();
                corsRule.allowedOrigins.Add("http://cloud.tencent.com");

                corsRule.allowedMethods = new List<string>();
                corsRule.allowedMethods.Add("PUT");

                corsRule.allowedHeaders = new List<string>();
                corsRule.allowedHeaders.Add("Host");

                corsRule.exposeHeaders = new List<string>();
                corsRule.exposeHeaders.Add("x-cos-meta-x1");

                request.SetCORSRule(corsRule);

                //执行请求
                PutBucketCORSResult result = cosXml.PutBucketCORS(request);
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

        // 获取存储桶跨域规则
        public void GetBucketCors()
        {
            try
            {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                string bucket = "examplebucket-1250000000";
                GetBucketCORSRequest request = new GetBucketCORSRequest(bucket);
                //执行请求
                GetBucketCORSResult result = cosXml.GetBucketCORS(request);
                //存储桶的 CORS 配置信息
                CORSConfiguration conf = result.corsConfiguration;
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

        // 实现 Object 跨域访问配置的预请求
        public void OptionObject()
        {
            try
            {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                string bucket = "examplebucket-1250000000";
                string key = "exampleobject"; //对象键
                string origin = "http://cloud.tencent.com";
                string accessMthod = "PUT";
                OptionObjectRequest request = new OptionObjectRequest(bucket, key, origin, accessMthod);
                //执行请求
                OptionObjectResult result = cosXml.OptionObject(request);
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

        // 删除存储桶跨域规则
        public void DeleteBucketCors()
        {
            try
            {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                string bucket = "examplebucket-1250000000";
                DeleteBucketCORSRequest request = new DeleteBucketCORSRequest(bucket);
                //执行请求
                DeleteBucketCORSResult result = cosXml.DeleteBucketCORS(request);
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

        public static void BucketCORSMain()
        {
            BucketCORSModel m = new BucketCORSModel();
            m.PutBucketCors();

            m.GetBucketCors();

            m.DeleteBucketCors();

            m.OptionObject();
        }

    }
}

using COSXML;
using COSXML.Auth;
using COSXML.Model.Bucket;
namespace COSXMLDemo
{
    public class BucketWebsiteModel
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

        BucketWebsiteModel()
        {
            InitCosXml();
            InitParams();
        }

        // 设置存储桶静态网站
        public void PutBucketWebsite()
        {
            try
            {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                string bucket = "examplebucket-1250000000";
                PutBucketWebsiteRequest putRequest = new PutBucketWebsiteRequest(bucket);
                putRequest.SetIndexDocument("index.html");
                putRequest.SetErrorDocument("eroror.html");
                putRequest.SetRedirectAllRequestTo("index.html");
                PutBucketWebsiteResult putResult = cosXml.PutBucketWebsite(putRequest);

                //请求成功
                Console.WriteLine(putResult.GetResultInfo());
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

        // 获取存储桶静态网站
        public void GetBucketWebsite()
        {
            try
            {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                string bucket = "examplebucket-1250000000";
                GetBucketWebsiteRequest request = new GetBucketWebsiteRequest(bucket);
                //执行请求
                GetBucketWebsiteResult result = cosXml.GetBucketWebsite(request);

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

        // 删除存储桶静态网站
        public void DeleteBucketWebsite()
        {
            try
            {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                string bucket = "examplebucket-1250000000";
                DeleteBucketWebsiteRequest request = new DeleteBucketWebsiteRequest(bucket);
                //执行请求
                DeleteBucketWebsiteResult result = cosXml.DeleteBucketWebsite(request);

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


        public static void BucketWebsiteMain()
        {
            BucketWebsiteModel m = new BucketWebsiteModel();

            m.PutBucketWebsite();

            m.GetBucketWebsite();

            m.DeleteBucketWebsite();

        }
    }
}

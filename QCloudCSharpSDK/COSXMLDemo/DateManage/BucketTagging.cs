using COSXML;
using COSXML.Auth;
using COSXML.Model.Bucket;
using COSXML.Model.Tag;
namespace COSXMLDemo
{
    public class BucketTaggingModel
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

        BucketTaggingModel()
        {
            InitCosXml();
            InitParams();
        }

        // 设置存储桶标签
        public void PutBucketTagging()
        {
            try
            {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                string bucket = "examplebucket-1250000000";
                PutBucketTaggingRequest request = new PutBucketTaggingRequest(bucket);
                string akey = "aTagKey";
                string avalue = "aTagValue";
                string bkey = "bTagKey";
                string bvalue = "bTagValue";

                request.AddTag(akey, avalue);
                request.AddTag(bkey, bvalue);

                //执行请求
                PutBucketTaggingResult result = cosXml.PutBucketTagging(request);
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

        // 获取存储桶标签
        public void GetBucketTagging()
        {
            try
            {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                string bucket = "examplebucket-1250000000";
                GetBucketTaggingRequest request = new GetBucketTaggingRequest(bucket);
                //执行请求
                GetBucketTaggingResult result = cosXml.GetBucketTagging(request);
                //请求成功
                Tagging tagging = result.tagging;
                Console.WriteLine(tagging);
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

        // 删除存储桶标签
        public void DeleteBucketTagging()
        {
            try
            {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                string bucket = "examplebucket-1250000000";
                DeleteBucketTaggingRequest request = new DeleteBucketTaggingRequest(bucket);
                //执行请求
                DeleteBucketTaggingResult result = cosXml.DeleteBucketTagging(request);
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


        public static void BucketTaggingMain()
        {
            BucketTaggingModel m = new BucketTaggingModel();

            m.PutBucketTagging();

            m.GetBucketTagging();

            m.DeleteBucketTagging();
        }

    }
}

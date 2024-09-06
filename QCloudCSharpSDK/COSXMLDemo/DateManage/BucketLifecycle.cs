using COSXML;
using COSXML.Auth;
using COSXML.Model.Bucket;
using COSXML.Model.Tag;
namespace COSXMLDemo
{
    public class BucketLifecycleModel
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
        
        BucketLifecycleModel()
        {
            InitCosXml();
            InitParams();
        }
        
        // 设置存储桶生命周期
        public void PutBucketLifecycle()
        {
            try
            {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                string bucket = "examplebucket-1250000000";
                PutBucketLifecycleRequest request = new PutBucketLifecycleRequest(bucket);
                //设置 lifecycle
                LifecycleConfiguration.Rule rule = new LifecycleConfiguration.Rule();
                rule.id = "lfiecycleConfigureId";
                rule.status = "Enabled"; //Enabled，Disabled

                rule.filter = new COSXML.Model.Tag.LifecycleConfiguration.Filter();
                rule.filter.prefix = "2/";

                //指定分片过期删除操作
                rule.abortIncompleteMultiUpload = new LifecycleConfiguration.AbortIncompleteMultiUpload();
                rule.abortIncompleteMultiUpload.daysAfterInitiation = 2;

                request.SetRule(rule);

                //执行请求
                PutBucketLifecycleResult result = cosXml.PutBucketLifecycle(request);
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

        // 获取存储桶生命周期
        public void GetBucketLifecycle()
        {
            try
            {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                string bucket = "examplebucket-1250000000";
                GetBucketLifecycleRequest request = new GetBucketLifecycleRequest(bucket);
                //执行请求
                GetBucketLifecycleResult result = cosXml.GetBucketLifecycle(request);
                //存储桶的生命周期配置
                LifecycleConfiguration conf = result.lifecycleConfiguration;
                if (result.httpCode == 200) {
                    Console.WriteLine(conf.GetInfo());
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
        
        // 删除存储桶生命周期
        public void DeleteBucketLifecycle()
        {
            try
            {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                string bucket = "examplebucket-1250000000";
                DeleteBucketLifecycleRequest request = new DeleteBucketLifecycleRequest(bucket);
                //执行请求
                DeleteBucketLifecycleResult result = cosXml.DeleteBucketLifecycle(request);
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
        
        public static void  BucketLifecycleMain()
        {
            BucketLifecycleModel m = new BucketLifecycleModel();
            
            m.PutBucketLifecycle();
            
            m.GetBucketLifecycle();
            
            m.DeleteBucketLifecycle();
        }
    }
}

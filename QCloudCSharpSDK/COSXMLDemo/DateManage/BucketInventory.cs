using COSXML;
using COSXML.Auth;
using COSXML.Model.Bucket;
using COSXML.Model.Object;
using COSXML.Model.Tag;
namespace COSXMLDemo
{
    public class BucketInventoryModel
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

        BucketInventoryModel()
        {
            InitCosXml();
            InitParams();
        }

        // 设置存储桶清单任务
        public void PutBucketInventory()
        {
            try
            {
                string inventoryId = "aInventoryId";
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                string bucket = "examplebucket-1250000000";
                PutBucketInventoryRequest putRequest = new PutBucketInventoryRequest(bucket, inventoryId);
                putRequest.SetDestination("CSV", "100000000001", "examplebucket-1250000000", "ap-guangzhou", "list1");
                putRequest.IsEnable(true);
                // 清单任务周期，枚举值：Daily、Weekly
                putRequest.SetScheduleFrequency("Daily");
                // 是否在清单中包含对象版本，枚举值：All、Current
                putRequest.SetIncludedObjectVersions("All");
                //执行请求
                PutBucketInventoryResult putResult = cosXml.PutBucketInventory(putRequest);
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

        // 获取存储桶清单任务
        public void GetBucketInventory()
        {
            try
            {
                string inventoryId = "aInventoryId";
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                string bucket = "examplebucket-1250000000";
                GetBucketInventoryRequest getRequest = new GetBucketInventoryRequest(bucket);
                getRequest.SetInventoryId(inventoryId);
                GetBucketInventoryResult getResult = cosXml.GetBucketInventory(getRequest);
                InventoryConfiguration configuration = getResult.inventoryConfiguration;
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

        // 删除存储桶清单任务
        public void DeleteBucketInventory()
        {
            try
            {
                string inventoryId = "aInventoryId";
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                string bucket = "examplebucket-1250000000";
                DeleteBucketInventoryRequest deleteRequest = new DeleteBucketInventoryRequest(bucket);
                deleteRequest.SetInventoryId(inventoryId);
                DeleteBucketInventoryResult deleteResult = cosXml.DeleteBucketInventory(deleteRequest);
                //请求成功
                Console.WriteLine(deleteResult.GetResultInfo());
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


        public static void BucketInventoryMain()
        {
            BucketInventoryModel m = new BucketInventoryModel();

            m.PutBucketInventory();

            m.GetBucketInventory();

            m.DeleteBucketInventory();
        }

    }
}

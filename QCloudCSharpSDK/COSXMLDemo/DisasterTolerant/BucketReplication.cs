using COSXML;
using COSXML.Auth;
using COSXML.Model.Bucket;
using COSXML.Model.Tag;
namespace COSXMLDemo.DisasterTolerant
{
    public class BucketReplicationModel
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

        BucketReplicationModel()
        {
            InitCosXml();
            InitParams();
        }
        
        // 设置存储桶跨地域复制规则
        public void PutBucketReplication()
        {
            // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
            string bucket = "examplebucket-1250000000";
            string ownerUin = "100000000001"; //发起者身份标示: OwnerUin
            string subUin = "100000000001"; //发起者身份标示: SubUin
            PutBucketReplicationRequest request = new PutBucketReplicationRequest(bucket);
            //设置 replication
            PutBucketReplicationRequest.RuleStruct ruleStruct =
                new PutBucketReplicationRequest.RuleStruct();
            ruleStruct.id = "replication_01"; //用来标注具体 Rule 的名称
            ruleStruct.isEnable = true; //标识 Rule 是否生效 :true, 生效； false, 不生效
            ruleStruct.appid = "1250000000"; //APPID
            ruleStruct.region = "ap-beijing"; //目标存储桶地域信息
            ruleStruct.bucket = "destinationbucket-1250000000"; //格式：BucketName-APPID
            ruleStruct.prefix = "34"; //前缀匹配策略
            List<PutBucketReplicationRequest.RuleStruct> ruleStructs = new List<PutBucketReplicationRequest.RuleStruct>();
            ruleStructs.Add(ruleStruct);
            request.SetReplicationConfiguration(ownerUin, subUin, ruleStructs);
            try
            {
                PutBucketReplicationResult result = cosXml.PutBucketReplication(request);
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

        // 获取存储桶跨地域复制规则
        public void GetBucketReplication()
        {
            // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
            string bucket = "examplebucket-1250000000";
            GetBucketReplicationRequest request = new GetBucketReplicationRequest(bucket);
            try
            {
                GetBucketReplicationResult result = cosXml.GetBucketReplication(request);
                // 存储桶的跨区域复制配置
                ReplicationConfiguration conf = result.replicationConfiguration;
                
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

        // 删除存储桶跨地域复制规则
        public void DeleteBucketReplication()
        {
            // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
            string bucket = "examplebucket-1250000000";
            DeleteBucketReplicationRequest request = new DeleteBucketReplicationRequest(bucket);
            try
            {
                DeleteBucketReplicationResult result = cosXml.DeleteBucketReplication(request);
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

        public static void BucketReplicationMain()
        {
            BucketReplicationModel m = new BucketReplicationModel();
            
            m.PutBucketReplication();
            
            m.GetBucketReplication();
            
            m.DeleteBucketReplication();
        }
    }
}

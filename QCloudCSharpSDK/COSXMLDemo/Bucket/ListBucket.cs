using COSXML;
using COSXML.Auth;
using COSXML.Model.Service;
using COSXML.Model.Tag;

namespace COSXMLDemo
{
    public class ListBucketModel
    {
        public CosXml cosXml;
        
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
        
        ListBucketModel()
        {
            InitCosXml();
        }
        
        public void GetService()
        {
            try
            {
                GetServiceRequest request = new GetServiceRequest();
                //执行请求
                GetServiceResult result = cosXml.GetService(request);
                //得到所有的 buckets
                List<ListAllMyBuckets.Bucket> allBuckets = result.listAllMyBuckets.buckets;
                foreach (var bucket  in allBuckets)
                {
                    Console.WriteLine(bucket.name);
                }
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
        
        public static void ListBucketModelMain()
        {
            ListBucketModel m = new ListBucketModel();
            m.GetService();
        }
    }
}

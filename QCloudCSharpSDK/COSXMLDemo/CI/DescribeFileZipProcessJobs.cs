using COSXML;
using COSXML.Auth;
using COSXML.Model.CI;

namespace COSXMLDemo
{
    public class DescribeFileZipProcessJobsModel
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

        DescribeFileZipProcessJobsModel()
        {
            InitCosXml();
        }
        
        public void DescribeFileZipProcessJobs()
        {
            try
            {
                string bucket = "bucketname-APPID";
                string jobId = "";
                DescribeFileZipProcessJobsRequest describeDocProcessJobsRequest = new DescribeFileZipProcessJobsRequest(bucket,jobId);
                DescribeFileZipProcessJobsResult describeFileZipProcessJobsResult= cosXml.describeFileZipProcessJobs(describeDocProcessJobsRequest);
                Console.WriteLine(describeFileZipProcessJobsResult.describeFileZipProcessJobsResult.JobsDetail.Count);
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
        
        public static void DescribeFileZipProcessJobsModelMain()
        {
            DescribeFileZipProcessJobsModel m = new DescribeFileZipProcessJobsModel();
            m.DescribeFileZipProcessJobs();
        }
    }
}

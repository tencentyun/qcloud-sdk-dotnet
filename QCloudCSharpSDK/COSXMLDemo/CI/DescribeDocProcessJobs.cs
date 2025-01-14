using COSXML;
using COSXML.Auth;
using COSXML.Model.CI;
namespace COSXMLDemo
{
    public class DescribeDocProcessJobsModel
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

        DescribeDocProcessJobsModel()
        {
            InitCosXml();
        }

        public void DescribeDocProcessJobs()
        {
            try
            {
                string bucket = "bucketname-APPID";
                string jobId = "";
                string queueId = "";
                DescribeDocProcessJobsRequest describeDocProcessJobsRequest = new DescribeDocProcessJobsRequest(bucket);
                describeDocProcessJobsRequest.SetTag("DocProcess");
                describeDocProcessJobsRequest.SetQueueId(queueId);
                describeDocProcessJobsRequest.SetOrderByTime("Asc");
                describeDocProcessJobsRequest.SetNextToken("1");
                describeDocProcessJobsRequest.SetStates("All");
                describeDocProcessJobsRequest.SetSize("15");
                describeDocProcessJobsRequest.SetStartCreationTime("2024-06-12T08:20:07+0800");
                describeDocProcessJobsRequest.SetEndCreationTime("2024-06-12T20:00:00+0800");

                DescribeDocProcessJobsResult describeDocProcessJobsResult = cosXml.DescribeDocProcessJobs(describeDocProcessJobsRequest);
                Console.WriteLine(describeDocProcessJobsResult.listDocProcessResult.JobsDetail.Count);
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
        
        public static void DescribeDocProcessJobsModelMain()
        {
            DescribeDocProcessJobsModel m = new DescribeDocProcessJobsModel();
            m.DescribeDocProcessJobs();
        }
    }
}

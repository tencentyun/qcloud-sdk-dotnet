using COSXML;
using COSXML.Auth;
using COSXML.Model.CI;

namespace COSXMLDemo
{
    public class CreateDocProcessJobsModel
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

        CreateDocProcessJobsModel()
        {
            InitCosXml();
        }
        
        public void CreateDocProcessJobs()
        {
            try
            {
                string bucket = "bucketname-APPID";
                string textKey = "";

                CreateDocProcessJobsRequest request = new CreateDocProcessJobsRequest(bucket);
                request.SetInputObject("demo.docx");
                request.SetTag("DocProcess");
                request.SetSrcType("docx");
                request.SetTgtType("jpg");
                request.SetStartPage("3");
                request.SetEndPage("5");
                request.SetImageParams("imageMogr2/cut/400x400");
                request.SetQuality("90");
                request.SetZoom("200");
                request.SetImageDpi("100");
                request.SetPicPagination("1");
                request.SetOutputBucket("");
                request.SetOutputObject("");
                request.SetOutputRegion("");
                request.SetSheetId("1");
                request.SetPaperDirection("1");
                request.SetPaperSize("1");
                CreateDocProcessJobsResult createDocProcessJobsResult = cosXml.CreateDocProcessJobs(request);
                Console.WriteLine(createDocProcessJobsResult.docProcessResponse.JobsDetail.JobId);
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
        
        public static void CreateDocProcessJobsModelMain()
        {
            CreateDocProcessJobsModel m = new CreateDocProcessJobsModel();
            m.CreateDocProcessJobs();
        }
    }
}

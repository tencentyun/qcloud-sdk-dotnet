using System.Text;
using System.Text.Json;
using COSXML;
using COSXML.Auth;
using COSXML.Model.CI;
using COSXML.Utils;

namespace COSXMLDemo
{
    public class CreateDocPreviewJobModel
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

        CreateDocPreviewJobModel()
        {
            InitCosXml();
        }
        
        public void CreateDocPreviewJob()
        {
            try
            {
                string bucket = "bucketname-APPID";
                string textKey = "";
                CreateDocPreviewRequest request = new CreateDocPreviewRequest(bucket,textKey);
                // 输入文件类型
                request.SetSrcType("txt");
                // 是否可复制。默认为可复制，填入值为1；不可复制，填入值为0
                request.SetCopyable("1");
                // 创建并初始化 HtmlParams 对象
                var htmlParams = new CreateDocPreviewRequest.HtmlParams
                {
                    CommonOptions = new CreateDocPreviewRequest.CommonOptions
                    {
                        IsShowTopArea = true,
                        IsShowHeader = true,
                        Language = "zh",
                        isBrowserViewFullscreen = true,
                        isIframeViewFullscreen = true,
                    },
                    WordOptions = new CreateDocPreviewRequest.WordOptions
                    {
                        isShowDocMap = false,
                        isBestScale = false,
                    },
                    PdfOptions = new CreateDocPreviewRequest.PdfOptions
                    {
                        isShowComment = false,
                        isInSafeMode = false,
                        isShowBottomStatusBar = false,
                    },
                };
                request.SetHtmlParams(DigestUtils.GetBase64(JsonSerializer.Serialize(htmlParams), Encoding.UTF8));
                request.SetHtmlWaterword("5pWw5o2u5LiH6LGhLeaWh+aho+mihOiniA==");
                request.SetHtmlFillstyle("cmdiYSgxMDIsMjA0LDI1NSwwLjMp");
                request.SetHtmlFront("Ym9sZCAyNXB4IFNlcmlm");
                request.SetHtmlRotate("315");
                request.SetHtmlHorizontal("50");
                request.SetHtmlVertical("100");
                request.SetHtmlTitle("6IW+6K6v5LqRLeaVsOaNruS4h+ixoQ==");
                // 设置预签名时间
                request.SetSignExpired(600);
                String createDocPreviewUrl = cosXml.createDocPreview(request);
                Console.WriteLine(createDocPreviewUrl);
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
        
        public static void CreateDocPrewivewJobModelMain()
        {
            CreateDocPreviewJobModel m = new CreateDocPreviewJobModel();
            m.CreateDocPreviewJob();
        }
    }
}

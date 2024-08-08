using System.Runtime.InteropServices;
using COSXML;
using COSXML.Auth;
using COSXML.Model.Object;
using COSXML.Model.Tag;
namespace COSXMLDemo
{
    public class SelectObjectDemo
    {
        public CosXml cosXml;
        // 初始化COS服务实例
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

        SelectObjectDemo()
        {
            InitParams();
            InitCosXml();
        }
        
        // 检索对象内容
        public void SelectObject()
        {
            try
            {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                string bucket = "examplebucket-1250000000";
                string key = "exampleObject"; //对象键,文件内容格式为json

                SelectObjectRequest request = new SelectObjectRequest(bucket, key);

                ObjectSelectionFormat.JSONFormat jSONFormat = new ObjectSelectionFormat.JSONFormat();
                jSONFormat.Type = "DOCUMENT";
                jSONFormat.RecordDelimiter = "\n";
            
                string outputFile = "select_local_file.json";

                request.SetExpression("Select * from COSObject")
                    .SetInputFormat(new ObjectSelectionFormat(null, jSONFormat))
                    .SetOutputFormat(new ObjectSelectionFormat(null, jSONFormat))
                    .OutputToFile(outputFile);
                
                request.SetCosProgressCallback(delegate(long progress, long total)
                {
                    Console.WriteLine("OnProgress : " + progress + "," + total);
                });
                
                SelectObjectResult selectObjectResult =  cosXml.SelectObject(request);
                Console.WriteLine(selectObjectResult.GetResultInfo());
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.StackTrace);
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
            }
        }

        public static void SelectObjectMain()
        {
            SelectObjectDemo demo = new SelectObjectDemo();
            demo.SelectObject();
        }
    }
}

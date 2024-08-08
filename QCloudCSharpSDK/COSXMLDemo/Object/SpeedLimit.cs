using COSXML;
using COSXML.Auth;
using COSXML.Model.Object;
using COSXML.Transfer;
namespace COSXMLDemo
{
    public class SpeedLimitModel
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

        SpeedLimitModel()
        {
            InitParams();
            InitCosXml();
        }
        
        // 下载时对单链接限速
        public async void DownloadObjectTrafficLimit()
        {
            TransferConfig transferConfig = new TransferConfig();
            // 初始化 TransferManager
            TransferManager transferManager = new TransferManager(cosXml, transferConfig);
        
            String bucket = "examplebucket-1250000000"; //存储桶，格式：BucketName-APPID
            String cosPath = "exampleobject"; //对象在存储桶中的位置标识符，即称对象键
            string localDir = System.IO.Path.GetTempPath();//本地文件夹
            string localFileName = "my-local-temp-file"; //指定本地保存的文件名
        
            GetObjectRequest request = new GetObjectRequest(bucket, cosPath, localDir, localFileName);
            request.LimitTraffic(8 * 1000 * 1024); // 限制为1MB/s

            COSXMLDownloadTask downloadTask = new COSXMLDownloadTask(request);
            await transferManager.DownloadAsync(downloadTask);
        }
        
        /// 上传时对单链接限速
        public async void UploadObjectTrafficLimit()
        {
            TransferConfig transferConfig = new TransferConfig();
            // 初始化 TransferManager
            TransferManager transferManager = new TransferManager(cosXml, transferConfig);
            // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
            string bucket = "examplebucket-1250000000";
            string cosPath = "dir/exampleObject"; // 对象键
            string srcPath = @"temp-source-file";//本地文件绝对路径

            PutObjectRequest putObjectRequest = new PutObjectRequest(bucket, cosPath, srcPath);
            putObjectRequest.LimitTraffic(8 * 1000 * 1000); // 限制为1MB/s

            COSXMLUploadTask uploadTask = new COSXMLUploadTask(putObjectRequest);

            uploadTask.SetSrcPath(srcPath);
            await transferManager.UploadAsync(uploadTask);
        }
        
        public static void SpeedLimitModelMain()
        {
            SpeedLimitModel demo = new SpeedLimitModel();
            demo.DownloadObjectTrafficLimit();
            demo.UploadObjectTrafficLimit();
        }
    }
}

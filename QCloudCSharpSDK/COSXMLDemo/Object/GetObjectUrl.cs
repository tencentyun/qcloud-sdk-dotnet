using COSXML;
using COSXML.Auth;
using COSXML.Model.Object;
using COSXML.Model.Tag;

namespace COSXMLDemo
{
    public class GetObjectUrlDemo
    {
        public CosXml cosXml;
        // 初始化COS服务实例
        public string bucket;
        public string appId;
        public string region;

        public void InitParams()
        {
            bucket = Environment.GetEnvironmentVariable("BUCKET");
            appId = Environment.GetEnvironmentVariable("APPID");
            region = Environment.GetEnvironmentVariable("COS_REGION");
        }

        // 构造初始化服务
        GetObjectUrlDemo()
        {
            InitCosXml();
            InitParams();
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

        // 生成预签名 URL => (下载)
        public void GetPreSignDownloadUrl()
        {
            try
            {
                PreSignatureStruct preSignatureStruct = new PreSignatureStruct();
                preSignatureStruct.appid = appId;//"1250000000"; //腾讯云账号 APPID
                preSignatureStruct.region = region;//"COS_REGION"; //存储桶地域
                preSignatureStruct.bucket = bucket;// "examplebucket-1250000000"; //存储桶
                preSignatureStruct.key = "exampleObject"; //对象键
                preSignatureStruct.httpMethod = "GET"; //HTTP 请求方法
                preSignatureStruct.isHttps = true; //生成 HTTPS 请求 URL
                preSignatureStruct.signDurationSecond = 600; //请求签名时间为600s
                preSignatureStruct.headers = null; //签名中需要校验的 header
                preSignatureStruct.queryParameters = null; //签名中需要校验的 URL 中请求参数
                string requestSignURL = cosXml.GenerateSignURL(preSignatureStruct);
                
                Console.WriteLine(requestSignURL);
                
                //下载请求预签名 URL (使用永久密钥方式计算的签名 URL)
                string localDir = System.IO.Path.GetTempPath(); //本地文件夹
                string localFileName = "my-local-temp-file"; //指定本地保存的文件名
                GetObjectRequest request = new GetObjectRequest(null, null, localDir, localFileName);
                //设置下载请求预签名 URL
                request.RequestURLWithSign = requestSignURL;
                //设置进度回调
                request.SetCosProgressCallback(delegate(long completed, long total)
                {
                    Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
                });
                //执行请求
                GetObjectResult result = cosXml.GetObject(request);
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

        // 生成预签名上传链接
        public void GetPreSignUploadUrl()
        {
            try
            {
                PreSignatureStruct preSignatureStruct = new PreSignatureStruct();
                // APPID 获取参考 https://console.cloud.tencent.com/developer
                preSignatureStruct.appid = appId;//"1250000000";
                // 存储桶所在地域, COS 地域的简称请参照 https://cloud.tencent.com/document/product/436/6224
                preSignatureStruct.region = region;//"COS_REGION";
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                preSignatureStruct.bucket = bucket;//"examplebucket-1250000000";
                preSignatureStruct.key = "exampleObject"; //对象键
                preSignatureStruct.httpMethod = "PUT"; //HTTP 请求方法
                preSignatureStruct.isHttps = true; //生成 HTTPS 请求 URL
                preSignatureStruct.signDurationSecond = 600; //请求签名时间为 600s
                preSignatureStruct.headers = null; //签名中需要校验的 header
                preSignatureStruct.queryParameters = null; //签名中需要校验的 URL 中请求参数
                //上传预签名 URL (使用永久密钥方式计算的签名 URL)
                string requestSignURL = cosXml.GenerateSignURL(preSignatureStruct);
                Console.WriteLine(requestSignURL);
                
                string srcPath = @"local-file-path"; //本地文件绝对路径
                PutObjectRequest request = new PutObjectRequest(null, null, srcPath);
                //设置上传请求预签名 URL
                request.RequestURLWithSign = requestSignURL;
                //设置进度回调
                request.SetCosProgressCallback(delegate(long completed, long total)
                {
                    Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
                });
                //执行请求
                PutObjectResult result = cosXml.PutObject(request);
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

        // 获取对象公有读链接
        public void GetObjectUrl()
        {
            try
            {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                string bucket = "examplebucket-1250000000";
                string key = "object"; //对象键
                // 生成链接（默认域名访问）
                String url = cosXml.GetObjectUrl(bucket, key);
                Console.WriteLine("Object Url is: " + url);
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

        //生成预签名 URL，并在签名中携带 Host
        public void GetPreSignUrlWithHost()
        {
            try
            {
                PreSignatureStruct preSignatureStruct = new PreSignatureStruct();
                // APPID 获取参考 https://console.cloud.tencent.com/developer
                preSignatureStruct.appid = "1250000000";
                // 存储桶所在地域, COS 地域的简称请参照 https://cloud.tencent.com/document/product/436/6224
                preSignatureStruct.region = "COS_REGION";
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                preSignatureStruct.bucket = "examplebucket-1250000000";
                preSignatureStruct.key = "exampleobject"; //对象键
                preSignatureStruct.httpMethod = "GET"; //HTTP 请求方法
                preSignatureStruct.isHttps = true; //生成 HTTPS 请求 URL
                preSignatureStruct.signDurationSecond = 600; //请求签名时间为600s
                preSignatureStruct.signHost = true; // 请求中签入Host，建议开启，能够有效防止越权请求，需要注意，开启后实际请求也需要携带Host请求头
                preSignatureStruct.headers = null; //签名中需要校验的 header
                preSignatureStruct.queryParameters = null; //签名中需要校验的 URL 中请求参数

                string requestSignURL = cosXml.GenerateSignURL(preSignatureStruct);
                Console.WriteLine("requestUrl is:" + requestSignURL);
                
                //下载请求预签名 URL (使用永久密钥方式计算的签名 URL)
                string localDir = System.IO.Path.GetTempPath(); //本地文件夹
                string localFileName = "my-local-temp-file"; //指定本地保存的文件名
                GetObjectRequest request = new GetObjectRequest(null, null, localDir, localFileName);
                //设置下载请求预签名 URL
                request.RequestURLWithSign = requestSignURL;
                //设置进度回调
                request.SetCosProgressCallback(delegate(long completed, long total)
                {
                    Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
                });
                //执行请求
                GetObjectResult result = cosXml.GetObject(request);
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

        //生成预签名URL，并在签名中携带请求参数
        public void GetPreSignUrlWithReqParam()
        {
            try
            {
                PreSignatureStruct preSignatureStruct = new PreSignatureStruct();
                // APPID 获取参考 https://console.cloud.tencent.com/developer
                preSignatureStruct.appid = "1250000000";
                // 存储桶所在地域, COS 地域的简称请参照 https://cloud.tencent.com/document/product/436/6224
                preSignatureStruct.region = "COS_REGION";
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                preSignatureStruct.bucket = "examplebucket-1250000000";
                preSignatureStruct.key = "exampleobject"; //对象键
                preSignatureStruct.httpMethod = "GET"; //HTTP 请求方法
                preSignatureStruct.isHttps = true; //生成 HTTPS 请求 URL
                preSignatureStruct.signDurationSecond = 600; //请求签名时间为600s
                preSignatureStruct.signHost = true; // 请求中签入Host，建议开启，能够有效防止越权请求，需要注意，开启后实际请求也需要携带Host请求头
                preSignatureStruct.headers = null; // 签名中需要校验的 header
                string ci_params = "imageMogr2/thumbnail/!50p";
                preSignatureStruct.queryParameters = new Dictionary<string, string>(); // 签名中需要校验的 URL 中请求参数，以请求万象图片处理为例
                preSignatureStruct.queryParameters.Add(ci_params, null);

                string requestSignURL = cosXml.GenerateSignURL(preSignatureStruct);
                Console.WriteLine("requestUrl is:" + requestSignURL);

                //下载请求预签名 URL (使用永久密钥方式计算的签名 URL)
                string localDir = System.IO.Path.GetTempPath(); //本地文件夹
                string localFileName = "my-local-temp-file"; //指定本地保存的文件名
                GetObjectRequest request = new GetObjectRequest(null, null, localDir, localFileName);
                //设置下载请求预签名 URL
                request.RequestURLWithSign = requestSignURL;
                //设置进度回调
                request.SetCosProgressCallback(delegate(long completed, long total)
                {
                    Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
                });
                //执行请求
                GetObjectResult result = cosXml.GetObject(request);
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

        public static void GetObjectUrlDemoMain()
        {
            GetObjectUrlDemo demo = new GetObjectUrlDemo();
            // 获取预签名下载链接
            // demo.GetPreSignDownloadUrl();
            // // 获取预签名上传链接
            // demo.GetPreSignUploadUrl();
            // 获取无签名访问链接
            demo.GetObjectUrl(); //done
            // 生成预签名 URL，并在签名中携带 Host
            // demo.GetPreSignUrlWithHost();
            // // 生成预签名URL，并在签名中携带请求参数
            // demo.GetPreSignUrlWithReqParam();
        }
    }
}
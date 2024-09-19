using COSXML;
using COSXML.Auth;
using COSXML.Common;
using COSXML.Model.Bucket;
using COSXML.Model.Object;
using COSXML.Model.Tag;
namespace COSXMLDemo
{
    public class PutObjectACLModel
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

        PutObjectACLModel()
        {
            InitCosXml();
            InitParams();
        }

        // 设置存储桶 ACL
        public void PutBucketAcl()
        {
            try
            {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                string bucket = "examplebucket-1250000000";
                PutBucketACLRequest request = new PutBucketACLRequest(bucket);
                //设置私有读写权限
                request.SetCosACL(CosACL.Private);
                //授予113197593账号读权限
                COSXML.Model.Tag.GrantAccount readAccount = new COSXML.Model.Tag.GrantAccount();
                readAccount.AddGrantAccount("113197593", "113197593");
                request.SetXCosGrantRead(readAccount);
                //执行请求
                PutBucketACLResult result = cosXml.PutBucketACL(request);
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

        /// 获取存储桶 ACL
        public void GetBucketAcl()
        {
            try
            {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                string bucket = "examplebucket-1250000000";
                GetBucketACLRequest request = new GetBucketACLRequest(bucket);
                //执行请求
                GetBucketACLResult result = cosXml.GetBucketACL(request);
                //存储桶的 ACL 信息
                AccessControlPolicy acl = result.accessControlPolicy;
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

        // 设置对象 ACL
        public void PutObjectAcl()
        {
            // 因为存储桶 ACL 最多1000条，为避免 ACL 达到上限，非必须情况不建议给对象单独设置 ACL(对象默认继承 bucket 权限).
            try
            {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                string bucket = "examplebucket-1250000000";
                string key = "exampleobject"; //对象键
                PutObjectACLRequest request = new PutObjectACLRequest(bucket, key);
                //设置私有读写权限 
                request.SetCosACL(CosACL.Private);
                //授予1131975903账号读权限 
                COSXML.Model.Tag.GrantAccount readAccount = new COSXML.Model.Tag.GrantAccount();
                readAccount.AddGrantAccount("1131975903", "1131975903");
                request.SetXCosGrantRead(readAccount);
                //执行请求
                PutObjectACLResult result = cosXml.PutObjectACL(request);
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

        // 获取对象 ACL
        public void GetObjectAcl()
        {
            try
            {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                string bucket = "examplebucket-1250000000";
                string key = "exampleobject"; //对象键
                GetObjectACLRequest request = new GetObjectACLRequest(bucket, key);
                //执行请求
                GetObjectACLResult result = cosXml.GetObjectACL(request);
                //对象的 ACL 信息
                AccessControlPolicy acl = result.accessControlPolicy;
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


        public static void PutObjectACLMain()
        {

            PutObjectACLModel m = new PutObjectACLModel();
            m.PutBucketAcl();
            m.GetBucketAcl();

            m.PutObjectAcl();
            m.GetObjectAcl();
        }
    }
}

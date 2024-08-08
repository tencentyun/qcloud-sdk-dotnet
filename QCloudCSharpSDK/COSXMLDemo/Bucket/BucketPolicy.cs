using COSXML;
using COSXML.Auth;
using COSXML.Model.Bucket;

namespace COSXMLDemo
{
    public class BucketPolicy
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
                string secretId = Environment.GetEnvironmentVariable("SECRET_ID");   // 云 API 密钥 SecretId, 获取 API 密钥请参照 https://console.cloud.tencent.com/cam/capi
                string secretKey = Environment.GetEnvironmentVariable("SECRET_KEY"); // 云 API 密钥 SecretKey, 获取 API 密钥请参照 https://console.cloud.tencent.com/cam/capi
                long durationSecond = 600; //每次请求签名有效时长，单位为秒
                QCloudCredentialProvider qCloudCredentialProvider = new DefaultQCloudCredentialProvider(secretId, secretKey, durationSecond); 
                this.cosXml = new CosXmlServer(config, qCloudCredentialProvider);
            }
            
            //获取桶的策略
            public void GetBucketPolicy()
            {
                // string bucket = "examplebucket-1250000000";
                GetBucketPolicyRequest  request = new GetBucketPolicyRequest(bucket);
                GetBucketPolicyResult result = cosXml.GetBucketPolicy(request);
                Console.WriteLine(result.Data); //返回数据，json格式
                Console.WriteLine(result.GetResultInfo());
            }
            
            //删除桶的策略
            public void DeleteBucketPolicy()
            {    
                // string bucket = "examplebucket-1250000000";
                DeleteBucketPolicyRequest  request = new DeleteBucketPolicyRequest(bucket);
                DeleteBucketPolicyResult result = cosXml.DeleteBucketPolicy(request);
                Console.WriteLine(result.GetResultInfo());
            }
            
            //设置桶的策略
            public void PutBucketPolicy()
            {
                PutBucketPolicyRequest request = new PutBucketPolicyRequest(bucket);
                // string region = Environment.GetEnvironmentVariable("COS_REGION");
                // string appId = Environment.GetEnvironmentVariable("APPID");
                // string resource = "qcs::cos:" + region + ":uid/" + appId + ":" + bucket + "/*";
                // string policy = "{\"Statement\":[{\"Action\":[\"name/cos:PutBucketPolicy\",\"name/cos:GetBucketPolicy\",\"name/cos:DeleteBucketPolicy\"],\"Effect\":\"Allow\",\"Principal\":{\"qcs\":[\"qcs::cam::uin/2832742109:uin/100032069732\"]},\"Resource\":[\"" + resource + "\"]}],\"Version\":\"2.0\"}";
                string policy = @"{
                     ""Statement"": [
                             {
                                 ""Action"": [
                                     ""name/cos:PutBucketPolicy"",
                                     ""name/cos:GetBucketPolicy"",
                                     ""name/cos:DeleteBucketPolicy""
                                 ],
                                 ""Effect"": ""Allow"",
                                 ""Principal"": {
                                     ""qcs"": [
                                         ""qcs::cam::uin/100000000011:uin/100000000011""
                                     ]
                                 },
                                 ""Resource"": [
                                     ""qcs::cos:ap-guangzhou:uid/1250000000:examplebucket-1250000000/*""
                                 ]
                             }
                         ],
                         ""Version"": ""2.0""
                     }";
                request.SetBucketPolicy(policy);
                PutBucketPolicyResult result = cosXml.PutBucketPolicy(request);
                Console.WriteLine(result.GetResultInfo());
            }
            
            public static void  BucketPolicyMain()
            {
                BucketPolicy demo = new BucketPolicy();
                //demo的自定义参数
                demo.InitParams();
                //初始化COS服务
                demo.InitCosXml();
                
                // demo.PutBucketPolicy();
                // demo.GetBucketPolicy();
                // demo.DeleteBucketPolicy();
            }
        }
}
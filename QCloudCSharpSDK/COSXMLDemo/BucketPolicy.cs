using COSXML;
using COSXML.Auth;
using COSXML.Model.Bucket;

namespace COSXMLDemo
{
    public class BucketPolicy
        {
            private CosXml cosXml;
            
            public void GetBucketPolicy()
            {
                string bucket = "examplebucket-1250000000";
                GetBucketPolicyRequest  request = new GetBucketPolicyRequest(bucket);
                GetBucketPolicyResult result = cosXml.GetBucketPolicy(request);
                Console.WriteLine(result.Data); //返回数据，json格式
                Console.WriteLine(result.GetResultInfo());
            }
            
            public void DeleteBucketPolicy()
            {    
                string bucket = "examplebucket-1250000000";
                DeleteBucketPolicyRequest  request = new DeleteBucketPolicyRequest(bucket);
                DeleteBucketPolicyResult result = cosXml.DeleteBucketPolicy(request);
                Console.WriteLine(result.GetResultInfo());
            }
            
            public void PutBucketPolicy()
            {
                string region = "ap-guangzhou";
                string appId = "1250000000";
                string bucket = "examplebucket-1250000000";
                PutBucketPolicyRequest request = new PutBucketPolicyRequest(bucket);
                string resource = "qcs::cos:" + region + ":uid/" + appId + ":" + bucket + "/*";
                string policy = "{\"Statement\":[{\"Action\":[\"name/cos:PutBucketPolicy\",\"name/cos:GetBucketPolicy\",\"name/cos:DeleteBucketPolicy\"],\"Effect\":\"Allow\",\"Principal\":{\"qcs\":[\"qcs::cam::uin/2832742109:uin/100032069732\"]},\"Resource\":[\"" + resource + "\"]}],\"Version\":\"2.0\"}";
                request.SetBucketPolicy(policy);
                PutBucketPolicyResult result = cosXml.PutBucketPolicy(request);
                Console.WriteLine(result.GetResultInfo());
            }
            
            // 初始化COS服务实例
            private void InitCosXml()
            {
                CosXmlConfig config = new CosXmlConfig.Builder()
                    .SetRegion("ap-guangzhou") // 设置默认的地域, COS 地域的简称请参照 https://cloud.tencent.com/document/product/436/6224
                    .Build(); 
                string secretId = "";   // 云 API 密钥 SecretId, 获取 API 密钥请参照 https://console.cloud.tencent.com/cam/capi
                string secretKey = ""; // 云 API 密钥 SecretKey, 获取 API 密钥请参照 https://console.cloud.tencent.com/cam/capi
                long durationSecond = 600; //每次请求签名有效时长，单位为秒
                QCloudCredentialProvider qCloudCredentialProvider = new DefaultQCloudCredentialProvider(secretId, secretKey, durationSecond); 
                this.cosXml = new CosXmlServer(config, qCloudCredentialProvider);
            }
            
            public static void  BucketPolicyMain()
            {
                // BucketPolicy demo = new BucketPolicy();
                // demo.InitCosXml();
                
                // demo.PutBucketPolicy();
                // demo.GetBucketPolicy();
                // demo.DeleteBucketPolicy();
            }
        }
}
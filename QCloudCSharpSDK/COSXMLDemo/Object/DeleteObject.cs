using COSXML.Model.Object;
using COSXML.Model.Tag;
using COSXML.Model.Bucket;
using COSXML.Auth;
using COSXML;

namespace COSXMLDemo
{
    public class DeleteObjectModel
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

        DeleteObjectModel()
        {
            InitCosXml();
            InitParams();
        }

        // 删除对象
        public void DeleteObject()
        {
            try
            {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                string bucket = "examplebucket-1250000000";
                string key = "exampleobject"; //对象键
                DeleteObjectRequest request = new DeleteObjectRequest(bucket, key);
                //执行请求
                DeleteObjectResult result = cosXml.DeleteObject(request);
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

        // 删除多个对象
        public void DeleteMultiObject()
        {
            try
            {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                string bucket = "examplebucket-1250000000";
                DeleteMultiObjectRequest request = new DeleteMultiObjectRequest(bucket);
                //设置返回结果形式
                request.SetDeleteQuiet(false);
                //对象key
                string key1 = "exampleobject"; //对象键
                string key2 = "exampleobject2"; //对象键
                List<string> objects = new List<string>();
                objects.Add(key1);
                objects.Add(key2);
                request.SetObjectKeys(objects);
                //执行请求
                DeleteMultiObjectResult result = cosXml.DeleteMultiObjects(request);
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

        // 指定前缀批量删除对象
        public void DeletePrefix()
        {
            try
            {
                String nextMarker = null;
                // 循环请求直到没有下一页数据
                do
                {
                    // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                    string bucket = "examplebucket-1250000000";
                    string prefix = "tt/"; //指定前缀
                    GetBucketRequest listRequest = new GetBucketRequest(bucket);
                    //获取 folder1/ 下的所有对象以及子目录
                    listRequest.SetPrefix(prefix);
                    listRequest.SetMarker(nextMarker);
                    //执行列出对象请求
                    GetBucketResult listResult = cosXml.GetBucket(listRequest);
                    ListBucket info = listResult.listBucket;
                    // 对象列表
                    List<ListBucket.Contents> objects = info.contentsList;
                    // 下一页的下标
                    nextMarker = info.nextMarker;

                    DeleteMultiObjectRequest deleteRequest = new DeleteMultiObjectRequest(bucket);
                    //设置返回结果形式
                    deleteRequest.SetDeleteQuiet(false);
                    //对象列表
                    List<string> deleteObjects = new List<string>();
                    foreach (var content in objects)
                    {
                        deleteObjects.Add(content.key);
                    }
                    deleteRequest.SetObjectKeys(deleteObjects);
                    //执行批量删除请求
                    DeleteMultiObjectResult deleteResult = cosXml.DeleteMultiObjects(deleteRequest);
                    //打印请求结果
                    Console.WriteLine(deleteResult.GetResultInfo());
                } while (nextMarker != null);
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

        public static void DeleteObjectModelMain()
        {
            DeleteObjectModel m = new DeleteObjectModel();

            //删除对象
            m.DeleteObject();
            // 删除多个对象
            m.DeleteMultiObject();
            // 指定前缀批量删除对象
            m.DeletePrefix();
        }
    }
}

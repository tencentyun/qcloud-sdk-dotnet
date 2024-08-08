using COSXML;
using COSXML.Auth;
using COSXML.Model.Bucket;
using COSXML.Model.Object;
using COSXML.Model.Tag;
using COSXML.Transfer;

namespace COSXMLDemo
{
    public class CopyObjectModel
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
        
        CopyObjectModel()
        {
            InitCosXml();
            InitParams();
        }
        
        // 高级复制对象
        public async Task TransferCopyObject()
        {
            TransferConfig transferConfig = new TransferConfig();
            //手动设置分块复制阈值，小于阈值的对象使用简单复制，大于阈值的对象使用分块复制，不设定则默认为5MB
            transferConfig.DdivisionForCopy = 5242880;
            //手动设置高级接口的自动分块大小，不设定则默认为2MB
            transferConfig.SliceSizeForCopy = 2097152;
            
            // 初始化 TransferManager
            TransferManager transferManager = new TransferManager(cosXml, transferConfig);

            string sourceAppid = "1250000000"; //账号 appid
            string sourceBucket = "sourcebucket-1250000000"; //"源对象所在的存储桶
            string sourceRegion = "COS_REGION"; //源对象的存储桶所在的地域
            string sourceKey = "sourceObject"; //源对象键
            //构造源对象属性
            CopySourceStruct copySource = new CopySourceStruct(sourceAppid, sourceBucket, sourceRegion, sourceKey);
            
            string bucket = "examplebucket-1250000000"; //目标存储桶，格式：BucketName-APPID
            string key = "exampleobject"; //目标对象的对象键
            COSXMLCopyTask copytask = new COSXMLCopyTask(bucket, key, copySource);

            try {
                COSXML.Transfer.COSXMLCopyTask.CopyTaskResult result = await transferManager.CopyAsync(copytask);
                Console.WriteLine(result.GetResultInfo());
                string eTag = result.eTag;
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

        //简单操作-复制对象-复制对象时保留对象属性
        public void SimpleCopyObject()
        {
            try
            {
                string sourceAppid = "1250000000"; //账号 appid
                string sourceBucket = "sourcebucket-1250000000"; //"源对象所在的存储桶
                string sourceRegion = "COS_REGION"; //源对象的存储桶所在的地域
                string sourceKey = "sourceObject"; //源对象键
                //构造源对象属性
                CopySourceStruct copySource = new CopySourceStruct(sourceAppid, sourceBucket, sourceRegion, sourceKey);

                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                string bucket = "examplebucket-1250000000";
                string key = "exampleobject"; //对象键
                CopyObjectRequest request = new CopyObjectRequest(bucket, key);
                //设置拷贝源
                request.SetCopySource(copySource);
                //设置是否拷贝还是更新,此处是拷贝
                request.SetCopyMetaDataDirective(COSXML.Common.CosMetaDataDirective.Copy);
                //执行请求
                CopyObjectResult result = cosXml.CopyObject(request);
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
        
        //修改对象存储类型
        public void CopyChangeStorageClass()
        {
            try
            {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                string bucket = "examplebucket-1250000000";
                string key = "exampleobject"; //对象键
                string appId = "1250000000"; //账号 appid
                string region = "COS_REGION"; //源对象的存储桶所在的地域
                //构造对象属性
                CopySourceStruct copySource = new CopySourceStruct(appId, bucket, 
                    region, key);

                CopyObjectRequest request = new CopyObjectRequest(bucket, key);
                //设置拷贝源
                request.SetCopySource(copySource);
                //设置是否拷贝还是更新,此处是拷贝
                request.SetCopyMetaDataDirective(COSXML.Common.CosMetaDataDirective.Replaced);
                // 修改为归档存储
                request.SetCosStorageClass("ARCHIVE");
                //执行请求
                CopyObjectResult result = cosXml.CopyObject(request);
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

        //复制对象时替换对象属性
        public void CopyChangeAttr()
        {
            try
            {
                string sourceAppid = "1250000000"; //账号 appid
                string sourceBucket = "sourcebucket-1250000000"; //"源对象所在的存储桶
                string sourceRegion = "COS_REGION"; //源对象的存储桶所在的地域
                string sourceKey = "sourceObject"; //源对象键
                //构造源对象属性
                CopySourceStruct copySource = new CopySourceStruct(sourceAppid, sourceBucket, 
                    sourceRegion, sourceKey);

                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                string bucket = "examplebucket-1250000000";
                string key = "exampleobject"; //对象键
                CopyObjectRequest request = new CopyObjectRequest(bucket, key);
                //设置拷贝源
                request.SetCopySource(copySource);
                //设置是否拷贝还是更新,此处是拷贝
                request.SetCopyMetaDataDirective(COSXML.Common.CosMetaDataDirective.Replaced);
                // 替换元数据
                request.SetRequestHeader("Content-Disposition", "attachment; filename=example.jpg");
                //执行请求
                CopyObjectResult result = cosXml.CopyObject(request);
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
        
        //修改对象元数据
        public void CopyChangeMata()
        {
            try
            {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                string bucket = "examplebucket-1250000000";
                string key = "exampleobject"; //对象键
                string appId = "1250000000"; //账号 appid
                string region = "COS_REGION"; //源对象的存储桶所在的地域
                //构造对象属性
                CopySourceStruct copySource = new CopySourceStruct(appId, bucket, 
                    region, key);

                CopyObjectRequest request = new CopyObjectRequest(bucket, key);
                //设置拷贝源
                request.SetCopySource(copySource);
                //设置是否拷贝还是更新,此处是拷贝
                request.SetCopyMetaDataDirective(COSXML.Common.CosMetaDataDirective.Replaced);
                // 替换元数据
                request.SetRequestHeader("Content-Disposition", "attachment; filename=example.jpg");
                request.SetRequestHeader("Content-Type", "image/png");
                //执行请求
                CopyObjectResult result = cosXml.CopyObject(request);
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

        //移动对象
        public async void MoveObject()
        {
            TransferConfig transferConfig = new TransferConfig();
            // 初始化 TransferManager
            TransferManager transferManager = new TransferManager(cosXml, transferConfig);
            
            string sourceAppid = "1250000000"; //账号 appid
            string sourceBucket = "sourcebucket-1250000000"; //"源对象所在的存储桶
            string sourceRegion = "COS_REGION"; //源对象的存储桶所在的地域
            string sourceKey = "sourceObject"; //源对象键
            //构造源对象属性
            CopySourceStruct copySource = new CopySourceStruct(sourceAppid, sourceBucket, sourceRegion, sourceKey);
            string bucket = "examplebucket-1250000000"; //目标存储桶，格式：BucketName-APPID
            string key = "exampleobject"; //目标对象的对象键

            COSXMLCopyTask copyTask = new COSXMLCopyTask(bucket, key, copySource);
            try {
                // 拷贝对象
                COSXML.Transfer.COSXMLCopyTask.CopyTaskResult result = await transferManager.CopyAsync(copyTask);
                Console.WriteLine(result.GetResultInfo());
                // 删除对象
                DeleteObjectRequest request = new DeleteObjectRequest(sourceBucket, sourceKey);
                DeleteObjectResult deleteResult = cosXml.DeleteObject(request);
                // 打印结果
                Console.WriteLine(deleteResult.GetResultInfo());
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
        
        public class PartCopyModel : CopyObjectModel
        {
            public string uploadId;

            public Dictionary<int, string> eTag;

            public void ListMultipartUploads()
            {
                try
                {
                    // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                    string bucket = "examplebucket-1250000000";
                    ListMultiUploadsRequest request = new ListMultiUploadsRequest(bucket);
                    //执行请求
                    ListMultiUploadsResult result = cosXml.ListMultiUploads(request);
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
            public void InitiateMultipartUpload()
            {
                try
                {
                    // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                    string bucket = "examplebucket-1250000000";
                    string key = "exampleobject"; //对象键
                    InitMultipartUploadRequest request = new InitMultipartUploadRequest(bucket, key);
                    //执行请求
                    InitMultipartUploadResult result = cosXml.InitMultipartUpload(request);
                    //请求成功
                    this.uploadId = result.initMultipartUpload.uploadId; //用于后续分块的 uploadId
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

            public void UploadPartCopy()
            {
                try
                {
                    string sourceAppid = "1250000000"; //账号 appid
                    string sourceBucket = "sourcebucket-1250000000"; //"源对象所在的存储桶
                    string sourceRegion = "COS_REGION"; //源对象的存储桶所在的地域
                    string sourceKey = "sourceObject"; //源对象键
                    //构造源对象属性
                    COSXML.Model.Tag.CopySourceStruct copySource = new CopySourceStruct(sourceAppid, 
                        sourceBucket, sourceRegion, sourceKey);

                    // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                    string bucket = "examplebucket-1250000000";
                    string key = "exampleobject"; //对象键
                    string uploadId = this.uploadId; //初始化分块上传/复制返回的uploadId
                    int partNumber = 1; //分块编号，必须从1开始递增
                    UploadPartCopyRequest request = new UploadPartCopyRequest(bucket, key, partNumber, uploadId);
                    //设置拷贝源
                    request.SetCopySource(copySource);
                    //设置复制分块（指定块的范围，如 0 ~ 1M）
                    request.SetCopyRange(0, 1024 * 1024);
                    //执行请求
                    UploadPartCopyResult result = cosXml.PartCopy(request);
                    //请求成功,获取返回分块的eTag,用于后续CompleteMultiUploads
                    this.eTag[partNumber] = result.copyPart.eTag;
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

            public void ListParts()
            {
                try
                {
                    // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                    string bucket = "examplebucket-1250000000";
                    string key = "exampleobject"; //对象键
                    string uploadId = "exampleUploadId"; //初始化分块上传/复制返回的uploadId
                    ListPartsRequest request = new ListPartsRequest(bucket, key, uploadId);
                    //执行请求
                    ListPartsResult result = cosXml.ListParts(request);
                    //请求成功,列举已上传/复制的分块
                    List<COSXML.Model.Tag.ListParts.Part> alreadyUploadParts = result.listParts.parts;
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
            
            public void CompleteMultipartUpload()
            {
                try
                {
                    // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                    string bucket = "examplebucket-1250000000";
                    string key = "exampleobject"; //对象键
                    string uploadId = "exampleUploadId"; //初始化分块/复制返回的uploadId
                    CompleteMultipartUploadRequest request = new CompleteMultipartUploadRequest(bucket, key, uploadId);
                    //设置已/复制的parts,必须有序，按照partNumber递增
                    foreach (int idx in eTag.Keys)
                    { 
                        request.SetPartNumberAndETag(idx, this.eTag[idx]);
                    }
                    //执行请求
                    CompleteMultipartUploadResult result = cosXml.CompleteMultiUpload(request);
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

            public void AbortMultipartUpload()
            {
                try
                {
                    // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                    string bucket = "examplebucket-1250000000";
                    string key = "exampleobject"; //对象键
                    string uploadId = "exampleUploadId"; //初始化分返回的uploadId
                    AbortMultipartUploadRequest request = new AbortMultipartUploadRequest(bucket, key, uploadId);
                    //执行请求
                    AbortMultipartUploadResult result = cosXml.AbortMultiUpload(request);
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
        }


        public void CopyObjectModelMain()
        {
            
        }
    }
}

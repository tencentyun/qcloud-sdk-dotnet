using COSXML;
using COSXML.Auth;
using COSXML.Model.Bucket;
using COSXML.Model.Object;
using COSXML.Model.Tag;
using COSXML.Transfer;

namespace COSXMLDemo
{
    public class DownloadObject
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
            string secretId = Environment.GetEnvironmentVariable("SECRET_ID");   // 云 API 密钥 SecretId, 获取 API 密钥请参照 https://console.cloud.tencent.com/cam/capi
            string secretKey = Environment.GetEnvironmentVariable("SECRET_KEY"); // 云 API 密钥 SecretKey, 获取 API 密钥请参照 https://console.cloud.tencent.com/cam/capi
            long durationSecond = 600; //每次请求签名有效时长，单位为秒
            QCloudCredentialProvider qCloudCredentialProvider = new DefaultQCloudCredentialProvider(secretId, secretKey, durationSecond); 
            this.cosXml = new CosXmlServer(config, qCloudCredentialProvider);
        }
        
        public async Task TransferDownloadObject()
        {
            // 初始化 TransferConfig
            TransferConfig transferConfig = new TransferConfig();
            // 手动设置高级下载接口的分块阈值为 20MB(默认为20MB), 从5.4.26版本开始支持！
            //transferConfig.DivisionForDownload = 20 * 1024 * 1024;
            // 手动设置高级下载接口的分块大小为 10MB(默认为5MB),设置过小的分块值可能导致频繁重试或下载速度不合预期
            //transferConfig.SliceSizeForDownload = 10 * 1024 * 1024;
            
            // 初始化 TransferManager
            TransferManager transferManager = new TransferManager(cosXml, transferConfig);

            // String bucket = "examplebucket-1250000000"; //存储桶，格式：BucketName-APPID
            String cosPath = "exampleobject"; //对象在存储桶中的位置标识符，即称对象键
            string localDir = Path.GetTempPath();//本地文件夹
            string localFileName = "my-local-temp-file"; //指定本地保存的文件名
            // 下载对象
            COSXMLDownloadTask downloadTask = new COSXMLDownloadTask(bucket, cosPath,
                localDir, localFileName);

            // 手动设置高级下载接口的并发数 (默认为5), 从5.4.26版本开始支持！
            // downloadTask.SetMaxTasks(10);
            downloadTask.progressCallback = delegate (long completed, long total)
            {
                Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
            };
            
            try {
                COSXMLDownloadTask.DownloadTaskResult result = await transferManager.DownloadAsync(downloadTask);
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
        
        public void DownloadToMemory() 
        {
            try
            {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                // string bucket = "examplebucket-1250000000";
                string key = "exampleobject"; //对象键
                
                GetObjectBytesRequest request = new GetObjectBytesRequest(bucket, key);
                //设置进度回调
                request.SetCosProgressCallback(delegate (long completed, long total)
                {
                    Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
                });
                //执行请求
                GetObjectBytesResult result = cosXml.GetObject(request);
                //获取内容到 byte 数组中
                byte[] content = result.content;
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

        public void BatchDownload()
        {
            TransferConfig transferConfig = new TransferConfig(); 
            // 初始化 TransferManager
            TransferManager transferManager = new TransferManager(cosXml, transferConfig); 
            // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
            string bucket = "examplebucket-1250000000";
            string localDir = System.IO.Path.GetTempPath();//本地文件夹
            
            for (int i = 0; i < 5; i++) {
                // 下载对象
                string cosPath = "exampleobject" + i; //对象在存储桶中的位置标识符，即称对象键
                string localFileName = "my-local-temp-file"; //指定本地保存的文件名
                COSXMLDownloadTask downloadTask = new COSXMLDownloadTask(bucket, cosPath, 
                    localDir, localFileName); 
                transferManager.DownloadAsync(downloadTask).Wait();
            }
        }
        
        public void GetObjectsFromFolder()
        {
            // 注意：COS中实际不存在文件夹下载的接口
            // 需要通过组合 “指定前缀列出” 和 “遍历列出的对象key做下载” 两种操作，实现类似文件夹下载的操作
            // 下面的操作，把对象列出到列表里，然后异步下载列表中的对象
            String nextMarker = null;
            List<string> downloadList = new List<string>();
            string bucket = "examplebucket-1250000000";
            string prefix = "folder1/"; //指定前缀
            // 循环请求直到没有下一页数据
            do {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
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
                //对象列表
                foreach (var content in objects)
                {
                    downloadList.Add(content.key);
                    Console.WriteLine("adding key:" + content.key);
                }
            } while (nextMarker != null);
            Console.WriteLine("download list construct done, " + downloadList.Count + " objects added");
            TransferConfig transferConfig = new TransferConfig();
            TransferManager transferManager = new TransferManager(cosXml, transferConfig);
            string localDir = System.IO.Path.GetTempPath(); //本地文件夹

            List<Task> taskList = new List<Task>();
            for (int i = 0; i < downloadList.Count; i++)
            {
                // 遍历待下载列表，下载内容写到 filename_i 文件中
                COSXMLDownloadTask downloadTask = new COSXMLDownloadTask(bucket, downloadList[i],
                    localDir, "filename_" + i.ToString());
                // 异步下载, 加入task队列
                Task task = transferManager.DownloadAsync(downloadTask);
                taskList.Add(task);
            }
            Console.WriteLine("download tasks submitted, total " + taskList.Count + " tasks added");
            //等待TaskList中所有Task结束

            foreach (Task task in taskList)
            {
                task.Wait();
                Console.WriteLine("download completed");
            }
        }
        
        public void GetObject()
        {
            try
            {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                string bucket = "examplebucket-1250000000";
                string key = "exampleobject"; //对象键
                string localDir = Path.GetTempPath();//本地文件夹
                string localFileName = "my-local-temp-file"; //指定本地保存的文件名
                GetObjectRequest request = new GetObjectRequest(bucket, key, localDir, localFileName);
                request.SetCosProgressCallback(delegate (long completed, long total)
                {
                    Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
                });
                //执行请求
                GetObjectResult result = cosXml.GetObject(request);
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

        
        //文件分块上传
        public class FileChunkUpload : DownloadObject
        {
            private string uploadId;

            private Dictionary<int, string> eTag;
            
            //查询指定存储桶中正在进行的分块上传
            public void ListMultipartUploads()
            {
                try
                {
                    // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                    ListMultiUploadsRequest request = new ListMultiUploadsRequest(bucket);
                    //执行请求
                    ListMultiUploadsResult result = cosXml.ListMultiUploads(request);
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
                    string bucket = "examplebucket-1250000000";
                    // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                    string key = "exampleobject"; //对象键
                    InitMultipartUploadRequest request = new InitMultipartUploadRequest(bucket, key);
                    //执行请求
                    InitMultipartUploadResult result = cosXml.InitMultipartUpload(request);
                    uploadId = result.initMultipartUpload.uploadId; //用于后续分块上传的 uploadId
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
            
            public void UploadPart()
            {
                try
                {
                    // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                    string bucket = "examplebucket-1250000000";
                    string key = "exampleobject"; //对象键
                    string uploadId = "exampleUploadId"; //初始化分块上传返回的uploadId
                    int partNumber = 1; //分块编号，必须从1开始递增
                    string srcPath = @"temp-source-file";//本地文件绝对路径
                    UploadPartRequest request = new UploadPartRequest(bucket, key, partNumber, uploadId, srcPath, 0, -1);
                    //设置进度回调
                    request.SetCosProgressCallback(delegate (long completed, long total)
                    {
                        Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
                    });
                    //执行请求
                    UploadPartResult result = cosXml.UploadPart(request);
                    //获取返回分块的eTag,用于后续CompleteMultiUploads
                    Console.WriteLine(result.eTag);
                    eTag[partNumber] = result.eTag;
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
                    string uploadId = "exampleUploadId"; //初始化分块上传返回的uploadId
                    ListPartsRequest request = new ListPartsRequest(bucket, key, uploadId);
                    //执行请求
                    ListPartsResult result = cosXml.ListParts(request);
                    //列举已上传的分块
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
                    string uploadId = "exampleUploadId"; //初始化分块上传返回的uploadId
                    CompleteMultipartUploadRequest request = new CompleteMultipartUploadRequest(bucket, 
                        key, uploadId);
                    //设置已上传的parts,必须有序，按照partNumber递增
                    request.SetPartNumberAndETag(1, this.eTag[1]);
                    //执行请求
                    CompleteMultipartUploadResult result = cosXml.CompleteMultiUpload(request);
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
                    string uploadId = "exampleUploadId"; //初始化分块上传返回的uploadId
                    AbortMultipartUploadRequest request = new AbortMultipartUploadRequest(bucket, key, uploadId);
                    //执行请求
                    AbortMultipartUploadResult result = cosXml.AbortMultiUpload(request);
                    //打印结果
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

        public void FileChunkUploadFunc()
        {
            FileChunkUpload fileChunkUploadDemo = new FileChunkUpload();
            
            fileChunkUploadDemo.ListMultipartUploads();
            
            fileChunkUploadDemo.InitiateMultipartUpload();
            
            fileChunkUploadDemo.UploadPart();
            
            fileChunkUploadDemo.ListParts();
            
            fileChunkUploadDemo.ListParts();

            fileChunkUploadDemo.CompleteMultipartUpload();
            
            fileChunkUploadDemo.AbortMultipartUpload();
        }
        
        
        public static void DownloadObjectMain()
        {
            DownloadObject demo = new DownloadObject();
            //demo的自定义参数
            demo.InitParams();
            //初始化COS服务
            demo.InitCosXml();
            
            demo.TransferDownloadObject().Wait();
            demo.DownloadToMemory();
            demo.GetObject();
            demo.GetObjectsFromFolder();
            demo.BatchDownload();
        }
        
    }
}

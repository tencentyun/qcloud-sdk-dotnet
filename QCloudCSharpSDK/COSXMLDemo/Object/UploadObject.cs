using COSXML.Auth;
using COSXML.Transfer;
using COSXML;
using COSXML.Model.Bucket;
using COSXML.Model.Object;

namespace COSXMLDemo
{
    public class UploadObject {
        
      private CosXml cosXml;

      // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
      private string bucket;
      
      public void InitParams()
      {
          bucket = Environment.GetEnvironmentVariable("BUCKET");
      }
      
      // 初始化COS服务实例
      private void InitCosXml()
      {
          //从Environment.GetEnvironmentVariable中设置变量，用户也可也直接赋值变量，如： string region = "ap-guagnzhou";
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
      
      UploadObject()
      {
          //demo的自定义参数
          InitParams();
          //初始化COS服务
          InitCosXml();
      }
      
      // 高级接口上传文件
      public async Task TransferUploadFile()
      {
          TransferConfig transferConfig = new TransferConfig();
          // 手动设置开始分块上传的大小阈值为10MB，默认值为5MB
          transferConfig.DivisionForUpload = 10 * 1024 * 1024;
          // 手动设置分块上传中每个分块的大小为2MB，默认值为1MB
          transferConfig.SliceSizeForUpload = 2 * 1024 * 1024;      
          // 初始化 TransferManager
          TransferManager transferManager = new TransferManager(cosXml, transferConfig);
          
          // 存储桶名称，此处填入格式必须为 BucketName-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
          String bucket = "examplebucket-1250000000"; 
          String cosPath = "exampleobject"; //对象在存储桶中的位置标识符，即称对象键
          String srcPath = "temp-source-file";//本地文件绝对路径  
          
          // 上传对象
          COSXMLUploadTask uploadTask = new COSXMLUploadTask(bucket, cosPath);
          uploadTask.SetSrcPath(srcPath);   
          uploadTask.progressCallback = delegate (long completed, long total)
          {
              Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
          };
          
          //开始上传
          try {
              COSXMLUploadTask.UploadTaskResult result = await transferManager.UploadAsync(uploadTask);
              Console.WriteLine(result.GetResultInfo()); 
          } catch (Exception e) {
              Console.WriteLine("CosException: " + e);
          }
      }
      
      public void UploadBytes()
      {
          try {
              // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
              string bucket = "examplebucket-1250000000";
              string cosPath = "exampleObject"; // 对象键
              byte[] data = new byte[1024]; // 二进制数据
              PutObjectRequest putObjectRequest = new PutObjectRequest(bucket, cosPath, data);
              // 发起上传
              PutObjectResult result = cosXml.PutObject(putObjectRequest);
              Console.WriteLine(result.GetResultInfo());
          } catch (COSXML.CosException.CosClientException clientEx) {
              //请求失败
              Console.WriteLine("CosClientException: " + clientEx);
          } catch (COSXML.CosException.CosServerException serverEx) {
              //请求失败
              Console.WriteLine("CosServerException: " + serverEx.GetInfo());
          }
      }
      
      // 文件流上传, 从 5.4.24 版本开始支持
      public void PutObjectStream()
      {
          try {
              // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
              string bucket = "examplebucket-1250000000";
              string key = "exampleobject"; //对象键
              string srcPath = @"temp-source-file";//本地文件绝对路径
              // 打开只读的文件流对象
              FileStream fileStream = new FileStream(srcPath, FileMode.Open, FileAccess.Read);
              // 组装上传请求，其中 offset sendLength 为可选参数
              long offset = 0L;
              long sendLength = fileStream.Length;
              
              PutObjectRequest request = new PutObjectRequest(bucket, key, fileStream, offset, sendLength);
              //设置进度回调
              request.SetCosProgressCallback(delegate (long completed, long total) {
                  Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
              });
              
              //执行请求
              PutObjectResult result = cosXml.PutObject(request);
              //关闭文件流
              fileStream.Close();
              
              //打印请求结果
              Console.WriteLine(result.GetResultInfo()); 
          } catch (COSXML.CosException.CosClientException clientEx) {
              Console.WriteLine("CosClientException: " + clientEx);
          } catch (COSXML.CosException.CosServerException serverEx) {
              Console.WriteLine("CosServerException: " + serverEx.GetInfo());
          }
      }

      public void PutObject()
      {
          try {
              // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
              string bucket = "examplebucket-1250000000";
              string key = "exampleobject"; //对象键
              string srcPath = @"temp-source-file";//本地文件绝对路径

              PutObjectRequest request = new PutObjectRequest(bucket, key, srcPath);
              //设置进度回调
              request.SetCosProgressCallback(delegate (long completed, long total) {
                  Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
              });
              PutObjectResult result = cosXml.PutObject(request);
              //打印返回结果
              Console.WriteLine(result.GetResultInfo());
          } catch (COSXML.CosException.CosClientException clientEx) {
              Console.WriteLine("CosClientException: " + clientEx);
          } catch (COSXML.CosException.CosServerException serverEx) {
              Console.WriteLine("CosServerException: " + serverEx.GetInfo());
          }
      }

      public void CreateDir()
      {
          try
          {
              // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
              string bucket = "examplebucket-1250000000";
              string cosPath = "dir/"; // 对象键
              PutObjectRequest putObjectRequest = new PutObjectRequest(bucket, cosPath, new byte[0]);
              PutObjectResult result = cosXml.PutObject(putObjectRequest);
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
      
      public void PostObject()
      {
          try {
              // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
              string bucket = "examplebucket-1250000000";
              string key = "exampleobject"; //对象键
              string srcPath = @"temp-source-file";//本地文件绝对路径
              PostObjectRequest request = new PostObjectRequest(bucket, key, srcPath);
              //设置进度回调
              request.SetCosProgressCallback(delegate (long completed, long total)
              {
                  Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
              });
              //执行请求
              PostObjectResult result = cosXml.PostObject(request);
              //请求成功
              Console.WriteLine(result.GetResultInfo());
          } catch (COSXML.CosException.CosClientException clientEx) {
              Console.WriteLine("CosClientException: " + clientEx);
          } catch (COSXML.CosException.CosServerException serverEx) {
              Console.WriteLine("CosServerException: " + serverEx.GetInfo());
          }
      }
      
      public void BatchUpload()
      {
          TransferConfig transferConfig = new TransferConfig(); 
          // 初始化 TransferManager
          TransferManager transferManager = new TransferManager(cosXml, transferConfig);
          // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
          string bucket = "examplebucket-1250000000";
          
          for (int i = 0; i < 5; i++) {
              // 上传对象
              string cosPath = "exampleobject" + i; //对象在存储桶中的位置标识符，即称对象键
              string srcPath = @"temp-source-file";//本地文件绝对路径
              COSXMLUploadTask uploadTask = new COSXMLUploadTask(bucket, cosPath); 
              uploadTask.SetSrcPath(srcPath);
              transferManager.UploadAsync(uploadTask).Wait();
          }
      }
      
      //分块上传
      public class UploadPartObject : UploadObject
      {
          public string uploadId;

          public Dictionary<int, string> eTag;
          
          public string key;
          
          //初始化分块上传
          public void InitiateMultipartUpload()
          {
              try
              {
                  string bucket = "examplebucket-1250000000";
                  string key = "exampleobject"; //对象键
                  InitMultipartUploadRequest request = new InitMultipartUploadRequest(bucket, key);
                  //执行请求
                  InitMultipartUploadResult result = cosXml.InitMultipartUpload(request);
                  //请求成功
                  this.uploadId = result.initMultipartUpload.uploadId; //用于后续分块上传的 uploadId
                  Console.WriteLine(result.GetResultInfo());
              }
              catch (COSXML.CosException.CosClientException clientEx)
              {
                  //请求失败
                  Console.WriteLine("CosClientException: " + clientEx);
              }
              catch (COSXML.CosException.CosServerException serverEx)
              {
                  //请求失败
                  Console.WriteLine("CosServerException: " + serverEx.GetInfo());
              }
          }

          //上传分块，需要对于文件按照分块大小进行分块并做序号
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
                  UploadPartRequest request = new UploadPartRequest(bucket, key, partNumber, 
                      uploadId, srcPath, 0, -1);
                  //设置进度回调
                  request.SetCosProgressCallback(delegate (long completed, long total)
                  {
                      Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
                  });
                  //执行请求
                  UploadPartResult result = cosXml.UploadPart(request);
                  //请求成功
                  //获取返回分块的eTag,用于后续CompleteMultiUploads
                  eTag[partNumber] = result.eTag;
                  Console.WriteLine(result.GetResultInfo());
              }
              catch (COSXML.CosException.CosClientException clientEx)
              {
                  //请求失败
                  Console.WriteLine("CosClientException: " + clientEx);
              }
              catch (COSXML.CosException.CosServerException serverEx)
              {
                  //请求失败
                  Console.WriteLine("CosServerException: " + serverEx.GetInfo());
              }
          }
          
          //查询正在进行的分块上传任务
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
                  //请求失败
                  Console.WriteLine("CosClientException: " + clientEx);
              }
              catch (COSXML.CosException.CosServerException serverEx)
              {
                  //请求失败
                  Console.WriteLine("CosServerException: " + serverEx.GetInfo());
              }
          }

          //查询已上传的分块
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
                  //请求成功
                  //列举已上传的分块
                  List<COSXML.Model.Tag.ListParts.Part> alreadyUploadParts = result.listParts.parts;
                  Console.WriteLine(result.GetResultInfo());
              }
              catch (COSXML.CosException.CosClientException clientEx)
              {
                  //请求失败
                  Console.WriteLine("CosClientException: " + clientEx);
              }
              catch (COSXML.CosException.CosServerException serverEx)
              {
                  //请求失败
                  Console.WriteLine("CosServerException: " + serverEx.GetInfo());
              }
          }
          
          //完成分块上传
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
                  foreach (int index in eTag.Keys)
                  {
                      request.SetPartNumberAndETag(index, eTag[index]);
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
          
          //终止分块上传
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

      public void UploadPartObjectFunc()
      {
          UploadPartObject demo = new UploadPartObject();
          
          demo.InitiateMultipartUpload();
          demo.UploadPart();
          demo.ListMultipartUploads();
          demo.ListParts();
          demo.CompleteMultipartUpload();
          demo.AbortMultipartUpload();
      }
      
      public static void UploadObjectMain()
      {
          UploadObject domo = new UploadObject();
          
          //表单上传
          domo.PostObject();
          //批量上传
          domo.BatchUpload();
          //创建文件夹
          domo.CreateDir();
          //上传文件
          domo.PutObject();
          //高级上传
          domo.TransferUploadFile().Wait();
          //流上传
          domo.PutObjectStream();
          //字节流上传
          domo.UploadBytes();
      }
    }
}
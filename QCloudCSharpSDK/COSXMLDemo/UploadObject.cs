using COSXML.Auth;
using COSXML.Transfer;
using COSXML;
using COSXML.Model.Object;

namespace COSXMLDemo
{
    public class UploadObject {
        
      private CosXml cosXml;
      
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
      
      public static void UploadObjectMain()
      {
          UploadObject domo = new UploadObject();
          //初始化COS服务
          domo.InitCosXml();
          // //表单上传
          // domo.PostObject();
          // //批量上传
          // domo.BatchUpload();
          // //创建文件夹
          // domo.CreateDir();
          // //上传文件
          // domo.PutObject();
          // //高级上传
          // domo.TransferUploadFile().Wait();
          // //流上传
          // domo.PutObjectStream();
          //字节流上传
          domo.UploadBytes();
      }
    }
}
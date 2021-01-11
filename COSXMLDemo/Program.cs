using COSXML;
using COSXML.Auth;
using COSXML.Common;
using COSXML.Utils;
using COSXML.Model;
using COSXML.Model.Object;
using COSXML.Model.Bucket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using COSXML.CosException;
using System.Threading;
using System.Threading.Tasks;
using COSXML.Transfer;

namespace COSXMLDemo
{
    public class Program
    {
        
        internal static string bucket = @"bucket-4-dotnet-demo-1253653367";

        public static async Task Main(string[] args)
        {

            // 腾讯云 SecretId
            string secretId = Environment.GetEnvironmentVariable("COS_KEY");
            // 腾讯云 SecretKey
            string secretKey = Environment.GetEnvironmentVariable("COS_SECRET"); 
            // 存储桶所在地域
            string region = "ap-guangzhou"; 

            // 普通初始化方式
            CosXmlConfig config = new CosXmlConfig.Builder()
                .SetRegion(region)
                .SetDebugLog(true)
                .Build();
            
            // TCE 初始化方式
            // string domain = "your.domain";  // 替换成您的 Domain
            // string endpoint = String.Format("cos.{0}.{1}", region, domain);
            // CosXmlConfig config = new CosXmlConfig.Builder()
            //     .setEndpointSuffix(endpoint)
            //     .SetRegion(region)
            //     .SetDebugLog(true)
            //     .Build();

            long keyDurationSecond = 600;
            QCloudCredentialProvider qCloudCredentialProvider = new DefaultQCloudCredentialProvider(secretId, secretKey, keyDurationSecond);

            // service 初始化完成
            CosXmlServer cosXml = new CosXmlServer(config, qCloudCredentialProvider);

            try 
            {
                // 创建存储痛
                Console.WriteLine(" ======= Put Bucket ======");
                PutBucket(cosXml);

                // 上传对象
                Console.WriteLine(" ======= Put Object ======");
                string cosKey = await PutObject(cosXml);

                // 下载对象
                Console.WriteLine(" ======= Get Object ======");
                await GetObject(cosXml, cosKey);

                // 删除对象
                Console.WriteLine(" ======= Delete Object ======");
                DeleteObject(cosXml, cosKey);

                // Console.WriteLine(" ======= Put Directory ======");
                // UploadDirectory(cosXml);
            } 
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.Message);
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
            }
            
            finally 
            {
                // 删除存储桶
                Console.WriteLine(" ======= Delete Bucket ======");
                DeleteBucket(cosXml);
            }

            Console.WriteLine(" ======= Program End. ======");
        }

        internal static void PutBucket(CosXmlServer cosXml) 
        {
            try
            {
                PutBucketRequest request = new PutBucketRequest(bucket);

                //执行请求
                PutBucketResult result = cosXml.PutBucket(request);

                Console.WriteLine(result.GetResultInfo());
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                if (serverEx.statusCode != 409)
                {
                    throw serverEx;
                } 
                else 
                {
                    Console.WriteLine("Bucket Already exists.");
                }
            }
        }

        internal static void DeleteBucket(CosXmlServer cosXml) 
        {
            DeleteBucketRequest request = new DeleteBucketRequest(bucket);

            DeleteBucketResult result = cosXml.DeleteBucket(request);

            Console.WriteLine(result.GetResultInfo());
        }

        internal static async Task<String> PutObject(CosXmlServer cosXml) 
        {
            string cosKey = "cosKey";
            //.cssg-snippet-body-start:[transfer-upload-file]
            // 初始化 TransferConfig
            TransferConfig transferConfig = new TransferConfig();
            
            // 初始化 TransferManager
            TransferManager transferManager = new TransferManager(cosXml, transferConfig);
            
            //对象在存储桶中的位置标识符，即称对象键
            String cosPath = cosKey; 
            //本地文件绝对路径
            String srcPath = @"本地绝对路径";
            
            // 上传对象
            COSXMLUploadTask uploadTask = new COSXMLUploadTask(bucket, cosPath);
            uploadTask.SetSrcPath(srcPath);
            
            uploadTask.progressCallback = delegate (long completed, long total)
            {
                Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
            };

            try 
            {
                COSXML.Transfer.COSXMLUploadTask.UploadTaskResult result = await 
                    transferManager.UploadAsync(uploadTask);
                Console.WriteLine(result.GetResultInfo());
                string eTag = result.eTag;
            } 
            catch (Exception e) 
            {
                Console.WriteLine("CosException: " + e);
            }

            return cosKey;
        }

        internal static void UploadDirectory(CosXmlServer cosXml)
        {
            //.cssg-snippet-body-start:[transfer-upload-file]
            // 初始化 TransferConfig
            TransferConfig transferConfig = new TransferConfig();
            
            // 初始化 TransferManager
            TransferManager transferManager = new TransferManager(cosXml, transferConfig);

            //本地文件夹绝对路径
            String dir = @"本地文件夹绝对路径";

            var files = System.IO.Directory.GetFiles(dir);

            var tasks = new List<Task>();
            foreach (var file in files)
            {
                Console.WriteLine("Enqueue Upload: " + file);
                //对象在存储桶中的位置标识符，即称对象键
                String cosPath = new FileInfo(file).Name;
                
                // 上传对象
                COSXMLUploadTask uploadTask = new COSXMLUploadTask(bucket, cosPath);
                uploadTask.SetSrcPath(file);
                
                tasks.Add(transferManager.UploadAsync(uploadTask));
            }

            try
            {
                // Wait for all the tasks to finish.
                Task.WaitAll(tasks.ToArray());

                // We should never get to this point
                Console.WriteLine("Upload Directory Complete");
            }
            catch (AggregateException e)
            {
                Console.WriteLine("\nThe following exceptions have been thrown by WaitAll(): (THIS WAS EXPECTED)");
                for (int j = 0; j < e.InnerExceptions.Count; j++)
                {
                    Console.WriteLine("\n-------------------------------------------------\n{0}", e.InnerExceptions[j].ToString());
                }
            }
            
        }

        internal static async Task GetObject(CosXmlServer cosXml, string cosKey) 
        {
            TransferConfig transferConfig = new TransferConfig();
        
            // 初始化 TransferManager
            TransferManager transferManager = new TransferManager(cosXml, transferConfig);
            
            //对象在存储桶中的位置标识符，即称对象键
            String cosPath = cosKey; 
            //本地文件夹
            string localDir = System.IO.Path.GetTempPath();
            //指定本地保存的文件名
            string localFileName = "my-local-temp-file"; 
            
            // 下载对象
            COSXMLDownloadTask downloadTask = new COSXMLDownloadTask(bucket, cosPath, 
            localDir, localFileName);
            
            downloadTask.progressCallback = delegate (long completed, long total)
            {
                Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
            };

            try 
            {
                COSXML.Transfer.COSXMLDownloadTask.DownloadTaskResult result = await 
                    transferManager.DownloadAsync(downloadTask);
                Console.WriteLine(result.GetResultInfo());
                string eTag = result.eTag;
            } 
            catch (Exception e) 
            {
                Console.WriteLine("CosException: " + e);
            }
        }

        internal static void DeleteObject(CosXmlServer cosXml, string cosKey) 
        {
            DeleteObjectRequest request = new DeleteObjectRequest(bucket, cosKey);

            DeleteObjectResult result = cosXml.DeleteObject(request);

            Console.WriteLine(result.GetResultInfo());
        }
    }
}

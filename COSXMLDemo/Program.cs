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

namespace COSXMLDemo
{
    class Program
    {
        
        static string bucket = @"bucket-4-csharp-demo-1253653367";

        static void Main(string[] args)
        {

            string secretId = Environment.GetEnvironmentVariable("COS_KEY"); // 腾讯云 SecretId
            string secretKey = Environment.GetEnvironmentVariable("COS_SECRET"); // 腾讯云 SecretKey
            string region = "ap-guangzhou"; // 存储桶所在地域

            // 普通初始化方式
            CosXmlConfig config = new CosXmlConfig.Builder()
                .SetRegion(region)
                .SetDebugLog(true)
                .Build();
            
            // TCE 初始化方式
            // string domain = "your.domain";  // 替换成您的 Domain
            // string endpoint = String.format("cos.%s.%s", region, domain);
            // CosXmlConfig config = new CosXmlConfig.Builder()
            //     .setEndpointSuffix(endpoint)
            //     .SetDebugLog(true)
            //     .Build();

            long keyDurationSecond = 600;
            QCloudCredentialProvider qCloudCredentialProvider = new DefaultQCloudCredentialProvider(secretId, secretKey, keyDurationSecond);

            // service 初始化完成
            CosXmlServer cosXml = new CosXmlServer(config, qCloudCredentialProvider);

            try {
                // 创建存储痛
                Console.WriteLine(" ======= Put Bucket ======");
                putBucket(cosXml);

                // 上传对象
                Console.WriteLine(" ======= Put Object ======");
                string cosKey = putObject(cosXml);

                // 删除对象
                Console.WriteLine(" ======= Delete Object ======");
                deleteObject(cosXml, cosKey);
            } 
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.Message);
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
            } finally {
                // 删除存储桶
                Console.WriteLine(" ======= Delete Bucket ======");
                deleteBucket(cosXml);
            }
            Console.WriteLine(" ======= Program End. ======");
        }

        static void putBucket(CosXmlServer cosXml) {
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
                } else {
                    Console.WriteLine("Bucket Already exists.");
                }
            }
        }

        static void deleteBucket(CosXmlServer cosXml) {
            DeleteBucketRequest request = new DeleteBucketRequest(bucket);

            DeleteBucketResult result = cosXml.DeleteBucket(request);

            Console.WriteLine(result.GetResultInfo());
        }

        static string putObject(CosXmlServer cosXml) {
            string cosKey = "cosKey";
            byte[] tmpData = new byte[1024];

            PutObjectRequest request = new PutObjectRequest(bucket, cosKey, tmpData);

            PutObjectResult result = cosXml.PutObject(request);

            Console.WriteLine(result.GetResultInfo());

            return cosKey;
        }

        static void deleteObject(CosXmlServer cosXml, string cosKey) {
            DeleteObjectRequest request = new DeleteObjectRequest(bucket, cosKey);

            DeleteObjectResult result = cosXml.DeleteObject(request);

            Console.WriteLine(result.GetResultInfo());
        }
    }
}

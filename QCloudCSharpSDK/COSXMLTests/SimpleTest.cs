using COSXML.Common;
using COSXML;
using COSXML.Auth;
using COSXML.Common;
using COSXML.Utils;
using COSXML.CosException;
using COSXML.Model;
using COSXML.Model.Object;
using COSXML.Model.Tag;
using COSXML.Utils;
using COSXML.Transfer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace COSXMLTests
{
    [TestFixture()]
    public class SimpleTest
    {

        COSXML.CosXml cosXml;
        TransferManager transferManager;
        string bucket;
        string bigFileSrcPath;
        string smallFileSrcPath;
        string localDir;
        string localFileName;

        public void init()
        {
            string region = "ap-guangzhou";

            CosXmlConfig config = new CosXmlConfig.Builder()
                .SetRegion(region)
                .SetDebugLog(true)
                .SetConnectionLimit(512)
                .Build();

            QCloudCredentialProvider qCloudCredentialProvider = new DefaultSessionQCloudCredentialProvider(
                "secretId",
                "secretKey",
                1597907089,
                "sessionToken"
            );

            cosXml = new CosXmlServer(config, qCloudCredentialProvider);

            transferManager = new TransferManager(cosXml, new TransferConfig());

            smallFileSrcPath = QCloudServer.CreateFile(TimeUtils.GetCurrentTime(TimeUnit.SECONDS) + ".txt", 1024 * 1024 * 1);
            bigFileSrcPath = QCloudServer.CreateFile(TimeUtils.GetCurrentTime(TimeUnit.SECONDS) + ".txt", 1024 * 1024 * 10);
            FileInfo fileInfo = new FileInfo(smallFileSrcPath);
            DirectoryInfo directoryInfo = fileInfo.Directory;
            localDir = directoryInfo.FullName;
            localFileName = "local.txt";
        }

        // [Test()]
        public void PutObject()
        {
            init();

            string bucket = "000000-1253653367";
            string objectKey = "文件夹/exampleobject";

            try
            {
                PutObjectRequest request = new PutObjectRequest(bucket, objectKey, smallFileSrcPath);

                //执行请求
                PutObjectResult result = cosXml.PutObject(request);

                Console.WriteLine(result.GetResultInfo());
            }
            catch (CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.Message);
            }
            catch (CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
            }
        }
    }
}

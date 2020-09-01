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

        public void init() {
          string region = "ap-guangzhou";
          
          CosXmlConfig config = new CosXmlConfig.Builder()
              .SetRegion(region)
              .SetDebugLog(true)
              .SetConnectionLimit(512)
              .Build();

          QCloudCredentialProvider qCloudCredentialProvider = new DefaultSessionQCloudCredentialProvider(
              "AKIDLT2qvPSOSRq5eM2TR11daHYBfT1rl8fuWgAJhtbo3eFwcbpFnZHAYkgpaFrzvtq4",
              "i19oUWRIvvpqGpiUgwqa3wufzSbDPyaxv0JYArSQFMc=",
              1597907089,
              "YiPWtzGhVPG5HQlOLQdxZrxHOitu1JXL903fda3a64f28616997f861116b2d7d1EyG_RmvCKqArCLtOXxKv9MGtugt4lPcUbESOTHdhmhEyMYgnPpJSLpbQoKPYzg7WwwvjjXqOKQvs_I9iCHyaBXJu_LToqeuQfO4NFpos8I_NWBKxg23xFpvuXHXJ7kDuyudwu7qInonvJpoXAnb_J-7rMY94aVWSg6kolryvO-x5_C3cBa1fIiKTrwkRP4keh0g3_asIwP99DA-WASJ147C_NsZl9fdTlWEBnIeQqB7uRHAZkF-_f03-fBYJofeYGe7FArrfai4_AO57QbTwacfzHk2s-43WuCIhoLud_-k"
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

        [Test()]
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
              Assert.True(false);
          }
          catch (CosServerException serverEx)
          {
              Console.WriteLine("CosServerException: " + serverEx.GetInfo());
              Assert.True(false);
          }
        }
    }
}

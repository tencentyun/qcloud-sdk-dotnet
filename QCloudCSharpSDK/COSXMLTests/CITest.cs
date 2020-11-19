using COSXML.Common;
using COSXML.CosException;
using COSXML.Model;
using COSXML.Model.Object;
using COSXML.Model.Tag;
using COSXML.Model.CI;
using COSXML.Utils;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace COSXMLTests
{

    [TestFixture()]
    public class CITest
    {
        private string localTempPhotoFilePath;

        private string photoKey;

        [SetUp]
        public void Setup()
        {
            string bucket = QCloudServer.Instance().bucketForBucketTest;
            photoKey = "example_photo.jpg";

            localTempPhotoFilePath = QCloudServer.CreateFile(TimeUtils.GetCurrentTime(TimeUnit.SECONDS) + ".jpg", 1);
            FileInfo fileInfo = new FileInfo(localTempPhotoFilePath);

            DirectoryInfo directoryInfo = fileInfo.Directory;


            GetObjectRequest request = new GetObjectRequest(bucket, photoKey, directoryInfo.FullName, fileInfo.Name);

            QCloudServer.Instance().cosXml.GetObject(request);
        }

        [TearDown]
        public void Clear()
        {
            QCloudServer.DeleteFile(localTempPhotoFilePath);
        }


        [Test]
        public void PutObjectWithDeSample()
        {
            string bucket = QCloudServer.Instance().bucketForBucketTest;
            string key = "original_photo.jpg";
            string srcPath = localTempPhotoFilePath;


            PutObjectRequest request = new PutObjectRequest(bucket, key, srcPath);

            JObject o = new JObject();

            // 不返回原图
            o["is_pic_info"] = 0;
            JArray rules = new JArray();
            JObject rule = new JObject();

            rule["bucket"] = bucket;
            rule["fileid"] = "desample_photo.jpg";
            //处理参数，规则参见：https://cloud.tencent.com/document/product/460/19017
            rule["rule"] = "imageMogr2/thumbnail/400x";
            rules.Add(rule);
            o["rules"] = rules;
            string ruleString = o.ToString(Formatting.None);


            Console.WriteLine(ruleString);

            request.SetRequestHeader("Pic-Operations", ruleString);
            //执行请求
            PutObjectResult result = QCloudServer.Instance().cosXml.PutObject(request);

            Console.WriteLine(result.GetResultInfo());

            Assert.True(result.uploadResult != null);
            Assert.True(result.uploadResult.processResults.results[0].Width <= 400);
            Assert.True(result.uploadResult.processResults.results[0].Height <= 400);
        }

        [Test]
        public void SensitiveRecognition()
        {
            string bucket = QCloudServer.Instance().bucketForBucketTest;
            //对象键
            //对象键
            string key = photoKey;


            SensitiveContentRecognitionRequest request = new SensitiveContentRecognitionRequest(bucket, key, "politics");

            SensitiveContentRecognitionResult result = QCloudServer.Instance().cosXml.sensitiveContentRecognition(request);


            Console.WriteLine(result.GetResultInfo());

            Assert.True(result.httpCode == 200);
            Assert.True(result.recognitionResult != null && result.recognitionResult.PoliticsInfo != null);
        }

        [Test]
        public void ImageProcess()
        {
            string bucket = QCloudServer.Instance().bucketForBucketTest;
            string key = photoKey;

            JObject o = new JObject();

            // 不返回原图
            o["is_pic_info"] = 0;
            JArray rules = new JArray();
            JObject rule = new JObject();

            rule["bucket"] = bucket;
            rule["fileid"] = "desample_photo.jpg";
            //处理参数，规则参见：https://cloud.tencent.com/document/product/460/19017
            rule["rule"] = "imageMogr2/thumbnail/400x400";
            rules.Add(rule);
            o["rules"] = rules;
            string ruleString = o.ToString(Formatting.None);


            Console.WriteLine(ruleString);

            ImageProcessRequest request = new ImageProcessRequest(bucket, key, ruleString);

            ImageProcessResult result = QCloudServer.Instance().cosXml.imageProcess(request);


            Console.WriteLine(result.GetResultInfo());

            Assert.True(result.httpCode == 200);
            Assert.True(result.uploadResult.processResults.results[0].Width <= 400);
            Assert.True(result.uploadResult.processResults.results[0].Height <= 400);
        }
    }
}
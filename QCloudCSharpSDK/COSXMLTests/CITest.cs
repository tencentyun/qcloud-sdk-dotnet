using COSXML.Common;
using COSXML.CosException;
using COSXML.Model;
using COSXML.Model.Object;
using COSXML.Model.Tag;
using COSXML.Model.CI;
using COSXML.Utils;
using COSXML.Transfer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace COSXMLTests
{

    [TestFixture()]
    public class CITest
    {
        private string localTempPhotoFilePath;
        private string localQRCodeTempPhotoFilePath;
        private string localSnapshotFilePath;
        private string localSnapshotFileName;
        private string photoKey;
        private string qrPhotoKey;
        private string videoKey;
        private string bucket;

        [OneTimeSetUp]
        public void Setup()
        {
            bucket = QCloudServer.Instance().bucketForObjectTest;

            photoKey = "example_photo.jpg";
            localTempPhotoFilePath = QCloudServer.CreateFile(TimeUtils.GetCurrentTime(TimeUnit.Seconds) + ".jpg", 1);
            FileInfo fileInfo = new FileInfo(localTempPhotoFilePath);
            DirectoryInfo directoryInfo = fileInfo.Directory;
            PutObjectRequest request = new PutObjectRequest(bucket, photoKey, fileInfo.Name);
            QCloudServer.Instance().cosXml.PutObject(request);

            qrPhotoKey = "qr_code_photo.jpg";
            localQRCodeTempPhotoFilePath = QCloudServer.CreateFile("qr_" + TimeUtils.GetCurrentTime(TimeUnit.Seconds) + ".jpg", 1);
            fileInfo = new FileInfo(localQRCodeTempPhotoFilePath);
            request = new PutObjectRequest(bucket, qrPhotoKey, fileInfo.Name);
            QCloudServer.Instance().cosXml.PutObject(request);
            videoKey = "CITestVideo.mp4";
            localSnapshotFilePath = "/tmp/snapshot.jpg";
        }

        [OneTimeTearDown]
        public void Clear()
        {
            QCloudServer.DeleteFile(localTempPhotoFilePath);
            QCloudServer.DeleteFile(localQRCodeTempPhotoFilePath);
            QCloudServer.DeleteFile(localSnapshotFilePath);
        }


        [Test]
        public void PutObjectWithDeSample()
        {
            string key = "original_photo.jpg";
            string srcPath = localTempPhotoFilePath;

            PutObjectRequest request = new PutObjectRequest(bucket, key, srcPath);

            JObject o = new JObject();

            // 返回原图
            o["is_pic_info"] = 1;
            JArray rules = new JArray();
            JObject rule = new JObject();

            rule["bucket"] = bucket;
            rule["fileid"] = "desample_photo.jpg";
            //处理参数，规则参见：https://cloud.tencent.com/document/product/460/19017
            rule["rule"] = "imageMogr2/thumbnail/400x";
            rules.Add(rule);
            o["rules"] = rules;
            string ruleString = o.ToString(Formatting.None);

            request.SetRequestHeader("Pic-Operations", ruleString);
            //执行请求
            PutObjectResult result = QCloudServer.Instance().cosXml.PutObject(request);
            var uploadResult = result.uploadResult;
            // Console.WriteLine(result.GetResultInfo());
            Assert.IsNotEmpty((result.GetResultInfo()));

            Assert.True(result.IsSuccessful());
            Assert.NotNull(uploadResult);

            Assert.NotNull(uploadResult.originalInfo);
            Assert.NotNull(uploadResult.originalInfo.ETag);
            Assert.NotNull(uploadResult.originalInfo.Key);
            Assert.NotNull(uploadResult.originalInfo.Location);
            Assert.NotNull(uploadResult.originalInfo.imageInfo.Ave);
            Assert.NotNull(uploadResult.originalInfo.imageInfo.Format);
            Assert.NotNull(uploadResult.originalInfo.imageInfo.Orientation);
            Assert.NotZero(uploadResult.originalInfo.imageInfo.Width);
            Assert.NotZero(uploadResult.originalInfo.imageInfo.Height);
            Assert.NotZero(uploadResult.originalInfo.imageInfo.Quality);

            Assert.NotNull(uploadResult.processResults);
            Assert.NotZero(uploadResult.processResults.results.Count);
            Assert.True(uploadResult.processResults.results[0].Width <= 400);
            Assert.True(uploadResult.processResults.results[0].Height <= 400);
            Assert.NotNull(uploadResult.processResults.results[0].ETag);
            Assert.NotNull(uploadResult.processResults.results[0].Format);
            Assert.NotNull(uploadResult.processResults.results[0].Key);
            Assert.NotNull(uploadResult.processResults.results[0].Location);
            Assert.NotZero(uploadResult.processResults.results[0].Quality);
            Assert.NotZero(uploadResult.processResults.results[0].Size);
            Assert.Zero(uploadResult.processResults.results[0].WatermarkStatus);
        }

        [Test]
        public void SensitiveRecognition()
        {
            //对象键
            //对象键
            string key = photoKey;

            SensitiveContentRecognitionRequest request = new SensitiveContentRecognitionRequest(bucket, key, "porn,terrorist,politics");

            SensitiveContentRecognitionResult result = QCloudServer.Instance().cosXml.SensitiveContentRecognition(request);

            // Console.WriteLine(result.GetResultInfo());
            Assert.IsNotEmpty((result.GetResultInfo()));

            Assert.True(result.httpCode == 200);
            Assert.NotNull(result.recognitionResult);
            Assert.NotNull(result.recognitionResult.PoliticsInfo);
            Assert.Zero(result.recognitionResult.PoliticsInfo.Code);
            Assert.NotNull(result.recognitionResult.PoliticsInfo.Score);
            Assert.NotNull(result.recognitionResult.PoliticsInfo.Count);
            Assert.NotNull(result.recognitionResult.PoliticsInfo.Msg);
            Assert.NotNull(result.recognitionResult.PoliticsInfo.Label);
            Assert.NotNull(result.recognitionResult.PoliticsInfo.HitFlag);
        }

        [Test]
        public void ImageProcess()
        {
            string key = photoKey;

            JObject o = new JObject();

            // 返回原图
            o["is_pic_info"] = 1;
            JArray rules = new JArray();
            JObject rule = new JObject();

            rule["bucket"] = bucket;
            rule["fileid"] = "desample_photo.jpg";
            //处理参数，规则参见：https://cloud.tencent.com/document/product/460/19017
            rule["rule"] = "imageMogr2/thumbnail/400x400";
            rules.Add(rule);
            o["rules"] = rules;
            string ruleString = o.ToString(Formatting.None);

            ImageProcessRequest request = new ImageProcessRequest(bucket, key, ruleString);

            ImageProcessResult result = QCloudServer.Instance().cosXml.ImageProcess(request);
            var uploadResult = result.uploadResult;

            // Console.WriteLine(result.GetResultInfo());
            Assert.IsNotEmpty((result.GetResultInfo()));
            Assert.True(result.IsSuccessful());
            Assert.NotNull(uploadResult);

            Assert.NotNull(uploadResult.originalInfo);
            Assert.NotNull(uploadResult.originalInfo.ETag);
            Assert.NotNull(uploadResult.originalInfo.Key);
            Assert.NotNull(uploadResult.originalInfo.Location);
            Assert.NotNull(uploadResult.originalInfo.imageInfo.Ave);
            Assert.NotNull(uploadResult.originalInfo.imageInfo.Format);
            Assert.NotNull(uploadResult.originalInfo.imageInfo.Orientation);
            Assert.NotZero(uploadResult.originalInfo.imageInfo.Width);
            Assert.NotZero(uploadResult.originalInfo.imageInfo.Height);
            Assert.NotZero(uploadResult.originalInfo.imageInfo.Quality);

            Assert.NotNull(uploadResult.processResults);
            Assert.NotZero(uploadResult.processResults.results.Count);
            Assert.True(uploadResult.processResults.results[0].Width <= 400);
            Assert.True(uploadResult.processResults.results[0].Height <= 400);
            Assert.NotNull(uploadResult.processResults.results[0].ETag);
            Assert.NotNull(uploadResult.processResults.results[0].Format);
            Assert.NotNull(uploadResult.processResults.results[0].Key);
            Assert.NotNull(uploadResult.processResults.results[0].Location);
            Assert.NotZero(uploadResult.processResults.results[0].Quality);
            Assert.NotZero(uploadResult.processResults.results[0].Size);
            Assert.Zero(uploadResult.processResults.results[0].WatermarkStatus);
        }


        [Test]
        public void QRCodeRecognition()
        {
            string key = qrPhotoKey;
            string srcPath = localQRCodeTempPhotoFilePath;

            PutObjectRequest request = new PutObjectRequest(bucket, key, srcPath);

            JObject o = new JObject();
            // 不返回原图
            o["is_pic_info"] = 1;
            JArray rules = new JArray();
            JObject rule = new JObject();
            rule["bucket"] = bucket;
            rule["fileid"] = "qrcode.jpg";
            //处理参数，规则参见：https://cloud.tencent.com/document/product/460/37513
            rule["rule"] = "QRcode/cover/0";
            rules.Add(rule);
            o["rules"] = rules;

            string ruleString = o.ToString(Formatting.None);
            
            request.SetRequestHeader("Pic-Operations", ruleString);
            //执行请求
            PutObjectResult result = QCloudServer.Instance().cosXml.PutObject(request);
            var uploadResult = result.uploadResult;

            Assert.IsNotEmpty((result.GetResultInfo()));
            Assert.True(result.IsSuccessful());
            Assert.NotNull(uploadResult);

            Assert.NotNull(uploadResult.originalInfo);
            Assert.NotNull(uploadResult.originalInfo.ETag);
            Assert.NotNull(uploadResult.originalInfo.Key);
            Assert.NotNull(uploadResult.originalInfo.Location);
            Assert.NotNull(uploadResult.originalInfo.imageInfo.Format);
            Assert.NotZero(uploadResult.originalInfo.imageInfo.Width);
            Assert.NotZero(uploadResult.originalInfo.imageInfo.Height);
            Assert.NotZero(uploadResult.originalInfo.imageInfo.Quality);

            Assert.NotNull(uploadResult.processResults);
            Assert.NotZero(uploadResult.processResults.results.Count);
            Assert.NotNull(uploadResult.processResults.results[0].ETag);
            Assert.NotNull(uploadResult.processResults.results[0].Format);
            Assert.NotNull(uploadResult.processResults.results[0].Key);
            Assert.NotNull(uploadResult.processResults.results[0].Location);
            Assert.NotZero(uploadResult.processResults.results[0].Quality);
            Assert.NotZero(uploadResult.processResults.results[0].Size);
            Assert.AreEqual(uploadResult.processResults.results[0].CodeStatus, 1);
            Assert.NotNull(uploadResult.processResults.results[0].QRcodeInfo);
            Assert.NotNull(uploadResult.processResults.results[0].QRcodeInfo.CodeUrl);
            Assert.NotNull(uploadResult.processResults.results[0].QRcodeInfo.CodeLocation);
            Assert.NotNull(uploadResult.processResults.results[0].QRcodeInfo.CodeLocation.Point);
            Assert.True(uploadResult.processResults.results[0].QRcodeInfo.CodeLocation.Point.Count > 0);

            QRCodeRecognitionRequest rRequest = new QRCodeRecognitionRequest(bucket, key, 0);

            QRCodeRecognitionResult rResult = QCloudServer.Instance().cosXml.QRCodeRecognition(rRequest);

            Assert.IsNotEmpty((rResult.GetResultInfo()));
            Assert.True(rResult.IsSuccessful());
            Assert.NotNull(rResult.recognitionResult);
            Assert.NotNull(rResult.recognitionResult.QRcodeInfo);
            Assert.Null(rResult.recognitionResult.ResultImage);
            Assert.AreEqual(rResult.recognitionResult.CodeStatus, 1);
            Assert.NotNull(rResult.recognitionResult.QRcodeInfo.CodeLocation);
            Assert.NotNull(rResult.recognitionResult.QRcodeInfo.CodeUrl);
            Assert.NotNull(rResult.recognitionResult.QRcodeInfo.CodeLocation.Point);
            Assert.True(rResult.recognitionResult.QRcodeInfo.CodeLocation.Point.Count > 0);

            // with cover
            rRequest = new QRCodeRecognitionRequest(bucket, key, 1);

            rResult = QCloudServer.Instance().cosXml.QRCodeRecognition(rRequest);

            Assert.IsNotEmpty((rResult.GetResultInfo()));
            Assert.True(rResult.IsSuccessful());
            Assert.NotNull(rResult.recognitionResult);
            Assert.NotNull(rResult.recognitionResult.QRcodeInfo);
            Assert.NotNull(rResult.recognitionResult.ResultImage);
            Assert.AreEqual(rResult.recognitionResult.CodeStatus, 1);
            Assert.NotNull(rResult.recognitionResult.QRcodeInfo.CodeLocation);
            Assert.NotNull(rResult.recognitionResult.QRcodeInfo.CodeUrl);
            Assert.NotNull(rResult.recognitionResult.QRcodeInfo.CodeLocation.Point);
            Assert.True(rResult.recognitionResult.QRcodeInfo.CodeLocation.Point.Count > 0);
        
        }

        [Test]
        public void TestGetSnapshotRequest()
        {
            try
            {
                string key = videoKey;
                GetSnapshotRequest request = new GetSnapshotRequest(bucket, key, 1.5F, localSnapshotFilePath);
                GetSnapshotResult result = QCloudServer.Instance().cosXml.GetSnapshot(request);
                Assert.True(File.Exists(localSnapshotFilePath));
                Assert.AreEqual(result.httpCode, 200);
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.Message);
                Assert.Fail();
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.Fail();
            }
            
        }

        [Test]
        public void TestGetMediaInfo()
        {
            try
            {
                string key = videoKey;
                GetMediaInfoRequest request = new GetMediaInfoRequest(bucket, key);
                GetMediaInfoResult result = QCloudServer.Instance().cosXml.GetMediaInfo(request);
                Assert.AreEqual(result.httpCode, 200);
                ForeachClassProperties<MediaInfoResult.MediaInfo.Stream.Video>(result.mediaInfoResult.mediaInfo.stream.video);
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.Message);
                Assert.Fail();
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.Fail();
            }
            
        }

        public static void ForeachClassProperties<T>(T model)
        {
            // TODO(shawnnqin) 这里反射check属性没有弄到，再优化一下
            Type t = model.GetType();
            PropertyInfo[] PropertyList = t.GetProperties();
            foreach (PropertyInfo item in PropertyList)
            {
                string name = item.Name;
                object value = item.GetValue(model, null);
                Assert.NotNull(value);
            }
        }

        [Test]
        public void TestVideoCensorJobCommit()
        {
            try
            {
                SubmitVideoCensorJobRequest request = new SubmitVideoCensorJobRequest(bucket);
                request.SetCensorObject(videoKey);
                request.SetDetectType("Porn,Terrorisim");
                request.SetSnapshotMode("Interval");
                SensitiveCencorJobResult result = QCloudServer.Instance().cosXml.SubmitVideoCensorJob(request);
                string id = result.censorJobsResponse.JobsDetail.JobId;
                Assert.NotNull(id);
                Assert.AreEqual(200, result.httpCode);
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.Message);
                Assert.Fail();
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.Fail();
            }
        }

        [Test]
        public void TestDescribeMediaBuckets()
        {
            try
            {
                DescribeMediaBucketsRequest request = new DescribeMediaBucketsRequest();
                DescribeMediaBucketsResult result = QCloudServer.Instance().cosXml.DescribeMediaBuckets(request);
                Assert.AreEqual(result.httpCode, 200);
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.Message);
                Assert.Fail();
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.Fail();
            }
        }
    }
}
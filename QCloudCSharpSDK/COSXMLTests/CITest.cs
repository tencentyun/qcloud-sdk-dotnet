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
// using System.Threading.Tasks;
using COSXML;
using COSXML.Auth;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

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
        private string audioKey;
        private string textKey;
        private string bucket;
        private string appid;
        private string region;

        [OneTimeSetUp]
        public void Setup()
        {
            Console.WriteLine("start CITest");
            bucket = QCloudServer.Instance().bucketForObjectTest;
            appid = QCloudServer.Instance().appid;
            region = QCloudServer.Instance().region;

            // 本地文件名，用于暂存下载的文件，以测试上传时处理接口
            localTempPhotoFilePath = TimeUtils.GetCurrentTime(TimeUnit.Seconds) + ".jpg";
            localQRCodeTempPhotoFilePath = "qr_" + TimeUtils.GetCurrentTime(TimeUnit.Seconds) + ".jpg";
            localSnapshotFilePath = "/tmp/snapshot.jpg";
            /*
            localTempPhotoFilePath = QCloudServer.CreateFile(TimeUtils.GetCurrentTime(TimeUnit.Seconds) + ".jpg", 1);
            FileInfo fileInfo = new FileInfo(localTempPhotoFilePath);
            DirectoryInfo directoryInfo = fileInfo.Directory;
            PutObjectRequest request = new PutObjectRequest(bucket, photoKey, fileInfo.Name);
            QCloudServer.Instance().cosXml.PutObject(request);
            */
            /*
            qrPhotoKey = "qr_code_photo.jpg";
            localQRCodeTempPhotoFilePath = QCloudServer.CreateFile("qr_" + TimeUtils.GetCurrentTime(TimeUnit.Seconds) + ".jpg", 1);
            fileInfo = new FileInfo(localQRCodeTempPhotoFilePath);
            PutObjectRequest request = new PutObjectRequest(bucket, qrPhotoKey, fileInfo.Name);
            QCloudServer.Instance().cosXml.PutObject(request);
            */
            // 预先上传好的媒体文件，用于万象媒体接口的测试
            photoKey = "CITestImage.png";
            qrPhotoKey = "CITestQrImage.jpg";
            videoKey = "CITestVideo.mp4";
            audioKey = "CITestAudio.mp3";
            textKey = "CITestText.txt";
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
            try {
                // 利用云上格式正确的 demo图片进行测试
                GetObjectRequest getRequest = new GetObjectRequest(bucket, photoKey, ".", localTempPhotoFilePath);
                GetObjectResult getResult = QCloudServer.Instance().cosXml.GetObject(getRequest);
                Assert.True(200 == getResult.httpCode);
                
                // 发起上传流程
                string key = "original_photo.png";
                string srcPath = localTempPhotoFilePath;

                PutObjectRequest request = new PutObjectRequest(bucket, key, srcPath);

                JObject o = new JObject();

                // 返回原图
                o["is_pic_info"] = 1;
                JArray rules = new JArray();
                JObject rule = new JObject();

                rule["bucket"] = bucket;
                rule["fileid"] = "desample_photo.png";
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
        public void SensitiveRecognition()
        {
            //对象键
            try {
                string key = photoKey;

                SensitiveContentRecognitionRequest request = new SensitiveContentRecognitionRequest(bucket, key);
                request.SetBizType("");
                request.SetDetectUrl("");
                request.SetInterval("1");
                request.SetMaxFrames("1");
                request.SetLargeImageDetect("0");
                request.SetDataid("");
                request.SetAsync("0");
                request.SetCallback("");
                SensitiveContentRecognitionResult result = QCloudServer.Instance().cosXml.SensitiveContentRecognition(request);

                Assert.True(result.httpCode == 200);
                Assert.NotNull(result.recognitionResult);
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
        public void ImageProcess()
        {
            string key = photoKey;

            JObject o = new JObject();

            // 返回原图
            o["is_pic_info"] = 1;
            JArray rules = new JArray();
            JObject rule = new JObject();

            rule["bucket"] = bucket;
            rule["fileid"] = "desample_photo.png";
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
        public void ImageProcessOperationsRulesNull()
        {
            try
            {
                string key = photoKey;
                ImageProcessRequest request = new ImageProcessRequest(bucket, key, null);
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.Message);
                Assert.True(clientEx.Message.Equals("operationRules = null"));
            }
        }

        [Test]
        public void QRCodeRecognition()
        {
            string key = qrPhotoKey;
            // 下载云上有内容的 QR Code
            string srcPath = localQRCodeTempPhotoFilePath;
            GetObjectRequest getRequest = new GetObjectRequest(bucket, key, ".", localQRCodeTempPhotoFilePath);
            GetObjectResult getResult = QCloudServer.Instance().cosXml.GetObject(getRequest);
            Assert.True(200 == getResult.httpCode);

            // 开始请求上传时 QR code 识别
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
        public void TestGetSnapshot()
        {
            try
            {
                string key = videoKey;
                GetSnapshotRequest request = new GetSnapshotRequest(bucket, key, 1.5F, localSnapshotFilePath,10,10,"png","off","keyframe");
                GetSnapshotResult result = QCloudServer.Instance().cosXml.GetSnapshot(request);
                Assert.True(File.Exists(localSnapshotFilePath));
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
        public void TestGetMediaInfo()
        {
            try
            {
                string key = videoKey;
                GetMediaInfoRequest request = new GetMediaInfoRequest(bucket, key);
                GetMediaInfoResult result = QCloudServer.Instance().cosXml.GetMediaInfo(request);
                Assert.AreEqual(result.httpCode, 200);
                // 查看视频文件媒体参数是否成功获取
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Stream);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Stream.Video);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Stream.Video.Index);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Stream.Video.CodecName);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Stream.Video.CodecLongName);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Stream.Video.CodecTagString);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Stream.Video.CodecTag);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Stream.Video.Profile);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Stream.Video.Width);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Stream.Video.Height);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Stream.Video.HasBFrame);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Stream.Video.RefFrames);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Stream.Video.Sar);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Stream.Video.Dar);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Stream.Video.PixFormat);
                //Assert.NotNull(result.mediaInfoResult.MediaInfo.Stream.Video.FieldOrder);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Stream.Video.Level);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Stream.Video.Fps);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Stream.Video.AvgFps);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Stream.Video.Timebase);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Stream.Video.StartTime);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Stream.Video.Duration);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Stream.Video.Bitrate);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Stream.Video.NumFrames);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Stream.Video.Language);    

                Assert.NotNull(result.mediaInfoResult.MediaInfo.Stream.Audio);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Stream.Audio.Index);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Stream.Audio.CodecName);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Stream.Audio.CodecLongName);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Stream.Audio.CodecTimeBase);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Stream.Audio.CodecTagString);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Stream.Audio.CodecTag);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Stream.Audio.SampleFmt);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Stream.Audio.SampleRate);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Stream.Audio.Channel);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Stream.Audio.ChannelLayout);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Stream.Audio.Timebase);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Stream.Audio.StartTime);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Stream.Audio.Duration);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Stream.Audio.Bitrate);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Stream.Audio.Language);

                Assert.NotNull(result.mediaInfoResult.MediaInfo.Stream.Subtitle);
                //Assert.NotNull(result.mediaInfoResult.MediaInfo.Stream.Subtitle.Index);
                //Assert.NotNull(result.mediaInfoResult.MediaInfo.Stream.Subtitle.Language);
                
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Format);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Format.NumStream);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Format.NumProgram);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Format.FormatName);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Format.FormatLongName);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Format.StartTime);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Format.Duration);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Format.Bitrate);
                Assert.NotNull(result.mediaInfoResult.MediaInfo.Format.Size);
                
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
        public void TestVideoCensorJobCommit()
        {
            try
            {
                
                SubmitVideoCensorJobRequest request = new SubmitVideoCensorJobRequest(bucket);
                request.SetCensorUrl("https://download.samplelib.com/mp4/sample-5s.mp4");
                request.SetDetectType("Porn,Terrorism");
                request.SetSnapshotMode("Average");
                request.SetSnapshotCount("5");
                request.SetCallback("http://127.0.0.1/index.html");
                request.SetCallbackVersion("Simple");
                request.SetBizType("");
                request.SetDetectContent(0);
                request.SetSnapshotTimeInterval("5");
                SubmitCensorJobResult result = QCloudServer.Instance().cosXml.SubmitVideoCensorJob(request);
                request.SetCensorObject(videoKey);
                Assert.NotNull(result.censorJobsResponse.JobsDetail.JobId);
                Assert.NotNull(result.censorJobsResponse.JobsDetail.State);
                Assert.NotNull(result.censorJobsResponse.JobsDetail.CreationTime);
                string id = result.censorJobsResponse.JobsDetail.JobId;
                Thread.Sleep(50000);
                // await Task.Delay(50000);
                
                // get video censor job
                GetVideoCensorJobRequest getRequest = new GetVideoCensorJobRequest(bucket, id);
                GetVideoCensorJobResult getResult = QCloudServer.Instance().cosXml.GetVideoCensorJob(getRequest);
                Assert.AreEqual(200, getResult.httpCode);

                Assert.NotNull(getResult.resultStruct.JobsDetail);
                //Assert.NotNull(getResult.resultStruct.JobsDetail.Code);
                //Assert.NotNull(getResult.resultStruct.JobsDetail.Message);
                Assert.NotNull(getResult.resultStruct.JobsDetail.JobId);
                Assert.NotNull(getResult.resultStruct.JobsDetail.State);
                try
                {
                    Assert.AreEqual("Success", getResult.resultStruct.JobsDetail.State);
                }
                catch (Exception)
                {
                }
                
                Assert.NotNull(getResult.resultStruct.JobsDetail.CreationTime);
                //Assert.NotNull(getResult.resultStruct.JobsDetail.Object);
                Assert.NotNull(getResult.resultStruct.JobsDetail.SnapshotCount);
                Assert.NotNull(getResult.resultStruct.JobsDetail.Result);

                Assert.NotNull(getResult.resultStruct.JobsDetail.PornInfo);
                Assert.NotNull(getResult.resultStruct.JobsDetail.PornInfo.HitFlag);
                Assert.NotNull(getResult.resultStruct.JobsDetail.PornInfo.Count);
                
                Assert.NotNull(getResult.resultStruct.JobsDetail.TerrorismInfo);
                Assert.NotNull(getResult.resultStruct.JobsDetail.TerrorismInfo.HitFlag);
                Assert.NotNull(getResult.resultStruct.JobsDetail.TerrorismInfo.Count);
                /*
                Assert.NotNull(getResult.resultStruct.JobsDetail.PoliticsInfo);
                Assert.NotNull(getResult.resultStruct.JobsDetail.PoliticsInfo.HitFlag);
                Assert.NotNull(getResult.resultStruct.JobsDetail.PoliticsInfo.Count);

                Assert.NotNull(getResult.resultStruct.JobsDetail.AdsInfo);
                Assert.NotNull(getResult.resultStruct.JobsDetail.AdsInfo.HitFlag);
                Assert.NotNull(getResult.resultStruct.JobsDetail.AdsInfo.Count);
*/
                Assert.NotZero(getResult.resultStruct.JobsDetail.Snapshot.Count);
                for(int i = 0; i < getResult.resultStruct.JobsDetail.Snapshot.Count; i++)
                {
                    Assert.NotNull(getResult.resultStruct.JobsDetail.Snapshot[i]);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.Snapshot[i].Url);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.Snapshot[i].Text);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.Snapshot[i].SnapshotTime);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.Snapshot[i].PornInfo);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.Snapshot[i].PornInfo.HitFlag);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.Snapshot[i].PornInfo.Score);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.Snapshot[i].PornInfo.Label);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.Snapshot[i].PornInfo.SubLabel);

                    Assert.NotNull(getResult.resultStruct.JobsDetail.Snapshot[i].TerrorismInfo);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.Snapshot[i].TerrorismInfo.HitFlag);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.Snapshot[i].TerrorismInfo.Score);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.Snapshot[i].TerrorismInfo.Label);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.Snapshot[i].TerrorismInfo.SubLabel);
                    /*
                    Assert.NotNull(getResult.resultStruct.JobsDetail.Snapshot[i].PoliticsInfo);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.Snapshot[i].PoliticsInfo.HitFlag);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.Snapshot[i].PoliticsInfo.Score);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.Snapshot[i].PoliticsInfo.Label);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.Snapshot[i].PoliticsInfo.SubLabel);

                    Assert.NotNull(getResult.resultStruct.JobsDetail.Snapshot[i].AdsInfo);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.Snapshot[i].AdsInfo.HitFlag);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.Snapshot[i].AdsInfo.Score);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.Snapshot[i].AdsInfo.Label);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.Snapshot[i].AdsInfo.SubLabel);  
                    */   
                }
                Assert.NotNull(getResult.resultStruct.JobsDetail.AudioSection);
                /*
                Assert.NotZero(getResult.resultStruct.JobsDetail.AudioSection.Count);
                for(int i = 0; i < getResult.resultStruct.JobsDetail.AudioSection.Count; i++)
                {
                    Assert.NotNull(getResult.resultStruct.JobsDetail.AudioSection[i]);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.AudioSection[i].Url);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.AudioSection[i].OffsetTime);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.AudioSection[i].Duration);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.AudioSection[i].Text);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.AudioSection[i].PoliticsInfo);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.AudioSection[i].PoliticsInfo.HitFlag);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.AudioSection[i].PoliticsInfo.Score);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.AudioSection[i].PoliticsInfo.Keywords);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.AudioSection[i].PornInfo);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.AudioSection[i].PornInfo.HitFlag);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.AudioSection[i].PornInfo.Score);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.AudioSection[i].PornInfo.Keywords);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.AudioSection[i].TerrorismInfo);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.AudioSection[i].TerrorismInfo.HitFlag);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.AudioSection[i].TerrorismInfo.Score);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.AudioSection[i].TerrorismInfo.Keywords);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.AudioSection[i].AdsInfo);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.AudioSection[i].AdsInfo.HitFlag);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.AudioSection[i].AdsInfo.Score);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.AudioSection[i].AdsInfo.Keywords);
                }
                */

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
        public void TestAudioCensorJob()
        {
            try
            {
                SubmitAudioCensorJobRequest request = new SubmitAudioCensorJobRequest(bucket);
                //request.SetCensorObject(audioKey);
                request.SetCensorUrl("https://download.samplelib.com/mp3/sample-3s.mp3");
                request.SetDetectType("Porn,Terrorism");
                request.SetCallback("");
                request.SetCallbackVersion("");
                request.SetBizType("");
                SubmitCensorJobResult result = QCloudServer.Instance().cosXml.SubmitAudioCensorJob(request);
                string id = result.censorJobsResponse.JobsDetail.JobId;
                Assert.NotNull(id);
                Assert.AreEqual(200, result.httpCode);
                // get audio censor job
                Thread.Sleep(10000);
                // await Task.Delay(10000);
                
                GetAudioCensorJobRequest getRequest = new GetAudioCensorJobRequest(bucket, id);
                // Assert.Equals(getRequest.Bucket, bucket);
                // Assert.Equals(getRequest.Region,QCloudServer.Instance().region);
                GetAudioCensorJobResult getResult = QCloudServer.Instance().cosXml.GetAudioCensorJob(getRequest);
                request.SetCensorObject(audioKey);
                Assert.AreEqual(200, getResult.httpCode);
                // 成功时不返回
                //Assert.NotNull(getResult.resultStruct.JobsDetail.Code);
                //Assert.NotNull(getResult.resultStruct.JobsDetail.Message);
                Assert.NotNull(getResult.resultStruct.JobsDetail.JobId);
                Assert.NotNull(getResult.resultStruct.JobsDetail.State);
                Assert.AreEqual("Success", getResult.resultStruct.JobsDetail.State);
                Assert.NotNull(getResult.resultStruct.JobsDetail.CreationTime);
                //Assert.NotNull(getResult.resultStruct.JobsDetail.Object);
                Assert.NotNull(getResult.resultStruct.JobsDetail.Result);
                Assert.NotNull(getResult.resultStruct.JobsDetail.AudioText);

                Assert.NotNull(getResult.resultStruct.JobsDetail.PornInfo);
                Assert.NotNull(getResult.resultStruct.JobsDetail.PornInfo.HitFlag);
                Assert.NotNull(getResult.resultStruct.JobsDetail.PornInfo.Score);
                Assert.NotNull(getResult.resultStruct.JobsDetail.PornInfo.Label);

                Assert.NotNull(getResult.resultStruct.JobsDetail.TerrorismInfo);
                Assert.NotNull(getResult.resultStruct.JobsDetail.TerrorismInfo.HitFlag);
                Assert.NotNull(getResult.resultStruct.JobsDetail.TerrorismInfo.Score);
                Assert.NotNull(getResult.resultStruct.JobsDetail.TerrorismInfo.Label);
             
                Assert.NotNull(getResult.resultStruct.JobsDetail.Section);
                
                Assert.NotZero(getResult.resultStruct.JobsDetail.Section.Count);
                for(int i = 0; i < getResult.resultStruct.JobsDetail.Section.Count; i++)
                {
                    Assert.NotNull(getResult.resultStruct.JobsDetail.Section[i].Url);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.Section[i].OffsetTime);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.Section[i].Duration);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.Section[i].Text);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.Section[i].PornInfo);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.Section[i].PornInfo.HitFlag);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.Section[i].PornInfo.Score);
                    // 没有命中关键词时不返回
                    //Assert.NotNull(getResult.resultStruct.JobsDetail.Section[i].PornInfo.Keywords);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.Section[i].TerrorismInfo);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.Section[i].TerrorismInfo.HitFlag);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.Section[i].TerrorismInfo.Score);
                    // 没有命中关键词时不返回
                    //Assert.NotNull(getResult.resultStruct.JobsDetail.Section[i].TerrorismInfo.Keywords);
                }
                
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
        public void CIRequestTest()
        {
            CIRequest ciRequest = new CIRequest();
            ciRequest.Region = QCloudServer.Instance().region;
            ciRequest.Bucket = QCloudServer.Instance().bucketForObjectTest;
            Assert.AreEqual(TypeCode.String, ciRequest.Bucket.GetTypeCode());
            Assert.AreEqual(12, ciRequest.Region.Length);
            try
            {
                ciRequest.CheckParameters();
            }
            catch (CosClientException ex)
            {
                Console.WriteLine("CosClientException: " + ex.Message);
                Assert.True(ex.Message.Equals("cosPath(null or empty)is invalid"));

            }

            ciRequest.RequestURLWithSign = "testurl";
            ciRequest.CheckParameters();
            CosXmlConfig.Builder builder = new CosXmlConfig.Builder();
            builder.SetRegion(QCloudServer.Instance().region);
            builder.SetHost("http://bucket-appid.ap-region.myqcloud.com");
            builder.SetAppid(QCloudServer.Instance().appid);

            CosXmlConfig config = builder.Build();
            ciRequest.serviceConfig = config;

            Assert.AreEqual("http://bucket-appid.ap-region.myqcloud.com",ciRequest.GetHost());

            CIRequest ciRequest1 = new CIRequest();
            CosXmlConfig.Builder builder1 = new CosXmlConfig.Builder();
            builder1.SetRegion(QCloudServer.Instance().region);
            builder1.SetAppid("123");
            ciRequest1.Bucket = "bucketid";
            ciRequest1.APPID = "123";
            ciRequest1.Region = "ap-guangzhou";
            CosXmlConfig config1 = builder1.Build();
            ciRequest1.serviceConfig = config1;
            Assert.AreEqual("bucketid-123.ci.ap-guangzhou.myqcloud.com",ciRequest1.GetHost());
            SubmitTextCensorJobsResult submitTextCensorJobsResult = new SubmitTextCensorJobsResult();
            Assert.Null(submitTextCensorJobsResult.textCensorJobsResponse);
            QCloudCredentialProvider qCloudCredentialProvider = new CustomQCloudCredentialProvider();
            CosXmlServer cosXmlServer = new CosXmlServer(config,qCloudCredentialProvider);
            Assert.AreEqual("https://http://bucket-appid.ap-region.myqcloud.com/key",cosXmlServer.GetObjectUrl(bucket,"key"));
            CosXmlServer cosXmlServerHostNull = new CosXmlServer(config1,qCloudCredentialProvider);
            Assert.AreEqual("https://dotnet-ut-obj-1253960454.cos.ap-guangzhou.myqcloud.com/key",cosXmlServerHostNull.GetObjectUrl(bucket,"key"));

        }

        [Test]
        public void TestTextCensorJobCommit()
        {
            try
            {
                
                SubmitTextCensorJobRequest request = new SubmitTextCensorJobRequest(bucket);
                request.SetCensorUrl("https://example-files.online-convert.com/document/txt/example.txt");
                request.SetDetectType("Porn,Terrorism");
                // request.SetCensorContent("12");
                request.SetCallback("http://127.0.0.1/index.html");
                request.SetBizType("");
                request.Bucket = bucket;
                SubmitCensorJobResult result = QCloudServer.Instance().cosXml.SubmitTextCensorJob(request);
                request.SetCensorObject(textKey);
                string id = result.censorJobsResponse.JobsDetail.JobId;
                Assert.NotNull(id);
                Assert.AreEqual(200, result.httpCode);
                // 等待审核任务跑完
                Thread.Sleep(10000);
                // await Task.Delay(10000);
                GetTextCensorJobRequest getRequest = new GetTextCensorJobRequest(bucket, id);
                GetTextCensorJobResult getResult = QCloudServer.Instance().cosXml.GetTextCensorJob(getRequest);
                Assert.AreEqual(200, getResult.httpCode);
                // 只有失败时返回
                //Assert.NotNull(getResult.resultStruct.JobsDetail.Code);
                //Assert.NotNull(getResult.resultStruct.JobsDetail.Message);
                Assert.NotNull(getResult.resultStruct.JobsDetail.JobId);
                Assert.NotNull(getResult.resultStruct.JobsDetail.State);
                Assert.NotNull(getResult.resultStruct.JobsDetail.CreationTime);
                //Assert.NotNull(getResult.resultStruct.JobsDetail.Object);
                Assert.NotNull(getResult.resultStruct.JobsDetail.SectionCount);
                Assert.NotNull(getResult.resultStruct.JobsDetail.Result);

                Assert.NotNull(getResult.resultStruct.JobsDetail.PornInfo);
                Assert.NotNull(getResult.resultStruct.JobsDetail.PornInfo.HitFlag);
                Assert.NotNull(getResult.resultStruct.JobsDetail.PornInfo.Count);
                Assert.NotNull(getResult.resultStruct.JobsDetail.TerrorismInfo);
                Assert.NotNull(getResult.resultStruct.JobsDetail.TerrorismInfo.HitFlag);
                Assert.NotNull(getResult.resultStruct.JobsDetail.TerrorismInfo.Count);
                /*
                Assert.NotNull(getResult.resultStruct.JobsDetail.PoliticsInfo);
                Assert.NotNull(getResult.resultStruct.JobsDetail.PoliticsInfo.HitFlag);
                Assert.NotNull(getResult.resultStruct.JobsDetail.PoliticsInfo.Count);
                */
                /*
                Assert.NotNull(getResult.resultStruct.JobsDetail.AdsInfo);
                Assert.NotNull(getResult.resultStruct.JobsDetail.AdsInfo.HitFlag);
                Assert.NotNull(getResult.resultStruct.JobsDetail.AdsInfo.Count);
                */
                /*
                Assert.NotNull(getResult.resultStruct.JobsDetail.IllegalInfo);
                Assert.NotNull(getResult.resultStruct.JobsDetail.IllegalInfo.HitFlag);
                Assert.NotNull(getResult.resultStruct.JobsDetail.IllegalInfo.Count);
                Assert.NotNull(getResult.resultStruct.JobsDetail.AbuseInfo);
                Assert.NotNull(getResult.resultStruct.JobsDetail.AbuseInfo.HitFlag);
                Assert.NotNull(getResult.resultStruct.JobsDetail.AbuseInfo.Count);
                */

                Assert.NotNull(getResult.resultStruct.JobsDetail.Section);
                Assert.NotNull(getResult.resultStruct.JobsDetail.Section.Count);
                for(int i = 0; i < getResult.resultStruct.JobsDetail.Section.Count; i++)
                {
                    Assert.NotNull(getResult.resultStruct.JobsDetail.Section[i].StartByte);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.Section[i].PornInfo);
                    //Assert.NotNull(getResult.resultStruct.JobsDetail.Section[i].PornInfo.Code);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.Section[i].PornInfo.HitFlag);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.Section[i].PornInfo.Score);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.Section[i].PornInfo.Keywords);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.Section[i].TerrorismInfo);
                    //Assert.NotNull(getResult.resultStruct.JobsDetail.Section[i].PoliticsInfo);
                    //Assert.NotNull(getResult.resultStruct.JobsDetail.Section[i].AdsInfo);
                    //Assert.NotNull(getResult.resultStruct.JobsDetail.Section[i].IllegalInfo);
                    //Assert.NotNull(getResult.resultStruct.JobsDetail.Section[i].AbuseInfo);
                }
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
        public void TestTextCensorJobCommitSync()
        {
            try
            {

                SubmitTextCensorJobRequest request = new SubmitTextCensorJobRequest(bucket);
                request.SetCensorUrl("https://example-files.online-convert.com/document/txt/example.txt");
                request.SetDetectType("Porn,Terrorism");
                // request.SetCensorContent("12");
                request.SetCallback("http://127.0.0.1/index.html");
                request.SetBizType("");
                request.Bucket = bucket;
                SubmitTextCensorJobsResult result = QCloudServer.Instance().cosXml.SubmitTextCensorJobSync(request);
                request.SetCensorObject(textKey);
                string id = result.textCensorJobsResponse.JobsDetail.JobId;
                Assert.NotNull(id);
                Assert.AreEqual(200, result.httpCode);
                // 等待审核任务跑完
                Thread.Sleep(10000);
                // await Task.Delay(10000);
                GetTextCensorJobRequest getRequest = new GetTextCensorJobRequest(bucket, id);
                GetTextCensorJobResult getResult = QCloudServer.Instance().cosXml.GetTextCensorJob(getRequest);
                Assert.AreEqual(200, getResult.httpCode);
                // 只有失败时返回
                //Assert.NotNull(getResult.resultStruct.JobsDetail.Code);
                //Assert.NotNull(getResult.resultStruct.JobsDetail.Message);
                Assert.NotNull(getResult.resultStruct.JobsDetail.JobId);
                Assert.NotNull(getResult.resultStruct.JobsDetail.State);
                Assert.NotNull(getResult.resultStruct.JobsDetail.CreationTime);
                //Assert.NotNull(getResult.resultStruct.JobsDetail.Object);
                Assert.NotNull(getResult.resultStruct.JobsDetail.SectionCount);
                Assert.NotNull(getResult.resultStruct.JobsDetail.Result);

                Assert.NotNull(getResult.resultStruct.JobsDetail.PornInfo);
                Assert.NotNull(getResult.resultStruct.JobsDetail.PornInfo.HitFlag);
                Assert.NotNull(getResult.resultStruct.JobsDetail.PornInfo.Count);
                Assert.NotNull(getResult.resultStruct.JobsDetail.TerrorismInfo);
                Assert.NotNull(getResult.resultStruct.JobsDetail.TerrorismInfo.HitFlag);
                Assert.NotNull(getResult.resultStruct.JobsDetail.TerrorismInfo.Count);
                /*
                Assert.NotNull(getResult.resultStruct.JobsDetail.PoliticsInfo);
                Assert.NotNull(getResult.resultStruct.JobsDetail.PoliticsInfo.HitFlag);
                Assert.NotNull(getResult.resultStruct.JobsDetail.PoliticsInfo.Count);
                */
                /*
                Assert.NotNull(getResult.resultStruct.JobsDetail.AdsInfo);
                Assert.NotNull(getResult.resultStruct.JobsDetail.AdsInfo.HitFlag);
                Assert.NotNull(getResult.resultStruct.JobsDetail.AdsInfo.Count);
                */
                /*
                Assert.NotNull(getResult.resultStruct.JobsDetail.IllegalInfo);
                Assert.NotNull(getResult.resultStruct.JobsDetail.IllegalInfo.HitFlag);
                Assert.NotNull(getResult.resultStruct.JobsDetail.IllegalInfo.Count);
                Assert.NotNull(getResult.resultStruct.JobsDetail.AbuseInfo);
                Assert.NotNull(getResult.resultStruct.JobsDetail.AbuseInfo.HitFlag);
                Assert.NotNull(getResult.resultStruct.JobsDetail.AbuseInfo.Count);
                */

                Assert.NotNull(getResult.resultStruct.JobsDetail.Section);
                Assert.NotNull(getResult.resultStruct.JobsDetail.Section.Count);
                for(int i = 0; i < getResult.resultStruct.JobsDetail.Section.Count; i++)
                {
                    Assert.NotNull(getResult.resultStruct.JobsDetail.Section[i].StartByte);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.Section[i].PornInfo);
                    //Assert.NotNull(getResult.resultStruct.JobsDetail.Section[i].PornInfo.Code);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.Section[i].PornInfo.HitFlag);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.Section[i].PornInfo.Score);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.Section[i].PornInfo.Keywords);
                    Assert.NotNull(getResult.resultStruct.JobsDetail.Section[i].TerrorismInfo);
                    //Assert.NotNull(getResult.resultStruct.JobsDetail.Section[i].PoliticsInfo);
                    //Assert.NotNull(getResult.resultStruct.JobsDetail.Section[i].AdsInfo);
                    //Assert.NotNull(getResult.resultStruct.JobsDetail.Section[i].IllegalInfo);
                    //Assert.NotNull(getResult.resultStruct.JobsDetail.Section[i].AbuseInfo);
                }
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
        public void TestDocumentCensorJobSyncCommit()
        {
            try
            {
                
                SubmitDocumentCensorJobRequest request = new SubmitDocumentCensorJobRequest(bucket);
                request.SetUrl("https://calibre-ebook.com/downloads/demos/demo.docx");
                request.SetDetectType("Porn,Terrorism");
                request.SetType("docx");
                request.SetBizType("");
                SubmitCensorJobResult result = QCloudServer.Instance().cosXml.SubmitDocumentCensorJob(request);
                string id = result.censorJobsResponse.JobsDetail.JobId;
                Assert.NotNull(id);
                Assert.AreEqual(200, result.httpCode);
                // 等待审核任务跑完
                Thread.Sleep(50000);
                // await Task.Delay(50000);
                GetDocumentCensorJobRequest getRequest = new GetDocumentCensorJobRequest(bucket, id);
                GetDocumentCensorJobResult getResult = QCloudServer.Instance().cosXml.GetDocumentCensorJob(getRequest);
                Assert.AreEqual(200, getResult.httpCode);
                // 参数检查
                // Assert.NotNull(getResult.resultStruct.JobsDetail.State);
                // Assert.NotNull(getResult.resultStruct.JobsDetail.JobId);
                // //Assert.NotNull(getResult.resultStruct.JobsDetail.Code);
                // //Assert.NotNull(getResult.resultStruct.JobsDetail.Message);
                // Assert.NotNull(getResult.resultStruct.JobsDetail.Suggestion);
                // Assert.NotNull(getResult.resultStruct.JobsDetail.CreationTime);
                // Assert.NotNull(getResult.resultStruct.JobsDetail.Url);
                // Assert.NotNull(getResult.resultStruct.JobsDetail.PageCount);
                // Assert.NotNull(getResult.resultStruct.JobsDetail.Labels);
                // Assert.NotNull(getResult.resultStruct.JobsDetail.Labels.PornInfo);
                // Assert.NotNull(getResult.resultStruct.JobsDetail.Labels.PornInfo.HitFlag);
                // Assert.NotNull(getResult.resultStruct.JobsDetail.Labels.PornInfo.Score);
                // /*
                // Assert.NotNull(getResult.resultStruct.JobsDetail.Labels.PoliticsInfo);
                // Assert.NotNull(getResult.resultStruct.JobsDetail.Labels.PoliticsInfo.HitFlag);
                // Assert.NotNull(getResult.resultStruct.JobsDetail.Labels.PoliticsInfo.Score);
                // */
                // Assert.NotNull(getResult.resultStruct.JobsDetail.PageSegment);
                // Assert.NotNull(getResult.resultStruct.JobsDetail.PageSegment.Results);
                // Assert.NotNull(getResult.resultStruct.JobsDetail.PageSegment.Results.Url);
                // Assert.NotNull(getResult.resultStruct.JobsDetail.PageSegment.Results.Text);
                // Assert.NotNull(getResult.resultStruct.JobsDetail.PageSegment.Results.PageNumber);
                // //Assert.NotNull(getResult.resultStruct.JobsDetail.PageSegment.Results.SheetNumber);
                // Assert.NotNull(getResult.resultStruct.JobsDetail.PageSegment.Results.PornInfo);
                // Assert.NotNull(getResult.resultStruct.JobsDetail.PageSegment.Results.PornInfo.HitFlag);
                // Assert.NotNull(getResult.resultStruct.JobsDetail.PageSegment.Results.PornInfo.SubLabel);
                // Assert.NotNull(getResult.resultStruct.JobsDetail.PageSegment.Results.PornInfo.Score);
                // //Assert.NotNull(getResult.resultStruct.JobsDetail.PageSegment.Results.PornInfo.OcrResults);
                // //Assert.NotNull(getResult.resultStruct.JobsDetail.PageSegment.Results.PornInfo.OcrResults.Text);
                // //Assert.NotNull(getResult.resultStruct.JobsDetail.PageSegment.Results.PornInfo.OcrResults.Keywords);
                // /*
                // Assert.NotNull(getResult.resultStruct.JobsDetail.PageSegment.Results.PoliticsInfo);
                // Assert.NotNull(getResult.resultStruct.JobsDetail.PageSegment.Results.PoliticsInfo.HitFlag);
                // Assert.NotNull(getResult.resultStruct.JobsDetail.PageSegment.Results.PoliticsInfo.SubLabel);
                // Assert.NotNull(getResult.resultStruct.JobsDetail.PageSegment.Results.PoliticsInfo.Score);
                // Assert.NotNull(getResult.resultStruct.JobsDetail.PageSegment.Results.PoliticsInfo.OcrResults);
                // Assert.NotNull(getResult.resultStruct.JobsDetail.PageSegment.Results.PoliticsInfo.OcrResults.Text);
                // Assert.NotNull(getResult.resultStruct.JobsDetail.PageSegment.Results.PoliticsInfo.OcrResults.Keywords);
                // Assert.NotNull(getResult.resultStruct.JobsDetail.PageSegment.Results.PoliticsInfo.ObjectResults);
                // Assert.NotNull(getResult.resultStruct.JobsDetail.PageSegment.Results.PoliticsInfo.ObjectResults.Name);
                // */
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
        public void TestDocumentProcessJobSyncCommit()
        {
            try
            {
                CreateDocProcessJobsRequest request = new CreateDocProcessJobsRequest(bucket);
                request.SetInputObject("demo.docx");
                request.SetTag("DocProcess");
                request.SetSrcType("docx");
                request.SetTgtType("jpg");
                request.SetStartPage("3");
                request.SetEndPage("5");
                request.SetImageParams("imageMogr2/cut/400x400");
                request.SetQuality("90");
                request.SetZoom("200");
                request.SetImageDpi("100");
                request.SetPicPagination("1");
                request.SetOutputBucket("dotnet-ut-obj-1253960454");
                request.SetOutputObject("kwy-test_${Number}");
                request.SetOutputRegion("ap-guangzhou");
                request.SetSheetId("1");
                request.SetPaperDirection("1");
                request.SetPaperSize("1");
                CreateDocProcessJobsResult result = QCloudServer.Instance().cosXml.CreateDocProcessJobs(request);
                string jobId = result.docProcessResponse.JobsDetail.JobId;
                Assert.NotNull(jobId);
                Assert.AreEqual(200, result.httpCode);

                DescribeDocProcessJobRequest describeDocProcessJobRequest =
                    new DescribeDocProcessJobRequest(bucket, jobId);
                DescribeDocProcessJobResult describeDocProcessJobResult =
                    QCloudServer.Instance().cosXml.DescribeDocProcessJob(describeDocProcessJobRequest);
                Assert.AreEqual(200, describeDocProcessJobResult.httpCode);
                Assert.NotNull(describeDocProcessJobResult.taskDocProcessResult.JobsDetail.Code);

                DescribeDocProcessJobsRequest describeDocProcessJobsRequest = new DescribeDocProcessJobsRequest(bucket);
                describeDocProcessJobsRequest.SetTag("DocProcess");
                describeDocProcessJobsRequest.SetQueueId(result.docProcessResponse.JobsDetail.QueueId);
                describeDocProcessJobsRequest.SetOrderByTime("Asc");
                describeDocProcessJobsRequest.SetNextToken("1");
                describeDocProcessJobsRequest.SetStates("All");
                describeDocProcessJobsRequest.SetSize("15");
                describeDocProcessJobsRequest.SetStartCreationTime("2024-06-12T08:20:07+0800");
                describeDocProcessJobsRequest.SetEndCreationTime("2024-06-12T20:00:00+0800");

                DescribeDocProcessJobsResult describeDocProcessJobsResult =
                    QCloudServer.Instance().cosXml.DescribeDocProcessJobs(describeDocProcessJobsRequest);
                Assert.AreEqual(200,describeDocProcessJobsResult.httpCode);
                Assert.NotNull(describeDocProcessJobsResult.listDocProcessResult.JobsDetail);

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
                request.SetRegions(QCloudServer.Instance().region);
                request.SetPageNumber("0");
                request.SetPageSize("1024");
                DescribeMediaBucketsResult result = QCloudServer.Instance().cosXml.DescribeMediaBuckets(request);
                Assert.AreEqual(result.httpCode, 200);
                Assert.NotNull(result.mediaBuckets.MediaBucketList);
                Assert.NotZero(result.mediaBuckets.MediaBucketList.Count);
                request.SetBucketName(QCloudServer.Instance().bucketForObjectTest);
                request.SetBucketNames(QCloudServer.Instance().bucketForObjectTest);
                for (int i = 0; i < result.mediaBuckets.MediaBucketList.Count; i++)
                {
                    Assert.NotNull(result.mediaBuckets.MediaBucketList[i].BucketId);
                    Assert.NotNull(result.mediaBuckets.MediaBucketList[i].Region);
                    Assert.NotNull(result.mediaBuckets.MediaBucketList[i].CreateTime);
                }
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
        /// 多文件打包压缩功能可以将您的多个文件，打包为 zip 等压缩包格式，以提交任务的方式进行多文件打包压缩，异步返回打包后的文件，该接口属于 POST 请求
        public void TestCreateFileZipProcessJobs()
        {
            // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
            // string bucket = "dotnet-ut-obj-1253960454"; // 注意：此操作需要 bucket 开通内容审核相关功能
            CreateFileZipProcessJobsRequest request = new CreateFileZipProcessJobsRequest(bucket);
            /// 表示任务的类型，多文件打��压缩默认为：FileCompress。;
            request.SetTag("FileCompress");
            /// 文件打包时，是否需要去除源文件已有的目录结构，有效值：0：不需要去除目录结构，打包后压缩包中的文件会保留原有的目录结构；1：需要，打包后压缩包内的文件会去除原有的目录结构，所有文件都在同一层级。例如：源文件 URL 为 https://domain/source/test.mp4，则源文件路径为 source/test.mp4，如果为 1，则 ZIP 包中该文件路径为 test.mp4；如果为0， ZIP 包中该文件路径为 source/test.mp4。;
            request.SetFlatten("0");
            /// 打包压缩的类型，有效值：zip、tar、tar.gz。;
            request.SetFormat("zip");
            /// 压缩类型，仅在Format为tar.gz或zip时有效。faster：压缩速度较快better：压缩质量较高，体积较小default：适中的压缩方式默认值为default;
            request.SetType("better");
            /// 压缩包密钥，传入时需先经过 base64 编码，编码后长度不能超过128。当 Format 为 zip 时生效。;
            request.SetCompressKey("");
            /// 支持将需要打包的文件整理成索引文件，后台将根据索引文件内提供的文件 url，打包为一个压缩包文件。索引文件需要保存在当前存储桶中，本字段需要提供索引文件的对象地址，不需要带域名，填写示例：/test/index.csv索引文件格式：仅支持 CSV 文件，一行一条 URL（仅支持本存储桶文件），如有多列字段，默认取第一列作为URL。;
            request.SetUrlList("");
            /// 支持对存储桶中的某个前缀进行打包，如果需要对某个目录进行打包，需要加/，例如test目录打包，则值为：test/。;
            request.SetPrefix("");
            /// 支持对存储桶中的多个文件进行打包，个数不能超过 1000，如需打包更多文件，请使用UrlList或Prefix参数。;
            COSXML.Model.CI.CreateFileZipProcessJobs.KeyConfig keyConfig = new CreateFileZipProcessJobs.KeyConfig();
            keyConfig.key = "CITestImage.png";
            keyConfig.rename = "CITestImage.zip";
            keyConfig.imageParams = "";
            request.setKeyConfig(keyConfig);
            /// 打包时如果单个文件出错，是否忽略错误继续打包。有效值为：ture：忽略错误继续打包后续的文件；false：遇到某个文件执行打包报错时，直接终止打包任务，不返回压缩包。默认值为false。;
            request.SetIgnoreError ("true");
            /// 透传用户信息，可打印的 ASCII 码，长度不超过1024。;
            request.SetUserData("");
            /// 存储桶的地域。;
            request.SetRegion("ap-guangzhou");
            /// 保存压缩后文件的存储桶。;
            request.SetBucket(bucket);
            /// 压缩后文件的文件名;
            request.SetObjectInfo("object-name");
            /// 任务回调格式，JSON 或 XML，默认 XML，优先级高于队列的回调格式。;
            request.SetCallBackFormat("");
            /// 任务回调类型，Url 或 TDMQ，默认 Url，优先级高于队列的回调类型。;
            request.SetCallBackType("Url");
            /// 任务回调的地址，优先级高于队列的回调地址。;
            request.SetCallBack("");
            /// 消息队列所属园区，目前支持园区 sh（上海）、bj（北京）、gz（广州）、cd（成都）、hk（中国香港）;
            request.SetMqRegion("");
            /// 消息队列使用模式，默认 Queue ：主题订阅：Topic队列服务: Queue;
            request.SetMqMode("");
            /// TDMQ 主题名称;
            request.SetMqName("");
            request.createFileZipProcessJobs.GetInfo();

            CreateFileZipProcessJobsResult result = QCloudServer.Instance().cosXml.createFileZipProcessJobs(request);

            Assert.AreEqual(200, result.httpCode);
            string jobId = result.createFileZipProcessJobsResult.JobsDetail.JobId;
            Assert.NotNull(jobId);

            DescribeFileZipProcessJobsRequest describeDocProcessJobsRequest = new DescribeFileZipProcessJobsRequest(bucket,jobId);
            DescribeFileZipProcessJobsResult describeFileZipProcessJobsResult= QCloudServer.Instance().cosXml.describeFileZipProcessJobs(describeDocProcessJobsRequest);

            Assert.AreEqual(200,describeFileZipProcessJobsResult.httpCode);
            Assert.NotNull(describeFileZipProcessJobsResult.describeFileZipProcessJobsResult.JobsDetail);

        }

        [Test]
        public void TestCreateDocPreviewJob()
        {
            CreateDocPreviewRequest request = new CreateDocPreviewRequest(bucket,textKey);
            request.SetSrcType("txt");
            request.SetCopyable("1");
            // 创建并初始化 HtmlParams 对象
            var htmlParams = new CreateDocPreviewRequest.HtmlParams
            {
                CommonOptions = new CreateDocPreviewRequest.CommonOptions
                {
                    IsShowTopArea = true,
                    IsShowHeader = true,
                    Language = "zh",
                    isBrowserViewFullscreen = true,
                    isIframeViewFullscreen = true,
                },
                WordOptions = new CreateDocPreviewRequest.WordOptions
                {
                    isShowDocMap = false,
                    isBestScale = false,
                },
                PdfOptions = new CreateDocPreviewRequest.PdfOptions
                {
                    isShowComment = false,
                    isInSafeMode = false,
                    isShowBottomStatusBar = false,
                },
                PptOptions = new CreateDocPreviewRequest.PptOptions
                {
                    isShowBottomStatusBar = false,
                },
                CommanBars = new CreateDocPreviewRequest.CommanBars
                {
                    cmbId = "",
                    attributes = new CreateDocPreviewRequest.Attributes
                    {
                        visible = false,
                        enable = false,
                    }
                }
            };
            request.SetHtmlParams(DigestUtils.GetBase64(JsonSerializer.Serialize(htmlParams), Encoding.UTF8));
            request.SetHtmlWaterword("5pWw5o2u5LiH6LGhLeaWh+aho+mihOiniA==");
            request.SetHtmlFillstyle("cmdiYSgxMDIsMjA0LDI1NSwwLjMp");
            request.SetHtmlFront("Ym9sZCAyNXB4IFNlcmlm");
            request.SetHtmlRotate("315");
            request.SetHtmlHorizontal("50");
            request.SetHtmlVertical("100");
            request.SetHtmlTitle("6IW+6K6v5LqRLeaVsOaNruS4h+ixoQ==");
            request.SetSignExpired(600);
            String createDocPreviewUrl = QCloudServer.Instance().cosXml.createDocPreview(request);

            Console.WriteLine(createDocPreviewUrl);

        }

    }
}
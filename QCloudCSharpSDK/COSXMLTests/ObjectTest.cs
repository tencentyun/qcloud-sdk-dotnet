using COSXML.Common;
using COSXML.CosException;
using COSXML.Model;
using COSXML.Model.Object;
using COSXML.Model.Tag;
using COSXML.Model.Bucket;
using COSXML.Utils;
using COSXML.Transfer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace COSXMLTests
{
    [TestFixture()]
    public class ObjectTest
    {

        internal COSXML.CosXml cosXml;

        internal TransferManager transferManager;

        internal string bucket;

        internal string bigFileSrcPath;

        internal string smallFileSrcPath;

        internal string commonKey;

        internal string imageKey;

        internal string copykey;

        internal string multiKey;

        internal string localDir;

        internal string localFileName;

        [SetUp]
        public void Init()
        {
            cosXml = QCloudServer.Instance().cosXml;
            bucket = QCloudServer.Instance().bucketForObjectTest;
            transferManager = new TransferManager(cosXml, new TransferConfig());
            smallFileSrcPath = QCloudServer.CreateFile(TimeUtils.GetCurrentTime(TimeUnit.Seconds) + ".txt", 1024 * 1024 * 1);
            bigFileSrcPath = QCloudServer.CreateFile(TimeUtils.GetCurrentTime(TimeUnit.Seconds) + ".txt", 1024 * 1024 * 10);
            FileInfo fileInfo = new FileInfo(smallFileSrcPath);

            DirectoryInfo directoryInfo = fileInfo.Directory;

            localDir = directoryInfo.FullName;
            localFileName = "local.txt";

            commonKey = "simpleObject" + TimeUtils.GetCurrentTime(TimeUnit.Seconds);
            multiKey = "bigObject" + TimeUtils.GetCurrentTime(TimeUnit.Seconds);
            copykey = commonKey;
            imageKey = commonKey;

            PutObject();
        }

        [TearDown()]
        public void Clear()
        {
            DeleteObject();
            MultiDeleteObject();

            QCloudServer.DeleteAllFile(localDir, "*.txt");
        }

        [Test()]
        public void PutObjectWithAES256()
        {

            try
            {
                string key = "objectWithAES256";

                PutObjectRequest request = new PutObjectRequest(bucket, key, smallFileSrcPath);


                request.SetCosServerSideEncryption();

                //执行请求
                PutObjectResult result = cosXml.PutObject(request);


                Console.WriteLine(result.GetResultInfo());
            }
            catch (CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.StackTrace);
                Assert.True(false);
            }
            catch (CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.True(false);
            }
        }

        [Test()]
        public void PutObjectWithCustomerKey()
        {

            try
            {
                string key = "objectWithSSEC";

                PutObjectRequest request = new PutObjectRequest(bucket, key, smallFileSrcPath);

                //32字符
                //32字符
                string customerKey = "25rN73uQtl1bUGnvHe0URgFWBNu4vBba";

                request.SetCosServerSideEncryptionWithCustomerKey(customerKey);

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

        public void PutObjectWithKMS()
        {

            try
            {
                string key = "objectWithKMS";

                PutObjectRequest request = new PutObjectRequest(bucket, key, smallFileSrcPath);


                request.SetCosServerSideEncryptionWithKMS(null, null);

                //执行请求
                PutObjectResult result = cosXml.PutObject(request);


                Console.WriteLine(result.GetResultInfo());
            }
            catch (CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.Message);
                //Assert.True(false);
            }
            catch (CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                //Assert.True(true);
            }
        }

        [Test()]
        public void PutObject()
        {

            try
            {
                PutObjectRequest request = new PutObjectRequest(bucket, commonKey, smallFileSrcPath);


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

        [Test()]
        public void PutObjectBytes()
        {

            try
            {
                byte[] data = File.ReadAllBytes(smallFileSrcPath);

                PutObjectRequest request = new PutObjectRequest(bucket, commonKey, data);


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

        [Test()]
        public void HeadObject()
        {

            try
            {
                HeadObjectRequest request = new HeadObjectRequest(bucket, commonKey);


                //执行请求
                HeadObjectResult result = cosXml.HeadObject(request);


                Console.WriteLine(result.GetResultInfo());
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.StackTrace);
                Assert.True(false);
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.True(false);
            }
        }

        [Test()]
        public void PutObjectACL()
        {

            try
            {
                PutObjectACLRequest request = new PutObjectACLRequest(bucket, commonKey);


                //添加acl
                request.SetCosACL(CosACL.Private);

                COSXML.Model.Tag.GrantAccount readAccount = new COSXML.Model.Tag.GrantAccount();

                readAccount.AddGrantAccount("1131975903", "1131975903");
                request.SetXCosGrantRead(readAccount);

                COSXML.Model.Tag.GrantAccount fullAccount = new COSXML.Model.Tag.GrantAccount();

                fullAccount.AddGrantAccount("2832742109", "2832742109");
                request.SetXCosReadWrite(fullAccount);

                //执行请求
                PutObjectACLResult result = cosXml.PutObjectACL(request);


                Console.WriteLine(result.GetResultInfo());
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.StackTrace);
                Assert.True(false);
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.True(false);
            }
        }

        [Test()]
        public void GetObjectACL()
        {

            try
            {
                GetObjectACLRequest request = new GetObjectACLRequest(bucket, commonKey);

                //执行请求
                GetObjectACLResult result = cosXml.GetObjectACL(request);


                Console.WriteLine(result.GetResultInfo());
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.StackTrace);
                Assert.True(false);
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.True(false);
            }
        }

        [Test()]
        public void OptionObject()
        {

            try
            {
                string origin = "http://cloud.tencent.com";
                string accessMthod = "PUT";

                OptionObjectRequest request = new OptionObjectRequest(bucket, commonKey, origin, accessMthod);


                //执行请求
                OptionObjectResult result = cosXml.OptionObject(request);


                Console.WriteLine(result.GetResultInfo());
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.StackTrace);
                Assert.True(false);
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.True(false);
            }
        }

        [Test()]
        public void CopyObject()
        {

            try
            {
                CopySourceStruct copySource = new CopySourceStruct(QCloudServer.Instance().appid,
                    bucket, QCloudServer.Instance().region, "copy_objecttest.txt");

                string tempBucket = "a-bucket-for-temp" + TimeUtils.GetCurrentTime(TimeUnit.Seconds);


                PutBucketRequest request = new PutBucketRequest(tempBucket);


                //执行请求
                PutBucketResult putBucketResult = cosXml.PutBucket(request);


                Console.WriteLine(putBucketResult.GetResultInfo());

                CopyObjectRequest copyObjectRequest = new CopyObjectRequest(tempBucket, multiKey);


                //设置拷贝源
                copyObjectRequest.SetCopySource(copySource);

                //执行请求
                CopyObjectResult result = cosXml.CopyObject(copyObjectRequest);


                Console.WriteLine(result.GetResultInfo());
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.StackTrace);
                Assert.True(false);
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.True(false);
            }
        }

        [Test()]
        public void MultiUpload()
        {
            string key = multiKey;


            try
            {
                InitMultipartUploadRequest initMultipartUploadRequest = new InitMultipartUploadRequest(bucket, multiKey);


                //执行请求
                InitMultipartUploadResult initMultipartUploadResult = cosXml.InitMultipartUpload(initMultipartUploadRequest);


                Console.WriteLine(initMultipartUploadResult.GetResultInfo());

                string uploadId = initMultipartUploadResult.initMultipartUpload.uploadId;


                ListPartsRequest listPartsRequest = new ListPartsRequest(bucket, key, uploadId);


                //执行请求
                ListPartsResult listPartsResult = cosXml.ListParts(listPartsRequest);


                Console.WriteLine(listPartsResult.GetResultInfo());

                int partNumber = 1;


                UploadPartRequest uploadPartRequest = new UploadPartRequest(bucket, key, partNumber, uploadId, bigFileSrcPath);


                //设置进度回调
                uploadPartRequest.SetCosProgressCallback(
                    delegate (long completed, long total)
                    {
                        Console.WriteLine(String.Format("{0} progress = {1} / {2} : {3:##.##}%",
                            DateTime.Now.ToString(), completed, total, completed * 100.0 / total));
                    }
                );

                //执行请求
                UploadPartResult uploadPartResult = cosXml.UploadPart(uploadPartRequest);


                Console.WriteLine(uploadPartResult.GetResultInfo());

                string eTag = uploadPartResult.eTag;

                CompleteMultipartUploadRequest completeMultiUploadRequest = new CompleteMultipartUploadRequest(bucket, key, uploadId);


                //设置已上传的parts
                completeMultiUploadRequest.SetPartNumberAndETag(partNumber, eTag);

                //执行请求
                CompleteMultipartUploadResult completeMultiUploadResult = cosXml.CompleteMultiUpload(completeMultiUploadRequest);


                Console.WriteLine(completeMultiUploadResult.GetResultInfo());

            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.StackTrace);
                Assert.True(false);
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.True(false);
            }
        }

        [Test()]
        public void AbortMultiUpload()
        {
            string key = multiKey;


            try
            {
                InitMultipartUploadRequest initMultipartUploadRequest = new InitMultipartUploadRequest(bucket, key);


                //执行请求
                InitMultipartUploadResult initMultipartUploadResult = cosXml.InitMultipartUpload(initMultipartUploadRequest);


                Console.WriteLine(initMultipartUploadResult.GetResultInfo());

                string uploadId = initMultipartUploadResult.initMultipartUpload.uploadId;


                AbortMultipartUploadRequest request = new AbortMultipartUploadRequest(bucket, key, uploadId);



                //执行请求
                AbortMultipartUploadResult result = cosXml.AbortMultiUpload(request);


                Console.WriteLine(result.GetResultInfo());
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.Message);
                Assert.True(false);
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.True(false);
            }
        }

        [Test()]
        public void PartCopyObject()
        {
            string key = commonKey;
            CopySourceStruct copySource = new CopySourceStruct(QCloudServer.Instance().appid,
                bucket, QCloudServer.Instance().region, copykey);


            try
            {
                InitMultipartUploadRequest initMultipartUploadRequest = new InitMultipartUploadRequest(bucket, key);


                //执行请求
                InitMultipartUploadResult initMultipartUploadResult = cosXml.InitMultipartUpload(initMultipartUploadRequest);


                Console.WriteLine(initMultipartUploadResult.GetResultInfo());

                string uploadId = initMultipartUploadResult.initMultipartUpload.uploadId;

                int partNumber = 1;


                UploadPartCopyRequest uploadPartCopyRequest = new UploadPartCopyRequest(bucket, key, partNumber, uploadId);


                //设置拷贝源
                uploadPartCopyRequest.SetCopySource(copySource);

                //设置拷贝范围
                uploadPartCopyRequest.SetCopyRange(0, 10);

                //执行请求
                UploadPartCopyResult uploadPartCopyResult = cosXml.PartCopy(uploadPartCopyRequest);


                Console.WriteLine(uploadPartCopyResult.GetResultInfo());

                string eTag = uploadPartCopyResult.copyObject.eTag;

                CompleteMultipartUploadRequest completeMultiUploadRequest = new CompleteMultipartUploadRequest(bucket, key, uploadId);


                //设置已上传的parts
                completeMultiUploadRequest.SetPartNumberAndETag(partNumber, eTag);

                //执行请求
                CompleteMultipartUploadResult completeMultiUploadResult = cosXml.CompleteMultiUpload(completeMultiUploadRequest);


                Console.WriteLine(completeMultiUploadResult.GetResultInfo());
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.Message);
                Assert.True(false);
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.True(false);
            }
        }

        public void RestoreObject()
        {

            try
            {
                RestoreObjectRequest request = new RestoreObjectRequest(bucket, commonKey);

                //恢复时间
                request.SetExpireDays(3);
                request.SetTier(COSXML.Model.Tag.RestoreConfigure.Tier.Bulk);

                //执行请求
                RestoreObjectResult result = cosXml.RestoreObject(request);


                Console.WriteLine(result.GetResultInfo());
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.Message);
                Assert.True(false);
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.True(false);
            }
        }

        [Test()]
        public void PostObject()
        {

            try
            {
                PostObjectRequest request = new PostObjectRequest(bucket, commonKey, smallFileSrcPath);
                List<string> headers = new List<string>();

                headers.Add("Host");

                request.SetCosProgressCallback(delegate (long completed, long total)
                {
                    Console.WriteLine(String.Format("progress = {0} / {1} : {2:##.##}%", completed, total, completed * 100.0 / total));
                });

                //设置policy
                request.SetPolicy(null);

                //执行请求
                PostObjectResult result = cosXml.PostObject(request);


                Console.WriteLine(result.GetResultInfo());
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.Message);
                Assert.True(false);
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.True(false);
            }
        }

        public void DeleteObject()
        {

            try
            {
                DeleteObjectRequest request = new DeleteObjectRequest(bucket, commonKey);


                //执行请求
                DeleteObjectResult result = cosXml.DeleteObject(request);


                Console.WriteLine(result.GetResultInfo());
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.Message);
                Assert.True(false);
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.True(false);
            }
        }

        public void MultiDeleteObject()
        {

            try
            {
                DeleteMultiObjectRequest request = new DeleteMultiObjectRequest(bucket);


                //设置返回结果形式
                request.SetDeleteQuiet(false);

                //设置删除的keys
                List<string> keys = new List<string>();

                keys.Add(commonKey);
                keys.Add(multiKey);
                request.SetObjectKeys(keys);

                //执行请求
                DeleteMultiObjectResult result = cosXml.DeleteMultiObjects(request);


                Console.WriteLine(result.GetResultInfo());
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.Message);
                Assert.True(false);
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.True(false);
            }
        }

        [Test()]
        public void TestCreateDirectory()
        {

            try
            {
                PutObjectRequest request = new PutObjectRequest(bucket,
                    "dir/", new byte[0]);


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

        [Test()]
        public void TestGetObject()
        {

            try
            {
                GetObjectRequest request = new GetObjectRequest(bucket, commonKey, localDir, localFileName);


                request.SetCosProgressCallback(delegate (long completed, long total)
                {
                    Console.WriteLine(String.Format("progress = {0} / {1} : {2:##.##}%", completed, total, completed * 100.0 / total));
                });

                //执行请求
                GetObjectResult result = cosXml.GetObject(request);

                Console.WriteLine(result.GetResultInfo());
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.Message);
                Assert.True(false);
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.True(false);
            }

        }

        [Test()]
        public void TestGetObjectByte()
        {

            try
            {
                HeadObjectRequest request = new HeadObjectRequest(bucket, commonKey);

                //执行请求
                HeadObjectResult result = cosXml.HeadObject(request);


                long contentLength = Int64.Parse(result.responseHeaders["Content-Length"][0]);

                GetObjectBytesRequest getObjectBytesRequest = new GetObjectBytesRequest(bucket, commonKey);


                getObjectBytesRequest.SetCosProgressCallback(delegate (long completed, long total)
                {
                    Console.WriteLine(String.Format("{0} progress = {1} / {2} : {3:##.##}%", DateTime.Now.ToString(), completed, total, completed * 100.0 / total));
                });

                GetObjectBytesResult getObjectBytesResult = cosXml.GetObject(getObjectBytesRequest);


                byte[] content = getObjectBytesResult.content;


                Assert.AreEqual(content.Length, contentLength);

            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx);
                Assert.True(false);
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.True(false);
            }
        }

        [Test()]
        public void TestSelectObjectToFile()
        {

            try
            {
                string key = "select_target.json";


                SelectObjectRequest request = new SelectObjectRequest(bucket, key);

                ObjectSelectionFormat.JSONFormat jSONFormat = new ObjectSelectionFormat.JSONFormat();

                jSONFormat.Type = "DOCUMENT";
                jSONFormat.RecordDelimiter = "\n";

                string outputFile = "select_local_file.json";


                request.SetExpression("Select * from COSObject")
                        .SetInputFormat(new ObjectSelectionFormat(null, jSONFormat))
                        .SetOutputFormat(new ObjectSelectionFormat(null, jSONFormat))
                        .SetCosProgressCallback(delegate (long progress, long total)
                        {
                            Console.WriteLine("OnProgress : " + progress + "," + total);
                        })
                        .OutputToFile(outputFile)
                        ;

                SelectObjectResult selectObjectResult = cosXml.SelectObject(request);


                Console.WriteLine(selectObjectResult.stat);

                Assert.AreEqual(selectObjectResult.stat.BytesReturned, new FileInfo(outputFile).Length);
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.StackTrace);
                Console.WriteLine("CosClientException: " + clientEx.Message);
                Assert.True(false);
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.True(false);
            }
        }

        [Test()]
        public void TestSelectObjectInMemory()
        {

            try
            {
                string key = "select_target.json";


                SelectObjectRequest request = new SelectObjectRequest(bucket, key);

                ObjectSelectionFormat.JSONFormat jSONFormat = new ObjectSelectionFormat.JSONFormat();

                jSONFormat.Type = "DOCUMENT";
                jSONFormat.RecordDelimiter = "\n";

                request.SetExpression("Select * from COSObject")
                        .SetInputFormat(new ObjectSelectionFormat(null, jSONFormat))
                        .SetOutputFormat(new ObjectSelectionFormat(null, jSONFormat))
                        .SetCosProgressCallback(delegate (long progress, long total)
                        {
                            Console.WriteLine("OnProgress : " + progress + "," + total);
                        })
                        ;

                SelectObjectResult selectObjectResult = cosXml.SelectObject(request);


                Console.WriteLine(selectObjectResult.stat);

                Assert.AreEqual(selectObjectResult.stat.BytesReturned, Encoding.UTF8.GetByteCount(
                    selectObjectResult.searchContent));
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.StackTrace);
                Console.WriteLine("CosClientException: " + clientEx.Message);
                Assert.True(false);
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.True(false);
            }
        }

        [Test()]
        public async Task TestUploadTask()
        {
            string key = multiKey;


            PutObjectRequest request = new PutObjectRequest(bucket, key, bigFileSrcPath);

            request.SetRequestHeader("Content-Type", "image/png");

            COSXMLUploadTask uploadTask = new COSXMLUploadTask(request);

            uploadTask.SetSrcPath(bigFileSrcPath);

            string eTag = null;

            uploadTask.progressCallback = delegate (long completed, long total)
            {
                Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
            };

            COSXMLUploadTask.UploadTaskResult result = await transferManager.UploadAsync(uploadTask);

            eTag = result.eTag;
            Assert.NotNull(eTag);
        }

        [Test()]
        public async Task TestDownloadTask()
        {
            long now = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
            GetObjectRequest request = new GetObjectRequest(bucket,
                commonKey, localDir, localFileName);


            request.LimitTraffic(8 * 1000 * 1024);
            //执行请求
            COSXMLDownloadTask downloadTask = new COSXMLDownloadTask(request);


            downloadTask.progressCallback = delegate (long completed, long total)
            {
                Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
            };

            long costTime = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds() - now;

            COSXMLDownloadTask.DownloadTaskResult result = await transferManager.DownloadAsync(downloadTask);

            Assert.True(result.httpCode == 200);
        }

        [Test()]
        public async Task TestCopyTask()
        {
            CopySourceStruct copySource = new CopySourceStruct(QCloudServer.Instance().appid,
                    bucket, QCloudServer.Instance().region, copykey);


            COSXMLCopyTask copyTask = new COSXMLCopyTask(bucket, multiKey, copySource);

            var autoEvent = new AutoResetEvent(false);

            copyTask.progressCallback = delegate (long completed, long total)
            {
                Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
            };

            COSXMLCopyTask.CopyTaskResult result = await transferManager.CopyAsync(copyTask);

            Assert.True(result.httpCode == 200);
        }

        [Test()]
        public void TestPutObjectUploadTrafficLimit()
        {

            try
            {
                long now = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
                PutObjectRequest request = new PutObjectRequest(bucket,
                    commonKey, smallFileSrcPath);


                request.LimitTraffic(8 * 1000 * 1024);
                //执行请求
                PutObjectResult result = cosXml.PutObject(request);
                long costTime = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds() - now;


                Console.WriteLine("costTime = " + costTime + "ms");
                Console.WriteLine(result.GetResultInfo());

                Assert.True(result.httpCode == 200);
                // Assert.True(costTime > 8000 && costTime < 14000);
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.Message);
                Assert.True(false);
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.True(false);
            }

        }

        [Test()]
        public void TestPostObjectTrafficLimit()
        {

            try
            {
                long now = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
                PostObjectRequest request = new PostObjectRequest(bucket,
                    commonKey, smallFileSrcPath);


                request.LimitTraffic(8 * 1000 * 1024);
                //执行请求
                PostObjectResult result = cosXml.PostObject(request);
                long costTime = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds() - now;


                Console.WriteLine("costTime = " + costTime + "ms");
                Console.WriteLine(result.GetResultInfo());

                Assert.True(result.httpCode == 204);
                // Assert.True(costTime > 8000 && costTime < 14000);
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.Message);
                Assert.True(false);
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.True(false);
            }
        }

        [Test()]
        public void GenerateSignUrl()
        {
            QCloudServer instance = QCloudServer.Instance();
            string key = commonKey;
            PreSignatureStruct signatureStruct = new PreSignatureStruct();

            signatureStruct.bucket = instance.bucketForObjectTest;
            signatureStruct.appid = instance.appid;
            signatureStruct.region = instance.region;
            signatureStruct.key = key;
            signatureStruct.httpMethod = "GET";
            signatureStruct.headers = new Dictionary<string, string>();
            string url = instance.cosXml.GenerateSignURL(signatureStruct);

            Console.WriteLine(url);
            Assert.NotNull(url);
        }

        [Test()]
        public async Task AsyncPutObject()
        {
            PutObjectRequest request = new PutObjectRequest(bucket, commonKey, smallFileSrcPath);


            PutObjectResult result = await cosXml.ExecuteAsync<PutObjectResult>(request);


            Assert.NotNull(result.GetResultInfo());
        }

        [Test()]
        public async Task TestUploadTaskWithError()
        {
            string key = multiKey;


            PutObjectRequest request = new PutObjectRequest("3838" + bucket, key, bigFileSrcPath);


            COSXMLUploadTask uploadTask = new COSXMLUploadTask(request);

            uploadTask.SetSrcPath(bigFileSrcPath);

            try
            {
                COSXMLUploadTask.UploadTaskResult result = await transferManager.UploadAsync(uploadTask);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Console.WriteLine("CosException: " + e);
                Assert.NotNull(e);
            }
        }
    }
}

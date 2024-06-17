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
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace COSXMLTests
{
    [TestFixture]
    public class ObjectTest
    {

        internal COSXML.CosXml cosXml;

        internal TransferManager transferManager;

        internal string bucket;

        internal string bigFileSrcPath;

        internal string smallFileSrcPath;

        internal string commonKey;

        internal string copykey;

        internal string bigCopyKey;

        internal string copySourceFilePath;

        internal string bigCopySourceFilePath;
        
        internal string copyKeySmall;

        internal string multiKey;

        internal string selectKey;

        internal string selectFilePath;

        internal string streamUploadFilePath;

        internal string appendKey;

        internal string localDir;

        internal string localFileName;

        [OneTimeSetUp]
        public void Init()
        {
            try
            {
                cosXml = QCloudServer.Instance().cosXml;
                bucket = QCloudServer.Instance().bucketForObjectTest;
                transferManager = new TransferManager(cosXml, new TransferConfig());
                long currentTime = TimeUtils.GetCurrentTime(TimeUnit.Seconds);
                smallFileSrcPath = QCloudServer.CreateFile("small_" + currentTime + ".txt", 1024 * 1024 * 1);
                bigFileSrcPath = QCloudServer.CreateFile("big_" + currentTime + ".txt", 1024 * 1024 * 10);
                streamUploadFilePath = QCloudServer.CreateFile("streaming_" + currentTime + ".txt", 1024 * 1024 * 10);
                FileInfo fileInfo = new FileInfo(smallFileSrcPath);

                DirectoryInfo directoryInfo = fileInfo.Directory;

                localDir = directoryInfo.FullName;
                localFileName = "local.txt";

                commonKey = "simpleObject" + currentTime;
                multiKey = "bigObject" + currentTime;
                copykey = "copy_objecttest.txt";
                copySourceFilePath = QCloudServer.CreateFile(copykey, 1024 * 1024 * 1);
                bigCopyKey = "copy_objecttext_big.txt";
                bigCopySourceFilePath = QCloudServer.CreateFile(bigCopyKey, 10 * 1024 * 1024);
                copyKeySmall = "copy_target";
                selectKey = "select_target.json";
                selectFilePath = QCloudServer.CreateJsonFile(selectKey);
                appendKey = "appendableObject";

                CreateBucketsIfNotExist(bucket);
                CreateBucketsIfNotExist(QCloudServer.Instance().bucketForLoggingTarget);
                CreateBucketsIfNotExist(QCloudServer.Instance().bucketVersioning);
                PutObject();
                MultiUpload();
            }
            catch (CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.Message);
                Assert.Fail();
            }
            catch (CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.Fail();
            }
        }

        [OneTimeTearDown]
        public void Clear()
        {
            MultiDeleteObject();

            QCloudServer.DeleteAllFile(localDir, "*.txt");
        }


        [Test()]
        public void TransferManagerObjectTest()
        {
            cosXml = QCloudServer.Instance().cosXml;
            bucket = QCloudServer.Instance().bucketForObjectTest;
            try {
                TransferManager transferM = new TransferManager(null, new TransferConfig());
            } catch(ArgumentNullException) {
                ;
            }
            try {
                TransferManager transferM = new TransferManager(cosXml, null);
            } catch(ArgumentNullException) {
                ;
            }
            
        }
            
        [Test()]
        public void AppendObject() 
        {
            try
            {
                string key = appendKey;
                AppendObjectRequest request = new AppendObjectRequest(bucket, key, smallFileSrcPath, 0);
                AppendObjectResult result = QCloudServer.Instance().cosXml.AppendObject(request);

                Assert.AreEqual(result.httpCode, 200);
                Assert.NotNull(result.nextAppendPosition);
                
                request = new AppendObjectRequest(bucket, key, smallFileSrcPath, result.nextAppendPosition);
                result = QCloudServer.Instance().cosXml.AppendObject(request);
                Assert.AreEqual(result.httpCode, 200);
                Assert.AreEqual(result.nextAppendPosition, 1024 * 1024 * 1 * 2);

                request = new AppendObjectRequest(bucket, key, smallFileSrcPath, result.nextAppendPosition, 0L, 1024L);
                result = QCloudServer.Instance().cosXml.AppendObject(request);
                Assert.AreEqual(result.httpCode, 200);
                Assert.NotNull(result.nextAppendPosition);

                byte[] data = new byte[] { 0x01, 0x02, 0x03 };
                request = new AppendObjectRequest(bucket, key, data, result.nextAppendPosition);
                request.SetCosProgressCallback(null);
                request.SetCosStorageClass("STANDARD");
                request.SetCosACL("private");
                request.LimitTraffic(1024 * 1024);
                request.SetCosACL(CosACL.Private);
                GrantAccount grantAccount = new GrantAccount();
                grantAccount.AddGrantAccount("111111", "111111");
                request.SetXCosGrantRead(grantAccount);
                request.SetXCosReadWrite(grantAccount);
                result = QCloudServer.Instance().cosXml.AppendObject(request);
                Assert.AreEqual(result.httpCode, 200);
                Assert.NotNull(result.nextAppendPosition);
            }
            catch (CosClientException clientEx)
            {
                Assert.Fail();
            }
            catch (CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.Fail();
            }
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


                Assert.True(result.httpCode == 200);
                Assert.NotNull(result.eTag);
            }
            catch (CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.StackTrace);
                Assert.Fail();
            }
            catch (CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.Fail();
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
                string customerKey = "25rN73uQtl1bUGnvHe0URgFWBNu4vBba";

                request.SetCosServerSideEncryptionWithCustomerKey(customerKey);

                //执行请求
                PutObjectResult result = cosXml.PutObject(request);


                Assert.True(result.httpCode == 200);
                Assert.NotNull(result.eTag);

                GetObjectRequest getObjectRequest = new GetObjectRequest(bucket, key, localDir, localFileName);
                getObjectRequest.SetCosServerSideEncryptionWithCustomerKey(customerKey);
                var getResult = cosXml.GetObject(getObjectRequest);
                Assert.True(getResult.httpCode == 200);
                FileInfo fileInfo = new FileInfo(localDir + System.IO.Path.DirectorySeparatorChar + localFileName);
                Assert.AreEqual(fileInfo.Length, new FileInfo(smallFileSrcPath).Length);
            }
            catch (CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.Message);
                Assert.Fail();
            }
            catch (CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.Fail();
            }
        }

        [Test()]
        public void PutObjectWithKMS()
        {

            try
            {
                string key = "objectWithKMS";

                PutObjectRequest request = new PutObjectRequest(bucket, key, smallFileSrcPath);

                request.SetCosServerSideEncryptionWithKMS(null, null);

                //执行请求
                PutObjectResult result = cosXml.PutObject(request);

                Assert.True(result.httpCode == 200);
                Assert.NotNull(result.eTag);
            }
            catch (CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.Message);
                //Assert.Fail();
            }
            catch (CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                //Assert.True(true);
            }
        }

        public void CreateBucketsIfNotExist(string bucket)
        {
            try
            {
                HeadBucketRequest request = new HeadBucketRequest(bucket);
                HeadBucketResult result = cosXml.HeadBucket(request);
                Assert.AreEqual(200, result.httpCode);
            }
            catch (CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.Message);
                Assert.Fail();
            }
            catch (CosServerException serverEx)
            {
                if (serverEx.statusCode == 404) {
                    PutBucketRequest request = new PutBucketRequest(bucket);
                    PutBucketResult result = cosXml.PutBucket(request);
                    Assert.AreEqual(200, result.httpCode);
                    return;
                } else {
                    Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                    Assert.Fail();
                }
            }
        }

        public void PutObject()
        {
            try
            {
                bucket = QCloudServer.Instance().bucketForObjectTest;
                PutObjectRequest request = new PutObjectRequest(bucket, commonKey, smallFileSrcPath);
                QCloudServer.SetRequestACLData(request);
                //执行请求
                PutObjectResult result = cosXml.PutObject(request);

                Console.WriteLine(result.GetResultInfo());
                Assert.AreEqual(200, result.httpCode);
                Assert.NotNull(result.eTag);
                Assert.True(COSXML.Utils.Crc64.CompareCrc64(smallFileSrcPath, result.crc64ecma));
                //Put Copy 测试的 Source Object
                request = new PutObjectRequest(bucket, copykey, copySourceFilePath);
                result = cosXml.PutObject(request);

                Console.WriteLine(result.GetResultInfo());
                Assert.AreEqual(200, result.httpCode);
                Assert.NotNull(result.eTag);
                Assert.True(COSXML.Utils.Crc64.CompareCrc64(copySourceFilePath, result.crc64ecma));

                string fileCrc = COSXML.Utils.Crc64.GetFileCrc64(copySourceFilePath);
                Assert.AreEqual(fileCrc, result.crc64ecma);

                //Put Copy 测试的大 Source Object
                request = new PutObjectRequest(bucket, bigCopyKey, bigCopySourceFilePath);
                result = cosXml.PutObject(request);

                Console.WriteLine(result.GetResultInfo());
                Assert.AreEqual(200, result.httpCode);
                Assert.NotNull(result.eTag);
                Assert.True(COSXML.Utils.Crc64.CompareCrc64(bigCopySourceFilePath, result.crc64ecma));

                request = new PutObjectRequest(bucket, copyKeySmall, copySourceFilePath);
                result = cosXml.PutObject(request);

                //Select 测试的 json 文件
                request = new PutObjectRequest(bucket, selectKey, selectFilePath);
                result = cosXml.PutObject(request);

                Console.WriteLine(result.GetResultInfo());
                Assert.AreEqual(200, result.httpCode);
                Assert.NotNull(result.eTag);
                Assert.True(COSXML.Utils.Crc64.CompareCrc64(selectFilePath, result.crc64ecma));
            
                request = new PutObjectRequest(bucket, copyKeySmall, copySourceFilePath);
                result = cosXml.PutObject(request);
            
                Console.WriteLine(result.GetResultInfo());
                Assert.AreEqual(200, result.httpCode);
                Assert.NotNull(result.eTag);
                Assert.True(COSXML.Utils.Crc64.CompareCrc64(copySourceFilePath, result.crc64ecma));
            
                request = new PutObjectRequest(bucket, copyKeySmall, copySourceFilePath);
                result = cosXml.PutObject(request);
            
                Console.WriteLine(result.GetResultInfo());
                Assert.AreEqual(200, result.httpCode);
                Assert.NotNull(result.eTag);
                Assert.True(COSXML.Utils.Crc64.CompareCrc64(copySourceFilePath, result.crc64ecma));
            }
            catch (CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.Message);
                Assert.Fail();
            }
            catch (CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.Fail();
            }
        }

        
        //大文件上传
        public string PutObjectBigFile()
        {        
            bucket = QCloudServer.Instance().bucketForObjectTest;
            if (!File.Exists(bigCopySourceFilePath))
            {
                bigCopySourceFilePath = SystemUtils.CreateFileByParam(".", bigCopyKey, 30 * 1024 * 1024);
            }
            PutObjectRequest request = new PutObjectRequest(bucket, bigCopyKey, bigCopySourceFilePath);
            PutObjectResult result = cosXml.PutObject(request);
            Assert.AreEqual(200, result.httpCode);
            Assert.True(COSXML.Utils.Crc64.CompareCrc64(bigCopySourceFilePath, result.crc64ecma));
            Assert.NotNull(result.eTag);
            return bigCopyKey;
        }

        public string PutObjectSmallFile()
        {
            bucket = QCloudServer.Instance().bucketForObjectTest;
            PutObjectRequest request = new PutObjectRequest(bucket, commonKey, smallFileSrcPath);
            QCloudServer.SetRequestACLData(request);
            //执行请求
            PutObjectResult result = cosXml.PutObject(request);

            Console.WriteLine(result.GetResultInfo());
            Assert.AreEqual(200, result.httpCode);
            Assert.NotNull(result.eTag);
            Assert.True(COSXML.Utils.Crc64.CompareCrc64(smallFileSrcPath, result.crc64ecma));
            return commonKey;
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


                Assert.True(result.httpCode == 200);
                Assert.NotNull(result.eTag);
            }
            catch (CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.Message);
                Assert.Fail();
            }
            catch (CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.Fail();
            }
        }

        [Test()]
        public void PutObjectStreamTest()
        {
            try
            {
                FileStream fileStream = new FileStream(streamUploadFilePath, FileMode.Open, FileAccess.Read);
                PutObjectRequest request = new PutObjectRequest(bucket, commonKey, fileStream);
                PutObjectResult result = cosXml.PutObject(request);
                fileStream.Close();
                //确定长度ok, crc64ok, 下载下来对比ok
                FileInfo info = new FileInfo(streamUploadFilePath);
                HeadObjectRequest headObjectRequest = new HeadObjectRequest(bucket, commonKey);
                HeadObjectResult headObjectResult = cosXml.HeadObject(headObjectRequest);
                headObjectRequest.SetVersionId("null");
                Assert.AreEqual(headObjectResult.size, info.Length);
                Assert.True(COSXML.Utils.Crc64.CompareCrc64(streamUploadFilePath, headObjectResult.crc64ecma));
            }
            catch (CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.Message);
                Assert.Fail();
            }
            catch (CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.Fail();
            }
        }

        [Test()]
        public void PutObjectStreamOffsetTest()
        {
            try
            {
                FileStream fileStream = new FileStream(streamUploadFilePath, FileMode.Open, FileAccess.Read);
                long offset = 1 * 1024 * 1024L;
                PutObjectRequest request = new PutObjectRequest(bucket, commonKey, fileStream, offset);
                PutObjectResult result = cosXml.PutObject(request);
                fileStream.Close();
                //确定长度ok, crc64ok, 下载下来对比ok
                FileInfo info = new FileInfo(streamUploadFilePath);
                HeadObjectRequest headObjectRequest = new HeadObjectRequest(bucket, commonKey);
                HeadObjectResult headObjectResult = cosXml.HeadObject(headObjectRequest);
                Assert.AreEqual(headObjectResult.size + offset, info.Length);
            }
            catch (CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.Message);
                Assert.Fail();
            }
            catch (CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.Fail();
            }
        }

        [Test()]
        public void PutObjectStreamContentLengthTest()
        {
            try
            {
                FileStream fileStream = new FileStream(streamUploadFilePath, FileMode.Open, FileAccess.Read);
                long offset = 1 * 1024 * 1024L;
                long contetnLength = 1 * 1024 * 1024L;
                PutObjectRequest request = new PutObjectRequest(bucket, commonKey, fileStream, offset, contetnLength);
                PutObjectResult result = cosXml.PutObject(request);
                fileStream.Close();
                //确定长度ok, crc64ok, 下载下来对比ok
                FileInfo info = new FileInfo(streamUploadFilePath);
                HeadObjectRequest headObjectRequest = new HeadObjectRequest(bucket, commonKey);
                HeadObjectResult headObjectResult = cosXml.HeadObject(headObjectRequest);
                Assert.AreEqual(headObjectResult.size, contetnLength);
            }
            catch (CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.Message);
                Assert.Fail();
            }
            catch (CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.Fail();
            }
        }

        [Test()]
        public void InvalidPutObjectStreamTest()
        {
            try
            {
                FileStream fileStream = new FileStream(streamUploadFilePath, FileMode.Open, FileAccess.Write);
                PutObjectRequest request = new PutObjectRequest(bucket, commonKey, fileStream);
                PutObjectResult result = cosXml.PutObject(request);
                Assert.Fail();
            }
            catch (CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.Message);
                Assert.Pass();
            }
            catch (CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.Pass();
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


                Assert.True(result.httpCode == 200);
                Assert.Null(result.cosStorageClass);
                Assert.NotZero(result.size);
                Assert.NotNull(result.eTag);
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.StackTrace);
                Assert.Fail();
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.Fail();
            }
        }

        [Test()]
        public void TestObjectACL()
        {

            try
            {
                PutObjectACLRequest request = new PutObjectACLRequest(bucket, commonKey);

                QCloudServer.SetRequestACLData(request);

                //执行请求
                PutObjectACLResult result = cosXml.PutObjectACL(request);

                Assert.True(result.httpCode == 200);

                GetObjectACLRequest getRequest = new GetObjectACLRequest(bucket, commonKey);

                //执行请求
                GetObjectACLResult getResult = cosXml.GetObjectACL(getRequest);

                AccessControlPolicy acl = getResult.accessControlPolicy;

                // Console.WriteLine(result.GetResultInfo());
                Assert.IsNotEmpty((result.GetResultInfo()));

                Assert.AreEqual(result.httpCode, 200);
                Assert.NotNull(acl.owner);
                Assert.NotNull(acl.owner.id);
                Assert.NotNull(acl.owner.displayName);
                Assert.NotNull(acl.accessControlList);
                Assert.NotNull(acl.accessControlList.grants);
                Assert.NotZero(acl.accessControlList.grants.Count);
                Assert.NotNull(acl.accessControlList.grants[0].permission);
                Assert.NotNull(acl.accessControlList.grants[0].grantee);
                Assert.NotNull(acl.accessControlList.grants[0].grantee.id);
                Assert.NotNull(acl.accessControlList.grants[0].grantee.displayName);
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.StackTrace);
                Assert.Fail();
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.Fail();
            }
        }

        [Test()]
        public void OptionObject()
        {

            try
            {
                PutBucketCORSRequest request = new PutBucketCORSRequest(bucket);

                //设置cors
                COSXML.Model.Tag.CORSConfiguration.CORSRule corsRule = new COSXML.Model.Tag.CORSConfiguration.CORSRule();

                corsRule.id = "corsconfigure1";
                corsRule.maxAgeSeconds = 6000;
                corsRule.allowedOrigins = new List<string>();
                corsRule.allowedOrigins.Add("http://cloud.tencent.com");

                corsRule.allowedMethods = new List<string>();
                corsRule.allowedMethods.Add("PUT");
                corsRule.allowedMethods.Add("DELETE");
                corsRule.allowedMethods.Add("POST");

                corsRule.allowedHeaders = new List<string>();
                corsRule.allowedHeaders.Add("Host");
                corsRule.allowedHeaders.Add("Authorizaiton");
                corsRule.allowedHeaders.Add("User-Agent");
                corsRule.allowedHeaders.Add("Content-Type");
                corsRule.allowedHeaders.Add("Content-Disposition");

                corsRule.exposeHeaders = new List<string>();
                corsRule.exposeHeaders.Add("x-cos-meta-x1");
                corsRule.exposeHeaders.Add("x-cos-meta-x2");

                request.SetCORSRule(corsRule);

                //执行请求
                PutBucketCORSResult result = cosXml.PutBucketCORS(request);

                Assert.AreEqual(result.httpCode, 200);

                string origin = "http://cloud.tencent.com";
                string accessMthod = "PUT";

                OptionObjectRequest optionRequest = new OptionObjectRequest(bucket, commonKey, origin, accessMthod);
                var header = new List<string>();
                header.Add("Content-Type");
                header.Add("Content-Disposition");
                optionRequest.SetAccessControlHeaders(header);

                //执行请求
                OptionObjectResult optionResult = cosXml.OptionObject(optionRequest);
                // Console.WriteLine(optionResult.GetResultInfo());
                Assert.IsNotEmpty((optionResult.GetResultInfo()));

                Assert.AreEqual(optionResult.httpCode, 200);
                Assert.NotNull(optionResult.accessControlAllowExposeHeaders);
                Assert.NotNull(optionResult.accessControlAllowHeaders);
                Assert.NotNull(optionResult.accessControlAllowMethods);
                Assert.NotNull(optionResult.accessControlAllowOrigin);
                Assert.NotZero(optionResult.accessControlMaxAge);

                // 错误参数测试
                optionRequest = new OptionObjectRequest(bucket, commonKey, null, accessMthod);
                try {
                    optionRequest.CheckParameters();
                    Assert.Fail();
                } catch (COSXML.CosException.CosClientException clientEx) {}
                optionRequest = new OptionObjectRequest(bucket, commonKey, origin, null);
                try {
                    optionRequest.CheckParameters();
                    Assert.Fail();
                } catch (COSXML.CosException.CosClientException clientEx) {}
                
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.StackTrace);
                Assert.Fail();
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.Fail();
            }
        }

        [Test()]
        public void CopyObject()
        {

            try
            {
                CopySourceStruct copySource = new CopySourceStruct(QCloudServer.Instance().appid,
                    bucket, QCloudServer.Instance().region, copykey);

                CopyObjectRequest copyObjectRequest = new CopyObjectRequest(bucket, commonKey);

                //设置拷贝源
                copyObjectRequest.SetCopySource(copySource);
                copyObjectRequest.SetCosStorageClass(CosStorageClass.Standard);
                QCloudServer.SetRequestACLData(copyObjectRequest);

                //执行请求
                CopyObjectResult result = cosXml.CopyObject(copyObjectRequest);
                var copyObject = result.copyObject;

                // Console.WriteLine(result.GetResultInfo());
                Assert.IsNotEmpty((result.GetResultInfo()));
                Assert.AreEqual(result.httpCode, 200);
                Assert.NotNull(copyObject.eTag);
                Assert.NotNull(copyObject.lastModified);
                Assert.Null(copyObject.versionId);
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.StackTrace);
                Assert.Fail();
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.Fail();
            }
        }



        public string GetMultiUplaodId(string coskey, string localfile)
        {

            string uploadId = "";
            try
            {
                InitMultipartUploadRequest initMultipartUploadRequest = new InitMultipartUploadRequest(bucket, coskey);
                QCloudServer.SetRequestACLData(initMultipartUploadRequest);

                //执行请求
                InitMultipartUploadResult initMultipartUploadResult = cosXml.InitMultipartUpload(initMultipartUploadRequest);
                uploadId = initMultipartUploadResult.initMultipartUpload.uploadId;
                
                var sliceSize = 1024 * 1024;
                var bigFile = new FileInfo(bigFileSrcPath);
                var sliceCount = bigFile.Length / sliceSize;

                for (int partNumber = 1; partNumber <= sliceCount-1; partNumber++) 
                {
                    UploadPartRequest uploadPartRequest = new UploadPartRequest(bucket, coskey, partNumber, uploadId, localfile, sliceSize * (partNumber - 1), sliceSize);
                    //设置进度回调
                    uploadPartRequest.SetCosProgressCallback(
                        delegate (long completed, long total)
                        {
                        }
                    );
                    //执行请求
                    UploadPartResult uploadPartResult = cosXml.UploadPart(uploadPartRequest);
                }
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.StackTrace);
                Assert.Fail();
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.Fail();
            }

            return uploadId;
        }
        
        [Test()]
        public void MultiUpload()
        {
            string key = multiKey;

            try
            {
                InitMultipartUploadRequest initMultipartUploadRequest = new InitMultipartUploadRequest(bucket, multiKey);
                QCloudServer.SetRequestACLData(initMultipartUploadRequest);

                //执行请求
                InitMultipartUploadResult initMultipartUploadResult = cosXml.InitMultipartUpload(initMultipartUploadRequest);
                Assert.AreEqual(initMultipartUploadResult.httpCode, 200);
                Assert.IsNotEmpty((initMultipartUploadResult.GetResultInfo()));

                string uploadId = initMultipartUploadResult.initMultipartUpload.uploadId;
                Assert.NotNull(uploadId);

                var sliceSize = 1024 * 1024;
                var bigFile = new FileInfo(bigFileSrcPath);
                var sliceCount = bigFile.Length / sliceSize;

                for (int partNumber = 1; partNumber <= sliceCount; partNumber++) 
                {
                    UploadPartRequest uploadPartRequest = new UploadPartRequest(bucket, key, partNumber, uploadId, bigFileSrcPath, sliceSize * (partNumber - 1), sliceSize);
                    //设置进度回调
                    uploadPartRequest.SetCosProgressCallback(
                        delegate (long completed, long total)
                        {
                            // Console.WriteLine(String.Format("{0} progress = {1} / {2} : {3:##.##}%",
                            //     DateTime.Now.ToString(), completed, total, completed * 100.0 / total));
                        }
                    );
                    //执行请求
                    UploadPartResult uploadPartResult = cosXml.UploadPart(uploadPartRequest);
                    Assert.AreEqual(uploadPartResult.httpCode, 200);
                    Assert.IsNotEmpty((uploadPartResult.GetResultInfo()));

                    Assert.NotNull(uploadPartResult.eTag);
                }
                

                ListPartsRequest listPartsRequest = new ListPartsRequest(bucket, key, uploadId);
                listPartsRequest.SetMaxParts(1000);
                listPartsRequest.SetEncodingType("url");
                // 执行请求
                ListPartsResult listPartsResult = cosXml.ListParts(listPartsRequest);
                TestUpdateSliceNums(listPartsResult);
                var parts = listPartsResult.listParts;

                Assert.IsNotEmpty((listPartsResult.GetResultInfo()));
                Assert.AreEqual(listPartsResult.httpCode, 200);
                Assert.NotNull(parts.bucket);
                Assert.NotNull(parts.key);
                Assert.NotNull(parts.uploadId);
                Assert.NotNull(parts.owner.id);
                Assert.NotNull(parts.owner.disPlayName);
                Assert.NotNull(parts.initiator.id);
                Assert.NotNull(parts.initiator.disPlayName);
                Assert.NotNull(parts.storageClass);
                Assert.NotNull(parts.maxParts);
                Assert.False(parts.isTruncated);
                Assert.AreEqual(parts.parts.Count, sliceCount);

                CompleteMultipartUploadRequest completeMultiUploadRequest = new CompleteMultipartUploadRequest(bucket, key, uploadId);

                foreach (var part in parts.parts) 
                {
                    Assert.NotNull(part.lastModified);
                    Assert.AreEqual(Int32.Parse(part.size), sliceSize);
                    Assert.NotZero(Int32.Parse(part.partNumber));
                    Assert.NotNull(part.eTag);
                    //设置已上传的parts
                    completeMultiUploadRequest.SetPartNumberAndETag(Int32.Parse(part.partNumber), part.eTag);
                }

                //执行请求
                CompleteMultipartUploadResult completeMultiUploadResult = cosXml.CompleteMultiUpload(completeMultiUploadRequest);
                var completeResult = completeMultiUploadResult.completeResult;

                Assert.IsNotEmpty((completeMultiUploadResult.GetResultInfo()));
                Assert.AreEqual(completeMultiUploadResult.httpCode, 200);
                Assert.NotNull(completeResult.location);
                Assert.NotNull(completeResult.bucket);
                Assert.NotNull(completeResult.key);
                Assert.NotNull(completeResult.eTag);

            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.StackTrace);
                Assert.Fail();
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.Fail();
            }
        }

        public void TestUpdateSliceNums(ListPartsResult lpR)
        {
            try
            {
                CopySourceStruct copySource = new CopySourceStruct(QCloudServer.Instance().appid, bucket, QCloudServer.Instance().region, "cosKey");
                COSXMLCopyTask copyTask = new COSXMLCopyTask(bucket, multiKey, copySource);
                copyTask.TestUpdateSliceNums(lpR);
            }
            catch (Exception)
            {
                
            }
        }

        [Test()]
        public void ListPartsInvalidRequestTest() {
                          
            ListPartsRequest listPartsRequest = new ListPartsRequest("bucket", "key", "uploadID");
            Dictionary<int, string> partNumberAndETag = new Dictionary<int, string>();
            partNumberAndETag[1] = "etag";

            CompleteMultipartUploadRequest completeMultiUploadRequest = new CompleteMultipartUploadRequest("bucket", "key", "uploadId");
            completeMultiUploadRequest.SetPartNumberAndETag(partNumberAndETag);

            listPartsRequest.SetPartNumberMarker(1);
            listPartsRequest.RequestURLWithSign = "string";
            listPartsRequest.CheckParameters();
            listPartsRequest = new ListPartsRequest("bucket", "key", null);
            try {
                listPartsRequest.CheckParameters();
            } catch (COSXML.CosException.CosClientException clientEx)
            {
                
            }
        }

        [Test()]
        public void CheckParameters_WhenPartsCountIsZero_ThrowsException(){
            var request = new CompleteMultipartUploadRequest("bucket", "key", "uploadId");
            var ex = Assert.Throws<CosClientException>(() => request.CheckParameters());
            Assert.That(ex.Message, Does.Contain("completeMultipartUpload.parts count = 0"));
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

                string uploadId = initMultipartUploadResult.initMultipartUpload.uploadId;
                Assert.AreEqual(initMultipartUploadResult.httpCode, 200);
                Assert.IsNotEmpty((initMultipartUploadResult.GetResultInfo()));
                Assert.NotNull(uploadId);

                AbortMultipartUploadRequest request = new AbortMultipartUploadRequest(bucket, key, uploadId);

                //执行请求
                AbortMultipartUploadResult result = cosXml.AbortMultiUpload(request);
                Assert.True(result.IsSuccessful());
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

        [Test()]
        public void PartCopyObject()
        {
            string key = commonKey;
            CopySourceStruct copySource = new CopySourceStruct(QCloudServer.Instance().appid,
                bucket, QCloudServer.Instance().region, multiKey);

            try
            {
                InitMultipartUploadRequest initMultipartUploadRequest = new InitMultipartUploadRequest(bucket, key);
                //执行请求
                InitMultipartUploadResult initMultipartUploadResult = cosXml.InitMultipartUpload(initMultipartUploadRequest);
                Assert.AreEqual(initMultipartUploadResult.httpCode, 200);
                Assert.IsNotEmpty((initMultipartUploadResult.GetResultInfo()));

                string uploadId = initMultipartUploadResult.initMultipartUpload.uploadId;
                Assert.NotNull(uploadId);

                var sliceSize = 1024 * 1024;
                var etags = new List<string>();

                for (int partNumber = 1; partNumber <= 2; partNumber++) 
                {
                    UploadPartCopyRequest uploadPartCopyRequest = new UploadPartCopyRequest(bucket, key, partNumber, uploadId);

                    //设置拷贝源
                    uploadPartCopyRequest.SetCopySource(copySource);
                    //设置拷贝范围
                    uploadPartCopyRequest.SetCopyRange((partNumber - 1) * sliceSize, partNumber * sliceSize);
                    //执行请求
                    UploadPartCopyResult uploadPartCopyResult = cosXml.PartCopy(uploadPartCopyRequest);

                    Assert.IsNotEmpty((uploadPartCopyResult.GetResultInfo()));
                    Assert.AreEqual(uploadPartCopyResult.httpCode, 200);

                    Assert.NotNull(uploadPartCopyResult.copyPart.eTag);
                    etags.Add(uploadPartCopyResult.copyPart.eTag);
                }
                

                CompleteMultipartUploadRequest completeMultiUploadRequest = new CompleteMultipartUploadRequest(bucket, key, uploadId);

                //设置已上传的parts
                for (int i = 0; i < etags.Count; i++) 
                {
                    completeMultiUploadRequest.SetPartNumberAndETag(i + 1, etags[i]);
                }

                CopyTestListParts(bucket, key, uploadId);
                
                //执行请求
                CompleteMultipartUploadResult completeMultiUploadResult = cosXml.CompleteMultiUpload(completeMultiUploadRequest);
                var completeResult = completeMultiUploadResult.completeResult;

                Assert.IsNotEmpty((completeMultiUploadResult.GetResultInfo()));
                Assert.AreEqual(completeMultiUploadResult.httpCode, 200);
                Assert.NotNull(completeResult.location);
                Assert.NotNull(completeResult.bucket);
                Assert.NotNull(completeResult.key);
                Assert.NotNull(completeResult.eTag);
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
        
        
        public void CopyTestListParts(string bucket, string cosKey, string uploadId)
        {
            try
            {
                cosXml = QCloudServer.Instance().cosXml;
                TransferConfig transferConfig = new TransferConfig();
                transferConfig.SliceSizeForUpload = 1024 * 1024;
                // 初始化 TransferManager
                TransferManager transferManager = new TransferManager(cosXml, transferConfig);

                CopySourceStruct copySource = new CopySourceStruct(QCloudServer.Instance().appid, bucket,
                    QCloudServer.Instance().region, cosKey);
                COSXMLCopyTask copyTask = new COSXMLCopyTask(bucket, multiKey, copySource);
                copyTask.SetUploadId(uploadId);
                copyTask.GetUploadId();
                Task<COSXMLCopyTask.CopyTaskResult> copyTaskResult = transferManager.CopyAsync(copyTask);
                copyTask.TestListParts();
                copyTask.Cancel();
            }
            catch (Exception)
            {
                
            }
           
        }
        
        

        [Test()]
        public void RestoreObject()
        {
            string objectKey = "archive_object";

            try
            {
                byte[] data = File.ReadAllBytes(smallFileSrcPath);
                PutObjectRequest putRequest = new PutObjectRequest(bucket, objectKey, data);
                putRequest.SetCosStorageClass("ARCHIVE");
                cosXml.PutObject(putRequest);

                RestoreObjectRequest request = new RestoreObjectRequest(bucket, objectKey);
                //恢复时间
                request.SetExpireDays(-1);
                request.SetExpireDays(3);
                request.SetTier(COSXML.Model.Tag.RestoreConfigure.Tier.Standard);

                //执行请求
                RestoreObjectResult result = cosXml.RestoreObject(request);

                Assert.True(result.IsSuccessful());

                request.SetVersionId("null");;

                DeleteObjectRequest deleteRequest = new DeleteObjectRequest(bucket, objectKey);
                deleteRequest.SetVersionId("null");
                DeleteObjectResult deleteObjectResult = cosXml.DeleteObject(deleteRequest);

                Assert.True(deleteObjectResult.IsSuccessful());
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

        [Test()]
        public void PostObject()
        {

            try
            {
                PostObjectRequest request = new PostObjectRequest(bucket, commonKey, smallFileSrcPath);

                request.SetCosProgressCallback(delegate (long completed, long total)
                {
                    // Console.WriteLine(String.Format("progress = {0} / {1} : {2:##.##}%", completed, total, completed * 100.0 / total));
                });

                //设置policy
                request.SetPolicy(null);
                request.SetCacheControl("no-cache");
                request.SetContentType("text/plain");
                request.SetContentDisposition("inline");
                request.SetContentEncoding("gzip");
                request.SetExpires("Wed, 21 Oct 2021 07:28:00 GMT");
                request.SetCustomerHeader("x-cos-key", "value");

                //执行请求
                PostObjectResult result = cosXml.PostObject(request);

                Assert.True(result.IsSuccessful());
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



        [Test()]
        public async Task PostObjectBytes()
        {

            try
            {
                byte[] data = File.ReadAllBytes(smallFileSrcPath);
                PostObjectRequest request = new PostObjectRequest(bucket, commonKey, data);

                //设置policy
                request.SetCosACL(CosACL.Private);
                request.SetContentType("image/jpeg");
                request.SetCosStorageClass("Standard");
                var policy = new PostObjectRequest.Policy();
                policy.SetExpiration(new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds() + 60000);
                policy.AddConditions("acl", "private", false);
                policy.AddConditions("$Content-Type", "image/", true);
                policy.AddContentConditions(0, data.Length + 1);
                request.SetPolicy(policy);

                //执行请求
                PostObjectResult result = await cosXml.ExecuteAsync<PostObjectResult>(request);

                Assert.True(result.IsSuccessful());
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

        public void DeleteObject()
        {

            try
            {
                DeleteObjectRequest request = new DeleteObjectRequest(bucket, commonKey);


                //执行请求
                DeleteObjectResult result = cosXml.DeleteObject(request);


                Assert.True(result.IsSuccessful());
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
                keys.Add(copykey);
                keys.Add(copyKeySmall);
                keys.Add(appendKey);
                request.SetObjectKeys(keys);

                //执行请求
                DeleteMultiObjectResult result = cosXml.DeleteMultiObjects(request);
                var deleteResult = result.deleteResult;

                Assert.True(result.IsSuccessful());
                Assert.IsNotEmpty((result.GetResultInfo()));
                Assert.AreEqual(deleteResult.deletedList.Count, 5);
                Assert.AreEqual(deleteResult.errorList.Count, 0);
                foreach (var deleted in deleteResult.deletedList)
                {
                    Assert.NotNull(deleted.key);
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

        [Test()]
        public void TestCreateDirectory()
        {

            try
            {
                PutObjectRequest request = new PutObjectRequest(bucket,
                    "dir/", new byte[0]);

                //执行请求
                PutObjectResult result = cosXml.PutObject(request);

                Assert.AreEqual(result.httpCode, 200);
            }
            catch (CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.Message);
                Assert.Fail();
            }
            catch (CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.Fail();
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
                    // Console.WriteLine(String.Format("progress = {0} / {1} : {2:##.##}%", completed, total, completed * 100.0 / total));
                });

                //执行请求
                GetObjectResult result = cosXml.GetObject(request);
                
                // 覆盖一些设置选项
                request.SetLocalFileOffset(0L);
                request.SetRange(-1L, 1024);
                request.SetVersionId("null");

                Assert.AreEqual(result.httpCode, 200);
                Assert.NotNull(result.eTag);
                Assert.NotNull(result.crc64ecma);
                Assert.True(COSXML.Utils.Crc64.CompareCrc64(localFileName, result.crc64ecma));
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

        [Test()]
        public void TestGetObjectMergePath()
        {
            string specialPath = "/././///abc/.//def//../../";
            GetObjectRequest request = new GetObjectRequest(bucket, specialPath, localDir, localFileName);
            request.SetObjectKeySimplifyCheck(true); 
            Assert.Throws<CosClientException>(delegate {  cosXml.GetObject(request); });
        }

        [Test()]
        public void TestGetObjectRepetitive()
        {
            try
            {
                GetObjectRequest request = new GetObjectRequest(bucket, commonKey, localDir, localFileName);

                request.SetCosProgressCallback(delegate (long completed, long total)
                {
                    // Console.WriteLine(String.Format("progress = {0} / {1} : {2:##.##}%", completed, total, completed * 100.0 / total));
                });

                //执行请求
                GetObjectResult result = cosXml.GetObject(request);

                Assert.AreEqual(result.httpCode, 200);
                Assert.NotNull(result.eTag);
                Assert.NotNull(result.crc64ecma);
                Assert.True(COSXML.Utils.Crc64.CompareCrc64(localFileName, result.crc64ecma));

                //request = new GetObjectRequest(bucket, commonKey, localDir, localFileName);
                result = cosXml.GetObject(request);
                Assert.AreEqual(result.httpCode, 200);
                Assert.NotNull(result.eTag);
                Assert.NotNull(result.crc64ecma);
                Assert.True(COSXML.Utils.Crc64.CompareCrc64(localFileName, result.crc64ecma));

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
                getObjectBytesRequest.SetCosProgressCallback(null);

                GetObjectBytesResult getObjectBytesResult = cosXml.GetObject(getObjectBytesRequest);

                byte[] content = getObjectBytesResult.content;

                getObjectBytesRequest.SetVersionId("null");
                getObjectBytesRequest.SetRange(-1L, 1024);
                getObjectBytesRequest.SetRange(1024, -1L);
                getObjectBytesRequest.SetRange(0);

                Assert.True(getObjectBytesResult.IsSuccessful());
                Assert.AreEqual(content.Length, contentLength);

            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx);
                Assert.Fail();
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.Fail();
            }
        }

        [Test()]
        public void TestGetObjectByteRange()
        {

            try
            {
                GetObjectBytesRequest getObjectBytesRequest = new GetObjectBytesRequest(bucket, commonKey);
                getObjectBytesRequest.SetRange(0, 199);

                GetObjectBytesResult getObjectBytesResult = cosXml.GetObject(getObjectBytesRequest);

                byte[] content = getObjectBytesResult.content;


                Assert.True(getObjectBytesResult.IsSuccessful());
                Assert.AreEqual(content.Length, 200);

            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx);
                Assert.Fail();
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.Fail();
            }
        }

        [Test()]
        public void TestSelectObjectToFile()
        {

            try
            {
                string key = selectKey;

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
                            // Console.WriteLine("OnProgress : " + progress + "," + total);
                        })
                        .OutputToFile(outputFile)
                        ;

                SelectObjectResult selectObjectResult = cosXml.SelectObject(request);

                Assert.AreEqual(selectObjectResult.httpCode, 200);
                Assert.Null(selectObjectResult.searchContent);
                Assert.NotZero(selectObjectResult.stat.BytesProcessed);
                Assert.NotZero(selectObjectResult.stat.BytesReturned);
                Assert.NotZero(selectObjectResult.stat.BytesScanned);
                Assert.AreEqual(selectObjectResult.stat.BytesReturned, new FileInfo(outputFile).Length);
                Assert.NotNull(selectObjectResult.stat.ToString());

                request.SetExpressionType("Select * from COSObject");
                request.SetExpression(null);
                try {
                    request.CheckParameters();
                    Assert.Fail();
                } catch (COSXML.CosException.CosClientException clientEx) {

                }
                request = new SelectObjectRequest(bucket, key);
                request.SetInputFormat(null);
                try {
                    request.CheckParameters();
                    Assert.Fail();
                } catch (COSXML.CosException.CosClientException clientEx) {
                    
                }
                request = new SelectObjectRequest(bucket, key);
                request.SetOutputFormat(null);
                try {
                    request.CheckParameters();
                    Assert.Fail();
                } catch (COSXML.CosException.CosClientException clientEx) {
                    
                }
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.StackTrace);
                Console.WriteLine("CosClientException: " + clientEx.Message);
                Assert.Fail();
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.Fail();
            }
        }

        [Test()]
        public void TestSelectObjectInMemory()
        {

            try
            {
                string key = selectKey;

                SelectObjectRequest request = new SelectObjectRequest(bucket, key);

                ObjectSelectionFormat.JSONFormat jSONFormat = new ObjectSelectionFormat.JSONFormat();

                jSONFormat.Type = "DOCUMENT";
                jSONFormat.RecordDelimiter = "\n";

                request.SetExpression("Select * from COSObject")
                        .SetInputFormat(new ObjectSelectionFormat(null, jSONFormat))
                        .SetOutputFormat(new ObjectSelectionFormat(null, jSONFormat))
                        .SetCosProgressCallback(delegate (long progress, long total)
                        {
                            // Console.WriteLine("OnProgress : " + progress + "," + total);
                        })
                        ;

                SelectObjectResult selectObjectResult = cosXml.SelectObject(request);

                Assert.AreEqual(selectObjectResult.httpCode, 200);
                Assert.NotZero(selectObjectResult.stat.BytesProcessed);
                Assert.NotZero(selectObjectResult.stat.BytesReturned);
                Assert.NotZero(selectObjectResult.stat.BytesScanned);
                Assert.AreEqual(selectObjectResult.stat.BytesReturned, Encoding.UTF8.GetByteCount(
                    selectObjectResult.searchContent));
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.StackTrace);
                Console.WriteLine("CosClientException: " + clientEx.Message);
                Assert.Fail();
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.Fail();
            }
        }

        [Test()]
        public async Task TestUploadTaskWithBigFile()
        {
            string key = multiKey;


            PutObjectRequest request = new PutObjectRequest(bucket, key, bigFileSrcPath);

            request.SetRequestHeader("Content-Type", "image/png");

            COSXMLUploadTask uploadTask = new COSXMLUploadTask(request);
            

            uploadTask.SetSrcPath(bigFileSrcPath);
            
            uploadTask.TestCompareSliceMD5(bigFileSrcPath, 1, 100, "sdsd");

            uploadTask.progressCallback = delegate (long completed, long total)
            {
                // Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
            };

            COSXMLUploadTask.UploadTaskResult result = await transferManager.UploadAsync(uploadTask);

            Assert.AreEqual(result.httpCode, 200);
            Assert.NotNull(result.eTag);

        }
        
        
        [Test()]
        public async Task TransferUploadFileCancel()
        {
            if (!File.Exists(bigFileSrcPath))
            {
                bigFileSrcPath = QCloudServer.CreateFile("big_"+TimeUtils.GetCurrentTimeSecond()+".txt", 40*1024*1024);
            }
           
            TransferConfig transferConfig = new TransferConfig();
            // 手动设置开始分块上传的大小阈值为10MB，默认值为5MB
            transferConfig.DivisionForUpload = 1 * 1024 * 1024;
            // 手动设置分块上传中每个分块的大小为2MB，默认值为1MB
            transferConfig.SliceSizeForUpload = 1 * 1024 * 1024;
            // 初始化 TransferManager
            TransferManager transferManager = new TransferManager(cosXml, transferConfig);
            
            // 上传对象
            COSXMLUploadTask uploadTask = new COSXMLUploadTask(bucket, multiKey);
            uploadTask.SetSrcPath(bigFileSrcPath);
            uploadTask.UseResumableUpload = false;
            
            uploadTask.progressCallback = delegate (long completed, long total)
            {
                if (completed / total > 0.3) {
                    for (int i= 0; i < 3; i++) {
                        try {
                            uploadTask.TestCheckAllUploadParts(uploadTask.GetUploadId());
                        }
                        catch (Exception)
                        {
                            
                        }
                       
                    }
                }
            };
            
            Task<COSXMLUploadTask.UploadTaskResult> result = transferManager.UploadAsync(uploadTask);
            Thread.Sleep(1500);
            uploadTask.TestCheckAllUploadParts(uploadTask.GetUploadId());
            uploadTask.Pause();
            uploadTask.TestDeleteObject();
            uploadTask.Cancel();
        }

        [Test()]
        public async Task TestUploadTaskWithBigFileNotResumable()
        {
            string key = multiKey;


            PutObjectRequest request = new PutObjectRequest(bucket, key, bigFileSrcPath);

            request.SetRequestHeader("Content-Type", "image/png");

            COSXMLUploadTask uploadTask = new COSXMLUploadTask(request);
            uploadTask.UseResumableUpload = false;

            uploadTask.SetSrcPath(bigFileSrcPath);

            uploadTask.progressCallback = delegate (long completed, long total)
            {
                // Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
            };

            COSXMLUploadTask.UploadTaskResult result = await transferManager.UploadAsync(uploadTask);

            Assert.AreEqual(result.httpCode, 200);
            Assert.NotNull(result.eTag);

        }

        [Test()]
        public async Task TestUploadTaskWithBigFileResumable()
        {
            string key = multiKey;
            
            PutObjectRequest request = new PutObjectRequest(bucket, key, bigFileSrcPath);

            request.SetRequestHeader("Content-Type", "image/png");

            COSXMLUploadTask uploadTask = new COSXMLUploadTask(request);
            
            uploadTask.SetSrcPath(bigFileSrcPath);
            uploadTask.progressCallback = delegate(long completed, long total)
            {
                // Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
            };
            //分片超过10000
            TransferConfig transferConfigTest = new TransferConfig();
         
            // 手动设置开始分块上传的大小阈值为10MB，默认值为5MB
            transferConfigTest.DivisionForUpload = 1;
                // 手动设置分块上传中每个分块的大小为2MB，默认值为1MB
            transferConfigTest.SliceSizeForUpload = 1;
                // 初始化 TransferManager
            TransferManager transferExcept = new TransferManager(cosXml, transferConfigTest);
            Assert.Throws<CosClientException>(delegate { transferExcept.UploadAsync(uploadTask); });
        }
        
        [Test()]
        public void TestUploadTaskWithBigFileResumableBreakAtPoint()
        {
            bigFileSrcPath = QCloudServer.CreateFile("big_"+TimeUtils.GetCurrentTimeSecond()+".txt", 40*1024*1024);
            string key = multiKey;
            PutObjectRequest request = new PutObjectRequest(bucket, key, bigFileSrcPath);
                
            COSXMLUploadTask uploadTask = new COSXMLUploadTask(request);
            TransferConfig transferConfigTest = new TransferConfig();
            uploadTask.SetSrcPath(bigFileSrcPath);
                
            transferConfigTest.DivisionForUpload = 1 * 1024 * 1024;
            transferConfigTest.SliceSizeForUpload = 1 * 1024 * 1024;
                
            TransferManager transferM = new TransferManager(cosXml, transferConfigTest);
            Task<COSXMLUploadTask.UploadTaskResult> rest = transferM.UploadAsync(uploadTask);
            string uploadId = "";
            for (int i=0;i<3;i++ )
            {
                Thread.Sleep(400);
                uploadId = uploadTask.GetUploadId();
                uploadTask.TestCheckAllUploadParts(uploadId);
            }
            uploadTask.Pause();
            uploadTask.Cancel();
            uploadTask.TestCheckAllUploadParts(uploadId);
            
            try {
                PutObjectRequest requestBreak = new PutObjectRequest(bucket, key, bigFileSrcPath);
                TransferConfig transferConfigBreak = new TransferConfig();
                transferConfigBreak.DivisionForUpload = 1 * 1024 * 1024;
                transferConfigBreak.SliceSizeForUpload = 1 * 1024 * 1024;
                
                COSXMLUploadTask uploadTaskBreak = new COSXMLUploadTask(requestBreak);
                uploadTaskBreak.SetSrcPath(bigFileSrcPath);
                uploadTaskBreak.UseResumableUpload = true;
                TransferManager transfe = new TransferManager(cosXml, transferConfigBreak);
                COSXMLUploadTask.UploadTaskResult result = transfe.UploadAsync(uploadTaskBreak).Result;
                Assert.AreEqual(result.httpCode, 200);
            }
            catch (Exception e)
            {
                PutObjectRequest requestBreak = new PutObjectRequest(bucket, key, bigFileSrcPath);
                TransferConfig transferConfigBreak = new TransferConfig();
                transferConfigBreak.DivisionForUpload = 1 * 1024 * 1024;
                transferConfigBreak.SliceSizeForUpload = 1 * 1024 * 1024;
                
                COSXMLUploadTask uploadTaskBreak = new COSXMLUploadTask(requestBreak);
                uploadTaskBreak.SetSrcPath(bigFileSrcPath);
                uploadTaskBreak.UseResumableUpload = true;
                TransferManager transfe = new TransferManager(cosXml, transferConfigBreak);
                COSXMLUploadTask.UploadTaskResult result = transfe.UploadAsync(uploadTaskBreak).Result;
                Assert.AreEqual(result.httpCode, 200);
            }
        }

        [Test()]
        public async Task TestMultiDownloadTaskBigFileTimeout()
        {
            string cosPath = PutObjectBigFile();
            GetObjectRequest request = new GetObjectRequest(bucket, cosPath, localDir+"/ab/", localFileName);
            request.LimitTraffic(8 * 1024 * 1024);
            
            //执行请求
            COSXMLDownloadTask downloadTask = new COSXMLDownloadTask(request);

            TransferConfig transferConfig = new TransferConfig();
            // 手动设置开始分块上传的大小阈值为10MB，默认值为5MB
            transferConfig.DivisionForDownload = 1 * 1024 * 1024;
            // 手动设置分块上传中每个分块的大小为2MB，默认值为1MB
            transferConfig.SliceSizeForDownload = 1 * 1024 * 1024;
            
            try 
            {
                downloadTask.SetSingleTaskTimeoutMs(1); //超时删除逻辑
                downloadTask.TestEnvTag = true;
                TransferManager transfe = new TransferManager(cosXml, transferConfig);
                COSXMLDownloadTask.DownloadTaskResult rest = await transfe.DownloadAsync(downloadTask); 
            }
            catch (Exception e)
            {
            }
            
        }
        
        //文件分片大小决定走不同的路径
        [Test()]
        public async Task TestNewFuncMultiDownloadTaskBigFile()
        {
            string cosPath = PutObjectBigFile();
            GetObjectRequest request = new GetObjectRequest(bucket, cosPath, localDir+"/ab/", localFileName);
            request.LimitTraffic(8 * 1024 * 1024);
            
            //执行请求
            COSXMLDownloadTask downloadTask = new COSXMLDownloadTask(request);
            downloadTask.SetObjectKeySimplifyCheck(true);
            downloadTask.SetEnableCRC64Check(true);
            TransferConfig transferConfig = new TransferConfig();
            // 手动设置开始分块上传的大小阈值为10MB，默认值为5MB
            transferConfig.DivisionForDownload = 1 * 1024 * 1024;
            // 手动设置分块上传中每个分块的大小为2MB，默认值为1MB
            transferConfig.SliceSizeForDownload = 1 * 1024 * 1024;
            
            downloadTask.progressCallback = delegate (long completed, long total) {
                // Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
            };
            transferConfig.ByNewFunc = true; //走特殊的下载路径下载
            
            try
            {
                downloadTask.SetSingleTaskTimeoutMs(1);
                TransferManager transfe = new TransferManager(cosXml, transferConfig);
                COSXMLDownloadTask.DownloadTaskResult rest = await transfe.DownloadAsync(downloadTask); //超时删除逻辑
            }
            catch (Exception e)
            {
                Assert.True(!Directory.Exists(localDir));
                string searchPattern = localFileName;
                string[] files = Directory.GetFiles(localDir);
                string[] filsAr = files.Where(file => Path.GetFileName(file).Contains(searchPattern)).ToArray();
                Assert.True(filsAr.Length == 0);
            }
            
            downloadTask.SetSingleTaskTimeoutMs(3000);
            // 初始化 TransferManager
            TransferManager transferManagerVar = new TransferManager(cosXml, transferConfig);
            COSXMLDownloadTask.DownloadTaskResult result = await transferManagerVar.DownloadAsync(downloadTask);
            Assert.True(result.httpCode == 200 || result.httpCode == 206);
            Assert.NotNull(result.eTag);
        }
        
        
        [Test()]
        public async Task TestNewFuncMultiDownloadTaskSmallFile()
        {
            string cosKey = PutObjectSmallFile();
            long timestamp = TimeUtils.GetCurrentTimeSecond();
            string localFileNameTim = localFileName + timestamp;
            GetObjectRequest request = new GetObjectRequest(bucket, cosKey, localDir, localFileName);
            request.LimitTraffic(8 * 1024 * 1024);
            //执行请求
            COSXMLDownloadTask downloadTask = new COSXMLDownloadTask(request);
            TransferConfig transferConfig = new TransferConfig();
            // 手动设置开始分块上传的大小阈值为10MB，默认值为5MB
            transferConfig.DivisionForUpload = 20 * 1024 * 1024;
            // 手动设置分块上传中每个分块的大小为2MB，默认值为1MB
            transferConfig.SliceSizeForUpload = 20 * 1024 * 1024;
            
            transferConfig.ByNewFunc = true; // 走特殊的下载路径下载

            //是否能进该判断，文件是否够大
            try
            {
                downloadTask.SetSingleTaskTimeoutMs(1);
                TransferManager transfe = new TransferManager(cosXml, transferConfig);
                COSXMLDownloadTask.DownloadTaskResult rest = await transfe.DownloadAsync(downloadTask); //超时删除逻辑
            }
            catch (Exception e)
            {
                Assert.True(!Directory.Exists(localDir));
                string searchPattern = localFileNameTim;
                string[] files = Directory.GetFiles(localDir);
                string[] filsAr = files.Where(file => Path.GetFileName(file).Contains(searchPattern)).ToArray();
                Assert.True(filsAr.Length == 0);
            }
            
            downloadTask.SetSingleTaskTimeoutMs(3000);
            // 初始化 TransferManager
            TransferManager transferManagerVar = new TransferManager(cosXml, transferConfig);
            COSXMLDownloadTask.DownloadTaskResult result = await transferManagerVar.DownloadAsync(downloadTask);
            Assert.True(result.httpCode == 200 || result.httpCode == 206);
            Assert.NotNull(result.eTag);
        }
        
        [Test()]
        public async Task TestUploadTaskWithSmallFile()
        {
            string key = commonKey;

            COSXMLUploadTask uploadTask = new COSXMLUploadTask(bucket, key);
            uploadTask.UseResumableUpload = false;
            uploadTask.progressCallback = delegate (long completed, long total)
            {
                // Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
            };

            uploadTask.SetSrcPath(smallFileSrcPath);

            COSXMLUploadTask.UploadTaskResult result = await transferManager.UploadAsync(uploadTask);

            Assert.AreEqual(200, result.httpCode);
            Assert.NotNull(result.eTag);
        }


        [Test()]
        public void TestUploadTaskWithInteractive()
        {
            string key = multiKey;

            PutObjectRequest putObjectRequest = new PutObjectRequest(bucket, key, bigFileSrcPath);

            putObjectRequest.LimitTraffic(4096000);

            COSXMLUploadTask uploadTask = new COSXMLUploadTask(putObjectRequest);

            uploadTask.SetSrcPath(bigFileSrcPath);

            uploadTask.progressCallback = delegate (long completed, long total)
            {
                // Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
            };

            transferManager.UploadAsync(uploadTask);

            Thread.Sleep(2000);
            uploadTask.Pause();
            Thread.Sleep(200);
            uploadTask.Resume();
            Thread.Sleep(2000);
            uploadTask.Pause();
            Thread.Sleep(1000);

            if (uploadTask.State() != TaskState.Completed)
            {

                QCloudServer.TestWithServerFailTolerance(() =>
                {
                    // new task
                    COSXMLUploadTask uploadTask2 = new COSXMLUploadTask(bucket, key);

                    uploadTask2.SetSrcPath(bigFileSrcPath);
                    uploadTask2.SetUploadId(uploadTask.GetUploadId());

                    uploadTask2.progressCallback = delegate (long completed, long total)
                    {
                        // Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
                    };
                    var asyncTask = transferManager.UploadAsync(uploadTask2);
                    asyncTask.Wait(10000);
                    COSXMLUploadTask.UploadTaskResult result = asyncTask.Result;

                    Assert.True(result.httpCode == 200);
                    Assert.NotNull(result.eTag);
                    Assert.NotNull(result.GetResultInfo());
                }
                );
            } 
            else 
            {
                Console.WriteLine("Upload is Complete");
                Assert.Pass();
            }
        }

        [Test()]
        public void TestUploadTaskSimpleUpload()
        {
            string key = multiKey;
            COSXMLUploadTask uploadTask = new COSXMLUploadTask(bucket, key);
            uploadTask.SetSrcPath(smallFileSrcPath);
            TransferConfig transferConfig = new TransferConfig();
            // 手动设置开始分块上传的大小阈值为10MB，默认值为5MB
            transferConfig.DivisionForUpload = 1000 * 1024 * 1024;
            // 手动设置分块上传中每个分块的大小为2MB，默认值为1MB
            transferConfig.SliceSizeForUpload = 1000 * 1024 * 1024;
            TransferManager transferManagerVar = new TransferManager(cosXml, transferConfig);
            var asyncTask = transferManagerVar.UploadAsync(uploadTask);
            COSXMLUploadTask.UploadTaskResult result = asyncTask.Result;
            Assert.True(result.httpCode == 200);
        }
        
        [Test()]
        public void TestUploadTaskCancelledAndRetry()
        {
            string key = multiKey;

            COSXMLUploadTask uploadTask = new COSXMLUploadTask(bucket, key);

            uploadTask.SetSrcPath(bigFileSrcPath);
            uploadTask.MaxConcurrent = 1;
            uploadTask.StorageClass = "Standard_IA";

            var asyncTask = transferManager.UploadAsync(uploadTask);

            Thread.Sleep(500);
            uploadTask.Cancel();
            Thread.Sleep(500);

            uploadTask = new COSXMLUploadTask(bucket, key);

            uploadTask.SetSrcPath(bigFileSrcPath);
            uploadTask.MaxConcurrent = 1;
            uploadTask.StorageClass = "Standard_IA";

            asyncTask = transferManager.UploadAsync(uploadTask);
            COSXMLUploadTask.UploadTaskResult result = asyncTask.Result;

            Assert.True(result.httpCode == 200);
            Assert.NotNull(result.eTag);
            Assert.NotNull(result.GetResultInfo());
        }
        

        [Test()]
        public async Task TestDownloadTask()
        {
            GetObjectRequest request = new GetObjectRequest(bucket, commonKey, localDir, localFileName);
            request.LimitTraffic(8 * 1024 * 1024);

            //执行请求
            COSXMLDownloadTask downloadTask = new COSXMLDownloadTask(request);

            COSXMLDownloadTask.DownloadTaskResult result = await transferManager.DownloadAsync(downloadTask);

            Assert.True(result.httpCode == 200 || result.httpCode == 206);
            Assert.NotNull(result.eTag);

        }
        
        [Test()]
        public async Task TestDownloadTaskDiffTaskInit()
        {
            long timestamp = TimeUtils.GetCurrentTime(TimeUnit.Seconds);
            //执行请求,不同的初始化方式
            COSXMLDownloadTask downloadTask = new COSXMLDownloadTask(bucket, commonKey, localDir, localFileName+timestamp);
            COSXMLDownloadTask.DownloadTaskResult result = await transferManager.DownloadAsync(downloadTask);
            Assert.True(result.GetResultInfo().Length != 0);
            
            Assert.True(result.httpCode == 200 || result.httpCode == 206);
            Assert.NotNull(result.eTag);
        }
        

        

        [Test()]
        public async Task TestDownloadTaskRanged()
        {
            HeadObjectRequest headRequest = new HeadObjectRequest(bucket, commonKey);

            //执行请求
            HeadObjectResult headResult = cosXml.HeadObject(headRequest);

            long contentLength = Int64.Parse(headResult.responseHeaders["Content-Length"][0]);

            GetObjectRequest request = new GetObjectRequest(bucket,
                commonKey, localDir, localFileName);
            request.SetRange(100);
            request.LimitTraffic(8 * 1024 * 1024);

            //执行请求
            COSXMLDownloadTask downloadTask = new COSXMLDownloadTask(request);
            downloadTask.SetRange(200, -1);

            COSXMLDownloadTask.DownloadTaskResult result = await transferManager.DownloadAsync(downloadTask);

            Assert.True(result.IsSuccessful());
            Assert.AreEqual(Int64.Parse(result.responseHeaders["Content-Length"][0]), contentLength - 200);
        }

        [Test()]
        public void TestDownloadTaskInteractive()
        {
            //执行请求
            GetObjectRequest getRequest = new GetObjectRequest(bucket, multiKey, localDir, localFileName);
            getRequest.LimitTraffic(8192000);

            COSXMLDownloadTask downloadTask = new COSXMLDownloadTask(getRequest);

            downloadTask.progressCallback = delegate (long completed, long total)
            {
                // Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
            };

            var asyncTask = transferManager.DownloadAsync(downloadTask);

            Thread.Sleep(200);
            downloadTask.Pause();

            Thread.Sleep(200);
            downloadTask.Resume();
            if (downloadTask.State() != TaskState.Completed)
            {
                downloadTask.SetResumableDownload(true);
                COSXMLDownloadTask.DownloadTaskResult result = asyncTask.Result;

                Assert.AreEqual(result.httpCode, 206);
                Assert.NotNull(result.GetResultInfo());
            } 
            else 
            {
                Console.WriteLine("Download is Complete");
                Assert.Pass();
            }
        }

        [Test()]
        public void TestDownloadResumableTask()
        {
            string cosKey = PutObjectBigFile();
            //执行请求
            GetObjectRequest getRequest = new GetObjectRequest(bucket, cosKey, localDir, localFileName);
            //执行请求
       
            getRequest.LimitTraffic(8 * 1024 * 1024);

            COSXMLDownloadTask downloadTask = new COSXMLDownloadTask(getRequest);
            downloadTask.SetResumableDownload(true);

            double progrss = 0;
            downloadTask.progressCallback = delegate (long completed, long total)
            {
                progrss = completed * 100.0 / total;
                // Console.WriteLine(String.Format("progress = {0:##.##}%", progrss));
            };

            var asyncTask = transferManager.DownloadAsync(downloadTask);

            asyncTask = downloadTask.AsyncTask<COSXMLDownloadTask.DownloadTaskResult>();
            downloadTask.SetResumableDownload(true);
            asyncTask.Wait();
            
            if (downloadTask.State() != TaskState.Completed && downloadTask.State() != TaskState.Failed)
            {
                Console.WriteLine("downloadTask.State() = " + downloadTask.State());
                Console.WriteLine("localFileCrc64 = " + downloadTask.GetLocalFileCrc64());
                if (progrss < 100)
                {
                    downloadTask.Cancel();
                }

                Thread.Sleep(500);
            }
            else 
            {
                Console.WriteLine("localFileCrc64 = " + downloadTask.GetLocalFileCrc64());
                Thread.Sleep(500);
            }
        }

        [Test()]
        public void TestDownloadPauseAndRetry()
        {
            //执行请求
            GetObjectRequest getRequest = new GetObjectRequest(bucket, multiKey, localDir, localFileName);
            getRequest.LimitTraffic(8 * 1024 * 1024);

            COSXMLDownloadTask downloadTask = new COSXMLDownloadTask(getRequest);
            downloadTask.SetResumableDownload(true);

            double progrss = 0;
            downloadTask.progressCallback = delegate (long completed, long total)
            {
                progrss = completed * 100.0 / total;
                // Console.WriteLine(String.Format("progress = {0:##.##}%", progrss));
            };

            var asyncTask = transferManager.DownloadAsync(downloadTask);

            asyncTask = downloadTask.AsyncTask<COSXMLDownloadTask.DownloadTaskResult>();
            
            if (downloadTask.State() != TaskState.Completed && downloadTask.State() != TaskState.Failed)
            {
                Console.WriteLine("downloadTask.State() = " + downloadTask.State());
                Console.WriteLine("localFileCrc64 = " + downloadTask.GetLocalFileCrc64());
                if (60 < progrss && progrss < 65)
                {
                    downloadTask.Pause();
                }

                Thread.Sleep(500);
            }

            getRequest = new GetObjectRequest(bucket, multiKey, localDir, localFileName);
            getRequest.LimitTraffic(8 * 1024 * 1024);

            downloadTask = new COSXMLDownloadTask(getRequest);
            downloadTask.SetResumableDownload(true);
            transferManager.Download(downloadTask);
        }

        [Test()]
        public void TestDownloadTaskCancelled()
        {
            GetObjectRequest request = new GetObjectRequest(bucket,
                commonKey, localDir, localFileName);

            //执行请求
            COSXMLDownloadTask downloadTask = new COSXMLDownloadTask(request);

            var asyncTask = transferManager.DownloadAsync(downloadTask);

            Thread.Sleep(2000);
            downloadTask.Cancel();
            Thread.Sleep(200);
            Assert.Pass();
        }
        
        [Test()]
        public void TestDownloadTaskMergeFileFileDelete()
        {
            try
            {
                string cosKey = PutObjectBigFile();
                TransferConfig transferConfig = new TransferConfig();
                // 手动设置开始分块上传的大小阈值为10MB，默认值为5MB
                transferConfig.DivisionForDownload = 1024 * 1024;
                // 手动设置分块上传中每个分块的大小为2MB，默认值为1MB
                transferConfig.SliceSizeForDownload = 1024 * 1024;
                // 先下载一个大文件
                GetObjectRequest request = new GetObjectRequest(bucket, cosKey, localDir, localFileName);
                COSXMLDownloadTask downloadTask = new COSXMLDownloadTask(request);
                var asyncTask = transferManager.DownloadAsync(downloadTask);
                string filename = localDir + "." + localFileName + ".cosresumable." + 2;
                for (int i = 0; i < 5000; i++) {
                    if (SystemUtils.DeleteFileByFileName(filename)) {
                        break;
                    }
                    if (SystemUtils.DeleteFileByFileName(localDir + localFileName))
                    {
                        break;
                    }
                    Thread.Sleep(100);
                }
                SystemUtils.CreateFileByParam("./txt/sd/", "txt.txt", 1024);
                asyncTask.Wait();
            }
            catch (Exception ex)
            {
                Assert.True(true);//进入则成功
            }
        }
        
        [Test()]
        public void TestDownloadTaskMergeFile()
        {
            try
            {
                string cosKey = PutObjectBigFile();
                // 先下载一个大文件
                GetObjectRequest request = new GetObjectRequest(bucket, cosKey, localDir, localFileName);
                COSXMLDownloadTask downloadTask = new COSXMLDownloadTask(request);
                
                string filenametmp = localDir + "/." + localFileName + ".cosresumable.1";
                if (!File.Exists(filenametmp)) 
                {
                    QCloudServer.CreateFile(filenametmp, 1024);
                }
                
                
                var asyncTask = transferManager.DownloadAsync(downloadTask);
                // Thread.Sleep(1300);
                Assert.False(SystemUtils.DeleteFileByFileName(".cmable.3"));
                string filename = localDir + "/." + localFileName + ".cosresumable.3";
                // Console.WriteLine(filename);
                for (int i = 0; i < 5000; i++)
                {
                    if (asyncTask.IsCompleted) {
                        break;
                    }

                    if (SystemUtils.DeleteFileByFileName(filename)) {
                        break;
                    }

                    Thread.Sleep(100);
                }

                asyncTask.Wait();
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
            catch (Exception ex)
            {
                Assert.True(true);//进入则成功
            }
        }
        
        [Test()]
        public void TestDownloadTaskOverwriteSameFile()
        {
            try {
                // 先下载一个大文件
                GetObjectRequest request = new GetObjectRequest(bucket, multiKey, localDir, localFileName);
                COSXMLDownloadTask downloadTask = new COSXMLDownloadTask(request);
                var asyncTask = transferManager.DownloadAsync(downloadTask);
                asyncTask.Wait();
                
                //GetObjectResult result = cosXml.GetObject(request);
                //Assert.AreEqual(200, result.httpCode);
                long bigLength = new FileInfo(localFileName).Length;
                // 再下载一个小文件
                // 存在一致性问题，把小文件重新上传一下
                PutObject();
                request = new GetObjectRequest(bucket, commonKey, localDir, localFileName);
                
                downloadTask = new COSXMLDownloadTask(request);
                // downloadTask.SetObjectKeySimplifyCheck(true);
                downloadTask.SetEnableCRC64Check(true);
                asyncTask = transferManager.DownloadAsync(downloadTask);
                asyncTask.Wait();
                
                //result = cosXml.GetObject(request);
                //Assert.AreEqual(200, result.httpCode);
                // 检查文件长度，是否覆盖写
                long smallLength = new FileInfo(localFileName).Length;
                
                Assert.True(smallLength < bigLength);
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

        [Test()]
        public void TestDownloadTaskInvalidParams()
        {
            GetObjectRequest request = new GetObjectRequest(bucket,
                commonKey, localDir, localFileName);

            //执行请求
            COSXMLDownloadTask downloadTask = new COSXMLDownloadTask(request);

            downloadTask.SetLocalFileOffset(1L);
            downloadTask.SetResumableTaskFile("taskFile");
            downloadTask.SetMaxTasks(100);
            try {
                downloadTask.SetMaxTasks(-1);
                Assert.Fail();
            } catch (COSXML.CosException.CosClientException clientEx) {

            }
            downloadTask.SetSliceSize(100);
            try {
                downloadTask.SetSliceSize(0);
                Assert.Fail();
            } catch (COSXML.CosException.CosClientException clientEx) {

            }
            downloadTask.SetDivisionSize(100);
            try {
                downloadTask.SetDivisionSize(0);
                Assert.Fail();
            } catch (COSXML.CosException.CosClientException clientEx) {

            }
            downloadTask.SetEnableCRC64Check(true);
            downloadTask.SetSingleTaskTimeoutMs(1);
            downloadTask.SetMaxRetries(1);
            transferManager.DownloadAsync(downloadTask);
            try {
                downloadTask.Pause();
            }
            catch (Exception)
            {
                Assert.Pass();
            }
            
        }
        
        [Test]
        public void CopyTaskOnFailed()
        {
            string cosKey = PutObjectBigFile();
            TransferConfig transferConfig = new TransferConfig();
            // 手动设置高级下载接口的分块阈值为 20MB(默认为20MB), 从5.4.26版本开始支持！
            transferConfig.DdivisionForCopy = 1 * 1024 * 1024;
            // 手动设置高级下载接口的分块大小为 10MB(默认为5MB),不建议此处设置过小的分块值,可能导致频繁重试或下载速度不合预期
            transferConfig.SliceSizeForCopy = 1 * 1024 * 1024;
            // 初始化 TransferManager
            TransferManager transferManager = new TransferManager(cosXml, transferConfig);
                
            CopySourceStruct copySource = new CopySourceStruct(QCloudServer.Instance().appid, bucket, QCloudServer.Instance().region, cosKey); 
            COSXMLCopyTask copyTask = new COSXMLCopyTask(bucket, multiKey, copySource);
         
            Task<COSXMLCopyTask.CopyTaskResult> copyTaskResult = transferManager.CopyAsync(copyTask);
            copyTask.Pause();
            copyTask.Cancel();
            try
            {
                CosClientException clientEx = new CosClientException(-1, "error", new Exception("df"));
                CosServerException serverEx = new CosServerException(-1, "error", new CosServerError());
                copyTask.OnFailed(clientEx, serverEx);
            }
            catch (Exception)
            {
            }
        }

        [Test()]
        public async Task TestUploadTaskWithCheckSlice()
        {
            string cosKey = PutObjectBigFile();
            string uploadId = "";
            uploadId = GetMultiUplaodId(cosKey, bigCopySourceFilePath);
            COSXMLUploadTask uploadTask = new COSXMLUploadTask(bucket, cosKey);
            uploadTask.SetSrcPath(bigCopySourceFilePath);
            
            TransferConfig transferConfig = new TransferConfig();
            // 手动设置高级下载接口的分块阈值为 20MB(默认为20MB), 从5.4.26版本开始支持！
            transferConfig.DdivisionForCopy = 1024 * 1024;
            // 手动设置高级下载接口的分块大小为 10MB(默认为5MB),不建议此处设置过小的分块值,可能导致频繁重试或下载速度不合预期
            transferConfig.SliceSizeForCopy = 1024 * 1024;
            uploadTask.SetUploadId(uploadId);
            transferManager.UploadAsync(uploadTask);
            uploadTask.TestCheckAllUploadParts(uploadId);

            try {
                ListPartsRequest listPartsRequest = new ListPartsRequest(bucket, cosKey, uploadId);
                ListPartsResult listPartsResult = cosXml.ListParts(listPartsRequest);
                TestUpdateSliceNums(listPartsResult);
            }
            catch (Exception)
            {
                
            }
            uploadTask.Cancel();
        }

        [Test()]
        public async Task TestCopyTaskWithBigFileBreakPoint()
        {
            string cosKey = PutObjectBigFile();
            string uploadId = "";
            uploadId = GetMultiUplaodId(cosKey, bigCopySourceFilePath);
            if (uploadId != "") {
                try
                {
                    TransferConfig transferConfig = new TransferConfig();
                    // 手动设置高级下载接口的分块阈值为 20MB(默认为20MB), 从5.4.26版本开始支持！
                    transferConfig.DdivisionForCopy = 1024 * 1024;
                    // 手动设置高级下载接口的分块大小为 10MB(默认为5MB),不建议此处设置过小的分块值,可能导致频繁重试或下载速度不合预期
                    transferConfig.SliceSizeForCopy = 1024 * 1024;
                    // 初始化 TransferManager
                    TransferManager transferManager = new TransferManager(cosXml, transferConfig);
                    CopySourceStruct copySource = new CopySourceStruct(QCloudServer.Instance().appid, bucket, QCloudServer.Instance().region, cosKey); 
                    COSXMLCopyTask copyTask = new COSXMLCopyTask(bucket, multiKey, copySource);
                    
       
                    
                    copyTask.SetUploadId(uploadId);
                    copyTask.progressCallback = delegate(long completed, long total) {
                        // Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
                    };
                    COSXMLCopyTask.CopyTaskResult resultBreak = await transferManager.CopyAsync(copyTask);
                    Assert.True(resultBreak.httpCode == 200);
                }
                catch (Exception e)
                {
                    ;
                }
            }
            else
            {
                Assert.Fail();
            }
        }
        
        [Test()]
        public async Task TestCopyTaskWithBigFile()
        {
            CopySourceStruct copySource = new CopySourceStruct(QCloudServer.Instance().appid,
                    bucket, QCloudServer.Instance().region, bigCopyKey);


            COSXMLCopyTask copyTask = new COSXMLCopyTask(bucket, multiKey, copySource);

            copyTask.progressCallback = delegate (long completed, long total)
            {
                // Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
            };
            COSXMLCopyTask.CopyTaskResult result = await transferManager.CopyAsync(copyTask);

            Assert.True(result.httpCode == 200);
            Assert.NotNull(result.eTag);
            Assert.NotNull(result.GetResultInfo());
        }

        [Test()]
        public async Task TestCopyTaskWithSmallFile()
        {
            string cosKey = PutObjectSmallFile();
            
            CopySourceStruct copySource = new CopySourceStruct(QCloudServer.Instance().appid,
                    bucket, QCloudServer.Instance().region, cosKey);
            COSXMLCopyTask copyTask = new COSXMLCopyTask(bucket, multiKey, copySource);
            COSXMLCopyTask.CopyTaskResult result = await transferManager.CopyAsync(copyTask);

            Assert.True(result.httpCode == 200);
            Assert.NotNull(result.eTag);
        }

        [Test()]
        public void TestCopyTaskWithInteractive()
        {
            CopySourceStruct copySource = new CopySourceStruct(QCloudServer.Instance().appid,
                    bucket, QCloudServer.Instance().region, copykey);


            COSXMLCopyTask copyTask = new COSXMLCopyTask(bucket, multiKey, copySource);

            copyTask.CompleteOnAllPartsCopyed = false;

            var asyncTask = transferManager.CopyAsync(copyTask);
            copyTask.Pause();
            Thread.Sleep(200);
            copyTask.Pause();

            Thread.Sleep(200);
            copyTask.CompleteOnAllPartsCopyed = true;
            copyTask.Resume();
            if (copyTask.State() != TaskState.Completed)
            {
                asyncTask = copyTask.AsyncTask<COSXMLCopyTask.CopyTaskResult>();
                COSXMLCopyTask.CopyTaskResult result = asyncTask.Result;

                Assert.True(result.httpCode == 200);
                Assert.NotNull(result.eTag);
            }
            else 
            {
                COSXMLCopyTask.CopyTaskResult result = asyncTask.Result;
                Assert.True(result.httpCode == 200);
                Assert.NotNull(result.eTag);
                Assert.Pass();
            }
        }

        [Test()]
        public void TestCopyTaskCancelled()
        {
            string bigKey = PutObjectBigFile();
            CopySourceStruct copySource = new CopySourceStruct(QCloudServer.Instance().appid,
                    bucket, QCloudServer.Instance().region, bigKey);
            COSXMLCopyTask copyTask = new COSXMLCopyTask(bucket, multiKey, copySource);
            TransferConfig transferConfig = new TransferConfig();
            // 手动设置开始分块上传的大小阈值为10MB，默认值为5MB
            transferConfig.DdivisionForCopy  = 100 * 1024 * 1024;
            // 手动设置分块上传中每个分块的大小为2MB，默认值为1MB
            transferConfig.SliceSizeForCopy = 100 * 1024 * 1024;
            // 初始化 TransferManager
            TransferManager transferManager = new TransferManager(cosXml, transferConfig);
            transferManager.CopyAsync(copyTask);
            Thread.Sleep(1000);
            copyTask.Cancel();
            transferManager.CopyAsync(copyTask);
            copyTask.Cancel();
            Assert.Pass();
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

                try {
                    
                    COSXMLUploadTask.UploadTaskResult uploadTaskResult = new COSXMLUploadTask.UploadTaskResult();
                    uploadTaskResult.SetResult(result);
                }
                catch (Exception e)
                {
                }
                
                long costTime = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds() - now;

                Assert.True(result.httpCode == 200);
                Assert.NotNull(result.eTag);
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

                Assert.True(result.IsSuccessful());
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

        [Test()]
        public void GenerateSignUrl()
        {
            string key = commonKey;
            PreSignatureStruct signatureStruct = new PreSignatureStruct();

            signatureStruct.bucket = bucket;
            signatureStruct.appid = QCloudServer.Instance().appid;
            signatureStruct.region = QCloudServer.Instance().region;
            signatureStruct.key = key;
            signatureStruct.httpMethod = "GET";
            signatureStruct.isHttps = true;
            signatureStruct.signDurationSecond = 600;
            signatureStruct.queryParameters = new Dictionary<string, string>();
            signatureStruct.queryParameters.Add("a", "1");
            signatureStruct.headers = new Dictionary<string, string>();
            string url = cosXml.GenerateSignURL(signatureStruct);

            Assert.NotNull(url);
        }

        [Test()]
        public void TestGenerateSignUrlWithRequestParams()
        {
            try
            {
                string key = "CITestImage.png";
                PreSignatureStruct signatureStruct = new PreSignatureStruct();

                signatureStruct.bucket = bucket;
                signatureStruct.appid = QCloudServer.Instance().appid;
                signatureStruct.region = QCloudServer.Instance().region;
                signatureStruct.key = key;
                signatureStruct.httpMethod = "GET";
                signatureStruct.isHttps = true;
                signatureStruct.signDurationSecond = 600;
                signatureStruct.queryParameters = new Dictionary<string, string>();
                //string ci_params = "watermark/3/type/3/text/dGVuY2VudCBjbG91ZA==";
                //string ci_params = "imageMogr2/thumbnail/!50p|watermark/2/text/5pWw5o2u5LiH6LGh/fill/I0ZGRkZGRg==/fontsize/30/dx/20/dy/20";
                //string ci_params = "imageMogr2/thumbnail/!50p";
                string ci_params = "";
                signatureStruct.queryParameters.Add(ci_params, null);
                signatureStruct.headers = new Dictionary<string, string>();
                string url = cosXml.GenerateSignURL(signatureStruct);
                Assert.NotNull(url);
                // TODO check
                //Assert.True(url.Contains(URLEncodeUtils.Encode(URLEncodeUtils.Encode(ci_params)).ToString().ToLower()));
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

        [Test()]
        public async Task AsyncPutObject()
        {
            PutObjectRequest request = new PutObjectRequest(bucket, commonKey, smallFileSrcPath);
            request.SetRequestHeader("Content-Type", "text/plain");

            PutObjectResult result = await cosXml.ExecuteAsync<PutObjectResult>(request);

            Assert.True(result.httpCode == 200);
            Assert.NotNull(result.eTag);
        }

        [Test()]
        public void TestListUploads()
        {
            try
            {
                ListMultiUploadsRequest request = new ListMultiUploadsRequest(bucket);
                request.SetMaxUploads("100");
                request.SetEncodingType("url");

                //执行请求
                ListMultiUploadsResult result = cosXml.ListMultiUploads(request);
                var uploads = result.listMultipartUploads;

                Assert.IsNotEmpty(result.GetResultInfo());
                Assert.NotNull(uploads.bucket);
                Assert.True(result.httpCode == 200);

                if (uploads.uploads != null && uploads.uploads.Count > 0) 
                {
                    foreach (var upload in uploads.uploads) 
                    {
                        Assert.NotNull(upload.key);
                        Assert.NotNull(upload.uploadID);
                        Assert.NotNull(upload.storageClass);
                        Assert.NotNull(upload.initiator.id);
                        Assert.NotNull(upload.initiator.displayName);
                        Assert.NotNull(upload.initiated);
                        Assert.NotNull(upload.owner.id);
                        Assert.NotNull(upload.owner.displayName);
                    }
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

        [Test()]
        public async Task AsyncMoveObject()
        {
            CopySourceStruct copySource = new CopySourceStruct(QCloudServer.Instance().appid,
                    bucket, QCloudServer.Instance().region, copyKeySmall);

            COSXMLCopyTask copyTask = new COSXMLCopyTask(bucket, commonKey, copySource);
        
            try 
            {
                // 拷贝对象
                COSXML.Transfer.COSXMLCopyTask.CopyTaskResult result = await 
                    transferManager.CopyAsync(copyTask);
                Console.WriteLine(result.GetResultInfo());

                // 删除对象
                DeleteObjectRequest request = new DeleteObjectRequest(bucket, commonKey);
                DeleteObjectResult deleteResult = cosXml.DeleteObject(request);
                // 打印结果
                Console.WriteLine(deleteResult.GetResultInfo());
            } 
            catch (Exception e) 
            {
                Console.WriteLine("CosException: " + e);
            }
        }


        [Test()]
        public void DeletePrefix()
        {
            try
            {
                String nextMarker = null;

                do
                {
                    //对象键
                    string prefix = "folder1/"; 
                    GetBucketRequest listRequest = new GetBucketRequest(bucket);
                    //获取 a/ 下的对象以及子目录
                    listRequest.SetPrefix(prefix);
                    listRequest.SetMarker(nextMarker);
                    //执行请求
                    GetBucketResult listResult = cosXml.GetBucket(listRequest);
                    //bucket的相关信息
                    ListBucket info = listResult.listBucket;
                    // 对象列表
                    List<ListBucket.Contents> objects = info.contentsList;
                    // 下一页的下标
                    nextMarker = info.nextMarker;
                    
                    DeleteMultiObjectRequest deleteRequest = new DeleteMultiObjectRequest(bucket);
                    //设置返回结果形式
                    deleteRequest.SetDeleteQuiet(false);
                    //对象key
                    List<string> deleteObjects = new List<string>();
                    foreach (var content in objects)
                    {
                        deleteObjects.Add(content.key);
                    }

                    deleteRequest.SetObjectKeys(deleteObjects);
                    //执行请求
                    DeleteMultiObjectResult deleteResult = cosXml.DeleteMultiObjects(deleteRequest);
                    //请求成功
                    Console.WriteLine(deleteResult.GetResultInfo());
                } while (nextMarker != null);
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

        [Test()]
        public void TestGenerateSignURL() {
            try {
                PreSignatureStruct preSignatureStruct = new PreSignatureStruct();
                preSignatureStruct.appid = QCloudServer.Instance().appid;
                preSignatureStruct.region = QCloudServer.Instance().region;
                preSignatureStruct.bucket = QCloudServer.Instance().bucketForObjectTest;
                preSignatureStruct.key = commonKey;
                preSignatureStruct.httpMethod = "GET";
                preSignatureStruct.isHttps = true;
                preSignatureStruct.signDurationSecond = 600;
                
                Dictionary<string, string> headers = new Dictionary<string, string>();
                headers.Add("host", QCloudServer.Instance().bucketForObjectTest 
                    + ".cos." 
                    + QCloudServer.Instance().region 
                    + ".myqcloud.com");
                preSignatureStruct.headers = headers;
                preSignatureStruct.queryParameters = null;
                string requstUrl = QCloudServer.Instance().cosXml.GenerateSignURL(preSignatureStruct);
                Assert.IsNotEmpty(requstUrl);
                /*
                GetObjectRequest request = new GetObjectRequest(null, null, "./", smallFileSrcPath);
                request.RequestURLWithSign = requstUrl;
                GetObjectResult result = QCloudServer.Instance().cosXml.GetObject(request);
                Assert.AreEqual(result.httpCode, 200);
                */
                // 错误参数测试: httpMethod为空
                preSignatureStruct.httpMethod = null;
                try {
                    requstUrl = QCloudServer.Instance().cosXml.GenerateSignURL(preSignatureStruct);
                    Assert.Fail();
                } catch (Exception) {}
                //  错误参数测试: key为空
                preSignatureStruct.httpMethod = "GET";
                preSignatureStruct.key = null;
                try {
                    requstUrl = QCloudServer.Instance().cosXml.GenerateSignURL(preSignatureStruct);
                    Assert.Fail();
                } catch (Exception) {}
                // 参数测试: https false
                preSignatureStruct.key = "key";
                preSignatureStruct.isHttps = false;
                requstUrl = QCloudServer.Instance().cosXml.GenerateSignURL(preSignatureStruct);
                Assert.IsNotEmpty(requstUrl);
                // 错误参数测试: bucket 为空
                preSignatureStruct.bucket = null;
                try {
                    requstUrl = QCloudServer.Instance().cosXml.GenerateSignURL(preSignatureStruct);
                    Assert.Fail();
                } catch (Exception) {}
                // 错误参数测试: bucket后自动拼接appid
                preSignatureStruct.bucket = "bucket";
                requstUrl = QCloudServer.Instance().cosXml.GenerateSignURL(preSignatureStruct);
                Assert.IsNotEmpty(requstUrl);
                // 参数测试: host自动入签
                preSignatureStruct.signHost = true;
                requstUrl = QCloudServer.Instance().cosXml.GenerateSignURL(preSignatureStruct);
                Assert.IsNotEmpty(requstUrl);
                // 参数测试: host主动入签
                preSignatureStruct = new PreSignatureStruct();
                preSignatureStruct.appid = QCloudServer.Instance().appid;
                preSignatureStruct.region = QCloudServer.Instance().region;
                preSignatureStruct.bucket = QCloudServer.Instance().bucketForObjectTest;
                preSignatureStruct.key = commonKey;
                preSignatureStruct.httpMethod = "GET";
                preSignatureStruct.signDurationSecond = 600;
                preSignatureStruct.signHost = true;
                preSignatureStruct.host = "cos.ap-guangzhou.myqcloud.com";
                requstUrl = QCloudServer.Instance().cosXml.GenerateSignURL(preSignatureStruct);
                Assert.IsNotEmpty(requstUrl);
                
            } catch (COSXML.CosException.CosClientException clientEx) {
                Console.WriteLine("CosClientException: " + clientEx);
                Assert.Fail();
            } catch (COSXML.CosException.CosServerException serverEx) {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.Fail();
            }
        }

        [Test()]
        public void TestGetObjectUrl() {
            try {
                string objectUrl = QCloudServer.Instance().cosXml.GetObjectUrl("bucket", "key");
                Assert.NotNull(objectUrl);
            } catch (COSXML.CosException.CosClientException clientEx) {
                Assert.Fail();
            } catch (COSXML.CosException.CosServerException serverEx) {
                Assert.Fail();
            }
        }

        [Test()]
        public void TestGenerateSign() {
            try {
                string requestSign = QCloudServer.Instance().cosXml.GenerateSign("GET", commonKey, null, null, 60, 60);
                Assert.IsNotEmpty(requestSign);
            } catch (COSXML.CosException.CosClientException clientEx) {
                Assert.Fail();
            } catch (COSXML.CosException.CosServerException serverEx) {
                Assert.Fail();
            }
        }

        [Test()]
        public void TestObjectTagging() {
            try {
                PutObjectTaggingRequest request = new PutObjectTaggingRequest(bucket, commonKey);
                request.AddTag("tag1", "val1");
                PutObjectTaggingResult result = cosXml.PutObjectTagging(request);
                request.SetVersionId("null");
                Assert.AreEqual(result.httpCode, 200);
                TestGetObjectTagging();
            } catch (COSXML.CosException.CosClientException clientEx) {
                Console.WriteLine("CosClientException: " + clientEx);
                Assert.Fail();
            } catch (COSXML.CosException.CosServerException serverEx) {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.Fail();
            }
        }

        [Test()]
        public void TestGetObjectTagging() {
            try {
                GetObjectTaggingRequest request = new GetObjectTaggingRequest(bucket, commonKey);
                GetObjectTaggingResult result = cosXml.GetObjectTagging(request);
                request.SetVersionId("null");
                Assert.AreEqual(result.httpCode, 200);
                Assert.NotNull(result.tagging);
                Assert.NotNull(result.tagging.tagSet);
                Assert.NotZero(result.tagging.tagSet.tags.Count);
                for (int i=0; i<result.tagging.tagSet.tags.Count; i++) {
                    Assert.NotNull(result.tagging.tagSet.tags[i]);
                    Assert.NotNull(result.tagging.tagSet.tags[i].key);
                    Assert.NotNull(result.tagging.tagSet.tags[i].value);
                }
                TestDeleteObjectTagging();
                
            } catch (COSXML.CosException.CosClientException clientEx) {
                Console.WriteLine("CosClientException: " + clientEx);
                Assert.Fail();
            } catch (COSXML.CosException.CosServerException serverEx) {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.Fail();
            }
        }

        [Test()]
        public void TestDeleteObjectTagging() {
            try {
                DeleteObjectTaggingRequest request = new DeleteObjectTaggingRequest(bucket, commonKey);
                request.SetVersionId("null");
                DeleteObjectTaggingResult result = cosXml.DeleteObjectTagging(request);
                Assert.AreEqual(result.httpCode, 204);
                
            } catch (COSXML.CosException.CosClientException clientEx) {
                Console.WriteLine("CosClientException: " + clientEx);
                Assert.Fail();
            } catch (COSXML.CosException.CosServerException serverEx) {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.Fail();
            }
        }

        [Test()]
        public void TestDoesObjectExist() {
            try {
                DoesObjectExistRequest request = new DoesObjectExistRequest(bucket, commonKey);
                bool result = cosXml.DoesObjectExist(request);
                request.SetVersionId("null");
                Assert.True(result);

                request = new DoesObjectExistRequest(bucket, "notexist");
                result = cosXml.DoesObjectExist(request);
                Assert.False(result);
                
            } catch (COSXML.CosException.CosClientException clientEx) {
                Console.WriteLine("CosClientException: " + clientEx);
                Assert.Fail();
            } catch (COSXML.CosException.CosServerException serverEx) {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.Fail();
            }
        }

        [Test()]
        public void TestUtils() {
            string encodedStr = DigestUtils.GetHamcSha1ToBase64("string", Encoding.UTF8, "key", Encoding.UTF8);
            Assert.IsNotEmpty(encodedStr);

            MemoryStream memoryStream = new MemoryStream();
            StreamWriter myStreamWriter = new StreamWriter(memoryStream);
            
            encodedStr = DigestUtils.GetMd5ToBase64(memoryStream);
            Assert.IsNotEmpty(encodedStr);

            encodedStr = DigestUtils.GetMD5HexString(memoryStream, 1);
            Assert.IsNotEmpty(encodedStr);

            byte[] data = new byte[] { 0x01, 0x02, 0x03 };
            encodedStr = DigestUtils.ByteArrayToHex(data);
            Assert.IsNotEmpty(encodedStr);
        }

        
    }

}

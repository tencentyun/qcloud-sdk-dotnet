using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using COSXML.Auth;
using COSXML.CosException;
using COSXML.Log;
using COSXML.Model.Bucket;
using COSXML.Model.Object;
using COSXML.Model.Tag;
using COSXML.Network;
using COSXML.Transfer;
using COSXMLTests;
using NUnit.Framework;



namespace COSXML.Utils.Tests{
    
    [TestFixture()]
    public class CosUtilsTests
    {
        
        [Test()]
        public void StringTest()
        {
            try {
                StringUtils.Compare("SD", null, true);
            }
            catch (CosClientException)
            {
            }
            StringUtils.Compare("SD", "SD", true);
            string path = StringUtils.MergePath("/asdf/../");
            Assert.AreEqual(path ,"/");

            string PathNull=null;
            Utils.URLEncodeUtils.EncodePathOfURL(PathNull);
            Utils.URLEncodeUtils.Decode(PathNull);
        }

        [Test()]
        public void LogTest()
        {
            QLog.Verbose("Test", "LogTestVerbose",new Exception("Test"));
            QLog.Verbose("Test", "LogTestVerbose");
            QLog.Error("Test", "LogTestError");
            QLog.Warn("Test", "LogTestError");
            QLog.Warn("Test", "LogTestWarn", new Exception("Test"));
            QLog.Info("Test", "LogTestInfo", new Exception("Test"));
            QLog.Info("Test", "LogTestInfo");
        }


        [Test()]
        public void ExceptionTest()
        {
            CosServerError serverError = new CosServerError();
            new CosServerException(1,"test", serverError);
        }

        public void CrcTest()
        {
            Crc64.Combine(0,0,0);
        }


        [Test()]
        public void InitTest2()
        {
            CosXmlSignSourceProvider provide = new CosXmlSignSourceProvider();
            List<string> list = new List<string>();
            list.Add("param1");
            list.Add("param2");
            provide.AddParameterKeys(list);
            
            HttpClientConfig.Builder build = new HttpClientConfig.Builder();
            build.SetMaxRetry(2);

            Response rsp = new Response();
            rsp.ContentLength = 10;
            Console.WriteLine(rsp.ContentLength);
            rsp.ContentType = "ContentType";
            Console.WriteLine(rsp.ContentType);
            
            ResponseBody rspbody = new ResponseBody();
            Console.WriteLine(rspbody.ContentLength);

            CosXmlSigner sig = new CosXmlSigner();
            try {
                sig.Sign(null, null, null);
            } catch (Exception) {
         
            }
            Request req = new Request();
            try {
                sig.Sign(req, null, null);
            } catch (Exception) {
         
            }

            CosXmlSignSourceProvider qc = new CosXmlSignSourceProvider();
            try {
                sig.Sign(req, qc, null);
            } catch (Exception) {
         
            }
            
            try {
                CosXmlSigner.GenerateSign("GET", "/", null, null, null, null, null);
            } catch (Exception) {
            }
            try {
                QCloudCredentials qerc = new QCloudCredentials("id", "key", "time");
                CosXmlSigner.GenerateSign("GET", "/", null, null, null, null, qerc);
            } catch (Exception) {
            }
        }
        [Test()]
        public void InitTest()
        {
            string bucket = "bucety";
            PutBucketPolicyRequest rep = new PutBucketPolicyRequest(bucket);
            
            PutBucketLoggingRequest log1 = new PutBucketLoggingRequest(bucket);
            Assert.Throws<CosClientException>(delegate { log1.SetTarget(null); });
            
            try {
                PutBucketLoggingRequest log = new PutBucketLoggingRequest(bucket);
                log.SetTarget(bucket, null); 
            }catch (CosClientException ex){
            }
            PutBucketLifecycleRequest life = new PutBucketLifecycleRequest(bucket);
            PutBucketPolicyRequest policy = new PutBucketPolicyRequest(bucket);
            PutBucketVersioningRequest verRe = new PutBucketVersioningRequest(bucket);
            verRe.IsEnableVersionConfig(false);
            
            try {
                IntelligentTieringConfiguration configuration = new IntelligentTieringConfiguration();
                configuration.Transition = null;
                configuration.Status = "";
            } catch (Exception) {
            }

            try {
                PutBucketIntelligentTieringRequest ter = new PutBucketIntelligentTieringRequest(bucket, null);
                ter.CheckParameters();
            } catch (Exception) {
            }
            
            PutBucketPolicyRequest polreq = new PutBucketPolicyRequest(bucket);
            
            HttpUrl hul = new HttpUrl();
            try {
                hul.Scheme = null;
            } catch (Exception e) {
                hul.Scheme = "http";
            }
            
            try {
                hul.Scheme = "httpsd";
            } catch (Exception) {
                
            }
            Console.WriteLine(hul.Scheme);

            try {
                hul.UserName = null;
            } catch (Exception e) {
               
            }
            
            hul.UserName = "UserName";
            Console.WriteLine(hul.UserName);
            try {
                hul.UserPassword = null;
            } catch (Exception e) {
            }
            hul.UserPassword = "";
            Console.WriteLine(hul.UserPassword);

            try {
                hul.Host = null;
            } catch (Exception e) {
            }
            hul.Host = "sat.com";
            Console.WriteLine(hul.Host);
            
            Assert.Throws<ArgumentException>(delegate { hul.Port = -1; });
            hul.Port = 3454;
            Console.WriteLine(hul.Port);
            hul.Fragment = "df";
            Console.WriteLine(hul.Fragment);
            hul.ToString();
            
            Request req = new Request();
            try
            { 
                req.AddHeader(null, null);
            }
            catch (Exception)
            {
                
            }
           
            req.AddHeader("cos-header", null);
            req.AddHeader("cos-header", "header");
            req.AddHeader("cos-header", "header");
            req.RequestUrlString = "";
            Assert.Throws<ArgumentNullException>(delegate { req.Url = null; });
            byte[] er = new byte[]{};
            ByteRequestBody rb = new ByteRequestBody(er);
            Console.WriteLine(rb.ProgressCallback);
            
            GetObjectRequest getObjectRequest = new GetObjectRequest("bucket", "key", "", "");
            try
            {
                getObjectRequest.GetSaveFilePath();
            }
            catch (Exception e)
            {
            }


            ObjectRequest objectRequest = new ObjectRequest("bucket", "key");
            objectRequest.Bucket = null;
            objectRequest.Key = null;
            try
            {
                objectRequest.CheckParameters();
            }
            catch (Exception e)
            {
            }
            objectRequest.SetCosServerSideEncryptionWithKMS("id", "key");


            SelectObjectRequest selectObjectRequest = new SelectObjectRequest("bucket","key");
            selectObjectRequest.SetInputFormat(null);
            Assert.Throws<CosClientException>(delegate
            {
                selectObjectRequest.CheckParameters();
            });
            selectObjectRequest.SetOutputFormat(null);
            selectObjectRequest.SetOutputFormat(new ObjectSelectionFormat());
            Assert.Throws<CosClientException>(delegate
            {
                selectObjectRequest.CheckParameters();
            });

            UploadPartCopyRequest uploadPartCopyRequest = new UploadPartCopyRequest("bucket", "key",1,null);

            try
            {
                uploadPartCopyRequest.SetCopySource(null);
            }
            catch (Exception e)
            {
            }
            
            uploadPartCopyRequest.SetCopySource(new CopySourceStruct("1","2","3","4","5"));
            Assert.Throws<CosClientException>(delegate
            {
                uploadPartCopyRequest.CheckParameters();
            });

            UploadPartCopyRequest uploadPartCopyRequesttmp = new UploadPartCopyRequest("bucket", "key",0,"Df");
            uploadPartCopyRequesttmp.SetCopySource(new CopySourceStruct("1","2","3","4","5"));
            Assert.Throws<CosClientException>(delegate
            {
                uploadPartCopyRequest.CheckParameters();
            });


            UploadPartRequest uploadPartRequest1 = new UploadPartRequest("", "", 0, "","",1, 1);
            
            Assert.Throws<CosClientException>(delegate
            {
                uploadPartRequest1.CheckParameters();
            });
            UploadPartRequest uploadPartRequest2 = new UploadPartRequest("", "", 0, "","dir",1, 1);
            Assert.Throws<CosClientException>(delegate
            {
                uploadPartRequest2.CheckParameters();
            });
            
            UploadPartRequest uploadPartRequest3 = new UploadPartRequest("", "", 0, "",".",1, 1);
            Assert.Throws<CosClientException>(delegate
            {
                uploadPartRequest3.CheckParameters();
            });

            UploadPartRequest uploadPartRequest4 = new UploadPartRequest("", "", 1, null,".",1, 1);
            Assert.Throws<CosClientException>(delegate
            {
                uploadPartRequest4.CheckParameters();
            });
        }

        [Test()]
        public void Crc64Test()
        {
            Assert.AreEqual(1,Crc64.Combine(1, 0, 0));

        }

        [Test()]
        public void DeleteFileByFileNameTest()
        {
            var fileName = "";
            var result = SystemUtils.DeleteFileByFileName(fileName);
            Assert.IsFalse(result);
        }

        [Test()]
        public void IsNeedUpdateNowTest()
        {
            DefaultSessionQCloudCredentialProvider  defaultSessionQCloudCredentialProviderTrue= new DefaultSessionQCloudCredentialProvider("","",1,"");
            Assert.True(defaultSessionQCloudCredentialProviderTrue.IsNeedUpdateNow());
        }

        [Test()]
        public void GetQCloudCredentialsTest()
        {
            try
            {
                DefaultSessionQCloudCredentialProvider defaultSessionQCloudCredentialProviderSecretIdNull =
                    new DefaultSessionQCloudCredentialProvider(null, null, 1, "");
                defaultSessionQCloudCredentialProviderSecretIdNull.GetQCloudCredentials();
            }
            catch (CosClientException ex)
            {
                Assert.True(ex.Message.Equals("secretId == null"));
            }
            try
            {
                DefaultSessionQCloudCredentialProvider defaultSessionQCloudCredentialProviderSecretKeyNull =
                    new DefaultSessionQCloudCredentialProvider("", null, 1, "");
                defaultSessionQCloudCredentialProviderSecretKeyNull.GetQCloudCredentials();
            }
            catch (CosClientException ex)
            {
                Assert.True(ex.Message.Equals("secretKey == null"));
            }

            QCloudCredentialProvider qCloudCredentialProvider = new CustomQCloudCredentialProvider();
            Assert.Null(qCloudCredentialProvider.GetQCloudCredentials());


        }

        [Test()]
        public void DoesBucketExistTest()
        {
            DoesBucketExistRequest request = new DoesBucketExistRequest(QCloudServer.Instance().bucketForObjectTest);
            CosXmlConfig config = new CosXmlConfig.Builder()
                .SetConnectionLimit(512)
                .SetEndpointSuffix("cos.accelerate.myqcloud.com")
                .Build();


            CosXmlServer cosXml = new CosXmlServer(config, null);
            try
            {
                cosXml.DoesBucketExist(request);
            }
            catch (Exception ex)
            {

            }
        }
        [Test()]
        public void DoesObjectExistTest()
        {
            DoesObjectExistRequest request = new DoesObjectExistRequest(QCloudServer.Instance().bucketForObjectTest,"key");
            CosXmlConfig config = new CosXmlConfig.Builder()
                .SetConnectionLimit(512)
                .SetEndpointSuffix("cos.accelerate.myqcloud.com")
                .Build();


            CosXmlServer cosXml = new CosXmlServer(config, null);
            try
            {
                cosXml.DoesObjectExist(request);
            }
            catch (Exception ex)
            {

            }
        }
        [Test()]
        public void GenerateSignTest()
        {
            // DoesObjectExistRequest request = new DoesObjectExistRequest(QCloudServer.Instance().bucketForObjectTest,"key");
            CosXmlConfig config = new CosXmlConfig.Builder()
                .SetConnectionLimit(512)
                .SetEndpointSuffix("cos.accelerate.myqcloud.com")
                .Build();


            CosXmlServer cosXml = new CosXmlServer(config, null);
            try
            {
                cosXml.GenerateSign(null, null, null, null, 0, 0);
                // cosXml.DoesObjectExist(request);
            }
            catch (CosClientException ex)
            {

            }
        }
    }
    
    
    
}

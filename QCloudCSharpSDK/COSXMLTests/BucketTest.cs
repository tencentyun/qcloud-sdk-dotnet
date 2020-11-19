using COSXML.Common;
using COSXML.CosException;
using COSXML.Model;
using COSXML.Model.Bucket;
using COSXML.Model.Tag;
using COSXML.Utils;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace COSXMLTests
{

    [TestFixture()]
    public class BucketTest
    {

        private Boolean isBucketCreatedByTest = false;

        public void PutBucket(COSXML.CosXml cosXml, string bucket)
        {

            try
            {
                PutBucketRequest request = new PutBucketRequest(bucket);


                // 添加acl
                request.SetCosACL(CosACL.PUBLIC_READ);

                COSXML.Model.Tag.GrantAccount readAccount = new COSXML.Model.Tag.GrantAccount();

                readAccount.AddGrantAccount("1131975903", "1131975903");
                request.SetXCosGrantRead(readAccount);

                COSXML.Model.Tag.GrantAccount writeAccount = new COSXML.Model.Tag.GrantAccount();

                writeAccount.AddGrantAccount("1131975903", "1131975903");
                request.SetXCosGrantWrite(writeAccount);

                COSXML.Model.Tag.GrantAccount fullAccount = new COSXML.Model.Tag.GrantAccount();

                fullAccount.AddGrantAccount("2832742109", "2832742109");
                request.SetXCosReadWrite(fullAccount);

                //执行请求
                PutBucketResult result = cosXml.PutBucket(request);


                Console.WriteLine(result.GetResultInfo());

                isBucketCreatedByTest = true;
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.Message);
                Assert.Fail();
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());

                if (serverEx.statusCode != 409)
                {
                    Assert.Fail();
                }
            }
        }

        public void HeadBucket(COSXML.CosXml cosXml, string bucket)
        {

            try
            {
                HeadBucketRequest request = new HeadBucketRequest(bucket);


                //执行请求
                HeadBucketResult result = cosXml.HeadBucket(request);

                Console.WriteLine(result.GetResultInfo());
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


        public void GetBucket(COSXML.CosXml cosXml, string bucket)
        {

            try
            {
                GetBucketRequest request = new GetBucketRequest(bucket);


                request.SetPrefix("a/中文/d");

                List<string> headerKeys = new List<string>();

                headerKeys.Add("Host");


                List<string> queryParameters = new List<string>();

                queryParameters.Add("prefix");
                queryParameters.Add("max-keys");


                //执行请求
                GetBucketResult result = cosXml.GetBucket(request);

                Assert.True(result.httpCode == 200);
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
        public void GetBucket()
        {

            try
            {
                QCloudServer instance = QCloudServer.Instance();


                GetBucketRequest request = new GetBucketRequest(instance.bucketForBucketTest);


                request.SetDelimiter("/");

                List<string> headerKeys = new List<string>();

                headerKeys.Add("Host");


                List<string> queryParameters = new List<string>();

                queryParameters.Add("prefix");
                queryParameters.Add("max-keys");
                //执行请求
                GetBucketResult result = instance.cosXml.GetBucket(request);

                Console.WriteLine(result.listBucket.commonPrefixesList.ToArray().Length);
                Assert.True(result.httpCode == 200);
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

        public void PutBucketACL(COSXML.CosXml cosXml, string bucket)
        {

            try
            {
                PutBucketACLRequest request = new PutBucketACLRequest(bucket);



                //添加acl
                request.SetCosACL(CosACL.PUBLIC_READ);

                COSXML.Model.Tag.GrantAccount readAccount = new COSXML.Model.Tag.GrantAccount();

                readAccount.AddGrantAccount("1131975903", "1131975903");
                request.SetXCosGrantRead(readAccount);

                COSXML.Model.Tag.GrantAccount writeAccount = new COSXML.Model.Tag.GrantAccount();

                writeAccount.AddGrantAccount("1131975903", "1131975903");
                request.SetXCosGrantWrite(writeAccount);

                COSXML.Model.Tag.GrantAccount fullAccount = new COSXML.Model.Tag.GrantAccount();

                fullAccount.AddGrantAccount("2832742109", "2832742109");
                request.SetXCosReadWrite(fullAccount);

                //执行请求
                PutBucketACLResult result = cosXml.PutBucketACL(request);

                Console.WriteLine(result.GetResultInfo());
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

        public void GetBucketACL(COSXML.CosXml cosXml, string bucket)
        {

            try
            {
                GetBucketACLRequest request = new GetBucketACLRequest(bucket);


                //执行请求
                GetBucketACLResult result = cosXml.GetBucketACL(request);

                Console.WriteLine(result.GetResultInfo());
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

        public void PutBucketCORS(COSXML.CosXml cosXml, string bucket)
        {

            try
            {
                PutBucketCORSRequest request = new PutBucketCORSRequest(bucket);


                //设置cors
                COSXML.Model.Tag.CORSConfiguration.CORSRule corsRule = new COSXML.Model.Tag.CORSConfiguration.CORSRule();


                corsRule.id = "corsconfigure1";
                corsRule.maxAgeSeconds = 6000;
                corsRule.allowedOrigin = "http://cloud.tencent.com";

                corsRule.allowedMethods = new List<string>();
                corsRule.allowedMethods.Add("PUT");
                corsRule.allowedMethods.Add("DELETE");
                corsRule.allowedMethods.Add("POST");

                corsRule.allowedHeaders = new List<string>();
                corsRule.allowedHeaders.Add("Host");
                corsRule.allowedHeaders.Add("Authorizaiton");
                corsRule.allowedHeaders.Add("User-Agent");

                corsRule.exposeHeaders = new List<string>();
                corsRule.exposeHeaders.Add("x-cos-meta-x1");
                corsRule.exposeHeaders.Add("x-cos-meta-x2");

                request.SetCORSRule(corsRule);

                //执行请求
                PutBucketCORSResult result = cosXml.PutBucketCORS(request);


                Console.WriteLine(result.GetResultInfo());
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

        public void GetBucketCORS(COSXML.CosXml cosXml, string bucket)
        {

            try
            {
                GetBucketCORSRequest request = new GetBucketCORSRequest(bucket);


                //执行请求
                GetBucketCORSResult result = cosXml.GetBucketCORS(request);


                Console.WriteLine(result.GetResultInfo());
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

        public void DeleteBucketCORS(COSXML.CosXml cosXml, string bucket)
        {

            try
            {
                DeleteBucketCORSRequest request = new DeleteBucketCORSRequest(bucket);


                //执行请求
                DeleteBucketCORSResult result = cosXml.DeleteBucketCORS(request);


                Console.WriteLine(result.GetResultInfo());
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

        public void PutBucketLifeCycle(COSXML.CosXml cosXml, string bucket)
        {

            try
            {
                PutBucketLifecycleRequest request = new PutBucketLifecycleRequest(bucket);


                //设置 lifecycle
                COSXML.Model.Tag.LifecycleConfiguration.Rule rule = new COSXML.Model.Tag.LifecycleConfiguration.Rule();

                rule.id = "lfiecycleConfigure2";
                //Enabled，Disabled
                //Enabled，Disabled
                rule.status = "Enabled";

                rule.filter = new COSXML.Model.Tag.LifecycleConfiguration.Filter();
                rule.filter.prefix = "2/";

                rule.abortIncompleteMultiUpload = new COSXML.Model.Tag.LifecycleConfiguration.AbortIncompleteMultiUpload();
                rule.abortIncompleteMultiUpload.daysAfterInitiation = 2;

                request.SetRule(rule);

                //执行请求
                PutBucketLifecycleResult result = cosXml.PutBucketLifecycle(request);


                Console.WriteLine(result.GetResultInfo());
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

        public void GetBucketLifeCycle(COSXML.CosXml cosXml, string bucket)
        {

            try
            {
                GetBucketLifecycleRequest request = new GetBucketLifecycleRequest(bucket);


                //执行请求
                GetBucketLifecycleResult result = cosXml.GetBucketLifecycle(request);


                Console.WriteLine(result.GetResultInfo());
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

        public void DeleteBucketLifeCycle(COSXML.CosXml cosXml, string bucket)
        {

            try
            {
                DeleteBucketLifecycleRequest request = new DeleteBucketLifecycleRequest(bucket);


                //执行请求
                DeleteBucketLifecycleResult result = cosXml.DeleteBucketLifecycle(request);


                Console.WriteLine(result.GetResultInfo());
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

        public void PutBucketReplication(COSXML.CosXml cosXml, string bucket)
        {

            try
            {
                PutBucketReplicationRequest request = new PutBucketReplicationRequest(bucket);


                //设置replication
                PutBucketReplicationRequest.RuleStruct ruleStruct = new PutBucketReplicationRequest.RuleStruct();

                ruleStruct.appid = "";
                ruleStruct.bucket = "";
                ruleStruct.region = "";
                ruleStruct.isEnable = true;
                ruleStruct.storageClass = "STANDARD";
                ruleStruct.id = "";
                ruleStruct.prefix = "";
                List<PutBucketReplicationRequest.RuleStruct> ruleStructs = new List<PutBucketReplicationRequest.RuleStruct>();


                request.SetReplicationConfiguration("2832742109", "2832742109", ruleStructs);

                //执行请求
                PutBucketReplicationResult result = cosXml.PutBucketReplication(request);


                Console.WriteLine(result.GetResultInfo());
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.Message);
                //Assert.Fail();
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                //Assert.Fail();
            }

        }

        public void GetBucketReplication(COSXML.CosXml cosXml, string bucket)
        {

            try
            {
                GetBucketReplicationRequest request = new GetBucketReplicationRequest(bucket);


                //执行请求
                GetBucketReplicationResult result = cosXml.GetBucketReplication(request);



                Console.WriteLine(result.GetResultInfo());
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.Message);
                //Assert.Fail();
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                // Assert.Fail();
            }

        }

        public void DeleteBucketReplication(COSXML.CosXml cosXml, string bucket)
        {

            try
            {
                DeleteBucketReplicationRequest request = new DeleteBucketReplicationRequest(bucket);


                //执行请求
                DeleteBucketReplicationResult result = cosXml.DeleteBucketReplication(request);


                Console.WriteLine(result.GetResultInfo());
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

        public void PutBucketVersioning(COSXML.CosXml cosXml, string bucket)
        {

            try
            {
                PutBucketVersioningRequest request = new PutBucketVersioningRequest(bucket);



                //开启版本控制
                request.IsEnableVersionConfig(true);

                //执行请求
                PutBucketVersioningResult result = cosXml.PutBucketVersioning(request);


                Console.WriteLine(result.GetResultInfo());
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

        public void GetBucketVersioning(COSXML.CosXml cosXml, string bucket)
        {

            try
            {
                GetBucketVersioningRequest request = new GetBucketVersioningRequest(bucket);


                //执行请求
                GetBucketVersioningResult result = cosXml.GetBucketVersioning(request);


                Console.WriteLine(result.GetResultInfo());
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

        public void ListBucketVersions(COSXML.CosXml cosXml, string bucket)
        {

            try
            {
                ListBucketVersionsRequest request = new ListBucketVersionsRequest(bucket);


                //执行请求
                ListBucketVersionsResult result = cosXml.ListBucketVersions(request);


                Assert.True(result.httpCode == 200);
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

        public void ListMultiUploads(COSXML.CosXml cosXml, string bucket)
        {

            try
            {
                ListMultiUploadsRequest request = new ListMultiUploadsRequest(bucket);


                //执行请求
                ListMultiUploadsResult result = cosXml.ListMultiUploads(request);


                Assert.True(result.httpCode == 200);
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

        public void DeleteBucket(COSXML.CosXml cosXml, string bucket)
        {

            try
            {
                DeleteBucketRequest request = new DeleteBucketRequest(bucket);

                //执行请求
                DeleteBucketResult result = cosXml.DeleteBucket(request);

                Console.WriteLine(result.GetResultInfo());
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

        [SetUp()]
        public void Setup()
        {
            QCloudServer instance = QCloudServer.Instance();

            PutBucket(instance.cosXml, instance.bucketForBucketTest);
        }

        [TearDown()]
        public void Clear()
        {
            QCloudServer instance = QCloudServer.Instance();

            if (isBucketCreatedByTest)
            {
                // DeleteBucket(instance.cosXml, instance.bucketForBucketTest);
            }
        }

        [Test()]
        public void TestBucketTagging()
        {
            QCloudServer instance = QCloudServer.Instance();

            try
            {
                // 设置 tag
                PutBucketTaggingRequest request = new PutBucketTaggingRequest(
                    instance.bucketForBucketTest);


                string akey = "aTagKey";
                string avalue = "aTagValue";
                string bkey = "bTagKey";
                string bvalue = "bTagValue";

                request.AddTag(akey, avalue);
                request.AddTag(bkey, bvalue);

                PutBucketTaggingResult result = instance.cosXml.PutBucketTagging(request);

                // 获取 tag
                GetBucketTaggingRequest getRequest = new GetBucketTaggingRequest(
                    instance.bucketForBucketTest);

                GetBucketTaggingResult getResult = instance.cosXml.GetBucketTagging(getRequest);


                Tagging tagging = getResult.tagging;

                Assert.AreEqual(tagging.tagSet.tags.Count, 2);

                foreach (Tagging.Tag tag in tagging.tagSet.tags)
                {

                    if (tag.key.Equals(akey))
                    {
                        Assert.AreEqual(avalue, tag.value);
                    }
                    else if (tag.key.Equals(bkey))
                    {
                        Assert.AreEqual(bvalue, tag.value);
                    }
                    else
                    {
                        Assert.Fail();
                    }
                }

                // 删除 tag
                DeleteBucketTaggingRequest deleteRequest = new DeleteBucketTaggingRequest(instance.bucketForBucketTest);

                DeleteBucketTaggingResult deleteResult = instance.cosXml.DeleteBucketTagging(deleteRequest);

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

            try
            {
                // 验证删除成功
                GetBucketTaggingRequest getRequest = new GetBucketTaggingRequest(
                    instance.bucketForBucketTest);

                GetBucketTaggingResult getResult = instance.cosXml.GetBucketTagging(getRequest);
                Assert.Fail();
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.Message);
                Assert.Fail();
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Assert.AreEqual(serverEx.statusCode, 404);
            }
        }

        [Test()]
        public void TestBucketDomain()
        {
            QCloudServer instance = QCloudServer.Instance();

            try
            {
                GetBucketDomainResult getResult = instance.cosXml.GetBucketDomain(
                    new GetBucketDomainRequest(instance.bucketForBucketTest));

                Assert.IsNotNull(getResult.domainConfiguration.rule);
                Assert.IsNull(getResult.domainConfiguration.rule.Name);

                DomainConfiguration domain = new DomainConfiguration();

                domain.rule = new DomainConfiguration.DomainRule();
                domain.rule.Name = "www.qq.com";
                domain.rule.Status = "ENABLED";
                domain.rule.Type = "WEBSITE";


                PutBucketDomainResult result = instance.cosXml.PutBucketDomain(new PutBucketDomainRequest(
                    instance.bucketForBucketTest, domain));


            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.Message);
                Assert.Fail();
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());

                if (serverEx.statusCode != 409 && serverEx.statusCode != 451)
                {
                    Assert.Fail();
                }
            }
        }

        [Test()]
        public void TestBucketLogging()
        {
            QCloudServer instance = QCloudServer.Instance();

            try
            {
                PutBucketLoggingRequest request = new PutBucketLoggingRequest(instance.bucketForBucketTest);

                request.SetTarget("bucket-cssg-source-1253653367", "/abc");
                PutBucketLoggingResult putResult = instance.cosXml.PutBucketLogging(request);


                Assert.IsTrue(putResult.httpCode == 200);

                GetBucketLoggingResult getResult = instance.cosXml.GetBucketLogging(
                    new GetBucketLoggingRequest(instance.bucketForBucketTest));

                BucketLoggingStatus status = getResult.bucketLoggingStatus;

                if (status != null && status.loggingEnabled != null)
                {
                    string targetBucket = status.loggingEnabled.targetBucket;
                    string targetPrefix = status.loggingEnabled.targetPrefix;
                    Assert.NotNull(targetBucket);
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

                if (serverEx.statusCode != 409 && serverEx.statusCode != 451)
                {
                    Assert.Fail();
                }
            }
        }

        [Test()]
        public void TestBucketWebsite()
        {
            QCloudServer instance = QCloudServer.Instance();

            try
            {
                PutBucketWebsiteRequest putRequest = new PutBucketWebsiteRequest(instance.bucketForBucketTest);

                putRequest.SetIndexDocument("index.html");
                putRequest.SetErrorDocument("eroror.html");
                putRequest.SetRedirectAllRequestTo("https");
                PutBucketWebsiteResult putResult = instance.cosXml.PutBucketWebsite(putRequest);

                Assert.IsTrue(putResult.httpCode == 200);

                GetBucketWebsiteRequest getRequest = new GetBucketWebsiteRequest(instance.bucketForBucketTest);

                GetBucketWebsiteResult getResult = instance.cosXml.GetBucketWebsite(getRequest);

                WebsiteConfiguration configuration = getResult.websiteConfiguration;

                Assert.NotNull(configuration);

                DeleteBucketWebsiteRequest deleteRequest = new DeleteBucketWebsiteRequest(instance.bucketForBucketTest);

                DeleteBucketWebsiteResult deleteResult = instance.cosXml.DeleteBucketWebsite(deleteRequest);

                Console.WriteLine(deleteResult.GetResultInfo());
                Assert.NotNull(deleteResult.GetResultInfo());

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
        public void TestBucketInventory()
        {
            QCloudServer instance = QCloudServer.Instance();

            try
            {
                string inventoryId = "id1";


                PutBucketInventoryRequest putRequest = new PutBucketInventoryRequest(instance.bucketForBucketTest, inventoryId);

                putRequest.SetDestination("CSV", "1278687956", "bucket-cssg-source-1253653367", instance.region, "list1");
                putRequest.IsEnable(true);
                putRequest.SetScheduleFrequency("Daily");
                putRequest.SetIncludedObjectVersions("All");
                PutBucketInventoryResult putResult = instance.cosXml.PutBucketInventory(putRequest);

                Assert.IsTrue(putResult.httpCode == 200);

                GetBucketInventoryRequest getRequest = new GetBucketInventoryRequest(instance.bucketForBucketTest);

                getRequest.SetInventoryId(inventoryId);
                GetBucketInventoryResult getResult = instance.cosXml.GetBucketInventory(getRequest);

                InventoryConfiguration configuration = getResult.inventoryConfiguration;

                Assert.NotNull(configuration);

                DeleteBucketInventoryRequest deleteRequest = new DeleteBucketInventoryRequest(instance.bucketForBucketTest);

                deleteRequest.SetInventoryId(inventoryId);
                DeleteBucketInventoryResult deleteResult = instance.cosXml.DeleteBucketInventory(deleteRequest);
                Assert.IsTrue(putResult.httpCode == 200);

            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.Message);
                Assert.Fail();
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());

                if (serverEx.statusCode != 409 && serverEx.statusCode != 451)
                {
                    Assert.Fail();
                }
            }
        }

        [Test()]
        public void TestBucketIntelligentTiering()
        {
            var instance = QCloudServer.Instance();


            // Query
            GetBucketIntelligentTieringRequest getRequest = new GetBucketIntelligentTieringRequest(instance.bucketForBucketTest);

            var getResult = instance.cosXml.GetBucketIntelligentTieringConfiguration(getRequest);

            Console.WriteLine(getResult.GetResultInfo());

            if (getResult.configuration == null || !getResult.configuration.IsEnabled())
            {
                IntelligentTieringConfiguration configuration = new IntelligentTieringConfiguration();

                configuration.Days = 60;
                PutBucketIntelligentTieringRequest putRequest = new PutBucketIntelligentTieringRequest(instance.bucketForBucketTest, configuration);

                var putResult = instance.cosXml.PutBucketIntelligentTiering(putRequest);

                Assert.AreEqual(putResult.httpCode, 200);

                getRequest = new GetBucketIntelligentTieringRequest(instance.bucketForBucketTest);
                getResult = instance.cosXml.GetBucketIntelligentTieringConfiguration(getRequest);
                Assert.AreEqual(getResult.configuration.Status, configuration.Status);
                Assert.AreEqual(getResult.configuration.Days, configuration.Days);
            }
        }

        [Test()]
        public void TestBucket()
        {
            QCloudServer instance = QCloudServer.Instance();


            HeadBucket(instance.cosXml, instance.bucketForBucketTest);

            GetBucket(instance.cosXml, instance.bucketForBucketTest);

            PutBucketACL(instance.cosXml, instance.bucketForBucketTest);
            GetBucketACL(instance.cosXml, instance.bucketForBucketTest);

            DeleteBucketCORS(instance.cosXml, instance.bucketForBucketTest);
            Thread.Sleep(300);
            PutBucketCORS(instance.cosXml, instance.bucketForBucketTest);
            Thread.Sleep(300);
            GetBucketCORS(instance.cosXml, instance.bucketForBucketTest);

            PutBucketLifeCycle(instance.cosXml, instance.bucketForBucketTest);
            Thread.Sleep(1000);
            GetBucketLifeCycle(instance.cosXml, instance.bucketForBucketTest);
            DeleteBucketLifeCycle(instance.cosXml, instance.bucketForBucketTest);

            PutBucketReplication(instance.cosXml, instance.bucketForBucketTest);
            GetBucketReplication(instance.cosXml, instance.bucketForBucketTest);
            DeleteBucketReplication(instance.cosXml, instance.bucketForBucketTest);

            PutBucketVersioning(instance.cosXml, instance.bucketForBucketTest);
            GetBucketVersioning(instance.cosXml, instance.bucketForBucketTest);

            ListBucketVersions(instance.cosXml, instance.bucketForBucketTest);

            ListMultiUploads(instance.cosXml, instance.bucketForBucketTest);


        }



    }
}

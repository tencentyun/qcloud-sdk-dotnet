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

    [TestFixture]
    public class BucketTest
    {

        private Boolean isBucketCreatedByTest = false;

        internal COSXML.CosXml cosXml;
        internal string bucket;
        internal string region;

        [OneTimeSetUp]
        public void Setup()
        {
            cosXml = QCloudServer.Instance().cosXml;
            bucket = QCloudServer.Instance().bucketForBucketTest;
            region = QCloudServer.Instance().region;

            PutBucket();
            Thread.Sleep(100);
        }

        [OneTimeTearDown]
        public void Clear()
        {
            if (isBucketCreatedByTest)
            {
                DeleteBucket();
            }
        }

        private void PutBucket()
        {

            try
            {
                PutBucketRequest request = new PutBucketRequest(bucket);
                QCloudServer.SetRequestACLData(request);

                //执行请求
                PutBucketResult result = cosXml.PutBucket(request);

                Assert.AreEqual(result.httpCode, 200);

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

        private void DeleteBucket()
        {

            try
            {
                DeleteBucketRequest request = new DeleteBucketRequest(bucket);

                //执行请求
                DeleteBucketResult result = cosXml.DeleteBucket(request);

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
        public void HeadBucket()
        {
            try
            {
                HeadBucketRequest request = new HeadBucketRequest(bucket);

                //执行请求
                HeadBucketResult result = cosXml.HeadBucket(request);

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

        [Test()]
        public void TestBucketACL()
        {
            PutBucketACL();
            GetBucketACL();
        }

        private void PutBucketACL()
        {

            try
            {
                PutBucketACLRequest request = new PutBucketACLRequest(bucket);
                QCloudServer.SetRequestACLData(request);

                //执行请求
                PutBucketACLResult result = cosXml.PutBucketACL(request);

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

        private void GetBucketACL()
        {

            try
            {
                GetBucketACLRequest request = new GetBucketACLRequest(bucket);

                //执行请求
                GetBucketACLResult result = cosXml.GetBucketACL(request);

                AccessControlPolicy acl = result.accessControlPolicy;

                // Console.WriteLine(result.GetResultInfo());
                Assert.IsNotEmpty(result.GetResultInfo());

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
        public void TestBucketCORS()
        {
            DeleteBucketCORS();
            PutBucketCORS();
            Thread.Sleep(100);
            QCloudServer.TestWithServerFailTolerance(() => GetBucketCORS());
        }

        private void PutBucketCORS()
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

                corsRule.exposeHeaders = new List<string>();
                corsRule.exposeHeaders.Add("x-cos-meta-x1");
                corsRule.exposeHeaders.Add("x-cos-meta-x2");

                request.SetCORSRule(corsRule);

                //执行请求
                PutBucketCORSResult result = cosXml.PutBucketCORS(request);

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

        private void GetBucketCORS()
        {

            try
            {
                GetBucketCORSRequest request = new GetBucketCORSRequest(bucket);


                //执行请求
                GetBucketCORSResult result = cosXml.GetBucketCORS(request);
                var configuration = result.corsConfiguration;

                // Console.WriteLine(result.GetResultInfo());
                Assert.IsNotEmpty(result.GetResultInfo());

                Assert.AreEqual(result.httpCode, 200);

                Assert.NotNull(configuration.corsRules);
                Assert.NotZero(configuration.corsRules.Count);
                Assert.NotNull(configuration.corsRules[0].id);

                Assert.NotNull(configuration.corsRules[0].allowedOrigins);
                Assert.NotZero(configuration.corsRules[0].allowedOrigins.Count);
                Assert.NotNull(configuration.corsRules[0].allowedOrigins[0]);

                Assert.NotNull(configuration.corsRules[0].allowedHeaders);
                Assert.NotZero(configuration.corsRules[0].allowedHeaders.Count);
                Assert.NotNull(configuration.corsRules[0].allowedHeaders[0]);

                Assert.NotNull(configuration.corsRules[0].allowedMethods);
                Assert.NotZero(configuration.corsRules[0].allowedMethods.Count);
                Assert.NotNull(configuration.corsRules[0].allowedMethods[0]);

                Assert.NotNull(configuration.corsRules[0].exposeHeaders);
                Assert.NotZero(configuration.corsRules[0].exposeHeaders.Count);
                Assert.NotNull(configuration.corsRules[0].exposeHeaders[0]);

                Assert.NotZero(configuration.corsRules[0].maxAgeSeconds);
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

        private void DeleteBucketCORS()
        {

            try
            {
                DeleteBucketCORSRequest request = new DeleteBucketCORSRequest(bucket);

                //执行请求
                DeleteBucketCORSResult result = cosXml.DeleteBucketCORS(request);

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
        public void TestBucketLifecycle()
        {
            PutBucketLifeCycle();
            Thread.Sleep(100);
            QCloudServer.TestWithServerFailTolerance(() =>
            {
                GetBucketLifeCycle();
            });
            DeleteBucketLifeCycle();
        }

        private void PutBucketLifeCycle()
        {

            try
            {
                PutBucketLifecycleRequest request = new PutBucketLifecycleRequest(bucket);

                var filter = new COSXML.Model.Tag.LifecycleConfiguration.Filter();
                filter.filterAnd = new LifecycleConfiguration.FilterAnd();
                filter.filterAnd.prefix = "dir/";
                filter.filterAnd.tags = new List<Tagging.Tag>();
                filter.filterAnd.tags.Add(new Tagging.Tag("key1", "value1"));

                //设置 lifecycle
                COSXML.Model.Tag.LifecycleConfiguration.Rule transitionRule1 = new COSXML.Model.Tag.LifecycleConfiguration.Rule();

                // rule 1
                transitionRule1.id = "transitionRule1";
                //Enabled，Disabled
                transitionRule1.status = "Enabled";
                transitionRule1.filter = filter;
                transitionRule1.transition = new LifecycleConfiguration.Transition();
                transitionRule1.transition.days = 5;
                transitionRule1.transition.storageClass = "ARCHIVE";
                request.SetRule(transitionRule1);

                // rule 2
                COSXML.Model.Tag.LifecycleConfiguration.Rule expirationRule2 = new COSXML.Model.Tag.LifecycleConfiguration.Rule();
                expirationRule2.id = "expirationRule2";
                expirationRule2.status = "Enabled";
                expirationRule2.filter = filter;
                expirationRule2.expiration = new LifecycleConfiguration.Expiration();
                expirationRule2.expiration.date = "2021-12-14T00:00:00+08:00";
                request.SetRule(expirationRule2);

                // rule 3
                COSXML.Model.Tag.LifecycleConfiguration.Rule expirationRule3 = new COSXML.Model.Tag.LifecycleConfiguration.Rule();
                expirationRule3.id = "expirationRule3";
                expirationRule3.status = "Enabled";
                expirationRule3.expiration = new LifecycleConfiguration.Expiration();
                // expiredObjectDeleteMarker 的时候 filter 不能有 tag
                expirationRule3.expiration.expiredObjectDeleteMarker = true;
                request.SetRule(expirationRule3);

                // rule 4
                COSXML.Model.Tag.LifecycleConfiguration.Rule abortRule4 = new COSXML.Model.Tag.LifecycleConfiguration.Rule();
                abortRule4.id = "abortRule4";
                abortRule4.status = "Enabled";
                abortRule4.abortIncompleteMultiUpload = new COSXML.Model.Tag.LifecycleConfiguration.AbortIncompleteMultiUpload();
                abortRule4.abortIncompleteMultiUpload.daysAfterInitiation = 2;
                request.SetRule(abortRule4);

                // rule 5
                COSXML.Model.Tag.LifecycleConfiguration.Rule ncve1 = new COSXML.Model.Tag.LifecycleConfiguration.Rule();
                ncve1.id = "noncurrentVersionExpiration1";
                ncve1.status = "Enabled";
                ncve1.noncurrentVersionExpiration = new LifecycleConfiguration.NoncurrentVersionExpiration();
                ncve1.noncurrentVersionExpiration.noncurrentDays = 2;
                request.SetRule(ncve1);

                // rule 6
                COSXML.Model.Tag.LifecycleConfiguration.Rule ncvt1 = new COSXML.Model.Tag.LifecycleConfiguration.Rule();
                ncvt1.id = "noncurrentVersionTransition1";
                ncvt1.status = "Enabled";
                ncvt1.noncurrentVersionTransition = new LifecycleConfiguration.NoncurrentVersionTransition();
                ncvt1.noncurrentVersionTransition.noncurrentDays = 2;
                ncvt1.noncurrentVersionTransition.storageClass = "ARCHIVE";
                request.SetRule(ncvt1);

                //执行请求
                PutBucketLifecycleResult result = cosXml.PutBucketLifecycle(request);

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

        private void GetBucketLifeCycle()
        {

            try
            {
                GetBucketLifecycleRequest request = new GetBucketLifecycleRequest(bucket);


                //执行请求
                GetBucketLifecycleResult result = cosXml.GetBucketLifecycle(request);
                var lifecycle = result.lifecycleConfiguration;

                // Console.WriteLine(result.GetResultInfo());
                Assert.IsNotEmpty((result.GetResultInfo()));

                Assert.AreEqual(result.httpCode, 200);

                Assert.NotNull(lifecycle);
                Assert.AreEqual(lifecycle.rules.Count, 6);


                Assert.AreEqual(lifecycle.rules[0].status, "Enabled");
                Assert.NotNull(lifecycle.rules[0].filter);
                Assert.NotNull(lifecycle.rules[0].filter.filterAnd);
                Assert.NotNull(lifecycle.rules[0].filter.filterAnd.prefix);
                Assert.NotNull(lifecycle.rules[0].filter.filterAnd.tags);
                Assert.NotNull(lifecycle.rules[0].filter.filterAnd.tags[0].key);
                Assert.NotNull(lifecycle.rules[0].filter.filterAnd.tags[0].value);
                Assert.NotNull(lifecycle.rules[0].transition);
                Assert.NotZero(lifecycle.rules[0].transition.days);
                Assert.NotNull(lifecycle.rules[0].transition.storageClass);

                Assert.NotNull(lifecycle.rules[1].expiration);
                Assert.NotNull(lifecycle.rules[1].expiration.date);

                Assert.NotNull(lifecycle.rules[2].expiration);
                Assert.True(lifecycle.rules[2].expiration.expiredObjectDeleteMarker);

                Assert.NotNull(lifecycle.rules[3].abortIncompleteMultiUpload);
                Assert.NotZero(lifecycle.rules[3].abortIncompleteMultiUpload.daysAfterInitiation);

                Assert.NotNull(lifecycle.rules[4].noncurrentVersionExpiration);
                Assert.NotZero(lifecycle.rules[4].noncurrentVersionExpiration.noncurrentDays);

                Assert.NotNull(lifecycle.rules[5].noncurrentVersionTransition);
                Assert.NotZero(lifecycle.rules[5].noncurrentVersionTransition.noncurrentDays);
                Assert.NotNull(lifecycle.rules[5].noncurrentVersionTransition.storageClass);
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

        private void DeleteBucketLifeCycle()
        {

            try
            {
                DeleteBucketLifecycleRequest request = new DeleteBucketLifecycleRequest(bucket);

                //执行请求
                DeleteBucketLifecycleResult result = cosXml.DeleteBucketLifecycle(request);


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
        public void TestBucketReplication()
        {
            PutBucketVersioning(true);
            Thread.Sleep(200);
            QCloudServer.TestWithServerFailTolerance(() =>
            {
                GetBucketVersioning();
            }
            );

            PutBucketReplication();
            Thread.Sleep(100);
            QCloudServer.TestWithServerFailTolerance(() =>
            {
                GetBucketReplication();
            }
            );
            DeleteBucketReplication();

            PutBucketVersioning(false);
        }

        private void PutBucketVersioning(bool enable)
        {
            try
            {
                PutBucketVersioningRequest request = new PutBucketVersioningRequest(bucket);

                //开启版本控制
                request.IsEnableVersionConfig(enable);

                //执行请求
                PutBucketVersioningResult result = cosXml.PutBucketVersioning(request);


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

        private void GetBucketVersioning()
        {

            try
            {
                GetBucketVersioningRequest request = new GetBucketVersioningRequest(bucket);

                //执行请求
                GetBucketVersioningResult result = cosXml.GetBucketVersioning(request);
                var versioning = result.versioningConfiguration;

                // Console.WriteLine(result.GetResultInfo());
                Assert.IsNotEmpty((result.GetResultInfo()));

                Assert.AreEqual(result.httpCode, 200);

                Assert.NotNull(versioning);
                Assert.Contains(versioning.status, new List<String>() { "Enabled", "Suspended" });
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

        private void PutBucketReplication()
        {

            try
            {
                PutBucketReplicationRequest request = new PutBucketReplicationRequest(bucket);


                //设置replication
                PutBucketReplicationRequest.RuleStruct ruleStruct = new PutBucketReplicationRequest.RuleStruct();

                ruleStruct.appid = QCloudServer.Instance().appid;
                ruleStruct.bucket = QCloudServer.Instance().bucketVersioning;
                ruleStruct.region = QCloudServer.Instance().regionForBucketVersioning;
                ruleStruct.isEnable = true;
                ruleStruct.storageClass = "STANDARD";
                ruleStruct.id = "replication1";
                ruleStruct.prefix = "dir/";

                request.SetReplicationConfiguration("2832742109", "2832742109", new List<PutBucketReplicationRequest.RuleStruct>() { ruleStruct });

                //执行请求
                PutBucketReplicationResult result = cosXml.PutBucketReplication(request);

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

        private void GetBucketReplication()
        {

            try
            {
                GetBucketReplicationRequest request = new GetBucketReplicationRequest(bucket);


                //执行请求
                GetBucketReplicationResult result = cosXml.GetBucketReplication(request);
                var replication = result.replicationConfiguration;

                // Console.WriteLine(result.GetResultInfo());
                Assert.IsNotEmpty((result.GetResultInfo()));

                Assert.AreEqual(result.httpCode, 200);

                Assert.NotNull(replication.role);
                Assert.NotZero(replication.rules.Count);

                Assert.NotNull(replication.rules[0].id);
                Assert.NotNull(replication.rules[0].prefix);
                Assert.AreEqual(replication.rules[0].status, "Enabled");

                Assert.NotNull(replication.rules[0].destination);
                Assert.NotNull(replication.rules[0].destination.bucket);
                Assert.NotNull(replication.rules[0].destination.storageClass);

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

        private void DeleteBucketReplication()
        {

            try
            {
                DeleteBucketReplicationRequest request = new DeleteBucketReplicationRequest(bucket);


                //执行请求
                DeleteBucketReplicationResult result = cosXml.DeleteBucketReplication(request);


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
        public void TestBucketTagging()
        {
            try
            {
                // 设置 tag
                PutBucketTaggingRequest request = new PutBucketTaggingRequest(bucket);


                string akey = "aTagKey";
                string avalue = "aTagValue";
                string bkey = "bTagKey";
                string bvalue = "bTagValue";

                request.AddTag(akey, avalue);
                request.AddTag(bkey, bvalue);

                PutBucketTaggingResult result = cosXml.PutBucketTagging(request);

                Assert.True(result.IsSuccessful());

                // 获取 tag
                GetBucketTaggingRequest getRequest = new GetBucketTaggingRequest(bucket);

                GetBucketTaggingResult getResult = cosXml.GetBucketTagging(getRequest);

                // Console.WriteLine(getResult.GetResultInfo());
                Assert.IsNotEmpty((result.GetResultInfo()));

                Tagging tagging = getResult.tagging;

                Assert.AreEqual(getResult.httpCode, 200);
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
                DeleteBucketTaggingRequest deleteRequest = new DeleteBucketTaggingRequest(bucket);

                DeleteBucketTaggingResult deleteResult = cosXml.DeleteBucketTagging(deleteRequest);
                Assert.True(deleteResult.IsSuccessful());

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
                GetBucketTaggingRequest getRequest = new GetBucketTaggingRequest(bucket);

                GetBucketTaggingResult getResult = cosXml.GetBucketTagging(getRequest);
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
            bool setDomain = false;
            try
            {
                DomainConfiguration domain = new DomainConfiguration();

                domain.rule = new DomainConfiguration.DomainRule();
                domain.rule.Name = "www.qq.com";
                domain.rule.Status = "ENABLED";
                domain.rule.Type = "WEBSITE";
                domain.rule.Replace = "";

                PutBucketDomainResult result = cosXml.PutBucketDomain(new PutBucketDomainRequest(bucket, domain));
                Assert.AreEqual(result.httpCode, 200);

                setDomain = true;

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

            try
            {
                GetBucketDomainResult getResult = cosXml.GetBucketDomain(new GetBucketDomainRequest(bucket));

                // Console.WriteLine(getResult.GetResultInfo());
                Assert.IsNotEmpty((getResult.GetResultInfo()));

                Assert.AreEqual(getResult.httpCode, 200);

                if (setDomain)
                {

                    Assert.NotNull(getResult.domainConfiguration.rule);
                    Assert.NotNull(getResult.domainConfiguration.rule.Name);
                    Assert.NotNull(getResult.domainConfiguration.rule.Status);
                    Assert.NotNull(getResult.domainConfiguration.rule.Type);
                    Assert.NotNull(getResult.domainConfiguration.rule.Replace);

                }
                else
                {
                    Assert.Null(getResult.domainConfiguration);
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
        public void TestBucketLogging()
        {
            try
            {
                PutBucketLoggingRequest request = new PutBucketLoggingRequest(bucket);

                request.SetTarget(QCloudServer.Instance().bucketForLoggingTarget, "/");
                PutBucketLoggingResult putResult = cosXml.PutBucketLogging(request);

                Assert.IsTrue(putResult.httpCode == 200);

                GetBucketLoggingResult getResult = cosXml.GetBucketLogging(new GetBucketLoggingRequest(bucket));
                Assert.AreEqual(getResult.httpCode, 200);

                // Console.WriteLine(getResult.GetResultInfo());
                Assert.IsNotEmpty((getResult.GetResultInfo()));
                Assert.IsNotEmpty(getResult.RawContentBodyString);

                BucketLoggingStatus status = getResult.bucketLoggingStatus;

                Assert.NotNull(status);
                Assert.NotNull(status.loggingEnabled);
                Assert.NotNull(status.GetInfo());

                string targetBucket = status.loggingEnabled.targetBucket;
                string targetPrefix = status.loggingEnabled.targetPrefix;
                Assert.NotNull(targetBucket);
                Assert.NotNull(targetPrefix);

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
            try
            {
                PutBucketWebsiteRequest putRequest = new PutBucketWebsiteRequest(bucket);

                putRequest.SetIndexDocument("index.html");
                putRequest.SetErrorDocument("eroror.html");
                putRequest.SetRedirectAllRequestTo("https");

                var rule = new WebsiteConfiguration.RoutingRule();
                rule.contidion = new WebsiteConfiguration.Contidion();
                // HttpErrorCodeReturnedEquals 与 KeyPrefixEquals 必选其一
                // 只支持配置4XX返回码，例如403或404
                rule.contidion.httpErrorCodeReturnedEquals = 0;
                rule.contidion.keyPrefixEquals = "test.html";

                rule.redirect = new WebsiteConfiguration.Redirect();
                rule.redirect.protocol = "https";
                // ReplaceKeyWith 与 ReplaceKeyPrefixWith 必选其一
                // rule.redirect.replaceKeyPrefixWith = "aaa";
                rule.redirect.replaceKeyWith = "bbb";
                putRequest.SetRoutingRules(new List<WebsiteConfiguration.RoutingRule>() { rule });

                PutBucketWebsiteResult putResult = cosXml.PutBucketWebsite(putRequest);

                Assert.IsTrue(putResult.httpCode == 200);

                QCloudServer.TestWithServerFailTolerance(() =>
                {
                    GetBucketWebsiteRequest getRequest = new GetBucketWebsiteRequest(bucket);

                    GetBucketWebsiteResult getResult = cosXml.GetBucketWebsite(getRequest);
                    // Console.WriteLine(getResult.GetResultInfo());
                    Assert.IsNotEmpty((getResult.GetResultInfo()));

                    WebsiteConfiguration configuration = getResult.websiteConfiguration;

                    Assert.NotNull(configuration);
                    Assert.NotNull(configuration.indexDocument);
                    Assert.NotNull(configuration.indexDocument.suffix);
                    Assert.NotNull(configuration.errorDocument);
                    Assert.NotNull(configuration.redirectAllRequestTo);
                    Assert.NotNull(configuration.redirectAllRequestTo.protocol);
                    Assert.NotZero(configuration.routingRules.Count);
                    Assert.NotNull(configuration.routingRules[0].contidion);
                    //Assert.NotNull(configuration.routingRules[0].contidion.httpErrorCodeReturnedEquals);
                    // Assert.NotNull(configuration.routingRules[0].contidion.keyPrefixEquals);
                    Assert.NotNull(configuration.routingRules[0].redirect);
                    Assert.NotNull(configuration.routingRules[0].redirect.protocol);
                    // Assert.NotNull(configuration.routingRules[0].redirect.replaceKeyPrefixWith);
                    Assert.NotNull(configuration.routingRules[0].redirect.replaceKeyWith);

                    DeleteBucketWebsiteRequest deleteRequest = new DeleteBucketWebsiteRequest(bucket);

                    DeleteBucketWebsiteResult deleteResult = cosXml.DeleteBucketWebsite(deleteRequest);

                    Assert.IsTrue(deleteResult.IsSuccessful());
                }
                );

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
            try
            {
                string inventoryId = "id1";

                PutBucketInventoryRequest putRequest = new PutBucketInventoryRequest(bucket, inventoryId);

                putRequest.IsEnable(true);
                putRequest.SetScheduleFrequency("Daily");
                putRequest.SetIncludedObjectVersions("All");
                putRequest.SetDestination("CSV", QCloudServer.Instance().uin, QCloudServer.Instance().bucketForObjectTest, region, "list1");
                putRequest.SetFilter("dir/");
                putRequest.SetOptionalFields("SIZE");
                putRequest.SetOptionalFields("ETag");
                putRequest.EnableSSE();
                PutBucketInventoryResult putResult = cosXml.PutBucketInventory(putRequest);

                Assert.IsTrue(putResult.httpCode == 200);

                GetBucketInventoryRequest getRequest = new GetBucketInventoryRequest(bucket);

                getRequest.SetInventoryId(inventoryId);
                GetBucketInventoryResult getResult = cosXml.GetBucketInventory(getRequest);
                InventoryConfiguration testConfig = getResult.inventoryConfiguration;

                // Console.WriteLine(getResult.GetResultInfo());
                Assert.IsNotEmpty((getResult.GetResultInfo()));

                ListBucketInventoryRequest listRequest = new ListBucketInventoryRequest(bucket);
                ListBucketInventoryResult listResult = cosXml.ListBucketInventory(listRequest);
                Assert.IsTrue(listResult.httpCode == 200);
                Assert.NotNull(listResult.GetResultInfo());
                Assert.IsEmpty(listResult.listInventoryConfiguration.continuationToken);
                Assert.False(listResult.listInventoryConfiguration.isTruncated);
                Assert.AreEqual(listResult.listInventoryConfiguration.inventoryConfigurations.Count, 1);
                InventoryConfiguration testConfig2 = listResult.listInventoryConfiguration.inventoryConfigurations[0];

                InventoryConfiguration[] configurations = new InventoryConfiguration[] { testConfig, testConfig2 };

                foreach (InventoryConfiguration configuration in configurations)
                {
                    Assert.NotNull(configuration);
                    Assert.NotNull(configuration.destination.cosBucketDestination.accountId);
                    Assert.NotNull(configuration.destination.cosBucketDestination.bucket);
                    Assert.NotNull(configuration.destination.cosBucketDestination.encryption.sSECOS);
                    Assert.NotNull(configuration.destination.cosBucketDestination.format);
                    Assert.NotNull(configuration.destination.cosBucketDestination.prefix);
                    Assert.True(configuration.isEnabled);
                    Assert.NotNull(configuration.schedule.frequency);
                    Assert.NotNull(configuration.includedObjectVersions);
                    Assert.NotNull(configuration.filter.prefix);
                    Assert.AreEqual(configuration.optionalFields.fields.Count, 2);
                }

                DeleteBucketInventoryRequest deleteRequest = new DeleteBucketInventoryRequest(bucket);

                deleteRequest.SetInventoryId(inventoryId);
                DeleteBucketInventoryResult deleteResult = cosXml.DeleteBucketInventory(deleteRequest);
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
            GetBucketIntelligentTieringRequest getRequest = new GetBucketIntelligentTieringRequest(bucket);

            var getResult = cosXml.GetBucketIntelligentTieringConfiguration(getRequest);

            // Console.WriteLine(getResult.GetResultInfo());

            if (getResult.configuration == null || !getResult.configuration.IsEnabled())
            {
                IntelligentTieringConfiguration configuration = new IntelligentTieringConfiguration();
                configuration.Transition = new IntelligentTieringConfiguration.IntelligentTieringTransition();
                configuration.Transition.Days = 60;
                PutBucketIntelligentTieringRequest putRequest = new PutBucketIntelligentTieringRequest(bucket, configuration);

                var putResult = cosXml.PutBucketIntelligentTiering(putRequest);

                Assert.AreEqual(putResult.httpCode, 200);

                getRequest = new GetBucketIntelligentTieringRequest(bucket);
                getResult = cosXml.GetBucketIntelligentTieringConfiguration(getRequest);
                IntelligentTieringConfiguration newConf = getResult.configuration;

                // Console.WriteLine(getResult.GetResultInfo());
                Assert.IsNotEmpty((getResult.GetResultInfo()));

                Assert.AreEqual(newConf.Status, configuration.Status);
                Assert.AreEqual(newConf.Transition.Days, configuration.Transition.Days);
                Assert.AreEqual(newConf.Transition.RequestFrequent, configuration.Transition.RequestFrequent);
            }
        }

        [Test()]
        public void TestBucketReferer() 
        {
            try
            {
                // Put Bucket Refer
                PutBucketRefererRequest request = new PutBucketRefererRequest(bucket);
                RefererConfiguration configuration = new RefererConfiguration();
                configuration.Status = "Enabled";
                configuration.RefererType = "White-List";
                configuration.domainList = new DomainList();
                configuration.domainList.AddDomain("*.domain1.com");
                configuration.EmptyReferConfiguration = "Deny";
                request.SetRefererConfiguration(configuration);
                PutBucketRefererResult result = cosXml.PutBucketReferer(request);
                Assert.AreEqual(result.httpCode, 200);

                // Get Bucket Refer
                GetBucketRefererRequest getRequest = new GetBucketRefererRequest(bucket);
                GetBucketRefererResult getResult = cosXml.GetBucketReferer(getRequest);
                Assert.AreEqual(getResult.httpCode, 200);
                Assert.IsNotEmpty(getResult.GetResultInfo());
                Assert.NotNull(getResult.refererConfiguration);
                Assert.AreEqual(getResult.refererConfiguration.Status, "Enabled");

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
        public void TestDeleteBucketReferer() 
        {
            try
            {
                // Put Bucket Refer
                PutBucketRefererRequest request = new PutBucketRefererRequest(bucket);
                RefererConfiguration configuration = new RefererConfiguration();
                configuration.Status = "Enabled";
                configuration.RefererType = "White-List";
                configuration.domainList = new DomainList();
                configuration.domainList.AddDomain("*.domain1.com");
                configuration.domainList.AddDomain("*.domain2.com");
                configuration.EmptyReferConfiguration = "Deny";
                request.SetRefererConfiguration(configuration);
                PutBucketRefererResult result = cosXml.PutBucketReferer(request);
                Assert.AreEqual(result.httpCode, 200);

                // Get Bucket Refer
                GetBucketRefererRequest getRequest = new GetBucketRefererRequest(bucket);
                GetBucketRefererResult getResult = cosXml.GetBucketReferer(getRequest);
                Assert.AreEqual(getResult.httpCode, 200);
                Assert.IsNotEmpty(getResult.GetResultInfo());
                Assert.NotNull(getResult.refererConfiguration);
                Assert.AreEqual(getResult.refererConfiguration.Status, "Enabled");
                Assert.NotNull(getResult.refererConfiguration.domainList);
                Assert.NotZero(getResult.refererConfiguration.domainList.domains.Count);
                List<string> domains = new List<string>();
                for (int i = 0; i < getResult.refererConfiguration.domainList.domains.Count; i++)
                {
                    string domain = getResult.refererConfiguration.domainList.domains[i];
                    if (!domain.Contains("domain2"))
                        domains.Add(domain);
                }

                // Put New BucketReferer
                request = new PutBucketRefererRequest(bucket);
                configuration = new RefererConfiguration();
                configuration.Status = "Enabled";
                configuration.RefererType = "White-List";
                configuration.domainList = new DomainList();
                foreach(string domain in domains)
                {
                    configuration.domainList.AddDomain(domain);
                }
                configuration.EmptyReferConfiguration = "Deny";
                request.SetRefererConfiguration(configuration);
                result = cosXml.PutBucketReferer(request);
                Assert.AreEqual(result.httpCode, 200);

                // Get Bucket Refer again
                getRequest = new GetBucketRefererRequest(bucket);
                getResult = cosXml.GetBucketReferer(getRequest);
                Assert.AreEqual(getResult.httpCode, 200);
                Assert.IsNotEmpty(getResult.GetResultInfo());
                Assert.NotNull(getResult.refererConfiguration);
                Assert.AreEqual(getResult.refererConfiguration.Status, "Enabled");
                Assert.NotNull(getResult.refererConfiguration.domainList);
                Assert.AreEqual(getResult.refererConfiguration.domainList.domains.Count, 1);
                for (int i = 0; i < getResult.refererConfiguration.domainList.domains.Count; i++)
                {
                    string domain = getResult.refererConfiguration.domainList.domains[i];
                    if (domain.Contains("domain2"))
                        Assert.Fail();
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
        public void TestDoesBucketExist() {
            try {
                DoesBucketExistRequest request = new DoesBucketExistRequest(bucket);
                bool exist = cosXml.DoesBucketExist(request);
                Assert.True(exist);
                
                request = new DoesBucketExistRequest("notexist" + bucket);
                exist = cosXml.DoesBucketExist(request);
                Assert.False(exist);

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

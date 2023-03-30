using COSXML.Log;
using COSXML.Auth;
using COSXML.Model.Service;
using COSXML;
using COSXML.Model.Object;
using COSXML.Model.Bucket;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace COSXMLTests
{

    public class ServiceConfigTest
    {


        [Test()]
        public void TestHostEndpoint()
        {
            CosXmlConfig config = new CosXmlConfig.Builder()
                      .SetConnectionLimit(512)
                      .SetEndpointSuffix("cos.accelerate.myqcloud.com")
                      .Build();


            CosXmlServer cosXml = new CosXmlServer(config, null);

            string bucket = "bucket-125000";

            GetObjectRequest request = new GetObjectRequest(bucket, "aKey", null, null);

            try
            {
                cosXml.GetObject(request);
            }
            catch (Exception)
            {
                // ignore
            }

            Assert.AreEqual(bucket + "." + config.endpointSuffix, request.GetHost());

            GetBucketRequest bucketRequest = new GetBucketRequest(bucket);

            try
            {
                cosXml.GetBucket(bucketRequest);
            }
            catch (Exception)
            {
                // ignore
            }

            Assert.AreEqual(bucket + "." + config.endpointSuffix, bucketRequest.GetHost());

            // test service request
            GetServiceRequest serviceRequest = new GetServiceRequest();

            Assert.AreEqual("service.cos.myqcloud.com", serviceRequest.GetHost());

            string serviceHost = "service.cos.csp.com";

            serviceRequest.host = serviceHost;
            Assert.AreEqual(serviceHost, serviceRequest.GetHost());
        }

        [Test()]
        public void TestCustomHost()
        {
            string customHost = "www.my.custom.host.com";

            // test host
            CosXmlConfig configWithHost = new CosXmlConfig.Builder()
                      .SetHost(customHost)
                      .Build();
            string bucket = "bucket-125000";
            CosXmlServer cosXml = new CosXmlServer(configWithHost, null);

            GetBucketRequest bucketRequest = new GetBucketRequest(bucket);

            try
            {
                cosXml.GetBucket(bucketRequest);
            }
            catch (Exception)
            {
                // ignore
            }

            Assert.AreEqual(customHost, bucketRequest.GetHost());

            GetObjectRequest request = new GetObjectRequest(bucket, "aKey", null, null);

            try
            {
                cosXml.GetObject(request);
            }
            catch (Exception)
            {
                // ignore
            }

            Assert.AreEqual(customHost, request.GetHost());

            GetServiceRequest serviceRequest = new GetServiceRequest();

            Assert.AreEqual("service.cos.myqcloud.com", serviceRequest.GetHost());
        }

        [Test()]
        public void TestInvalidParams()
        {
            CosXmlServer cosXml;
            try {
                cosXml = new CosXmlServer(null, null);
                Assert.Fail();
            } catch (Exception)
            {

            }
            

        }

        [Test()]
        public void ServiceSchedueTest()
        {
            CosXmlConfig config = new CosXmlConfig.Builder()
                      .SetConnectionLimit(512)
                      .Build();

            string secretId = Environment.GetEnvironmentVariable("SECRET_ID");
            string secretKey = Environment.GetEnvironmentVariable("SECRET_KEY");

            QCloudCredentialProvider qCloudCredentialProvider = new DefaultQCloudCredentialProvider(secretId, secretKey, 600);
            CosXmlServer cosXml = new CosXmlServer(config, qCloudCredentialProvider);

            GetServiceRequest serviceRequest = new GetServiceRequest();
            try {
                cosXml.GetService(serviceRequest, null, null);
            } catch (Exception)
            {

            }

            PutBucketRequest putBucketRequest = new PutBucketRequest("dotnet-ut-obj-" + Environment.GetEnvironmentVariable("APPID"));
            try {
                cosXml.PutBucket(putBucketRequest, null, null);
            } catch (Exception)
            {

            }

            DeleteBucketRequest deleteBucketRequest = new DeleteBucketRequest("not-exist-bucket-" + Environment.GetEnvironmentVariable("APPID"));
            try {
                cosXml.DeleteBucket(deleteBucketRequest, null, null);
            } catch (Exception)
            {

            }

            HeadBucketRequest headBucketRequest = new HeadBucketRequest("dotnet-ut-obj-" + Environment.GetEnvironmentVariable("APPID"));
            try {
                cosXml.HeadBucket(headBucketRequest, null, null);
            } catch (Exception)
            {

            }

            PutBucketACLRequest pubBucketAclRequest = new PutBucketACLRequest("not-exist-bucket-" + Environment.GetEnvironmentVariable("APPID"));
            try {
                cosXml.PutBucketACL(pubBucketAclRequest, null, null);
            } catch (Exception)
            {

            }

            GetBucketACLRequest getBucketAclRequest = new GetBucketACLRequest("not-exist-bucket-" + Environment.GetEnvironmentVariable("APPID"));
            try {
                cosXml.GetBucketACL(getBucketAclRequest, null, null);
            } catch (Exception)
            {

            }

            PutBucketCORSRequest putBucketCORSRequest = new PutBucketCORSRequest("not-exist-bucket-" + Environment.GetEnvironmentVariable("APPID"));
            try {
                cosXml.PutBucketCORS(putBucketCORSRequest, null, null);
            } catch (Exception)
            {

            }

            GetBucketCORSRequest getBucketCORSRequest = new GetBucketCORSRequest("not-exist-bucket-" + Environment.GetEnvironmentVariable("APPID"));
            try {
                cosXml.GetBucketCORS(getBucketCORSRequest, null, null);
            } catch (Exception)
            {

            }

            DeleteBucketCORSRequest deleteBucketCORSRequest = new DeleteBucketCORSRequest("bucket");
            try {
                cosXml.DeleteBucketCORS(deleteBucketCORSRequest, null, null);
            } catch (Exception)
            {

            }

            PutBucketLifecycleRequest putBucketLifecycleRequest = new PutBucketLifecycleRequest("bucket");
            try {
                cosXml.PutBucketLifecycle(putBucketLifecycleRequest, null, null);
            } catch (Exception)
            {

            }

            GetBucketLifecycleRequest getBucketLifecycleRequest = new GetBucketLifecycleRequest("bucket");
            try {
                cosXml.GetBucketLifecycle(getBucketLifecycleRequest, null, null);
            } catch (Exception)
            {

            }

            DeleteBucketLifecycleRequest deleteBucketLifecycleRequest = new DeleteBucketLifecycleRequest("bucket");
            try {
                cosXml.DeleteBucketLifecycle(deleteBucketLifecycleRequest, null, null);
            } catch (Exception)
            {

            }

            PutBucketReplicationRequest putBucketReplicationRequest = new PutBucketReplicationRequest("bucket");
            try {
                cosXml.PutBucketReplication(putBucketReplicationRequest, null, null);
            } catch (Exception)
            {

            }

            GetBucketReplicationRequest getBucketReplicationRequest = new GetBucketReplicationRequest("dotnet-ut-obj-" + Environment.GetEnvironmentVariable("APPID"));
            try {
                cosXml.GetBucketReplication(getBucketReplicationRequest, null, null);
            } catch (Exception)
            {

            }
            try {
                cosXml.GetBucketReplication(getBucketReplicationRequest);
            } catch (Exception)
            {

            }

            DeleteBucketReplicationRequest deleteBucketReplicationRequest = new DeleteBucketReplicationRequest("not-exist-bucket-" + Environment.GetEnvironmentVariable("APPID"));
            try {
                cosXml.DeleteBucketReplication(deleteBucketReplicationRequest);
            } catch (Exception)
            {

            }
            try {
                cosXml.DeleteBucketReplication(deleteBucketReplicationRequest, null, null);
            } catch (Exception)
            {

            }

            PutBucketVersioningRequest putBucketVersioningRequest = new PutBucketVersioningRequest("bucket");
            try {
                cosXml.PutBucketVersioning(putBucketVersioningRequest, null, null);
            } catch (Exception)
            {

            }

            GetBucketVersioningRequest getBucketVersioningRequest = new GetBucketVersioningRequest("bucket");
            try {
                cosXml.GetBucketVersioning(getBucketVersioningRequest, null, null);
            } catch (Exception)
            {

            }

            ListBucketVersionsRequest listBucketVersionsRequest = new ListBucketVersionsRequest("bucket");
            try {
                cosXml.ListBucketVersions(listBucketVersionsRequest, null, null);
            } catch (Exception)
            {

            }

            PutBucketRefererRequest putBucketRefererRequest = new PutBucketRefererRequest("bucket");
            try {
                cosXml.PutBucketReferer(putBucketRefererRequest, null, null);
            } catch (Exception)
            {

            }

            GetBucketRefererRequest getBucketRefererRequest = new GetBucketRefererRequest("bucket");
            try {
                cosXml.GetBucketReferer(getBucketRefererRequest, null, null);
            } catch (Exception)
            {

            }

            DeleteBucketPolicyRequest deleteBucketPolicyRequest = new DeleteBucketPolicyRequest("bucket");
            try {
                cosXml.DeleteBucketPolicy(deleteBucketPolicyRequest, null, null);
            } catch (Exception)
            {

            }
            try {
                cosXml.DeleteBucketPolicy(deleteBucketPolicyRequest);
            } catch (Exception)
            {

            }

            AppendObjectRequest appendObjectRequest = new AppendObjectRequest("bucket", "key", "src_path", 0L);
            try {
                cosXml.AppendObject(appendObjectRequest, null, null);
            } catch (Exception)
            {

            }

            Assert.Pass();

        }
    }
}
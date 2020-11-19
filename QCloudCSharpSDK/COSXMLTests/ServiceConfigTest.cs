using COSXML.Log;
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
        public void testHostEndpoint()
        {
            CosXmlConfig config = new CosXmlConfig.Builder()
                      .SetConnectionLimit(512)
                      .setEndpointSuffix("cos.accelerate.myqcloud.com")
                      .Build();

            CosXmlServer cosXml = new CosXmlServer(config, null);

            string bucket = "bucket-125000";
            GetObjectRequest request = new GetObjectRequest(bucket, "aKey", null, null);
            try
            {
                cosXml.GetObject(request);
            }
            catch (Exception e)
            {
                // ignore
            }
            Assert.AreEqual(bucket + "." + config.endpointSuffix, request.GetHost());

            GetBucketRequest bucketRequest = new GetBucketRequest(bucket);
            try
            {
                cosXml.GetBucket(bucketRequest);
            }
            catch (Exception e)
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
        public void testCustomHost()
        {
            string customHost = "www.my.custom.host.com";
            // test host
            CosXmlConfig configWithHost = new CosXmlConfig.Builder()
                      .setHost(customHost)
                      .Build();
            string bucket = "bucket-125000";
            CosXmlServer cosXml = new CosXmlServer(configWithHost, null);

            GetBucketRequest bucketRequest = new GetBucketRequest(bucket);
            try
            {
                cosXml.GetBucket(bucketRequest);
            }
            catch (Exception e)
            {
                // ignore
            }
            Assert.AreEqual(customHost, bucketRequest.GetHost());

            GetObjectRequest request = new GetObjectRequest(bucket, "aKey", null, null);
            try
            {
                cosXml.GetObject(request);
            }
            catch (Exception e)
            {
                // ignore
            }
            Assert.AreEqual(customHost, request.GetHost());

            GetServiceRequest serviceRequest = new GetServiceRequest();
            Assert.AreEqual("service.cos.myqcloud.com", serviceRequest.GetHost());
        }
    }
}
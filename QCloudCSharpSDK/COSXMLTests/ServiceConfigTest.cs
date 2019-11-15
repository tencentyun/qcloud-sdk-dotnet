using COSXML.Log;
using COSXML.Model.Service;
using COSXML;
using COSXML.Model.Object;
using COSXML.Model.Bucket;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace COSXMLTests {

  public class ServiceConfigTest {


    [Test()]
    public void testHost() {
      CosXmlConfig config = new CosXmlConfig.Builder()
                .SetConnectionLimit(512)
                .setEndpointSuffix("cos.accelerate.myqcloud.com")
                .Build();

      CosXmlServer cosXml = new CosXmlServer(config, null);

      string bucket = "bucket-125000";
      GetObjectRequest request = new GetObjectRequest(bucket, "aKey", null, null);
      try {
        cosXml.GetObject(request);
      } catch(Exception e) {
        // ignore
      }
      Assert.AreEqual(bucket + "." + config.endpointSuffix, request.GetHost());

      GetBucketRequest bucketRequest = new GetBucketRequest(bucket);
      try {
        cosXml.GetBucket(bucketRequest);
      } catch(Exception e) {
        // ignore
      }
      Assert.AreEqual(bucket + "." + config.endpointSuffix, bucketRequest.GetHost());

      GetServiceRequest serviceRequest = new GetServiceRequest();
      Assert.AreEqual("service.cos.myqcloud.com", serviceRequest.GetHost());

      string serviceHost = "service.cos.csp.com";
      serviceRequest.host = serviceHost;
      Assert.AreEqual(serviceHost, serviceRequest.GetHost());
    }
  }
}
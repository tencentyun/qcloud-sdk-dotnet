using System;
using System.Collections.Generic;
using System.Linq;
using COSXML.Network;
using NUnit.Framework;
using System.Reflection;
using COSXML;
using COSXML.Auth;
using COSXML.CosException;
using COSXML.Model;
using COSXML.Model.Bucket;


namespace COSXML.Utils.Tests{
    
    [TestFixture()]
    public class RequestsTest
    {

        [SetUp]
        public void Setup()
        {
        }

        [Test()]
        public void InitTest()
        {
            Request req = new Request();
            string name = "TestHeader";
            string value = "TestValue";

            req.AddHeader(name, value);
            req.AddHeader(name, "New Value");
            req.AddHeader(name, "");
            try
            {
                req.Url = null;
            }
            catch (ArgumentNullException ex)
            {
                Assert.True(ex.Message.Equals("Value cannot be null. (Parameter 'httpUrl == null')"));
            }

            req.RequestUrlString = "test-url";
            HttpClient httpClient = HttpClient.GetInstance();
            var maxRetry = httpClient.MaxRetry = 5;
            Assert.AreEqual(5, maxRetry);
            // CosXmlConfig.Builder cosXmlConfig = new CosXmlConfig();
            try
            {
                CosXmlConfig.Builder builder = new CosXmlConfig.Builder();
                builder.SetRegion("");
            }
            catch (CosClientException ex)
            {
                Assert.True(ex.Message.Equals("region cannot be empty"));
            }
            CosRequest cosRequest =new ListMultiUploadsRequest("bucket");
            cosRequest.Method = "http";
            cosRequest.SetQueryParameter("key",null,false);
            cosRequest.SetRequestHeader("key",null,true);
            ResponseBody responseBody = new ResponseBody();
            // Console.WriteLine("--------- {0}",responseBody.ContentType);
            Assert.AreEqual(null,responseBody.ContentType);
            Assert.AreEqual(null,responseBody.ProgressCallback);
            Assert.AreEqual(null,responseBody.ParseStream);
            SessionQCloudCredentials sessionQCloudCredentials = new SessionQCloudCredentials("", "", "", "1");
            Assert.AreEqual(0, sessionQCloudCredentials.Token.Length);
            cosRequest.IsNeedMD5 = true;
            var signStartTimeSecond = 123L;
            var durationSecond = 456L;
            var headerKeys = new List<string> { "header1", "header2" };
            var queryParameterKeys = new List<string> { "param1", "param2" };
            cosRequest.SetSign(signStartTimeSecond,durationSecond,headerKeys,queryParameterKeys);
            cosRequest.SetSign("sign");
            RequestBody requestBody = new MultipartRequestBody();
            requestBody.ContentLength = 1;
            requestBody.ContentType = "typetest";
            try
            {
                requestBody.GetMD5();
            }
            catch (NotImplementedException ex)
            {

            }

            Assert.Null(requestBody.ProgressCallback);
        }

    }

    
    
    
}

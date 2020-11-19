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
    public class AsyncTests
    {

        [Test()]
        public void AsyncGetBucket()
        {
            ManualResetEvent manualResetEvent = new ManualResetEvent(false);

            GetBucketRequest request = new GetBucketRequest(QCloudServer.Instance().bucketForBucketTest);
            List<string> queryParameters = new List<string>();

            ///执行请求
            QCloudServer.Instance().cosXml.GetBucket(request,
                delegate (CosResult cosResult)
                {
                    GetBucketResult result = cosResult as GetBucketResult;
                    Assert.True(result.httpCode == 200);
                    manualResetEvent.Set();
                },
            delegate (CosClientException clientEx, CosServerException serverEx)
            {
                if (clientEx != null)
                {
                    Console.WriteLine("CosClientException: " + clientEx.Message);
                }
                if (serverEx != null)
                {
                    Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                }
                Assert.Fail();
                manualResetEvent.Set();
            });

            manualResetEvent.WaitOne();

        }
    }
}
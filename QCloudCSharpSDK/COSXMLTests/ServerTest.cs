using COSXML.Log;
using COSXML.Model.Service;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace COSXMLTests
{
    [TestFixture()]
    public class ServerTest
    {
        [Test()]
        public void GetServerTest()
        {
            try
            {
                QCloudServer instance = QCloudServer.Instance();
                GetServiceRequest request = new GetServiceRequest();
                GetServiceResult result = instance.cosXml.GetService(request);
                Console.WriteLine(result.GetResultInfo());
                Assert.True(true);
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
        public void AsyncGetServerTest()
        {
            bool isSuccess = false;
            ManualResetEvent manualResetEvent = new ManualResetEvent(false);
            QCloudServer instance = QCloudServer.Instance();
            GetServiceRequest request = new GetServiceRequest();
            instance.cosXml.GetService(request,
                delegate (COSXML.Model.CosResult cosResult)
                {
                    GetServiceResult result = cosResult as GetServiceResult;
                    Console.WriteLine(result.GetResultInfo());
                    isSuccess = true;
                    manualResetEvent.Set();
                },
                delegate (COSXML.CosException.CosClientException clientEx, COSXML.CosException.CosServerException serverEx)
                {
                    Console.WriteLine(clientEx == null ? serverEx.GetInfo() : clientEx.Message);
                    isSuccess = false;
                    manualResetEvent.Set();
                });
            manualResetEvent.WaitOne();
            Assert.True(isSuccess);
        }

    }
}

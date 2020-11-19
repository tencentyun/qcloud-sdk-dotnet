using COSXML.Log;
using COSXML.Model.Service;
using NUnit.Framework;
using System;
using System.Collections.Generic;
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

                Assert.True(result.httpCode == 200);
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
        public async Task AsyncGetServerTest()
        {
            QCloudServer instance = QCloudServer.Instance();
            GetServiceRequest request = new GetServiceRequest();

            GetServiceResult result = await instance.cosXml.ExecuteAsync<GetServiceResult>(request);

            Assert.True(result.httpCode == 200);
        }

    }
}

using COSXML.Log;
using COSXML.Model.Service;
using COSXML.Model.Tag;
using COSXML.Auth;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using COSXML.Network;

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
                QCloudServer instance = new QCloudServer();
                GetServiceRequest request = new GetServiceRequest();

                GetServiceResult result = instance.cosXml.GetService(request);

                Assert.True(result.httpCode == 200);
                // Console.WriteLine(result.GetResultInfo());
                Assert.IsNotEmpty((result.GetResultInfo()));
                ValidateBucketList(result.listAllMyBuckets);
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
        public void SyncGetServerTest()
        {
            QCloudServer instance = QCloudServer.Instance();
            GetServiceRequest request = new GetServiceRequest();

            GetServiceResult result = instance.cosXml.Execute<GetServiceResult>(request);

            Assert.True(result.httpCode == 200);
            Assert.IsNotEmpty((result.GetResultInfo()));
            ValidateBucketList(result.listAllMyBuckets);
        }

        [Test()]
        public async Task AsyncGetServerTest()
        {
            QCloudServer instance = QCloudServer.Instance();
            GetServiceRequest request = new GetServiceRequest();

            GetServiceResult result = await instance.cosXml.ExecuteAsync<GetServiceResult>(request);

            Assert.True(result.httpCode == 200);
            // Console.WriteLine(result.GetResultInfo());
            Assert.IsNotEmpty((result.GetResultInfo()));
            ValidateBucketList(result.listAllMyBuckets);
        }

        private void ValidateBucketList(ListAllMyBuckets bucketList)
        {
            Assert.True(bucketList.buckets.Count > 0);
            Assert.NotNull(bucketList.owner);
            Assert.NotNull(bucketList.owner.id);
            Assert.NotNull(bucketList.owner.disPlayName);

            foreach (var bucket in bucketList.buckets) 
            {
                Assert.NotNull(bucket.createDate);
                Assert.NotNull(bucket.name);
                Assert.NotNull(bucket.location);
            }
        }

    }

    public class CustomQCloudCredentialProvider : QCloudCredentialProvider
    {
        public override QCloudCredentials GetQCloudCredentials()
        {
            return base.GetQCloudCredentials();
        }

        public override void Refresh()
        {
            throw new NotImplementedException();
        }
    }
    
    public class CustomQCloudCredentialProvider2 : QCloudCredentialProvider
    {
        public override QCloudCredentials GetQCloudCredentialsWithRequest(Request request)
        {
            return base.GetQCloudCredentialsWithRequest(request);
        }

        public override void Refresh()
        {
            throw new NotImplementedException();
        }
    }
}

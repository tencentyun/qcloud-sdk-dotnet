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
using COSXML;

namespace COSXMLTests
{

    [TestFixture]
    public class ListObjectsTest
    {

        [Test()]
        public void GetBucket()
        {
            try
            {
                GetBucketRequest request = new GetBucketRequest(QCloudServer.Instance().bucketForObjectTest);

                request.SetDelimiter("/");
                request.SetEncodingType("url");
                request.SetMaxKeys("1000");

                List<string> headerKeys = new List<string>();

                headerKeys.Add("Host");

                List<string> queryParameters = new List<string>();

                queryParameters.Add("prefix");
                queryParameters.Add("max-keys");
                //执行请求
                GetBucketResult result = QCloudServer.Instance().cosXml.GetBucket(request);
                Assert.True(result.httpCode == 200);
                // Console.WriteLine(result.GetResultInfo());
                Assert.IsNotEmpty((result.GetResultInfo()));

                var listObjects = result.listBucket;

                Assert.NotNull(listObjects.delimiter);
                Assert.NotNull(listObjects.encodingType);
                Assert.NotNull(listObjects.name);
                Assert.NotZero(listObjects.maxKeys);
                Assert.IsFalse(listObjects.isTruncated);

                Assert.That(listObjects.prefix, Is.Null.Or.Empty);
                Assert.That(listObjects.marker, Is.Null.Or.Empty);
                Assert.That(listObjects.nextMarker, Is.Null.Or.Empty);

                Assert.NotZero(listObjects.commonPrefixesList.Count);
                foreach (var commonPrefix in listObjects.commonPrefixesList) 
                {
                    Assert.NotNull(commonPrefix.prefix);
                }

                Assert.NotZero(listObjects.contentsList.Count);
                foreach (var content in listObjects.contentsList) 
                {
                    Assert.NotNull(content.eTag);
                    Assert.NotNull(content.owner);
                    Assert.NotNull(content.owner.id);
                    Assert.NotNull(content.key);
                    Assert.NotNull(content.lastModified);
                    Assert.NotNull(content.size);
                    Assert.NotNull(content.storageClass);
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
        public void ListBucketVersions()
        {

            try
            {
                var service = QCloudServer.Instance().NewService(QCloudServer.Instance().regionForBucketVersioning);

                ListBucketVersionsRequest request = new ListBucketVersionsRequest(QCloudServer.Instance().bucketVersioning);
                request.SetDelimiter("/");
                request.SetEncodingType("url");
                request.SetMaxKeys("500");

                //执行请求
                ListBucketVersionsResult result = service.ListBucketVersions(request);

                Assert.True(result.httpCode == 200);
                // Console.WriteLine(result.GetResultInfo());
                Assert.IsNotEmpty((result.GetResultInfo()));

                var listObjects = result.listBucketVersions;

                Assert.NotNull(listObjects.delimiter);
                Assert.NotNull(listObjects.encodingType);
                Assert.NotNull(listObjects.name);
                Assert.NotZero(listObjects.maxKeys);
                Assert.IsFalse(listObjects.isTruncated);

                Assert.That(listObjects.prefix, Is.Null.Or.Empty);
                Assert.That(listObjects.keyMarker, Is.Null.Or.Empty);
                Assert.That(listObjects.versionIdMarker, Is.Null.Or.Empty);
                Assert.That(listObjects.nextVersionIdMarker, Is.Null.Or.Empty);
                Assert.That(listObjects.nextKeyMarker, Is.Null.Or.Empty);

                Assert.NotZero(listObjects.commonPrefixesList.Count);
                foreach (var commonPrefix in listObjects.commonPrefixesList) 
                {
                    Assert.NotNull(commonPrefix.prefix);
                }

                Assert.NotZero(listObjects.objectVersionList.Count);
                foreach (var content in listObjects.objectVersionList) 
                {
                    Assert.NotNull(content.eTag);
                    Assert.NotNull(content.owner);
                    Assert.NotNull(content.owner.uid);
                    Assert.NotNull(content.owner.displayName);
                    Assert.NotNull(content.key);
                    Assert.NotNull(content.lastModified);
                    Assert.NotNull(content.size);
                    Assert.NotNull(content.storageClass);
                    Assert.NotNull(content.versionId);
                    Assert.NotNull(content.isLatest);
                }

                Assert.NotZero(listObjects.deleteMarkers.Count);
                foreach (var content in listObjects.deleteMarkers) 
                {
                    Assert.NotNull(content.owner);
                    Assert.NotNull(content.owner.uid);
                    Assert.NotNull(content.owner.displayName);
                    Assert.NotNull(content.key);
                    Assert.NotNull(content.lastModified);
                    Assert.NotNull(content.versionId);
                    Assert.NotNull(content.isLatest);
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
    }
}
using COSXML.Common;
using COSXML.CosException;
using COSXML.Model;
using COSXML.Model.Bucket;
using COSXML.Model.Object;
using COSXML.Model.Tag;
using COSXML.Utils;
using COSXML.Transfer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace COSXMLTests
{

    [TestFixture()]
    public class InvalidRequestTest
    {

        private string notExistBucket;

        private COSXML.CosXml cosXml;

        private TransferManager transferManager;
        private string localFilePath;

        [OneTimeSetUp]
        public void Setup()
        {
            notExistBucket = "not-exist-bucket-suwjsdjwujehdfkd-" + QCloudServer.Instance().appid;
            cosXml = QCloudServer.Instance().cosXml;
            var config = new TransferConfig();
            config.DivisionForUpload = 1 * 1024 * 1024;
            config.DdivisionForCopy = 1 * 1024 * 1024;
            config.SliceSizeForCopy = 1 * 1024 * 1024;
            config.SliceSizeForUpload = 1 * 1024 * 1024;
            transferManager = new TransferManager(cosXml, config);
            localFilePath = QCloudServer.CreateFile(TimeUtils.GetCurrentTime(TimeUnit.Seconds) + ".txt", 1024 * 1024 * 1);
        }

        [Test]
        public void TestGetNotExistBucket()
        {
            GetBucketRequest request = new GetBucketRequest(notExistBucket);

            Assert.ThrowsAsync<CosServerException>(async() => await cosXml.ExecuteAsync<GetBucketResult>(request));
        }

        [Test()]
        public void TestUploadFileNotExisted()
        {
            PutObjectRequest request = new PutObjectRequest(notExistBucket, "remote_key", localFilePath);

            COSXMLUploadTask uploadTask = new COSXMLUploadTask(request);

            Assert.ThrowsAsync<CosClientException>(async() => await transferManager.UploadAsync(uploadTask));
        }

        [Test()]
        public void TestCopySourceNotExisted()
        {
            COSXMLCopyTask copyTask = new COSXMLCopyTask(notExistBucket, "remote_key", null);

            Assert.ThrowsAsync<CosClientException>(async() => await transferManager.CopyAsync(copyTask));
            
            CopySourceStruct notExistSource = new CopySourceStruct(QCloudServer.Instance().appid,
                    notExistBucket, QCloudServer.Instance().region, "example_key");

            COSXMLCopyTask copyTask2 = new COSXMLCopyTask(notExistBucket, "remote_key", notExistSource);

            Assert.ThrowsAsync<CosServerException>(async() => await transferManager.CopyAsync(copyTask2));
        }
    }
}
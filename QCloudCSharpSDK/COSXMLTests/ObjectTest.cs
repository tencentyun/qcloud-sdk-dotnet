using COSXML.Common;
using COSXML.CosException;
using COSXML.Model;
using COSXML.Model.Object;
using COSXML.Model.Tag;
using COSXML.Utils;
using COSXML.Transfer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace COSXMLTests
{
    [TestFixture()]
    public class ObjectTest
    {
        ManualResetEvent manualResetEvent = null;

        public void PutObjectWithAES256(COSXML.CosXml cosXml, string bucket, string key, string srcPath)
        {
            try
            {
                PutObjectRequest request = new PutObjectRequest(bucket, key, srcPath);
                //设置签名有效时长
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);

                //设置进度回调
                request.SetCosProgressCallback(delegate (long completed, long total)
                {
                    //Console.WriteLine(String.Format("{0} progress = {1} / {2} : {3:##.##}%", DateTime.Now.ToString(), completed, total, completed * 100.0 / total));
                });

                request.SetCosServerSideEncryption();

                //执行请求
                PutObjectResult result = cosXml.PutObject(request);

                Console.WriteLine(result.GetResultInfo());
            }
            catch (CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.StackTrace);
                Assert.True(false);
            }
            catch (CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.True(false);
            }
        }

        public void PutObjectWithCustomerKey(COSXML.CosXml cosXml, string bucket, string key, string srcPath)
        {
            try
            {
                PutObjectRequest request = new PutObjectRequest(bucket, key, srcPath);
                //设置签名有效时长
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
                request.IsHttps = true;
                //设置进度回调
                request.SetCosProgressCallback(delegate (long completed, long total)
                {
                    //Console.WriteLine(String.Format("{0} progress = {1} / {2} : {3:##.##}%", DateTime.Now.ToString(), completed, total, completed * 100.0 / total));
                });

                string customerKey = "25rN73uQtl1bUGnvHe0URgFWBNu4vBba";//32字符
                request.SetCosServerSideEncryptionWithCustomerKey(customerKey);

                //执行请求
                PutObjectResult result = cosXml.PutObject(request);

                Console.WriteLine(result.GetResultInfo());
            }
            catch (CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.Message);
                Assert.True(false);
            }
            catch (CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.True(false);
            }
        }

        public void PutObjectWithKMS(COSXML.CosXml cosXml, string bucket, string key, string srcPath)
        {
            try
            {
                PutObjectRequest request = new PutObjectRequest(bucket, key, srcPath);
                //设置签名有效时长
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
                request.IsHttps = true;
                //设置进度回调
                request.SetCosProgressCallback(delegate (long completed, long total)
                {
                    //Console.WriteLine(String.Format("{0} progress = {1} / {2} : {3:##.##}%", DateTime.Now.ToString(), completed, total, completed * 100.0 / total));
                });

                request.SetCosServerSideEncryptionWithKMS(null, null);

                //执行请求
                PutObjectResult result = cosXml.PutObject(request);

                Console.WriteLine(result.GetResultInfo());
            }
            catch (CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.Message);
                //Assert.True(false);
            }
            catch (CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                //Assert.True(true);
            }
        }

        public void PutObject(COSXML.CosXml cosXml, string bucket, string key, string srcPath)
        {
            try
            {
                PutObjectRequest request = new PutObjectRequest(bucket, key, srcPath);
                //设置签名有效时长
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);

                //设置进度回调
                request.SetCosProgressCallback(delegate (long completed, long total)
                {
                    //Console.WriteLine(String.Format("{0} progress = {1} / {2} : {3:##.##}%", DateTime.Now.ToString(), completed, total, completed * 100.0 / total));
                });

                //执行请求
                PutObjectResult result = cosXml.PutObject(request);

                Console.WriteLine(result.GetResultInfo());
            }
            catch (CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.Message);
                Assert.True(false);
            }
            catch (CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.True(false);
            }
        }

        public void AsynPutObject(COSXML.CosXml cosXml, string bucket, string key, string srcPath)
        {
            PutObjectRequest request = new PutObjectRequest(bucket, key, srcPath);
            //设置签名有效时长
            request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);

            //设置进度回调
            request.SetCosProgressCallback(delegate (long completed, long total)
            {
                //Console.WriteLine(String.Format("progress = {0} / {1} : {2:##.##}%", completed, total, completed * 100.0 / total));
            });
            //添加acl
            request.SetCosACL(CosACL.PRIVATE);

            COSXML.Model.Tag.GrantAccount readAccount = new COSXML.Model.Tag.GrantAccount();
            readAccount.AddGrantAccount("1131975903", "1131975903");
            request.SetXCosGrantRead(readAccount);

            COSXML.Model.Tag.GrantAccount writeAccount = new COSXML.Model.Tag.GrantAccount();
            writeAccount.AddGrantAccount("1131975903", "1131975903");
            request.SetXCosGrantWrite(writeAccount);

            COSXML.Model.Tag.GrantAccount fullAccount = new COSXML.Model.Tag.GrantAccount();
            fullAccount.AddGrantAccount("2832742109", "2832742109");
            request.SetXCosReadWrite(fullAccount);
            cosXml.PutObject(request,
                delegate (CosResult cosResult)
                {
                    PutObjectResult result = cosResult as PutObjectResult;
                    Console.WriteLine(result.GetResultInfo());
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

                manualResetEvent.Set();
            });

        }

        public void HeadObject(COSXML.CosXml cosXml, string bucket, string key)
        {
            try
            {
                HeadObjectRequest request = new HeadObjectRequest(bucket, key);
                //设置签名有效时长
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);

                //执行请求
                HeadObjectResult result = cosXml.HeadObject(request);

                Console.WriteLine(result.GetResultInfo());
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.StackTrace);
                Assert.True(false);
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.True(false);
            }
        }

        public void AsynHeadObject(COSXML.CosXml cosXml, string bucket, string key)
        {
            HeadObjectRequest request = new HeadObjectRequest(bucket, key);
            //设置签名有效时长
            request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);

            cosXml.HeadObject(request,
                delegate (CosResult cosResult)
                {
                    HeadObjectResult result = cosResult as HeadObjectResult;
                    Console.WriteLine(result.GetResultInfo());
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

                manualResetEvent.Set();
            });

        }

        public void PutObjectACL(COSXML.CosXml cosXml, string bucket, string key)
        {
            try
            {
                PutObjectACLRequest request = new PutObjectACLRequest(bucket, key);
                //设置签名有效时长
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);

                //添加acl
                request.SetCosACL(CosACL.PRIVATE);

                COSXML.Model.Tag.GrantAccount readAccount = new COSXML.Model.Tag.GrantAccount();
                readAccount.AddGrantAccount("1131975903", "1131975903");
                request.SetXCosGrantRead(readAccount);

                COSXML.Model.Tag.GrantAccount writeAccount = new COSXML.Model.Tag.GrantAccount();
                writeAccount.AddGrantAccount("1131975903", "1131975903");
                request.SetXCosGrantWrite(writeAccount);

                COSXML.Model.Tag.GrantAccount fullAccount = new COSXML.Model.Tag.GrantAccount();
                fullAccount.AddGrantAccount("2832742109", "2832742109");
                request.SetXCosReadWrite(fullAccount);

                //执行请求
                PutObjectACLResult result = cosXml.PutObjectACL(request);

                Console.WriteLine(result.GetResultInfo());
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.StackTrace);
                Assert.True(false);
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.True(false);
            }
        }

        public void AsynPutObjectACL(COSXML.CosXml cosXml, string bucket, string key)
        {
            PutObjectACLRequest request = new PutObjectACLRequest(bucket, key);
            //设置签名有效时长
            request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);


            //添加acl
            request.SetCosACL(CosACL.PRIVATE);

            COSXML.Model.Tag.GrantAccount readAccount = new COSXML.Model.Tag.GrantAccount();
            readAccount.AddGrantAccount("1131975903", "1131975903");
            request.SetXCosGrantRead(readAccount);

            COSXML.Model.Tag.GrantAccount writeAccount = new COSXML.Model.Tag.GrantAccount();
            writeAccount.AddGrantAccount("1131975903", "1131975903");
            request.SetXCosGrantWrite(writeAccount);

            COSXML.Model.Tag.GrantAccount fullAccount = new COSXML.Model.Tag.GrantAccount();
            fullAccount.AddGrantAccount("2832742109", "2832742109");
            request.SetXCosReadWrite(fullAccount);

            cosXml.PutObjectACL(request,
                delegate (CosResult cosResult)
                {
                    PutObjectACLResult result = cosResult as PutObjectACLResult;
                    Console.WriteLine(result.GetResultInfo());
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

                manualResetEvent.Set();
            });

        }

        public void GetObjectACL(COSXML.CosXml cosXml, string bucket, string key)
        {
            try
            {
                GetObjectACLRequest request = new GetObjectACLRequest(bucket, key);
                //设置签名有效时长
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);

                //执行请求
                GetObjectACLResult result = cosXml.GetObjectACL(request);

                Console.WriteLine(result.GetResultInfo());
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.StackTrace);
                Assert.True(false);
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.True(false);
            }
        }

        public void AsynGetObjectACL(COSXML.CosXml cosXml, string bucket, string key)
        {
            GetObjectACLRequest request = new GetObjectACLRequest(bucket, key);
            //设置签名有效时长
            request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);

            cosXml.GetObjectACL(request,
                delegate (CosResult cosResult)
                {
                    GetObjectACLResult result = cosResult as GetObjectACLResult;
                    Console.WriteLine(result.GetResultInfo());
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

                manualResetEvent.Set();
            });

        }


        public void OptionObject(COSXML.CosXml cosXml, string bucket, string key)
        {
            try
            {
                string origin = "http://cloud.tencent.com";
                string accessMthod = "PUT";
                OptionObjectRequest request = new OptionObjectRequest(bucket, key, origin, accessMthod);
                //设置签名有效时长
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);

                //执行请求
                OptionObjectResult result = cosXml.OptionObject(request);

                Console.WriteLine(result.GetResultInfo());
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.StackTrace);
                Assert.True(false);
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.True(false);
            }
        }

        public void AsynOptionObject(COSXML.CosXml cosXml, string bucket, string key)
        {
            string origin = "http://cloud.tencent.com";
            string accessMthod = "PUT";
            OptionObjectRequest request = new OptionObjectRequest(bucket, key, origin, accessMthod);
            //设置签名有效时长
            request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);

            cosXml.OptionObject(request,
                delegate (CosResult cosResult)
                {
                    OptionObjectResult result = cosResult as OptionObjectResult;
                    Console.WriteLine(result.GetResultInfo());
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

                manualResetEvent.Set();
            });

        }

        public void CopyObject(COSXML.CosXml cosXml, string bucket, string key, COSXML.Model.Tag.CopySourceStruct copySource)
        {
            try
            {
                CopyObjectRequest request = new CopyObjectRequest(bucket, key);
                //设置签名有效时长
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);

                //设置拷贝源
                request.SetCopySource(copySource);

                //设置是否拷贝还是更新
                request.SetCopyMetaDataDirective(COSXML.Common.CosMetaDataDirective.COPY);

                //执行请求
                CopyObjectResult result = cosXml.CopyObject(request);

                Console.WriteLine(result.GetResultInfo());
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.StackTrace);
                Assert.True(false);
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.True(false);
            }
        }

        public void AsynCopyObject(COSXML.CosXml cosXml, string bucket, string key, COSXML.Model.Tag.CopySourceStruct copySource)
        {
            CopyObjectRequest request = new CopyObjectRequest(bucket, key);
            //设置签名有效时长
            request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);

            //设置拷贝源
            request.SetCopySource(copySource);

            //设置是否拷贝还是更新
            request.SetCopyMetaDataDirective(COSXML.Common.CosMetaDataDirective.COPY);

            cosXml.CopyObject(request,
                delegate (CosResult cosResult)
                {
                    CopyObjectResult result = cosResult as CopyObjectResult;
                    Console.WriteLine(result.GetResultInfo());
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

                manualResetEvent.Set();
            });

        }

        public void MultiUpload(COSXML.CosXml cosXml, string bucket, string key, string srcPath)
        {
            try
            {
                InitMultipartUploadRequest initMultipartUploadRequest = new InitMultipartUploadRequest(bucket, key);
                //设置签名有效时长
                initMultipartUploadRequest.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);

                //执行请求
                InitMultipartUploadResult initMultipartUploadResult = cosXml.InitMultipartUpload(initMultipartUploadRequest);

                Console.WriteLine(initMultipartUploadResult.GetResultInfo());

                string uploadId = initMultipartUploadResult.initMultipartUpload.uploadId;

                ListPartsRequest listPartsRequest = new ListPartsRequest(bucket, key, uploadId);
                //设置签名有效时长
                listPartsRequest.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);

                //执行请求
                ListPartsResult listPartsResult = cosXml.ListParts(listPartsRequest);

                Console.WriteLine(listPartsResult.GetResultInfo());

                int partNumber = 1;

                UploadPartRequest uploadPartRequest = new UploadPartRequest(bucket, key, partNumber, uploadId, srcPath);
                //设置签名有效时长
                uploadPartRequest.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);

                //设置进度回调
                uploadPartRequest.SetCosProgressCallback(delegate (long completed, long total)
                {
                    //Console.WriteLine(String.Format("{0} progress = {1} / {2} : {3:##.##}%", DateTime.Now.ToString(), completed, total, completed * 100.0 / total));
                });

                //执行请求
                UploadPartResult uploadPartResult = cosXml.UploadPart(uploadPartRequest);

                Console.WriteLine(uploadPartResult.GetResultInfo());

                string eTag = uploadPartResult.eTag;

                CompleteMultipartUploadRequest completeMultiUploadRequest = new CompleteMultipartUploadRequest(bucket, key, uploadId);
                //设置签名有效时长
                completeMultiUploadRequest.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);

                //设置已上传的parts
                completeMultiUploadRequest.SetPartNumberAndETag(partNumber, eTag);

                //执行请求
                CompleteMultipartUploadResult completeMultiUploadResult = cosXml.CompleteMultiUpload(completeMultiUploadRequest);

                Console.WriteLine(completeMultiUploadResult.GetResultInfo());

            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.StackTrace);
                Assert.True(false);
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.True(false);
            }
        }

        public void AsynInitMultiUpload(COSXML.CosXml cosXml, string bucket, string key)
        {
            InitMultipartUploadRequest request = new InitMultipartUploadRequest(bucket, key);
            //设置签名有效时长
            request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);

            cosXml.InitMultipartUpload(request,
                delegate (CosResult cosResult)
                {
                    InitMultipartUploadResult result = cosResult as InitMultipartUploadResult;
                    Console.WriteLine(result.GetResultInfo());

                },
                delegate (CosClientException clientEx, CosServerException serverEx)
                {
                    Console.WriteLine(clientEx == null ? serverEx.GetInfo() : clientEx.Message);
                });

        }


        public void AsynListParts(COSXML.CosXml cosXml, string bucket, string key, string uploadId)
        {
            ListPartsRequest request = new ListPartsRequest(bucket, key, uploadId);
            //设置签名有效时长
            request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);

            cosXml.ListParts(request,
                delegate (CosResult cosResult)
                {
                    ListPartsResult result = cosResult as ListPartsResult;
                    Console.WriteLine(result.GetResultInfo());
                },
                delegate (CosClientException clientEx, CosServerException serverEx)
                {
                    Console.WriteLine(clientEx == null ? serverEx.GetInfo() : clientEx.Message);
                });

        }


        public void AsynUploadParts(COSXML.CosXml cosXml, string bucket, string key, string uploadId, int partNumber, string srcPath)
        {
            UploadPartRequest request = new UploadPartRequest(bucket, key, partNumber, uploadId, srcPath);
            //设置签名有效时长
            request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);

            request.SetCosProgressCallback(delegate (long completed, long total)
            {
                Console.WriteLine(String.Format("progress = {0} / {1} : {2:##.##}%", completed, total, completed * 100.0 / total));
            });

            //执行请求
            cosXml.UploadPart(request, delegate (CosResult result)
            {
                UploadPartResult getObjectResult = result as UploadPartResult;
                Console.WriteLine(getObjectResult.GetResultInfo());
            }, delegate (CosClientException clientEx, CosServerException serverEx)
            {
                Console.WriteLine(clientEx == null ? serverEx.GetInfo() : clientEx.Message);
            });
        }

        public void AsynCompleteMultiUpload(COSXML.CosXml cosXml, string bucket, string key, string uploadId, Dictionary<int, string> partNumberAndEtags)
        {
            CompleteMultipartUploadRequest request = new CompleteMultipartUploadRequest(bucket, key, uploadId);
            //设置签名有效时长
            request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);

            //设置已上传的parts
            request.SetPartNumberAndETag(partNumberAndEtags);

            //执行请求
            cosXml.CompleteMultiUpload(request, delegate (CosResult result)
            {
                CompleteMultipartUploadResult getObjectResult = result as CompleteMultipartUploadResult;
                Console.WriteLine(getObjectResult.GetResultInfo());
            }, delegate (CosClientException clientEx, CosServerException serverEx)
            {
                Console.WriteLine(clientEx == null ? serverEx.GetInfo() : clientEx.Message);
            });
        }

        public void AbortMultiUpload(COSXML.CosXml cosXml, string bucket, string key, string srcPath)
        {
            try
            {
                InitMultipartUploadRequest initMultipartUploadRequest = new InitMultipartUploadRequest(bucket, key);
                //设置签名有效时长
                initMultipartUploadRequest.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);

                //执行请求
                InitMultipartUploadResult initMultipartUploadResult = cosXml.InitMultipartUpload(initMultipartUploadRequest);

                Console.WriteLine(initMultipartUploadResult.GetResultInfo());

                string uploadId = initMultipartUploadResult.initMultipartUpload.uploadId;

                AbortMultipartUploadRequest request = new AbortMultipartUploadRequest(bucket, key, uploadId);
                //设置签名有效时长
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
                //执行请求
                AbortMultipartUploadResult result = cosXml.AbortMultiUpload(request);

                Console.WriteLine(result.GetResultInfo());
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

        public void AsynAbortMultiUpload(COSXML.CosXml cosXml, string bucket, string key, string uploadId)
        {
            AbortMultipartUploadRequest request = new AbortMultipartUploadRequest(bucket, key, uploadId);
            //设置签名有效时长
            request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);

            //执行请求
            cosXml.AbortMultiUpload(request, delegate (CosResult result)
            {
                AbortMultipartUploadResult getObjectResult = result as AbortMultipartUploadResult;
                Console.WriteLine(getObjectResult.GetResultInfo());
            }, delegate (CosClientException clientEx, CosServerException serverEx)
            {
                Console.WriteLine(clientEx == null ? serverEx.GetInfo() : clientEx.Message);
            });
        }

        public void PartCopyObject(COSXML.CosXml cosXml, string bucket, string key,
            COSXML.Model.Tag.CopySourceStruct copySource)
        {
            try
            {
                InitMultipartUploadRequest initMultipartUploadRequest = new InitMultipartUploadRequest(bucket, key);
                //设置签名有效时长
                initMultipartUploadRequest.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);

                //执行请求
                InitMultipartUploadResult initMultipartUploadResult = cosXml.InitMultipartUpload(initMultipartUploadRequest);

                Console.WriteLine(initMultipartUploadResult.GetResultInfo());

                string uploadId = initMultipartUploadResult.initMultipartUpload.uploadId;

                int partNumber = 1;

                UploadPartCopyRequest uploadPartCopyRequest = new UploadPartCopyRequest(bucket, key, partNumber, uploadId);
                //设置签名有效时长
                uploadPartCopyRequest.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);

                //设置拷贝源
                uploadPartCopyRequest.SetCopySource(copySource);

                //设置拷贝范围
                uploadPartCopyRequest.SetCopyRange(0, 10);

                //执行请求
                UploadPartCopyResult uploadPartCopyResult = cosXml.PartCopy(uploadPartCopyRequest);

                Console.WriteLine(uploadPartCopyResult.GetResultInfo());

                string eTag = uploadPartCopyResult.copyObject.eTag;

                CompleteMultipartUploadRequest completeMultiUploadRequest = new CompleteMultipartUploadRequest(bucket, key, uploadId);
                //设置签名有效时长
                completeMultiUploadRequest.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);

                //设置已上传的parts
                completeMultiUploadRequest.SetPartNumberAndETag(partNumber, eTag);

                //执行请求
                CompleteMultipartUploadResult completeMultiUploadResult = cosXml.CompleteMultiUpload(completeMultiUploadRequest);

                Console.WriteLine(completeMultiUploadResult.GetResultInfo());
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

        public void AsynPartCopyObject(COSXML.CosXml cosXml, string bucket, string key,
            COSXML.Model.Tag.CopySourceStruct copySource, string uploadId, int partNumber)
        {
            UploadPartCopyRequest request = new UploadPartCopyRequest(bucket, key, partNumber, uploadId);
            //设置签名有效时长
            request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);

            //设置拷贝源
            request.SetCopySource(copySource);

            //设置拷贝范围
            request.SetCopyRange(0, 10);

            //执行请求
            cosXml.PartCopy(request, delegate (CosResult result)
            {
                UploadPartCopyResult getObjectResult = result as UploadPartCopyResult;
                Console.WriteLine(getObjectResult.GetResultInfo());
            }, delegate (CosClientException clientEx, CosServerException serverEx)
            {
                Console.WriteLine(clientEx == null ? serverEx.GetInfo() : clientEx.Message);
            });
        }

        public void RestoreObject(COSXML.CosXml cosXml, string bucket, string key)
        {
            try
            {
                RestoreObjectRequest request = new RestoreObjectRequest(bucket, key);
                //设置签名有效时长
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);

                //恢复时间
                request.SetExpireDays(3);
                request.SetTier(COSXML.Model.Tag.RestoreConfigure.Tier.Bulk);

                //执行请求
                RestoreObjectResult result = cosXml.RestoreObject(request);

                Console.WriteLine(result.GetResultInfo());
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

        public void AsynRestoreObject(COSXML.CosXml cosXml, string bucket, string key)
        {
            RestoreObjectRequest request = new RestoreObjectRequest(bucket, key);
            //设置签名有效时长
            request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);

            //恢复时间
            request.SetExpireDays(3);
            request.SetTier(COSXML.Model.Tag.RestoreConfigure.Tier.Bulk);

            //执行请求
            cosXml.RestoreObject(request, delegate (CosResult result)
            {
                RestoreObjectResult getObjectResult = result as RestoreObjectResult;
                Console.WriteLine(getObjectResult.GetResultInfo());
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

                manualResetEvent.Set();
            });
        }

        public void PostObject(COSXML.CosXml cosXml, string bucket, string key, string srcPath, PostObjectRequest.Policy policy)
        {
            try
            {
                PostObjectRequest request = new PostObjectRequest(bucket, key, srcPath);
                //设置签名有效时长
                //request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);
                List<string> headers = new List<string>();
                headers.Add("Host");
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600, headers, null);

                request.SetCosProgressCallback(delegate (long completed, long total)
                {
                   // Console.WriteLine(String.Format("progress = {0} / {1} : {2:##.##}%", completed, total, completed * 100.0 / total));
                });

                //设置policy
                request.SetPolicy(policy);

                //执行请求
                PostObjectResult result = cosXml.PostObject(request);

                Console.WriteLine(result.GetResultInfo());
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


        public void AsynPostObject(COSXML.CosXml cosXml, string bucket, string key, string srcPath, PostObjectRequest.Policy policy)
        {
            PostObjectRequest request = new PostObjectRequest(bucket, key, srcPath);
            //设置签名有效时长
            request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);

            request.SetCosProgressCallback(delegate (long completed, long total)
            {
                Console.WriteLine(String.Format("progress = {0} / {1} : {2:##.##}%", completed, total, completed * 100.0 / total));
            });

            //设置policy
            request.SetPolicy(policy);

            //执行请求
            cosXml.PostObject(request, delegate (CosResult result)
            {
                PostObjectResult getObjectResult = result as PostObjectResult;
                Console.WriteLine(getObjectResult.GetResultInfo());
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

                manualResetEvent.Set();
            });
        }


        public void GetObject(COSXML.CosXml cosXml, string bucket, string key, string localDir, string localFileName)
        {

            try
            {
                GetObjectRequest request = new GetObjectRequest(bucket, key, localDir, localFileName);
                //设置签名有效时长
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);

                request.SetCosProgressCallback(delegate (long completed, long total)
                {
                   // Console.WriteLine(String.Format("progress = {0} / {1} : {2:##.##}%", completed, total, completed * 100.0 / total));
                });

                //执行请求
                GetObjectResult result = cosXml.GetObject(request);

                Console.WriteLine(result.GetResultInfo());
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


        public void AsyncGetObject(COSXML.CosXml cosXml, string bucket, string key, string localDir, string localFileName)
        {
            GetObjectRequest request = new GetObjectRequest(bucket, key, localDir, localFileName);
            //设置签名有效时长
            request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);

            request.SetCosProgressCallback(delegate (long completed, long total)
            {
                Console.WriteLine(String.Format("progress = {0} / {1} : {2:##.##}%", completed, total, completed * 100.0 / total));
            });

            //执行请求
            cosXml.GetObject(request, delegate (CosResult result)
            {
                GetObjectResult getObjectResult = result as GetObjectResult;
                Console.WriteLine(getObjectResult.GetResultInfo());
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

                manualResetEvent.Set();
            });
        }

        public void DeleteObject(COSXML.CosXml cosXml, string bucket, string key)
        {
            try
            {
                DeleteObjectRequest request = new DeleteObjectRequest(bucket, key);
                //设置签名有效时长
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);

                //执行请求
                DeleteObjectResult result = cosXml.DeleteObject(request);

                Console.WriteLine(result.GetResultInfo());
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


        public void AsynDeleteObject(COSXML.CosXml cosXml, string bucket, string key)
        {
            DeleteObjectRequest request = new DeleteObjectRequest(bucket, key);
            //设置签名有效时长
            request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);

            //执行请求
            cosXml.DeleteObject(request, delegate (CosResult result)
            {
                DeleteObjectResult getObjectResult = result as DeleteObjectResult;
                Console.WriteLine(getObjectResult.GetResultInfo());
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

                manualResetEvent.Set();
            });
        }

        public void MultiDeleteObject(COSXML.CosXml cosXml, string bucket, List<string> keys)
        {
            try
            {
                DeleteMultiObjectRequest request = new DeleteMultiObjectRequest(bucket);
                //设置签名有效时长
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);

                //设置返回结果形式
                request.SetDeleteQuiet(false);

                //设置删除的keys
                request.SetObjectKeys(keys);

                //执行请求
                DeleteMultiObjectResult result = cosXml.DeleteMultiObjects(request);

                Console.WriteLine(result.GetResultInfo());
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

        public void AsyncMultiDeleteObject(COSXML.CosXml cosXml, string bucket, List<string> keys)
        {
            DeleteMultiObjectRequest request = new DeleteMultiObjectRequest(bucket);
            //设置签名有效时长
            request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);

            //设置返回结果形式
            request.SetDeleteQuiet(false);

            //设置删除的keys
            request.SetObjectKeys(keys);

            //执行请求
            cosXml.DeleteMultiObjects(request, delegate (CosResult result)
            {
                DeleteMultiObjectResult getObjectResult = result as DeleteMultiObjectResult;
                Console.WriteLine(getObjectResult.GetResultInfo());
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

                manualResetEvent.Set();
            });
        }

        [Test()]
        public void testCreateDirectory() {
            QCloudServer instance = QCloudServer.Instance();
            try
            {
                PutObjectRequest request = new PutObjectRequest(instance.bucketForObjectTest, 
                    "dir/", new byte[0]);
                //设置签名有效时长
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);

                //执行请求
                PutObjectResult result = instance.cosXml.PutObject(request);

                Console.WriteLine(result.GetResultInfo());
            }
            catch (CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.Message);
                Assert.True(false);
            }
            catch (CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.True(false);
            }
        }

        [Test()]
        public void testGetObjectByte() {
            QCloudServer instance = QCloudServer.Instance();

            string key = "testGetObjectBytes.MOV";

            try
            {
                HeadObjectResult headResult = instance.cosXml.HeadObject(new HeadObjectRequest(instance.bucketForObjectTest, key));
                
            } 
            catch (COSXML.CosException.CosServerException serverEx)
            {
                if (serverEx.statusCode == 404) {
                    long fileLength = 1024 * 1024 * 10;
                    string srcPath = QCloudServer.CreateFile(TimeUtils.GetCurrentTime(TimeUnit.SECONDS) + ".docx", fileLength);
                    PutObject(instance.cosXml, instance.bucketForObjectTest, key, @srcPath);
                }
            }

            try
            {
                HeadObjectRequest request = new HeadObjectRequest(instance.bucketForObjectTest, key);
                //设置签名有效时长
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);

                //执行请求
                HeadObjectResult result = instance.cosXml.HeadObject(request);

                long contentLength = Int64.Parse(result.responseHeaders["Content-Length"][0]);

                GetObjectBytesRequest getObjectBytesRequest = new GetObjectBytesRequest(instance.bucketForObjectTest, key);
                //设置签名有效时长
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.SECONDS), 600);

                getObjectBytesRequest.SetCosProgressCallback(delegate (long completed, long total)
                {
                    Console.WriteLine(String.Format("{0} progress = {1} / {2} : {3:##.##}%", DateTime.Now.ToString(), completed, total, completed * 100.0 / total));
                });

                GetObjectBytesResult getObjectBytesResult = instance.cosXml.GetObject(getObjectBytesRequest);

                byte[] content = getObjectBytesResult.content;

                File.WriteAllBytes(key, content);

            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx);
                Assert.True(false);
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.True(false);
            }
        }

        [Test()]
        public void testSelectObjectToFile() {
            try
            {
                QCloudServer instance = QCloudServer.Instance();
                string key = "select_target.json";

                SelectObjectRequest request = new SelectObjectRequest(instance.bucketForObjectTest, key);

                ObjectSelectionFormat.JSONFormat jSONFormat = new ObjectSelectionFormat.JSONFormat();
                jSONFormat.Type = "DOCUMENT";
                jSONFormat.RecordDelimiter = "\n";
                
                string outputFile = "select_local_file.json";

                request.setExpression("Select * from COSObject")
                        .setInputFormat(new ObjectSelectionFormat(null, jSONFormat))
                        .setOutputFormat(new ObjectSelectionFormat(null, jSONFormat))
                        .SetCosProgressCallback(delegate (long progress, long total) {
                            Console.WriteLine("OnProgress : " + progress + "," + total);
                        })
                        .outputToFile(outputFile)
                        ;

                SelectObjectResult selectObjectResult =  instance.cosXml.selectObject(request);

                Console.WriteLine(selectObjectResult.stat);
                
                Assert.AreEqual(selectObjectResult.stat.BytesReturned, new FileInfo(outputFile).Length);
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.StackTrace);
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
        public void testSelectObjectInMemory() {
            try
            {
                QCloudServer instance = QCloudServer.Instance();
                string key = "select_target.json";

                SelectObjectRequest request = new SelectObjectRequest(instance.bucketForObjectTest, key);

                ObjectSelectionFormat.JSONFormat jSONFormat = new ObjectSelectionFormat.JSONFormat();
                jSONFormat.Type = "DOCUMENT";
                jSONFormat.RecordDelimiter = "\n";

                request.setExpression("Select * from COSObject")
                        .setInputFormat(new ObjectSelectionFormat(null, jSONFormat))
                        .setOutputFormat(new ObjectSelectionFormat(null, jSONFormat))
                        .SetCosProgressCallback(delegate (long progress, long total) {
                            Console.WriteLine("OnProgress : " + progress + "," + total);
                        })
                        ;

                SelectObjectResult selectObjectResult =  instance.cosXml.selectObject(request);

                Console.WriteLine(selectObjectResult.stat);

                Assert.AreEqual(selectObjectResult.stat.BytesReturned, Encoding.UTF8.GetByteCount(
                    selectObjectResult.searchContent));
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.StackTrace);
                Console.WriteLine("CosClientException: " + clientEx.Message);
                Assert.True(false);
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.True(false);
            }
        }
        public void testUploadTask() {
            QCloudServer instance = QCloudServer.Instance();
            string key = "uploadTaskTest.txt";
            string srcPath = QCloudServer.CreateFile(TimeUtils.GetCurrentTime(TimeUnit.SECONDS) + ".txt", 1024 * 1024 * 10);

            TransferManager transferManager = new TransferManager(instance.cosXml, new TransferConfig());
            COSXMLUploadTask uploadTask = new COSXMLUploadTask(instance.bucketForObjectTest, instance.region, key);
            uploadTask.SetSrcPath(srcPath);

            var autoEvent = new AutoResetEvent(false);
            string eTag = null;

            uploadTask.progressCallback = delegate (long completed, long total)
            {
                Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
            };
            uploadTask.successCallback = delegate (CosResult cosResult) 
            {
                COSXML.Transfer.COSXMLUploadTask.UploadTaskResult result = cosResult as COSXML.Transfer.COSXMLUploadTask.UploadTaskResult;
                Console.WriteLine(result.GetResultInfo());
                autoEvent.Set();
                eTag = result.eTag;
            };
            uploadTask.failCallback = delegate (CosClientException clientEx, CosServerException serverEx) 
            {
                if (clientEx != null)
                {
                    Console.WriteLine("CosClientException: " + clientEx);
                }
                if (serverEx != null)
                {
                    Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                }
                autoEvent.Set();
            };
            transferManager.Upload(uploadTask);
            autoEvent.WaitOne();
            Assert.NotNull(eTag);
        }

        [Test()]
        public void testSimpleUpload() {
            QCloudServer instance = QCloudServer.Instance();
            string key = @"simpleUploadBigFile.txt";
            string srcPath = QCloudServer.CreateFile(TimeUtils.GetCurrentTime(TimeUnit.SECONDS) + ".txt", 1024 * 1024 * 10);

            PutObject(instance.cosXml, instance.bucketForObjectTest, key, @srcPath);
        }

        [Test()]
        public void testDownloadTrafficLimit() {
            QCloudServer instance = QCloudServer.Instance();
            string key = @"simpleUploadBigFile.txt";
            string filePath = @"localTempFile";
            FileInfo fileInfo = new FileInfo(filePath);

            try
            {
                long now = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
                GetObjectRequest request = new GetObjectRequest(instance.bucketForObjectTest, 
                    key, fileInfo.Directory.FullName, filePath);
                
                request.LimitTraffic(8 * 1000 * 1024);
                //执行请求
                GetObjectResult result = instance.cosXml.GetObject(request);
                long costTime = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds() - now;

                Console.WriteLine("costTime = " + costTime + "ms");
                Console.WriteLine(result.GetResultInfo());

                Assert.True(result.httpCode == 200);
                Assert.True(costTime > 8000 && costTime < 14000);
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
        public void testUploadTrafficLimit() {
            QCloudServer instance = QCloudServer.Instance();
            string key = @"simpleUploadBigFile.txt";
            string filePath = @"localTempFile";
            FileInfo fileInfo = new FileInfo(filePath);

            try
            {
                long now = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
                PutObjectRequest request = new PutObjectRequest(instance.bucketForObjectTest, 
                    key, fileInfo.FullName);
                
                request.LimitTraffic(8 * 1000 * 1024);
                //执行请求
                PutObjectResult result = instance.cosXml.PutObject(request);
                long costTime = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds() - now;
                
                Console.WriteLine("costTime = " + costTime + "ms");
                Console.WriteLine(result.GetResultInfo());

                Assert.True(result.httpCode == 200);
                Assert.True(costTime > 8000 && costTime < 14000);
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
        public void testPutObjectTrafficLimit() {
            QCloudServer instance = QCloudServer.Instance();
            string key = @"simpleUploadBigFile.txt";
            string filePath = @"localTempFile";
            FileInfo fileInfo = new FileInfo(filePath);

            try
            {
                long now = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
                PostObjectRequest request = new PostObjectRequest(instance.bucketForObjectTest, 
                    key, fileInfo.FullName);
                
                request.LimitTraffic(8 * 1000 * 1024);
                //执行请求
                PostObjectResult result = instance.cosXml.PostObject(request);
                long costTime = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds() - now;
                
                Console.WriteLine("costTime = " + costTime + "ms");
                Console.WriteLine(result.GetResultInfo());

                Assert.True(result.httpCode == 204);
                Assert.True(costTime > 8000 && costTime < 14000);
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
        public void testMultiUploadTrafficLimit()
        {
            QCloudServer instance = QCloudServer.Instance();
            string key = @"simpleUploadBigFile.txt";
            string filePath = @"localTempFile";
            FileInfo fileInfo = new FileInfo(filePath);
            string bucket = instance.bucketForObjectTest;

            try
            {
                InitMultipartUploadRequest initMultipartUploadRequest = new InitMultipartUploadRequest(bucket, key);

                //执行请求
                InitMultipartUploadResult initMultipartUploadResult = instance.cosXml.InitMultipartUpload(initMultipartUploadRequest);

                Console.WriteLine(initMultipartUploadResult.GetResultInfo());

                string uploadId = initMultipartUploadResult.initMultipartUpload.uploadId;

                CompleteMultipartUploadRequest completeMultiUploadRequest = new CompleteMultipartUploadRequest(bucket, key, uploadId);

                const int partCount = 2;
                long partLength = fileInfo.Length / partCount;
                for (int partNumber = 1; partNumber < 3; partNumber++) {
                    UploadPartRequest uploadPartRequest = new UploadPartRequest(bucket, key, partNumber, uploadId, filePath,
                        partLength * (partNumber - 1), partLength);
                    uploadPartRequest.LimitTraffic(8 * 500 * 1024);
                    
                    long now = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
                    UploadPartResult uploadPartResult = instance.cosXml.UploadPart(uploadPartRequest);
                    long costTime = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds() - now;
                    
                    Console.WriteLine("costTime = " + costTime + "ms");
                    Console.WriteLine(uploadPartResult.GetResultInfo());
                    Assert.True(costTime > 8000 && costTime < 14000);
                    
                    string eTag = uploadPartResult.eTag;
                    completeMultiUploadRequest.SetPartNumberAndETag(partNumber, eTag);
                }

                //执行请求
                CompleteMultipartUploadResult completeMultiUploadResult = instance.cosXml.CompleteMultiUpload(completeMultiUploadRequest);

                Console.WriteLine(completeMultiUploadResult.GetResultInfo());

            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx.StackTrace);
                Assert.True(false);
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                Assert.True(false);
            }
        }

        [Test()]
        public void testObject()
        {

            QCloudServer instance = QCloudServer.Instance();

            string key = "objecttest.txt";
            string srcPath = QCloudServer.CreateFile(TimeUtils.GetCurrentTime(TimeUnit.SECONDS) + ".txt", 1024 * 1024 * 1);
            FileInfo fileInfo = new FileInfo(srcPath);
            DirectoryInfo directoryInfo = fileInfo.Directory;
            QCloudServer.DeleteAllFile(directoryInfo.FullName, "*.txt");
            Console.WriteLine(srcPath);
            srcPath = QCloudServer.CreateFile(TimeUtils.GetCurrentTime(TimeUnit.SECONDS) + ".txt", 1024 * 1024 * 1);
            fileInfo = new FileInfo(srcPath);
            directoryInfo = fileInfo.Directory;
            PutObject(instance.cosXml, instance.bucketForObjectTest, key, @srcPath);

            PutObjectWithAES256(instance.cosXml, instance.bucketForObjectTest, "aes256_" + key, @srcPath);

            PutObjectWithCustomerKey(instance.cosXml, instance.bucketForObjectTest, "customerKey_" + key, @srcPath);

            // PutObjectWithKMS(instance.cosXml, instance.bucketForObjectTest, "KMS" + key, @srcPath);

            HeadObject(instance.cosXml, instance.bucketForObjectTest, key);

            PutObjectACL(instance.cosXml, instance.bucketForObjectTest, key);
            GetObjectACL(instance.cosXml, instance.bucketForObjectTest, key);

            OptionObject(instance.cosXml, instance.bucketForObjectTest, key);

            string localDir = directoryInfo.FullName;
            Console.WriteLine(localDir);
            GetObject(instance.cosXml, instance.bucketForObjectTest, key, localDir, "download.txt");

            QCloudServer.DeleteFile(localDir + Path.DirectorySeparatorChar + "download.txt");

            DeleteObject(instance.cosXml, instance.bucketForObjectTest, key);

            key = "multiObjecttest.txt";
            MultiUpload(instance.cosXml, instance.bucketForObjectTest, key, srcPath);


            AbortMultiUpload(instance.cosXml, instance.bucketForObjectTest, key, srcPath);

            string sourceAppid = instance.appid;
            string sourceBucket = "bucket-cssg-source-1253653367";
            string sourceRegion = instance.region;
            string sourceKey = "sourceObject";
            CopySourceStruct copySource = new CopySourceStruct(sourceAppid, sourceBucket, sourceRegion, sourceKey);
            key = "copy_" + key;
            CopyObject(instance.cosXml, instance.bucketForObjectTest, key, copySource);


            DeleteObject(instance.cosXml, instance.bucketForObjectTest, key);

            key = "multi_" + key;
            PartCopyObject(instance.cosXml, instance.bucketForObjectTest, key, copySource);
            DeleteObject(instance.cosXml, instance.bucketForObjectTest, key);

            key = "post_" + key;
            PostObject(instance.cosXml, instance.bucketForObjectTest, key, srcPath, null);

            List<string> keys = new List<string>();
            keys.Add(key);
            MultiDeleteObject(instance.cosXml, instance.bucketForObjectTest, keys);

            QCloudServer.DeleteFile(srcPath);

            Assert.Pass();

            //manualResetEvent = new ManualResetEvent(false);

            //key = "objecttest.txt";
            //AsynPutObject(instance.cosXml, instance.bucketForObjectTest, key, @srcPath);

            //manualResetEvent.WaitOne();
            //AsynHeadObject(instance.cosXml, instance.bucketForObjectTest, key);

            //manualResetEvent.WaitOne();
            //AsynPutObjectACL(instance.cosXml, instance.bucketForObjectTest, key);

            //manualResetEvent.WaitOne();
            //AsynGetObjectACL(instance.cosXml, instance.bucketForObjectTest, key);

            //manualResetEvent.WaitOne();
            //AsynOptionObject(instance.cosXml, instance.bucketForObjectTest, key);

            //manualResetEvent.WaitOne();
            //AsynDeleteObject(instance.cosXml, instance.bucketForObjectTest, key);

            //manualResetEvent.WaitOne();
            //key = "post_" + key;
            //AsynPostObject(instance.cosXml, instance.bucketForObjectTest, key, srcPath, null);

            //manualResetEvent.WaitOne();
            //keys = new List<string>();
            //keys.Add(key);
            //AsyncMultiDeleteObject(instance.cosXml, instance.bucketForObjectTest, keys);

            //manualResetEvent.WaitOne();

        }
    }
}

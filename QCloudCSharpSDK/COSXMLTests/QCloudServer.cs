using COSXML;
using COSXML.Auth;
using COSXML.Log;
using COSXML.Common;
using COSXML.Utils;
using COSXML.CosException;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using COSXML.Model;

namespace COSXMLTests
{
    public class QCloudServer
    {
        internal CosXml cosXml;

        private const int ServerFailTolerance = 3;

        internal string bucketForBucketTest
        {
            get
            {
                return "dotnet-ut-temp-" + TimeUtils.GetCurrentTime(TimeUnit.Seconds) + "-1251668577";
            }
        }

        internal string bucketForObjectTest;
        internal string bucketForLoggingTarget;

        internal string bucketVersioning;
        internal string regionForBucketVersioning;

        internal string region;

        internal string appid;

        internal string uin;

        private static QCloudServer instance;

        private string secretId;
        private string secretKey;

        public QCloudServer()
        {
            QLog.SetLogLevel(Level.V);

            uin = "2779643970";
            appid = "1251668577";
            bucketVersioning = "dotnet-ut-versioning-1251668577";
            regionForBucketVersioning = "ap-beijing";
            bucketForObjectTest = "dotnet-ut-obj-1251668577";
            bucketForLoggingTarget = "dotnet-ut-logging-target-1251668577";
            region = "ap-guangzhou";

            secretId = Environment.GetEnvironmentVariable("SECRET_ID");
            secretKey = Environment.GetEnvironmentVariable("SECRET_KEY");
            if (secretId == null)
            {
                secretId = Environment.GetEnvironmentVariable("SECRET_ID", EnvironmentVariableTarget.Machine);
                secretKey = Environment.GetEnvironmentVariable("SECERT_KEY", EnvironmentVariableTarget.Machine);
            }

            CosXmlConfig config = new CosXmlConfig.Builder()
                .SetRegion(region)
                .SetDebugLog(true)
                .IsHttps(true)
                .SetConnectionLimit(512)
                .SetConnectionTimeoutMs(10 * 1000)
                .SetReadWriteTimeoutMs(10 * 1000)
                .Build();


            long keyDurationSecond = 600;

            QCloudCredentialProvider qCloudCredentialProvider = new DefaultQCloudCredentialProvider(secretId, secretKey, keyDurationSecond);


            cosXml = new CosXmlServer(config, qCloudCredentialProvider);
        }

        public CosXml NewService(string newRegion)
        {
            CosXmlConfig config = new CosXmlConfig.Builder()
                .SetAppid(appid)
                .SetRegion(newRegion)
                .SetDebugLog(true)
                .SetConnectionLimit(512)
                .Build();

            QCloudCredentialProvider qCloudCredentialProvider = new DefaultQCloudCredentialProvider(secretId, secretKey, 600);

            return new CosXmlServer(config, qCloudCredentialProvider);
        }

        public static QCloudServer Instance()
        {
            lock (typeof(QCloudServer))
            {

                if (instance == null)
                {
                    instance = new QCloudServer();
                }

            }

            return instance;
        }

        public static string CreateFile(string filename, long size)
        {

            try
            {
                string path = null;
                FileStream fs = new FileStream(filename, FileMode.Create);

                fs.SetLength(size);
                path = fs.Name;
                fs.Close();

                return path;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void SetRequestACLData(CosRequest request)
        {
            request.GetType().GetMethod("SetCosACL", new[] { typeof(CosACL) }).Invoke(request, new object[] { CosACL.Private });

            COSXML.Model.Tag.GrantAccount readAccount = new COSXML.Model.Tag.GrantAccount();
            readAccount.AddGrantAccount("1131975903", "1131975903");
            request.GetType().GetMethod("SetXCosGrantRead").Invoke(request, new object[] { readAccount });

            COSXML.Model.Tag.GrantAccount writeAccount = new COSXML.Model.Tag.GrantAccount();
            writeAccount.AddGrantAccount("1131975903", "1131975903");
            var writeMethod = request.GetType().GetMethod("SetXCosGrantWrite");

            if (writeMethod != null)
            {
                writeMethod.Invoke(request, new object[] { writeAccount });
            }

            COSXML.Model.Tag.GrantAccount fullControlAccount = new COSXML.Model.Tag.GrantAccount();
            fullControlAccount.AddGrantAccount("2832742109", "2832742109");
            request.GetType().GetMethod("SetXCosReadWrite").Invoke(request, new object[] { fullControlAccount });
        }

        public static void DeleteFile(string path)
        {
            FileInfo fileInfo = new FileInfo(path);

            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
        }

        public static void DeleteAllFile(string dirPath, string regix)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(dirPath);

            if (directoryInfo.Exists)
            {
                FileInfo[] files = directoryInfo.GetFiles(regix);

                if (files != null && files.Length > 0)
                {

                    for (int i = 0, count = files.Length; i < count; i++)
                    {
                        Console.WriteLine(files[i].Name);
                        files[i].Delete();
                    }
                }
            }
        }

        internal static void TestWithServerFailTolerance(Action action)
        {
            for (int i = 0; i < ServerFailTolerance; i++)
            {
                try
                {
                    action.Invoke();
                    break;
                }
                catch (CosServerException ex)
                {
                    Console.WriteLine("Fail But With Tolerance: " + ex.StackTrace);
                }
            }
        }
    }
}

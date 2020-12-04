using COSXML;
using COSXML.Auth;
using COSXML.Common;
using COSXML.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace COSXMLTests
{
    public class QCloudServer
    {
        internal CosXml cosXml;

        internal string bucketForBucketTest 
        {
            get 
            {
                return "dotnet-ut-temp-" + TimeUtils.GetCurrentTime(TimeUnit.Seconds) + "-1253653367";
            }
        }

        internal string bucketForObjectTest;

        internal string bucketVersioning;
        internal string regionForBucketVersioning;

        internal string region;

        internal string appid;

        internal string uin;

        private static QCloudServer instance;

        private string secretId;
        private string secretKey;

        private QCloudServer()
        {
            uin = "1278687956";
            appid = "1253653367";
            bucketVersioning = "dotnet-ut-versioning-1253653367";
            regionForBucketVersioning = "ap-beijing";
            bucketForObjectTest = "dotnet-ut-obj-1253653367";
            region = "ap-guangzhou";

            secretId = Environment.GetEnvironmentVariable("COS_KEY");
            secretKey = Environment.GetEnvironmentVariable("COS_SECRET");
            if (secretId == null)
            {
                secretId = Environment.GetEnvironmentVariable("COS_KEY", EnvironmentVariableTarget.Machine);
                secretKey = Environment.GetEnvironmentVariable("COS_SECRET", EnvironmentVariableTarget.Machine);
            }

            CosXmlConfig config = new CosXmlConfig.Builder()
                .SetAppid(appid)
                .SetRegion(region)
                .SetDebugLog(true)
                .SetConnectionLimit(512)
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
    }
}

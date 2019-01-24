using COSXML;
using COSXML.Auth;
using COSXML.Common;
using COSXML.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


/* ============================================================================== 
* Copyright 2016-2019 Tencent Cloud. All Rights Reserved.
* Auth：bradyxiao 
* Date：2019/1/22 19:34:45 
* ==============================================================================*/

namespace COSXMLTests
{
    public class QCloudServer
    {
        internal CosXml cosXml;
        internal string bucketForBucketTest;
        internal string bucketForObjectTest;
        internal string region;
        internal string appid;

        private static QCloudServer instance;

        private QCloudServer()
        {
            appid = Environment.GetEnvironmentVariable("APPID");
            string secretId = Environment.GetEnvironmentVariable("SECRETID");
            string secretKey = Environment.GetEnvironmentVariable("SECRETKEY");
            region = Environment.GetEnvironmentVariable("REGION");
            bucketForBucketTest = Environment.GetEnvironmentVariable("BUCKETONE");
            bucketForObjectTest = Environment.GetEnvironmentVariable("BUCKETTWO");

            if (appid == null)
            {
                appid = Environment.GetEnvironmentVariable("APPID", EnvironmentVariableTarget.Machine);
                secretId = Environment.GetEnvironmentVariable("SECRETID", EnvironmentVariableTarget.Machine);
                secretKey = Environment.GetEnvironmentVariable("SECRETKEY", EnvironmentVariableTarget.Machine);
                region = Environment.GetEnvironmentVariable("REGION", EnvironmentVariableTarget.Machine);
                bucketForBucketTest = Environment.GetEnvironmentVariable("BUCKETONE", EnvironmentVariableTarget.Machine);
                bucketForObjectTest = Environment.GetEnvironmentVariable("BUCKETTWO", EnvironmentVariableTarget.Machine);
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

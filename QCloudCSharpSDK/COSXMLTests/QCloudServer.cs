using COSXML;
using COSXML.Auth;
using System;
using System.Collections.Generic;
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
        internal string bucketForBucketAPI;
        internal string bucketForObjectAPI;
        internal string region;
        internal string appid;

        private static QCloudServer instance;

        private QCloudServer()
        {
            string secretId = "";
            string secretKey = "";
            
            appid = Environment.GetEnvironmentVariable("APPID", EnvironmentVariableTarget.Machine);


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
    }
}

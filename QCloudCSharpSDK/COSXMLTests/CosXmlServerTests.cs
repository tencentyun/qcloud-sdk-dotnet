using NUnit.Framework;
using COSXML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COSXMLTests;

namespace COSXML.Tests
{
    [TestFixture()]
    public class CosXmlServerTests
    {
        [Test()]
        public void GetAppid()
        {
            QCloudServer qCloudServer = QCloudServer.Instance();
            Console.WriteLine(qCloudServer.appid);
            Assert.AreEqual(123, qCloudServer.appid);
        }
        
    }
}


/* ============================================================================== 
* Copyright 2016-2019 Tencent Cloud. All Rights Reserved.
* Auth：bradyxiao 
* Date：2019/1/22 20:01:29 
* ==============================================================================*/

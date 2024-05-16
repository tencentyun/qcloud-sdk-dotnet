using System;
using System.Collections.Concurrent;
using COSXML.CosException;
using COSXML.Log;
using COSXML.Model.Tag;
using COSXML.Transfer;
using NUnit.Framework;



namespace COSXML.Utils.Tests{
    
    [TestFixture()]
    public class CosUtilsTests
    {
        
        [Test()]
        public void StringTest()
        {
            try {
                StringUtils.Compare("SD", null, true);
            }
            catch (CosClientException)
            {
            }
            StringUtils.Compare("SD", "SD", true);
            string path = StringUtils.MergePath("/asdf/../");
            Assert.AreEqual(path ,"/");

            string PathNull=null;
            Utils.URLEncodeUtils.EncodePathOfURL(PathNull);
            Utils.URLEncodeUtils.Decode(PathNull);
        }

        [Test()]
        public void LogTest()
        {
            QLog.Verbose("Test", "LogTestVerbose",new Exception("Test"));
            QLog.Verbose("Test", "LogTestVerbose");
            QLog.Error("Test", "LogTestError");
            QLog.Warn("Test", "LogTestError");
            QLog.Warn("Test", "LogTestWarn", new Exception("Test"));
            QLog.Info("Test", "LogTestInfo", new Exception("Test"));
            QLog.Info("Test", "LogTestInfo");
        }


        [Test()]
        public void ExceptionTest()
        {
            CosServerError serverError = new CosServerError();
            new CosServerException(1,"test", serverError);
        }

        public void CrcTest()
        {
            Crc64.Combine(0,0,0);
        }
    }
    
    
    
}

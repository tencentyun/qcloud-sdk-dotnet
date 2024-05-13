using System;
using System.Collections.Concurrent;
using COSXML.CosException;
using COSXML.Log;
using NUnit.Framework;



namespace COSXML.Utils.Tests{
    
    [TestFixture()]
    public class CosUtilsTests
    {
        
        [Test()]
        public void StringTest()
        {
            try {
                StringUtils.Compare("SD", "", true);
            }
            catch (CosClientException)
            {
            }
            StringUtils.Compare("SD", "SD", true);
            string path = StringUtils.MergePath("/asdf/../");
            Assert.AreEqual(path ,"/");
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
    }
    
    
    
}

using NUnit.Framework;
using System;
using System.IO;
using COSXML;
using COSXML.Auth;
using COSXML.Model.Object;
using System.Linq;


namespace COSXMLTests
{
    [TestFixture()]
    public class CosRetryTest
    {

        private static QCloudServer instance;
        private CosXml cosXml;
        internal string region;
        internal string appid;
        // private string errHost;

        private string secretId;
        private string secretKey;

        private string bucket;




        [OneTimeSetUp]
        public void Setup()
        {
            bucket = Environment.GetEnvironmentVariable("ErrBucket");
            appid = Environment.GetEnvironmentVariable("ErrAppid");
            region = Environment.GetEnvironmentVariable("ErrRegion");
            secretId = Environment.GetEnvironmentVariable("SECRET_ID");
            secretKey = Environment.GetEnvironmentVariable("SECRET_KEY");
            // errHost = Environment.GetEnvironmentVariable("ERR_HOST");

            if (secretId == null)
            {
                secretId = Environment.GetEnvironmentVariable("SECRET_ID", EnvironmentVariableTarget.Machine);
                secretKey = Environment.GetEnvironmentVariable("SECERT_KEY", EnvironmentVariableTarget.Machine);
            }

            CosXmlConfig config = new CosXmlConfig.Builder()
                .SetRegion(region)
                .SetAppid(appid)
                .Build();

            long keyDurationSecond = 600;
            QCloudCredentialProvider qCloudCredentialProvider = new DefaultQCloudCredentialProvider(secretId, secretKey, keyDurationSecond);
            cosXml = new CosXmlServer(config, qCloudCredentialProvider);
        }

        public struct ErrorMsg
        {
            public int httpCode;
            public string Msg;
            public GetObjectResult result;
        }

        public ErrorMsg GetObject(string objkey, bool keepDefaultDomain)
        {
            ErrorMsg err = new ErrorMsg();
            try
            {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                string localDir = Path.GetTempPath(); //本地文件夹
                string localFileName = "my-local-temp-file"; //指定本地保存的文件名
                GetObjectRequest request = new GetObjectRequest(bucket, objkey, localDir, localFileName);
                //执行请求
                request.RetryKeepDefaultDomain = keepDefaultDomain;
                GetObjectResult result = cosXml.GetObject(request);
                err.httpCode = result.httpCode;
                err.Msg = "";
                err.result = result;
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                err.httpCode = clientEx.errorCode;
                err.Msg = clientEx.StackTrace;
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                err.httpCode = serverEx.statusCode;
                err.Msg = serverEx.StackTrace;
            }
            return err;
        }

        [Test()]
        public void getObject2xx()
        {
            ErrorMsg err;
            err = GetObject("200r", false);
            // int nums = CalcExecTimes(err.Msg);
            // Assert.True(err.httpCode == 200 && nums == 0);
            Assert.True(err.result.retryTimes == 0);


            err = GetObject("206r", false);
            Assert.True(err.result.retryTimes == 0);
            // nums = CalcExecTimes(err.Msg);
            // Assert.True(err.httpCode == 206 && nums == 0);
        }

        [Test()]
        public void getObject3xx()
        {
            ErrorMsg err;
            err = GetObject("301", false);
            // int nums = CalcExecTimes(err.Msg);
            // Assert.True(nums == 4);
            Assert.True(err.result.retryTimes >= 1);

            err = GetObject("301r", false);
            // nums = CalcExecTimes(err.Msg);
            // // Assert.True(nums == 0);
            Assert.True(err.result.retryTimes == 0);

            err = GetObject("302", false);
            // nums = CalcExecTimes(err.Msg);
            // // Assert.True(nums == 4);
            Assert.True(err.result.retryTimes >= 1);

            err = GetObject("302r", false);
            // nums = CalcExecTimes(err.Msg);
            // // Assert.True(nums == 0);
            Assert.True(err.result.retryTimes == 0);
        }

        //400是直接跑出异常的，而其他是ut设置成正确返回的
        [Test()]
        public void getObject4xx()
        {
            //因为reqId暴露出异常
            ErrorMsg err = GetObject("404", false);
            int nums = CalcExecTimes(err.Msg);
            Assert.True(nums >= 1);

            //404本身是一种正确的状态，所以不存在异常
            err = GetObject("404r", false);
            nums = CalcExecTimes(err.Msg);
            Assert.True(nums == 0);

            err = GetObject("402r", false);
            nums = CalcExecTimes(err.Msg);
            Assert.True(nums == 0);

            err = GetObject("402", false);
            nums = CalcExecTimes(err.Msg);
            Assert.True(nums >= 1);

        }

        [Test()]
        public void getObject5xx()
        {
            ErrorMsg err;
            err = GetObject("500r", false);
            // int nums = CalcExecTimes(err.Msg);
            // Assert.True(nums == 4);
            Assert.True(err.result.retryTimes >= 1);


            err = GetObject("500", false);
            // nums = CalcExecTimes(err.Msg);
            // Assert.True(nums == 4);
            Assert.True(err.result.retryTimes >= 1);

            err = GetObject("504", false);
            // nums = CalcExecTimes(err.Msg);
            // Assert.True(nums == 4);
            Assert.True(err.result.retryTimes >= 1);

            err = GetObject("504r", false);
            // nums = CalcExecTimes(err.Msg);
            // Assert.True(nums == 4);
            Assert.True(err.result.retryTimes >= 1);

            err = GetObject("503", false);
            // nums = CalcExecTimes(err.Msg);
            // Assert.True(nums == 4);
            Assert.True(err.result.retryTimes >= 1);

            err = GetObject("503r", false);
            // nums = CalcExecTimes(err.Msg);
            // Assert.True(nums == 4);
            Assert.True(err.result.retryTimes >= 1);
        }


        [OneTimeTearDown]
        public void Clear()
        {

        }


        public static int CalcExecTimes(string stack)
        {
            return CountSubstringWithLinq(stack, "InternalExcute");
        }

        public static int CountSubstringWithLinq(string input, string substring)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(substring) || substring.Length > input.Length)
                return 0;
            return Enumerable.Range(0, input.Length - substring.Length + 1)
                .Count(start => input.Substring(start, substring.Length) == substring);
        }
    }
}

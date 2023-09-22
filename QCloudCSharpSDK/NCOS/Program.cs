using COSXML.Model.Object;
using COSXML.Auth;
using COSXML.Transfer;
using System;
using COSXML;
using System.Threading.Tasks;
using COSXML.Model.Tag;
using COSXML.Model.Bucket;


namespace Process
{
    public class Process
    {
        private CosXml cosXml;

        //永久密钥
        string secretId = "";
        string secretKey = "";
        string uin = "";
        string appid = "";
        string region = "";

        string bucket = "";

        long DurationSecond = 24 * 60 * 60;

        static void Main(string[] args)
        {
            Process Process = new Process();
            Process.SetEnvironmentVariable();
            Process.InitCosXml();
            Process.DoSomething();
        }

        public void DoSomething()
        {
            Console.WriteLine("-----");
        }


        public void InitCosXml()
        {
            uin = Environment.GetEnvironmentVariable("UIN");
            appid = Environment.GetEnvironmentVariable("APPID");
            region = Environment.GetEnvironmentVariable("COS_REGION");
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
           
            QCloudCredentialProvider qCloudCredentialProvider = new DefaultQCloudCredentialProvider(secretId, secretKey, DurationSecond); //注意这个时间哈
            cosXml = new CosXmlServer(config, qCloudCredentialProvider);
        }

        public void SetEnvironmentVariable()
        {
            Environment.SetEnvironmentVariable("UIN", "");
            Environment.SetEnvironmentVariable("APPID", "");
            Environment.SetEnvironmentVariable("COS_REGION", "ap-guangzhou");
            Environment.SetEnvironmentVariable("SECRET_ID", "");
            Environment.SetEnvironmentVariable("SECRET_KEY", "");
        }

    }
}
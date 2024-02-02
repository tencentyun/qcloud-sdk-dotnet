using COSXML.Model.Object;
using COSXML.Auth;
using COSXML.Transfer;
using System;
using System.Diagnostics;
using System.Text;
using COSXML;
using System.Threading.Tasks;
using COSXML.CosException;
using COSXML.Model;
using COSXML.Model.Tag;
using COSXML.Model.Bucket;

namespace Process
{
    using COSXML.Model.Object;
    using COSXML.Auth;
    using COSXML.Transfer;
    using System;
    using COSXML;
    using System.Threading.Tasks;

    namespace COSSnippet
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
            private string localPath = "";


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
                Dictionary<int, string> map = new Dictionary<int, string>();
                // map[0] = "cos-access-log/2023/08/08/202308081720_4cf3c3e7-a872-4435-a82c-9b83f36f8cc8_000";
                map[0] = "wewe/Test";
                map[1] = "tmp.zip";
                
                foreach (var cospath in map)
                {
                    TransferDownloadObject( cospath.Value, "ssdsds"+cospath.Value).Wait();
                }
            }
            

            /// 高级接口下载对象
            public async Task TransferDownloadObject(string cosPathvar, string localFileNamevar)
            {
                // 初始化 TransferConfig
                TransferConfig transferConfig = new TransferConfig();
                // 手动设置高级下载接口的分块阈值为 20MB(默认为20MB), 从5.4.26版本开始支持！
                transferConfig.DivisionForDownload = 3 * 1024 * 1024;
                // 手动设置高级下载接口的分块大小为 10MB(默认为5MB),不建议此处设置过小的分块值,可能导致频繁重试或下载速度不合预期
                transferConfig.SliceSizeForDownload = 2 * 1024 * 1024;
                
                transferConfig.ByNewFunc = true;
                
                // 初始化 TransferManager
                TransferManager transferManager = new TransferManager(cosXml, transferConfig);
                
                String cosPath = "hadn.zip"; //对象在存储桶中的位置标识符，即称对象键======
                
                string localFileName = "wewew" ; //指定本地保存的文件名 done
                // 下载对象
                COSXMLDownloadTask downloadTask = new COSXMLDownloadTask(bucket, cosPath, localPath, localFileName);
                // 手动设置高级下载接口的并发数 (默认为5), 从5.4.26版本开始支持！
                downloadTask.SetMaxTasks(16);
                downloadTask.progressCallback = delegate(long completed, long total)
                {
                    Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
                };
                try
                {
                    COSXML.Transfer.COSXMLDownloadTask.DownloadTaskResult result = await transferManager.DownloadAsync(downloadTask);
                    Console.WriteLine(result.GetResultInfo());
                    string eTag = result.eTag;
                }
                catch (COSXML.CosException.CosClientException clientEx)
                {
                    //请求失败
                    Console.WriteLine("CosClientException: " + clientEx);
                }
                catch (COSXML.CosException.CosServerException serverEx)
                {
                    //请求失败
                    Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                }
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
                    .IsHttps(false)
                    .SetConnectionTimeoutMs(5000)
                    .SetReadWriteTimeoutMs(5000)
                    .Build();
                QCloudCredentialProvider qCloudCredentialProvider =
                    new DefaultQCloudCredentialProvider(secretId, secretKey, DurationSecond);
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
}
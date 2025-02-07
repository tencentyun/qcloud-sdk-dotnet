using COSXML;
using COSXML.Auth;
using COSXML.Model.Bucket;
using COSXML.Model.Tag;
namespace COSXMLDemo
{
    public class ListObjectModel
    {
        public CosXml cosXml;
        // 初始化COS服务实例
        public string bucket;

        public void InitParams()
        {
            bucket = Environment.GetEnvironmentVariable("BUCKET");
        }

        // 初始化COS服务实例
        private void InitCosXml()
        {
            string region = Environment.GetEnvironmentVariable("COS_REGION");
            CosXmlConfig config = new CosXmlConfig.Builder()
                .SetRegion(region) // 设置默认的地域, COS 地域的简称请参照 https://cloud.tencent.com/document/product/436/6224
                .Build();
            string secretId = Environment.GetEnvironmentVariable("SECRET_ID"); // 云 API 密钥 SecretId, 获取 API 密钥请参照 https://console.cloud.tencent.com/cam/capi
            string secretKey = Environment.GetEnvironmentVariable("SECRET_KEY"); // 云 API 密钥 SecretKey, 获取 API 密钥请参照 https://console.cloud.tencent.com/cam/capi
            long durationSecond = 600; //每次请求签名有效时长，单位为秒
            QCloudCredentialProvider qCloudCredentialProvider = new DefaultQCloudCredentialProvider(secretId, secretKey, durationSecond);
            this.cosXml = new CosXmlServer(config, qCloudCredentialProvider);
        }

        ListObjectModel()
        {
            InitCosXml();
            InitParams();
        }

        // 获取对象多版本列表第一页数据
        public void ListObjectsVersioning()
        {
            try
            {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                string bucket = "examplebucket-version-1250000000";
                ListBucketVersionsRequest request = new ListBucketVersionsRequest(bucket);
                //执行请求
                ListBucketVersionsResult result = cosXml.ListBucketVersions(request);
                //bucket的相关信息
                ListBucketVersions info = result.listBucketVersions;
                //请求结果状态
                Console.WriteLine(result.GetResultInfo());
                List<ListBucketVersions.Version> objects = info.objectVersionList;
                List<ListBucketVersions.CommonPrefixes> prefixes = info.commonPrefixesList;
                //返回信息
                Console.WriteLine(info);
                if (info.isTruncated)
                {
                    // 数据被截断，记录下数据下标
                    this.keyMarker = info.nextKeyMarker;
                    this.versionIdMarker = info.nextVersionIdMarker;
                }
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx);
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
            }
        }

        private string keyMarker = "";
        private string versionIdMarker = "";
        // 获取对象多版本列表下一页数据
        public void ListObjectsVersioningNextPage()
        {
            try
            {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                string bucket = "examplebucket-version-1250000000";
                ListBucketVersionsRequest request = new ListBucketVersionsRequest(bucket);
                // 上一页的数据结束下标
                request.SetKeyMarker(this.keyMarker);
                request.SetVersionIdMarker(this.versionIdMarker);
                //执行请求
                ListBucketVersionsResult result = cosXml.ListBucketVersions(request);
                //请求结果状态
                Console.WriteLine(result.GetResultInfo());
                ListBucketVersions info = result.listBucketVersions;
                Console.WriteLine(info.GetInfo());
                if (info.isTruncated)
                {
                    // 数据被截断，记录下数据下标
                    this.keyMarker = info.nextKeyMarker;
                    this.versionIdMarker = info.nextVersionIdMarker;
                }
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx);
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
            }
        }

        public string nextMarker;
        
        
        // 获取第一页对象列表
        public void GetBucketFirstPage()
        {
            try
            {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                string bucket = "examplebucket-1250000000";
                GetBucketRequest request = new GetBucketRequest(bucket);
                //执行请求
                GetBucketResult result = cosXml.GetBucket(request);
                //bucket的相关信息
                COSXML.Model.Tag.ListBucket info = result.listBucket;
                if (info.isTruncated)
                {
                    // 数据被截断，记录下数据下标
                    this.nextMarker = info.nextMarker;
                }
                Console.WriteLine(result.GetResultInfo());
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx);
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
            }
        }

        // 获取第二页对象列表
        public void GetBucketNextPage()
        {
            try
            {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                string bucket = "examplebucket-1250000000";
                GetBucketRequest request = new GetBucketRequest(bucket);
                //上一次拉取数据的下标
                request.SetMarker(this.nextMarker);
                //执行请求
                GetBucketResult result = cosXml.GetBucket(request);
                //bucket的相关信息
                COSXML.Model.Tag.ListBucket info = result.listBucket;
                Console.WriteLine(result.GetResultInfo());
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx);
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
            }
        }

        // 获取对象列表与子目录
        public void GetBucketWithDelimiter()
        {
            try
            {
                // 存储桶名称，此处填入格式必须为 bucketname-APPID, 其中 APPID 获取参考 https://console.cloud.tencent.com/developer
                string bucket = "examplebucket-1250000000";
                GetBucketRequest request = new GetBucketRequest(bucket);
                //获取 a/ 下的对象以及子目录
                request.SetPrefix("dir/");
                request.SetDelimiter("/");
                //执行请求
                GetBucketResult result = cosXml.GetBucket(request);
                //bucket的相关信息
                COSXML.Model.Tag.ListBucket info = result.listBucket;
                // 对象列表
                List<COSXML.Model.Tag.ListBucket.Contents> objects = info.contentsList;
                // 子目录列表
                List<COSXML.Model.Tag.ListBucket.CommonPrefixes> subDirs = info.commonPrefixesList;
                Console.WriteLine(result.GetResultInfo());
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                Console.WriteLine("CosClientException: " + clientEx);
            }
            catch (COSXML.CosException.CosServerException serverEx)
            {
                Console.WriteLine("CosServerException: " + serverEx.GetInfo());
            }
        }
        
        public static void ListObjectModelMain()
        {
            ListObjectModel demo = new ListObjectModel();
            //获取列表对象
            // demo.GetBucketFirstPage();
            // demo.GetBucketNextPage();
            // demo.GetBucketWithDelimiter();
            demo.ListObjectsVersioning();
            demo.ListObjectsVersioningNextPage();
        }

    }
}

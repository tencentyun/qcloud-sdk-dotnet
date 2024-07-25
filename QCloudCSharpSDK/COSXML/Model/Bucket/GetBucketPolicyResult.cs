namespace COSXML.Model.Bucket
{
    /// <summary>
    /// 获取 Bucket 权限策略
    /// <see href="https://cloud.tencent.com/document/product/436/8276"/>
    /// </summary>
    public sealed class GetBucketPolicyResult : CosDataStringResult
    {
        public string Data
        { 
            get{ return _data; } 
        }
    }
}

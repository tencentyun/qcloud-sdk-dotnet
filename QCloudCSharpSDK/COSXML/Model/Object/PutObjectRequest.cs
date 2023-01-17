using COSXML.Common;
using System.IO;
using COSXML.Model.Tag;
using COSXML.Utils;
using COSXML.CosException;
using COSXML.Network;

namespace COSXML.Model.Object
{
    /// <summary>
    /// 简单上传对象
    /// <see href="https://cloud.tencent.com/document/product/436/7749"/>
    /// </summary>
    public sealed class PutObjectRequest : ObjectRequest
    {
        private static string TAG = typeof(PutObjectRequest).FullName;

        /// <summary>
        /// 本地文件路径
        /// </summary>
        private string srcPath;

        /// <summary>
        /// 上传文件指定起始位置
        /// </summary>
        private long fileOffset = 0L;

        /// <summary>
        /// 上传data数据
        /// </summary>
        private byte[] data;

        /// <summary>
        /// 上传指定内容的长度
        /// </summary>
        private long contentLength = -1L;

        /// <summary>
        /// 上传流
        /// </summary>
        private Stream stream;

        /// <summary>
        /// 上传回调
        /// </summary>
        private COSXML.Callback.OnProgressCallback progressCallback;

        /// <summary>
        /// 上传整个文件
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="key"></param>
        /// <param name="srcPath"></param>
        public PutObjectRequest(string bucket, string key, string srcPath)
            : this(bucket, key, srcPath, -1L, -1L)
        {

        }

        /// <summary>
        /// 上传文件的指定内容
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="key"></param>
        /// <param name="srcPath"></param>
        /// <param name="fileOffset">文件指定起始位置</param>
        /// <param name="needSendLength">文件指定内容长度</param>
        public PutObjectRequest(string bucket, string key, string srcPath, long fileOffset, long needSendLength)
            : base(bucket, key)
        {
            this.method = CosRequestMethod.PUT;
            this.srcPath = srcPath;
            this.fileOffset = fileOffset < 0 ? 0 : fileOffset;
            this.contentLength = needSendLength < 0 ? -1L : needSendLength;
        }

        /// <summary>
        /// 上传data数据
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public PutObjectRequest(string bucket, string key, byte[] data) : base(bucket, key)
        {
            this.method = CosRequestMethod.PUT;
            this.data = data;
        }

        /// <summary>
        /// 流式上传
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="key"></param>
        /// <param name="stream"></param>
        public PutObjectRequest(string bucket, string key, Stream stream) : base(bucket, key)
        {
            this.stream = stream;
            this.method = CosRequestMethod.PUT;
        }

        /// <summary>
        /// 指定 offset 的流式上传
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="key"></param>
        /// <param name="stream"></param>
        /// <param name="offset"></param>
        public PutObjectRequest(string bucket, string key, Stream stream, long offset) : this(bucket, key, stream)
        {
            this.fileOffset = offset;
        }

        /// <summary>
        /// 指定 offset + content-length 的流式上传
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="key"></param>
        /// <param name="stream"></param>
        /// <param name="offset"></param>
        /// <param name="needSendLength"></param>
        public PutObjectRequest(string bucket, string key, Stream stream, long offset, long needSendLength) : this(bucket, key, stream, offset)
        {
            this.contentLength = needSendLength < 0 ? -1L : needSendLength;
        }

        

        /// <summary>
        /// 上传回调
        /// </summary>
        /// <param name="progressCallback"></param>
        public void SetCosProgressCallback(COSXML.Callback.OnProgressCallback progressCallback)
        {
            this.progressCallback = progressCallback;
        }

        /// <summary>
        /// 设置 Object 的存储级别
        /// <see href="Common.CosStorageClass"/>
        /// </summary>
        /// <param name="cosStorageClass"></param>
        public void SetCosStorageClass(string cosStorageClass)
        {
            SetRequestHeader(CosRequestHeaderKey.X_COS_STORAGE_CLASS, cosStorageClass);
        }

        public override void CheckParameters()
        {

            if (srcPath == null && data == null && stream == null)
            {
                throw new CosClientException((int)(CosClientError.InvalidArgument), "data source = null");
            }

            if (srcPath != null)
            {

                if (!File.Exists(srcPath))
                {
                    throw new CosClientException((int)(CosClientError.InvalidArgument), "file not exist");
                }
            }

            if (stream != null)
            {
                if (stream.Length <= fileOffset ||
                    !stream.CanRead ||
                    !stream.CanSeek)
                    {
                        throw new CosClientException((int)(CosClientError.InvalidArgument), "stream invalid");
                    }
            }

            base.CheckParameters();
        }

        public override Network.RequestBody GetRequestBody()
        {
            RequestBody body = null;

            if (srcPath != null)
            {
                FileInfo fileInfo = new FileInfo(srcPath);

                if (contentLength == -1 || contentLength + fileOffset > fileInfo.Length)
                {
                    contentLength = fileInfo.Length - fileOffset;
                }

                body = new FileRequestBody(srcPath, fileOffset, contentLength);
                body.ProgressCallback = progressCallback;
            }
            else if (data != null)
            {
                body = new ByteRequestBody(data);
                body.ProgressCallback = progressCallback;
            }
            else if (stream != null)
            {
                if (fileOffset < 0)
                {
                    fileOffset = 0L;
                }
                if (contentLength == -1L)
                {
                    contentLength = stream.Length - fileOffset;
                }
                if (fileOffset + contentLength > stream.Length)
                {
                    throw new CosException.CosClientException(
                        (int)COSXML.Common.CosClientError.InvalidArgument, 
                        "stream offset + contentLength greater than stream.Length");
                }
                body = new StreamRequestBody(stream, fileOffset, contentLength);
            }

            return body;
        }

        /// <summary>
        /// 定义 Object 的 acl 属性。有效值：private，public-read-write，public-read；默认值：private
        /// <see href="Common.CosACL"/>
        /// </summary>
        /// <param name="cosACL"></param>
        public void SetCosACL(string cosACL)
        {

            if (cosACL != null)
            {
                SetRequestHeader(CosRequestHeaderKey.X_COS_ACL, cosACL);
            }
        }

        /// <summary>
        /// 最大上传速度，单位是 bit/s
        /// </summary>
        /// <param name="rate"></param>
        public void LimitTraffic(long rate)
        {
            SetRequestHeader(CosRequestHeaderKey.X_COS_TRAFFIC_LIMIT, rate.ToString());
        }

        /// <summary>
        /// 定义 Object 的 acl 属性。有效值：private，public-read-write，public-read；默认值：private
        /// <see href="Common.CosACL"/>
        /// </summary>
        /// <param name="cosACL"></param>
        public void SetCosACL(CosACL cosACL)
        {
            SetCosACL(EnumUtils.GetValue(cosACL));
        }

        /// <summary>
        /// 赋予被授权者读的权限
        /// <see href="Model.Tag.GrantAccount"/>
        /// </summary>
        /// <param name="grantAccount"></param>
        public void SetXCosGrantRead(GrantAccount grantAccount)
        {

            if (grantAccount != null)
            {
                SetRequestHeader(CosRequestHeaderKey.X_COS_GRANT_READ, grantAccount.GetGrantAccounts());
            }
        }

        /// <summary>
        /// 赋予被授权者所有的权限
        /// <see href="Model.Tag.GrantAccount"/>
        /// </summary>
        /// <param name="grantAccount"></param>
        public void SetXCosReadWrite(GrantAccount grantAccount)
        {

            if (grantAccount != null)
            {
                SetRequestHeader(CosRequestHeaderKey.X_COS_GRANT_FULL_CONTROL, grantAccount.GetGrantAccounts());
            }
        }

    }
}

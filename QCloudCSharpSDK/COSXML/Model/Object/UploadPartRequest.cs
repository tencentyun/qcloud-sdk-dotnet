using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;
using System.IO;
using COSXML.Log;
using COSXML.CosException;
using COSXML.Network;

namespace COSXML.Model.Object
{
    /// <summary>
    /// 分片上传
    /// <see cref="https://cloud.tencent.com/document/product/436/7750"/>
    /// </summary>
    public sealed class UploadPartRequest : ObjectRequest
    {
        private static string TAG = typeof(UploadPartRequest).FullName;

        /// <summary>
        /// 分片块编号
        /// </summary>
        private int partNumber;

        /// <summary>
        /// 分片上传的UploadId
        /// </summary>
        private string uploadId;

        /// <summary>
        /// 本地文件路径
        /// </summary>
        private string srcPath;

        /// <summary>
        /// 上传文件指定起始位置
        /// </summary>
        private long fileOffset = -1L;

        /// <summary>
        /// 上传指定内容的长度
        /// </summary>
        private long contentLength = -1L;

        /// <summary>
        /// 上传回调
        /// </summary>
        private COSXML.Callback.OnProgressCallback progressCallback;


        private UploadPartRequest(string bucket, string key, int partNumber, string uploadId)
            : base(bucket, key)
        {
            this.method = CosRequestMethod.PUT;
            this.partNumber = partNumber;
            this.uploadId = uploadId;
        }

        /// <summary>
        /// 上传文件的指定内容
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="key"></param>
        /// <param name="partNumber"></param>
        /// <param name="uploadId"></param>
        /// <param name="srcPath"></param>
        /// <param name="fileOffset">文件指定起始位置</param>
        /// <param name="fileSendLength">文件指定内容长度</param>
        public UploadPartRequest(string bucket, string key, int partNumber, string uploadId, string srcPath, long fileOffset,
            long fileSendLength)
            : this(bucket, key, partNumber, uploadId)
        {
            this.srcPath = srcPath;
            this.fileOffset = fileOffset < 0 ? 0 : fileOffset;
            this.contentLength = fileSendLength < 0 ? -1L : fileSendLength;
        }

        /// <summary>
        /// 最大上传速度，单位是 bit/s
        /// </summary>
        /// <param name="start"></param>
        public void LimitTraffic(long rate)
        {
            SetRequestHeader(CosRequestHeaderKey.X_COS_TRAFFIC_LIMIT, rate.ToString());
        }

        /// <summary>
        /// 设置回调
        /// </summary>
        /// <param name="progressCallback"></param>
        public void SetCosProgressCallback(COSXML.Callback.OnProgressCallback progressCallback)
        {
            this.progressCallback = progressCallback;
        }

        public override void CheckParameters()
        {

            if (srcPath == null)
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

            if (requestUrlWithSign != null)
            {

                return;
            }

            base.CheckParameters();

            if (partNumber <= 0)
            {
                throw new CosClientException((int)CosClientError.InvalidArgument, "partNumber < 1");
            }

            if (uploadId == null)
            {
                throw new CosClientException((int)CosClientError.InvalidArgument, "uploadId = null");
            }
        }

        protected override void InternalUpdateQueryParameters()
        {

            try
            {
                queryParameters.Add("uploadId", uploadId);
            }
            catch (ArgumentException)
            {
                queryParameters["uploadId"] = uploadId;
            }

            try
            {
                queryParameters.Add("partNumber", partNumber.ToString());
            }
            catch (ArgumentException)
            {
                queryParameters["partNumber"] = partNumber.ToString();
            }
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

            return body;
        }
    }
}

using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Model.Tag;
using COSXML.Common;
using COSXML.CosException;
using COSXML.Network;

namespace COSXML.Model.Object
{
    /// <summary>
    /// 实现完成整个分块上传
    /// <see cref="https://cloud.tencent.com/document/product/436/7742"/>
    /// </summary>
    public sealed class CompleteMultipartUploadRequest : ObjectRequest
    {
        /// <summary>
        /// 本次分块上传的所有信息
        /// <see cref="Model.Tag.CompleteMultipartUpload"/>
        /// </summary>
        private CompleteMultipartUpload completeMultipartUpload;

        /// <summary>
        /// 标识本次分块上传的 ID,
        /// 使用 Initiate Multipart Upload 接口初始化分片上传时会得到一个 uploadId，
        /// 该 ID 不但唯一标识这一分块数据，也标识了这分块数据在整个文件内的相对位置
        /// </summary>
        private string uploadId;

        public CompleteMultipartUploadRequest(string bucket, string key, string uploadId)
            : base(bucket, key)
        {
            this.method = CosRequestMethod.POST;
            this.uploadId = uploadId;
            completeMultipartUpload = new CompleteMultipartUpload();
            completeMultipartUpload.parts = new List<CompleteMultipartUpload.Part>();
        }

        /// <summary>
        /// 添加分片块（块编号，块ETag值）
        /// </summary>
        /// <param name="partNumber"></param>
        /// <param name="eTag"></param>
        public void SetPartNumberAndETag(int partNumber, string eTag)
        {
            CompleteMultipartUpload.Part part = new CompleteMultipartUpload.Part();

            part.partNumber = partNumber;
            part.eTag = eTag;
            completeMultipartUpload.parts.Add(part);
        }

        /// <summary>
        /// 添加分片块（块编号，块ETag值）
        /// </summary>
        /// <param name="partNumberAndETags"></param>
        public void SetPartNumberAndETag(Dictionary<int, string> partNumberAndETags)
        {

            if (partNumberAndETags != null)
            {

                foreach (KeyValuePair<int, string> pair in partNumberAndETags)
                {
                    SetPartNumberAndETag(pair.Key, pair.Value);
                }
            }
        }

        public override Network.RequestBody GetRequestBody()
        {
            return GetXmlRequestBody(completeMultipartUpload);
        }

        public override void CheckParameters()
        {

            if (completeMultipartUpload.parts.Count == 0)
            {
                throw new CosClientException((int)CosClientError.InvalidArgument, "completeMultipartUpload.parts count = 0");
            }

            if (requestUrlWithSign != null)
            {

                return;
            }

            base.CheckParameters();

            if (uploadId == null)
            {
                throw new CosClientException((int)CosClientError.InvalidArgument, "uploadId is null");
            }
        }

        protected override void InternalUpdateQueryParameters()
        {

            try
            {
                this.queryParameters.Add("uploadId", uploadId);
            }
            catch (ArgumentException)
            {
                this.queryParameters["uploadId"] = uploadId;
            }

        }
    }
}

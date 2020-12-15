using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;
using COSXML.CosException;

namespace COSXML.Model.Object
{
    /// <summary>
    /// 舍弃一个分块上传并删除已上传的块
    /// <see cref="https://cloud.tencent.com/document/product/436/7740"/>
    /// </summary>
    public sealed class AbortMultipartUploadRequest : ObjectRequest
    {
        /// <summary>
        /// 分片块的UploadId,使用 Initiate Multipart Upload 接口初始化分片上传时会得到一个 uploadId，该 ID 不但唯一标识这一分块数据，也标识了这分块数据在整个文件内的相对位置
        /// </summary>
        private string uploadId;

        public AbortMultipartUploadRequest(string bucket, string key, string uploadId)
            : base(bucket, key)
        {
            this.uploadId = uploadId;
            this.method = CosRequestMethod.DELETE;
        }

        public override void CheckParameters()
        {

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

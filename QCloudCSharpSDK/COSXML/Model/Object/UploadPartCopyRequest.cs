using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;
using COSXML.Model.Tag;
using COSXML.CosException;

namespace COSXML.Model.Object
{
    /// <summary>
    /// 分片复制
    /// <see cref="https://cloud.tencent.com/document/product/436/12633"/>
    /// </summary>
    public sealed class UploadPartCopyRequest : ObjectRequest
    {
        /// <summary>
        /// 拷贝的数据源
        /// <see cref="Model.Tag.CopySourceStruct"/>
        /// </summary>
        private CopySourceStruct copySourceStruct;
        /**Specified part number*/
        private int partNumber = -1;
        /**init upload generate' s uploadId by service*/
        private String uploadId = null;

        public UploadPartCopyRequest(string bucket, string key, int partNumber, string uploadId)
            : base(bucket, key)
        {
            this.method = CosRequestMethod.PUT;
            this.partNumber = partNumber;
            this.uploadId = uploadId;
        }
        /// <summary>
        /// 设置拷贝数据源
        /// <see cref="Model.Tag.CopySourceStruct"/>
        /// </summary>
        /// <param name="copySource"></param>
        public void SetCopySource(CopySourceStruct copySource)
        {
            this.copySourceStruct = copySource;
        }
        /// <summary>
        /// 设置拷贝的分片范围
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void SetCopyRange(long start, long end)
        {
            if (start >= 0 && end >= start)
            {
                string bytes = String.Format("bytes={0}-{1}", start, end);
                SetRequestHeader(CosRequestHeaderKey.X_COS_COPY_SOURCE_RANGE, bytes);
            }
        }
        /// <summary>
        /// 当 Object 在指定时间后被修改，则执行操作，否则返回 412。
        /// 可与 x-cos-copy-source-If-None-Match 一起使用，与其他条件联合使用返回冲突
        /// </summary>
        /// <param name="sourceIfModifiedSince"></param>
        public void SetCopyIfModifiedSince(string sourceIfModifiedSince)
        {
            if (sourceIfModifiedSince != null)
            {
                SetRequestHeader(CosRequestHeaderKey.X_COS_COPY_SOURCE_IF_MODIFIED_SINCE, sourceIfModifiedSince);
            }
        }
        /// <summary>
        /// 当 Object 在指定时间后未被修改，则执行操作，否则返回 412。
        /// 可与 x-cos-copy-source-If-Match 一起使用，与其他条件联合使用返回冲突。
        /// </summary>
        /// <param name="sourceIfUnmodifiedSince"></param>
        public void SetCopyIfUnmodifiedSince(string sourceIfUnmodifiedSince)
        {
            if (sourceIfUnmodifiedSince != null)
            {
                SetRequestHeader(CosRequestHeaderKey.X_COS_COPY_SOURCE_IF_UNMODIFIED_SINCE, sourceIfUnmodifiedSince);
            }
        }
        /// <summary>
        ///  Object 的 Etag 和给定一致时，则执行操作，否则返回 412。
        ///  可与 x-cos-copy-source-If-Unmodified-Since 一起使用，与其他条件联合使用返回冲突。
        /// </summary>
        /// <param name="eTag"></param>
        public void SetCopyIfMatch(string eTag)
        {
            if (eTag != null)
            {
                SetRequestHeader(CosRequestHeaderKey.X_COS_COPY_SOURCE_IF_MATCH, eTag);
            }
        }
        /// <summary>
        /// 当 Object 的 Etag 和给定不一致时，则执行操作，否则返回 412。
        /// 可与 x-cos-copy-source-If-Modified-Since 一起使用，与其他条件联合使用返回冲突
        /// </summary>
        /// <param name="eTag"></param>
        public void SetCopyIfNoneMatch(string eTag)
        {
            if (eTag != null)
            {
                SetRequestHeader(CosRequestHeaderKey.X_COS_COPY_SOURCE_IF_NONE_MATCH, eTag);
            }
        }

        public override void CheckParameters()
        {
            if (copySourceStruct == null)
            {
                throw new CosClientException((int)CosClientError.INVALID_ARGUMENT, "copy source = null");
            }
            else
            {
                copySourceStruct.CheckParameters();
            }
            if (requestUrlWithSign != null) return;
            base.CheckParameters();
            if (partNumber <= 0)
            {
                throw new CosClientException((int)CosClientError.INVALID_ARGUMENT, "partNumber < 1");
            }
            if (uploadId == null)
            {
                throw new CosClientException((int)CosClientError.INVALID_ARGUMENT, "uploadID = null");
            }
        }

        protected override void InternalUpdateQueryParameters()
        {
            try
            {
                this.queryParameters.Add("partNumber", partNumber.ToString());
            }
            catch (ArgumentException)
            {
                this.queryParameters["partNumber"] = partNumber.ToString();
            }
            try
            {
                this.queryParameters.Add("uploadId", uploadId);
            }
            catch (ArgumentException)
            {
                this.queryParameters["uploadId"] = uploadId;
            }
        }

        protected override void InteranlUpdateHeaders()
        {
            try
            {
                this.headers.Add(CosRequestHeaderKey.X_COS_COPY_SOURCE, copySourceStruct.GetCopySouce());
            }
            catch (ArgumentException)
            {
                this.headers[CosRequestHeaderKey.X_COS_COPY_SOURCE] = copySourceStruct.GetCopySouce();
            }
        }

    }
}

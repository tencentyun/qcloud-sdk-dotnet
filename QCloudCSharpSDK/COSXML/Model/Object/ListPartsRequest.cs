using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;
using COSXML.CosException;

namespace COSXML.Model.Object
{
    /// <summary>
    /// 查询特定分块上传中的已上传的块
    /// <see cref="https://cloud.tencent.com/document/product/436/7747"/>
    /// </summary>
    public sealed class ListPartsRequest : ObjectRequest
    {
        /// <summary>
        /// 标识本次分块上传的 ID
        /// </summary>
        private string uploadId;

        public ListPartsRequest(string bucket, string key, string uploadId)
            : base(bucket, key)
        {
            this.method = CosRequestMethod.GET;
            this.uploadId = uploadId;
        }

        /// <summary>
        /// 单次返回最大的条目数量，默认 1000
        /// </summary>
        /// <param name="maxParts"></param>
        public void SetMaxParts(int maxParts)
        {
            SetQueryParameter(CosRequestHeaderKey.MAX_PARTS, maxParts.ToString());
        }

        /// <summary>
        /// 默认以 UTF-8 二进制顺序列出条目，所有列出条目从 marker 开始
        /// </summary>
        /// <param name="partNumberMarker"></param>
        public void SetPartNumberMarker(int partNumberMarker)
        {
            SetQueryParameter(CosRequestHeaderKey.PART_NUMBER_MARKER, partNumberMarker.ToString());
        }

        /// <summary>
        /// 规定返回值的编码方式
        /// </summary>
        /// <param name="encodingType"></param>
        public void SetEncodingType(string encodingType)
        {
            SetQueryParameter(CosRequestHeaderKey.ENCODING_TYPE, encodingType);
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

        }


    }
}

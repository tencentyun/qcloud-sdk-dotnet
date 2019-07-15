

using COSXML.Common;
using System;

namespace COSXML.Model.Object
{
    public sealed class GetObjectBytesRequest : ObjectRequest
    {
        /// <summary>
        /// 下载进度回调
        /// </summary>
        private COSXML.Callback.OnProgressCallback progressCallback;

        public GetObjectBytesRequest(string bucket, string key) : base(bucket, key)
        {
            this.method = CosRequestMethod.GET;
        }

        /// <summary>
        /// 下载进度回调
        /// </summary>
        /// <param name="progressCallback"></param>
        public void SetCosProgressCallback(COSXML.Callback.OnProgressCallback progressCallback)
        {
            this.progressCallback = progressCallback;
        }

        internal COSXML.Callback.OnProgressCallback GetCosProgressCallback()
        {
            return progressCallback;
        }

        /// <summary>
        /// 下载内容范围
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void SetRange(long start, long end)
        {
            if (start < 0) return;
            if (end < start) end = -1;
            SetRequestHeader(CosRequestHeaderKey.RANGE, String.Format("bytes={0}-{1}", start,
                (end == -1 ? "" : end.ToString())));

        }
        /// <summary>
        /// 下载内容的起始偏移量
        /// </summary>
        /// <param name="start"></param>
        public void SetRange(long start)
        {
            SetRange(start, -1);
        }
        /// <summary>
        /// 下载特定版本的对象
        /// </summary>
        /// <param name="versionId"></param>
        public void SetVersionId(string versionId)
        {
            if (versionId != null)
            {
                SetQueryParameter(CosRequestHeaderKey.VERSION_ID, versionId);
            }
        }
        /// <summary>
        /// 响应头部中的 Content-Type 参数
        /// </summary>
        /// <param name="responseContentType"></param>
        public void SetResponseContentType(string responseContentType)
        {
            if (responseContentType != null)
            {
                SetQueryParameter(CosRequestHeaderKey.RESPONSE_CONTENT_TYPE, responseContentType);
            }
        }
        /// <summary>
        /// 响应头部中的 Content-Language 参数
        /// </summary>
        /// <param name="responseContentLanguage"></param>
        public void SetResponseContentLanguage(string responseContentLanguage)
        {
            if (responseContentLanguage != null)
            {
                SetQueryParameter(CosRequestHeaderKey.RESPONSE_CONTENT_LANGUAGE, responseContentLanguage);
            }
        }
        /// <summary>
        /// 响应头部中的 Cache-Control 参数
        /// </summary>
        /// <param name="responseCacheControl"></param>
        public void SetResponseCacheControl(string responseCacheControl)
        {
            if (responseCacheControl != null)
            {
                SetQueryParameter(CosRequestHeaderKey.RESPONSE_CACHE_CONTROL, responseCacheControl);
            }
        }
        /// <summary>
        /// 响应头部中的 Content-Disposition 参数
        /// </summary>
        /// <param name="responseDisposition"></param>
        public void SetResponseContentDisposition(string responseDisposition)
        {
            if (responseDisposition != null)
            {
                SetQueryParameter(CosRequestHeaderKey.RESPONSE_CONTENT_DISPOSITION, responseDisposition);
            }
        }
        /// <summary>
        /// 响应头部中的 Content-Encoding 参数
        /// </summary>
        /// <param name="responseContentEncoding"></param>
        public void SetResponseContentEncoding(string responseContentEncoding)
        {
            if (responseContentEncoding != null)
            {
                SetQueryParameter(CosRequestHeaderKey.RESPONSE_CONTENT_ENCODING, responseContentEncoding);
            }
        }
        /// <summary>
        /// 响应头部中的 Content-Expires 参数
        /// </summary>
        /// <param name="responseExpires"></param>
        public void SetResponseExpires(string responseExpires)
        {
            if (responseExpires != null)
            {
                SetQueryParameter(CosRequestHeaderKey.RESPONSE_EXPIRES, responseExpires);
            }
        }
    }
}

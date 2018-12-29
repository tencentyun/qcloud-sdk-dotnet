using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;
using COSXML.CosException;
using System.IO;

namespace COSXML.Model.Object
{
    /// <summary>
    /// 下载对象
    /// <see cref="https://cloud.tencent.com/document/product/436/7753"/>
    /// </summary>
    public sealed class GetObjectRequest : ObjectRequest
    {
        /// <summary>
        /// 保存文件的本地文件夹路径
        /// </summary>
        private string localDir;
        /// <summary>
        /// 保存文件的本地的文件名
        /// </summary>
        private string localFileName;
        /// <summary>
        /// 保存文件的本地偏移位置，下载的数据从此处开始append该文件后面
        /// </summary>
        private long localFileOffset = 0;
        /// <summary>
        /// 下载进度回调
        /// </summary>
        private COSXML.Callback.OnProgressCallback progressCallback;

        
        public GetObjectRequest(string bucket, string key, string localDir, string localFileName)
            : base(bucket, key)
        {
            this.method = CosRequestMethod.GET;
            this.localDir = localDir;
            this.localFileName = localFileName;
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
        /// 保存文件的本地偏移位置，下载的数据从此处开始append该文件后面
        /// </summary>
        /// <param name="localFileOffset"></param>
        public void SetLocalFileOffset(long localFileOffset)
        {
            this.localFileOffset = localFileOffset > 0 ? localFileOffset : 0;
        }

        public long GetLocalFileOffset()
        {
            return localFileOffset;
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
        
        public override void CheckParameters()
        {
            if (localDir == null) throw new CosClientException((int)CosClientError.INVALID_ARGUMENT, "localDir = null");
            if(requestUrlWithSign != null && localFileName == null) throw new CosClientException((int)CosClientError.INVALID_ARGUMENT, "localFileName = null");
            base.CheckParameters();
        }
        /// <summary>
        /// 获取本地文件保存路径
        /// </summary>
        /// <returns></returns>
        public string GetSaveFilePath()
        {
            string result = localDir;
            DirectoryInfo dirInfo = new DirectoryInfo(localDir);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            if (String.IsNullOrEmpty(localFileName)) localFileName = path;
            if (localDir.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                result = result + localFileName;
            }
            else
            {
                result = result + System.IO.Path.DirectorySeparatorChar + localFileName;
            }
            
            return result;
        }
    }
}

using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;
using System.IO;
using COSXML.Utils;
using COSXML.CosException;
using COSXML.Auth;
using COSXML.Log;
using COSXML.Network;

namespace COSXML.Model.Object
{
    /// <summary>
    /// 使用者用表单的形式将文件（Object）上传至指定 Bucket 中.
    /// <see cref="https://cloud.tencent.com/document/product/436/14690"/>
    /// </summary>
    public sealed class PostObjectRequest : ObjectRequest
    {

        /// <summary>
        /// 表单字段
        /// <see cref="FormStruct"/>
        /// </summary>
        private FormStruct formStruct;

        private PostObjectRequest(string bucket, string key)
            : base(bucket, "/")
        {
            this.method = CosRequestMethod.POST;
            formStruct = new FormStruct();
            formStruct.key = key;
            this.headers.Add(CosRequestHeaderKey.CONTENT_TYPE, "multipart/form-data; boundary=" + MultipartRequestBody.BOUNDARY);
            this.needMD5 = false;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="key"></param>
        /// <param name="srcPath"></param>
        public PostObjectRequest(string bucket, string key, string srcPath)
            : this(bucket, key, srcPath, -1L, -1L)
        {
        }

        /// <summary>
        /// 上传文件的指定部分
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="key"></param>
        /// <param name="srcPath"></param>
        /// <param name="fileOffset">指定文件内容的起始位置</param>
        /// <param name="sendContentLength">指定文件内容的大小</param>
        public PostObjectRequest(string bucket, string key, string srcPath, long fileOffset, long sendContentLength)
            : this(bucket, key)
        {
            formStruct.srcPath = srcPath;
            formStruct.fileOffset = fileOffset < 0 ? 0 : fileOffset;
            formStruct.contentLength = sendContentLength < 0L ? -1L : sendContentLength;
        }

        /// <summary>
        /// 上传data数据
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public PostObjectRequest(string bucket, string key, byte[] data)
            : this(bucket, key)
        {
            formStruct.data = data;
        }

        /// <summary>
        /// 设置进度回调
        /// </summary>
        /// <param name="progressCallback"></param>
        public void SetCosProgressCallback(COSXML.Callback.OnProgressCallback progressCallback)
        {
            formStruct.progressCallback = progressCallback;
        }

        /// <summary>
        /// 定义 Object 的 acl 属性。有效值：private，public-read-write，public-read；默认值：private
        /// <see cref="Common.CosACL"/>
        /// </summary>
        /// <param name="cosACL"></param>
        public void SetCosACL(CosACL cosACL)
        {
            SetCosACL(EnumUtils.GetValue(cosACL));
        }

        /// <summary>
        /// 定义 Object 的 acl 属性。有效值：private，public-read-write，public-read；默认值：private
        /// <see cref="Common.CosACL"/>
        /// </summary>
        /// <param name="cosACL"></param>
        public void SetCosACL(string cosACL)
        {

            if (cosACL != null)
            {
                formStruct.acl = cosACL;
            }
        }

        /// <summary>
        /// 设置对象的 cacheControl
        /// </summary>
        /// <param name="cacheControl"></param>
        public void SetCacheControl(string cacheControl)
        {
            SetHeader("Cache-Control", cacheControl);
        }

        /// <summary>
        /// 设置对象的contentType
        /// </summary>
        /// <param name="contentType"></param>
        public void SetContentType(string contentType)
        {
            SetHeader("Content-Type", contentType);
        }

        /// <summary>
        /// 设置对象的contentDisposition
        /// </summary>
        /// <param name="contentDisposition"></param>
        public void SetContentDisposition(string contentDisposition)
        {
            SetHeader("Content-Disposition", contentDisposition);
        }

        /// <summary>
        /// 设置对象的contentEncoding
        /// </summary>
        /// <param name="contentEncoding"></param>
        public void SetContentEncoding(string contentEncoding)
        {
            SetHeader("Content-Encoding", contentEncoding);
        }

        /// <summary>
        /// 设置对象 Expire
        /// </summary>
        /// <param name="expires"></param>
        public void SetExpires(string expires)
        {
            SetHeader("Expires", expires);
        }

        /// <summary>
        /// 设置对象header属性
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetHeader(string key, string value)
        {

            try
            {
                formStruct.headers.Add(key, value);
            }
            catch (ArgumentException)
            {
                formStruct.headers[key] = value;
            }
        }

        /// <summary>
        /// 设置对象自定义的header属性
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetCustomerHeader(string key, string value)
        {

            try
            {
                formStruct.customHeaders.Add(key, value);
            }
            catch (ArgumentException)
            {
                formStruct.customHeaders[key] = value;
            }
        }

        /// <summary>
        /// 设置对象的存储类型
        /// <see cref="Common.CosStorageClass"/>
        /// </summary>
        /// <param name="cosStorageClass"></param>
        public void SetCosStorageClass(string cosStorageClass)
        {
            formStruct.xCosStorageClass = cosStorageClass;
        }

        /// <summary>
        /// 最大上传速度，单位是 bit/s
        /// </summary>
        /// <param name="start"></param>
        public void LimitTraffic(long rate)
        {
            formStruct.xCOSTrafficLimit = rate.ToString();
        }

        /// <summary>
        /// 若设置优先生效，返回 303 并提供 Location 头部，
        /// 会在 URL 尾部加上 bucket={bucket}&lt;key={key}&gt;etag={%22etag%22} 参数。
        /// </summary>
        /// <param name="redirectHost"></param>
        public void SetSuccessActionRedirect(string redirectHost)
        {
            formStruct.successActionRedirect = redirectHost;
        }

        /// <summary>
        /// successHttpCode can be 200, 201, 204, default value 204
        /// 若填写 success_action_redirect 则会略此设置。
        /// </summary>
        /// <param name="successHttpCode"></param>
        public void SetSuccessActionStatus(int successHttpCode)
        {
            formStruct.successActionStatus = successHttpCode.ToString();
        }

        /// <summary>
        /// 用于做请求检查，如果请求的内容和 Policy 指定的条件不符，返回 403 AccessDenied。
        /// <see cref="Policy"/>
        /// </summary>
        /// <param name="policy"></param>
        public void SetPolicy(Policy policy)
        {
            formStruct.policy = policy;
        }

        public override void CheckParameters()
        {
            formStruct.CheckParameter();
            base.CheckParameters();
        }

        public override CosXmlSignSourceProvider GetSignSourceProvider()
        {
            var signSourceProvider = base.GetSignSourceProvider();
            
            if (signSourceProvider != null)
            {
                signSourceProvider.RemoveHeaderKey("content-type");
                signSourceProvider.RemoveHeaderKey("content-length");
                signSourceProvider.RemoveHeaderKey("content-md5");
                signSourceProvider.onGetSign = delegate (Request request, string sign)
                {
                    //添加参数 sign
                    ((MultipartRequestBody)request.Body).AddParameter("Signature", sign);
                };
            }

            return signSourceProvider;
        }

        public override RequestBody GetRequestBody()
        {
            MultipartRequestBody requestBody = new MultipartRequestBody();

            requestBody.AddParamters(formStruct.GetFormParameters());

            if (formStruct.data != null)
            {
                requestBody.AddData(formStruct.data, "file", "tmp");
            }
            else if (formStruct.srcPath != null)
            {
                FileInfo fileInfo = new FileInfo(this.formStruct.srcPath);

                string fileName = fileInfo.Name;

                if (formStruct.contentLength == -1L || formStruct.contentLength + formStruct.fileOffset > fileInfo.Length)
                {
                    formStruct.contentLength = fileInfo.Length - formStruct.fileOffset;
                }

                requestBody.AddData(formStruct.srcPath, formStruct.fileOffset, formStruct.contentLength, "file", fileName);
            }

            requestBody.ProgressCallback = formStruct.progressCallback;

            return requestBody;
        }

        private class FormStruct
        {
            /// <summary>
            /// 对象的ACL
            /// </summary>
            public string acl;

            /// <summary>
            /// 对象的header元数据
            /// </summary>
            public Dictionary<string, string> headers;

            /// <summary>
            /// 上传后的文件名，使用 ${filename} 则会进行替换。
            /// 例如a/b/${filename}，上传文件 a1.txt，那么最终的上传路径就是 a/b/a1.txt
            /// </summary>
            public string key;

            /// <summary>
            /// 若设置优先生效，返回 303 并提供 Location 头部
            /// </summary>
            public string successActionRedirect;

            /// <summary>
            /// 可选 200，201，204 默认返回 204。若填写 success_action_redirect 则会略此设置。
            /// </summary>
            public string successActionStatus;

            /// <summary>
            /// 对象的自定义元数据
            /// </summary>
            public Dictionary<string, string> customHeaders;

            /// <summary>
            /// 对象存储类型
            /// </summary>
            public string xCosStorageClass;

            /// <summary>
            /// 速度限制
            /// </summary>
            public string xCOSTrafficLimit;

            /// <summary>
            /// 请求检查策略
            /// <see cref="Policy"/>
            /// </summary>
            public Policy policy;

            /// <summary>
            /// 上传文件的本地路径
            /// </summary>
            public string srcPath;

            /// <summary>
            /// 上传文件指定起始位置
            /// </summary>
            public long fileOffset = 0L;

            /// <summary>
            /// 上传文件指定内容大小
            /// </summary>
            public long contentLength = -1L;

            /// <summary>
            /// 上传data数据
            /// </summary>
            public byte[] data;

            /// <summary>
            /// 上传回调
            /// </summary>
            public COSXML.Callback.OnProgressCallback progressCallback;


            public FormStruct()
            {
                headers = new Dictionary<string, string>();
                customHeaders = new Dictionary<string, string>();
            }

            public Dictionary<string, string> GetFormParameters()
            {
                Dictionary<string, string> formParameters = new Dictionary<string, string>();

                if (acl != null)
                {
                    formParameters.Add("Acl", acl);
                }

                foreach (KeyValuePair<string, string> pair in headers)
                {
                    formParameters.Add(pair.Key, pair.Value);
                }

                formParameters.Add("key", key);

                if (successActionRedirect != null)
                {
                    formParameters.Add("success_action_redirect", successActionRedirect);
                }

                if (successActionStatus != null)
                {
                    formParameters.Add("success_action_status", successActionStatus);
                }

                foreach (KeyValuePair<string, string> pair in customHeaders)
                {
                    formParameters.Add(pair.Key, pair.Value);
                }

                if (xCosStorageClass != null)
                {
                    formParameters.Add("x-cos-storage-class", xCosStorageClass);
                }

                if (xCOSTrafficLimit != null)
                {
                    formParameters.Add(CosRequestHeaderKey.X_COS_TRAFFIC_LIMIT, xCOSTrafficLimit);
                }

                if (policy != null)
                {
                    formParameters.Add("policy", DigestUtils.GetBase64(policy.Content(), Encoding.UTF8));
                }

                return formParameters;
            }

            public void CheckParameter()
            {

                if (String.IsNullOrEmpty(key))
                {
                    throw new CosClientException((int)CosClientError.InvalidArgument, "FormStruct.key(null or empty) is invalid");
                }

                if (srcPath == null && data == null)
                {
                    throw new CosClientException((int)CosClientError.InvalidArgument, "data source = null");
                }

                if (srcPath != null)
                {

                    if (!File.Exists(srcPath))
                    {
                        throw new CosClientException((int)CosClientError.InvalidArgument, "srcPath not exist");
                    }
                }
            }
        }

        public class Policy
        {
            /// <summary>
            /// 过期时间
            /// </summary>
            private string expiration;

            /// <summary>
            /// 检查条件
            /// </summary>
            private StringBuilder conditions = new StringBuilder();

            public void SetExpiration(long endTimeMills)
            {
                this.expiration = TimeUtils.GetFormatTime("yyyy-MM-dd'T'HH:mm:ss.SSS'Z'", endTimeMills, TimeUnit.Milliseconds);
            }

            public void AddConditions(string key, string value, bool isPrefixMatch)
            {

                if (conditions.Length > 0)
                {
                    conditions.Append(',');
                }

                if (isPrefixMatch)
                {
                    conditions.Append('[')
                        .Append("\"starts-with\"")
                        .Append(",\"")
                        .Append(key)
                        .Append("\",\"")
                        .Append(value)
                        .Append("\"]");
                }
                else
                {
                    conditions.Append("{\"")
                        .Append(key)
                        .Append("\":\"")
                        .Append(value)
                        .Append("\"}");
                }
            }

            public void AddContentConditions(int start, int end)
            {
                if (conditions.Length > 0)
                {
                    conditions.Append(',');
                }
                
                conditions.Append('[')
                    .Append("\"content-length-range\"")
                    .Append(',')
                    .Append(start)
                    .Append(',')
                    .Append(end)
                    .Append(']');
            }

            public string Content()
            {
                StringBuilder content = new StringBuilder();

                content.Append('{');

                if (expiration != null)
                {
                    content.Append(String.Format("\"expiration\":\"{0}\"", expiration));
                }

                if (conditions.Length > 0)
                {
                    if (expiration != null) 
                    {
                        content.Append(",");
                    }

                    content.Append(String.Format("\"conditions\":[{0}]", conditions));
                }

                content.Append('}');

                return content.ToString();
            }
        }
    }
}

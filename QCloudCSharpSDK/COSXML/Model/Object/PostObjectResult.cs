using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Model.Tag;
using COSXML.Transfer;

namespace COSXML.Model.Object
{
    /// <summary>
    /// 使用者用表单的形式将文件（Object）上传至指定 Bucket 中.
    /// <see cref="https://cloud.tencent.com/document/product/436/14690"/>
    /// </summary>
    public sealed class PostObjectResult : CosResult
    {
        /// <summary>
        /// 对象的eTag
        /// </summary>
        public string eTag;
        /// <summary>
        /// 若指定了上传 success_action_redirect 则返回对应的值，若无指定则返回对象完整的路径
        /// </summary>
        public string location;
        /// <summary>
        /// post object返回的信息
        /// <see cref="Model.Tag.PostResponse"/>
        /// </summary>
        public PostResponse postResponse;

        internal override void InternalParseResponseHeaders()
        {
            List<string> values;
            this.responseHeaders.TryGetValue("ETag", out values);
            if (values != null && values.Count > 0)
            {
                eTag = values[0];
            }
            this.responseHeaders.TryGetValue("Location", out values);
            if (values != null && values.Count > 0)
            {
                location = values[0];
            }

        }

        internal override void ParseResponseBody(System.IO.Stream inputStream, string contentType, long contentLength)
        {
            if (contentLength <= 0) return;
            postResponse = new PostResponse();
            XmlParse.ParsePostResponse(inputStream, postResponse);
        }

        public override string GetResultInfo()
        {
            return base.GetResultInfo() + (postResponse == null ? "" : ('\n' + postResponse.GetInfo()));
        }
    }
}

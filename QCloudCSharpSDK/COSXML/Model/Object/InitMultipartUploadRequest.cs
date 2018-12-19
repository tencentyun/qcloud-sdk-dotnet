using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;
using COSXML.Utils;
using COSXML.Model.Tag;

namespace COSXML.Model.Object
{
    /// <summary>
    /// 实现初始化分片上传，成功执行此请求以后会返回 UploadId 用于后续的 Upload Part 请求
    /// <see cref="https://cloud.tencent.com/document/product/436/7746"/>
    /// </summary>
    public sealed class InitMultipartUploadRequest : ObjectRequest
    {
        public InitMultipartUploadRequest(string bucket, string key)
            : base(bucket, key)
        {
            this.method = CosRequestMethod.POST;
            this.queryParameters.Add("uploads", null);
        }

        /// <summary>
        /// 定义的缓存策略，将作为 Object 元数据保存
        /// </summary>
        /// <param name="cacheControl"></param>
        public void SetCacheControl(string cacheControl)
        {
            if (cacheControl != null)
            {
                SetRequestHeader(CosRequestHeaderKey.CACHE_CONTROL, cacheControl);
            }
        }
        /// <summary>
        /// 定义的文件名称，将作为 Object 元数据保存
        /// </summary>
        /// <param name="contentDisposition"></param>
        public void SetContentDisposition(string contentDisposition)
        {
            if (contentDisposition != null)
            {
                SetRequestHeader(CosRequestHeaderKey.CONTENT_DISPOSITION, contentDisposition);
            }
        }
        /// <summary>
        /// 定义的编码格式，将作为 Object 元数据保存
        /// </summary>
        /// <param name="contentEncoding"></param>
        public void SetContentEncoding(string contentEncoding)
        {
            if (contentEncoding != null)
            {
                SetRequestHeader(CosRequestHeaderKey.CONTENT_ENCODING, contentEncoding);
            }
        }
        /// <summary>
        /// 定义的内容类型（MIME），将作为 Object 元数据保存
        /// </summary>
        /// <param name="expires"></param>
        public void SetExpires(string expires)
        {
            if (expires != null)
            {
                SetRequestHeader(CosRequestHeaderKey.EXPIRES, expires);
            }
        }

        /// <summary>
        /// Object 的 ACL 属性，有效值：private，public-read-write，public-read，
        /// default；默认值：default(继承 Bucket 权限)
        /// 当前访问策略条目限制为 1000 条，
        /// 如果您不需要进行 Object ACL 控制，请填 default 或者此项不进行设置，
        /// 默认继承 Bucket 权限
        /// <see cref="Common.CosACL"/>
        /// </summary>
        /// <param name="cosACL"></param>
        public void SetCosACL(string cosACL)
        {
            if (cosACL != null)
            {
                SetRequestHeader(CosRequestHeaderKey.X_COS_ACL, cosACL);
            }
        }
        /// Object 的 ACL 属性，有效值：private，public-read-write，public-read，
        /// default；默认值：default(继承 Bucket 权限)
        /// 当前访问策略条目限制为 1000 条，
        /// 如果您不需要进行 Object ACL 控制，请填 default 或者此项不进行设置，
        /// 默认继承 Bucket 权限
        /// <see cref="Common.CosACL"/>
        public void SetCosACL(CosACL cosACL)
        {
            SetRequestHeader(CosRequestHeaderKey.X_COS_ACL, EnumUtils.GetValue(cosACL));
        }
        /// <summary>
        /// 赋予被授权者读的权限
        /// <see cref="Model.Tag.GrantAccount"/>
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
        /// 赋予被授权者写的权限
        /// <see cref="Model.Tag.GrantAccount"/>
        /// </summary>
        /// <param name="grantAccount"></param>
        public void SetXCosGrantWrite(GrantAccount grantAccount)
        {
            if (grantAccount != null)
            {
                SetRequestHeader(CosRequestHeaderKey.X_COS_GRANT_WRITE, grantAccount.GetGrantAccounts());
            }
        }
        /// <summary>
        /// 赋予被授权者读写的权限
        /// <see cref="Model.Tag.GrantAccount"/>
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

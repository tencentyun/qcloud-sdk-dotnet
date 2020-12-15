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
        /// 设置 Object 的存储级别
        /// <see cref="Common.CosStorageClass"/>
        /// </summary>
        /// <param name="cosStorageClass"></param>
        public void SetCosStorageClass(string cosStorageClass)
        {
            if (cosStorageClass != null)
            {
                SetRequestHeader(CosRequestHeaderKey.X_COS_STORAGE_CLASS, cosStorageClass);
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
            SetCosACL(EnumUtils.GetValue(cosACL));
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

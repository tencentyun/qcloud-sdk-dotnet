using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;
using COSXML.Utils;
using COSXML.Model.Tag;

namespace COSXML.Model.Object
{
    /// <summary>
    /// 设置 对象 的ACL
    /// <see cref="https://cloud.tencent.com/document/product/436/7748"/>
    /// </summary>
    public sealed class PutObjectACLRequest : ObjectRequest
    {
        public PutObjectACLRequest(string bucket, string key)
            : base(bucket, key)
        {
            this.method = CosRequestMethod.PUT;
            this.queryParameters.Add("acl", null);
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
                SetRequestHeader(CosRequestHeaderKey.X_COS_ACL, cosACL);
            }
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
        /// 赋予被授权者所有的权限
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

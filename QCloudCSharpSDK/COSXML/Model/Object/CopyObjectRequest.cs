using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;
using COSXML.CosException;
using COSXML.Model.Tag;
using COSXML.Utils;

namespace COSXML.Model.Object
{
    /// <summary>
    /// 实现将一个文件从源路径复制到目标路径
    /// <see cref="https://cloud.tencent.com/document/product/436/10881"/>
    /// </summary>
    public sealed class CopyObjectRequest : ObjectRequest
    {
        /// <summary>
        /// 拷贝的对象源
        /// <see cref="Model.Tag.CopySourceStruct"/>
        /// </summary>
        private CopySourceStruct copySourceStruct;

        public CopyObjectRequest(string bucket, string key)
            : base(bucket, key)
        {
            this.method = CosRequestMethod.PUT;
        }

        /// <summary>
        /// 设置复制的对象源
        /// <see cref="Model.Tag.CopySourceStruct"/>
        /// </summary>
        /// <param name="copySource"></param>
        public void SetCopySource(CopySourceStruct copySource)
        {
            this.copySourceStruct = copySource;
        }

        /// <summary>
        /// 是否拷贝源文件的元数据，枚举值：Copy, Replaced，默认值 Copy。
        /// 假如标记为 Copy，则拷贝源文件的元数据；
        /// 假如标记为 Replaced，则按本次请求的 Header 信息修改元数据。
        /// 当目标路径和源路径一致，即用户试图修改元数据时，则标记必须为 Replaced
        /// </summary>
        /// <param name="metaDataDirective"></param>
        public void SetCopyMetaDataDirective(CosMetaDataDirective metaDataDirective)
        {
            SetRequestHeader(CosRequestHeaderKey.X_COS_METADATA_DIRECTIVE, EnumUtils.GetValue(metaDataDirective));
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
        /// <summary>
        /// 设置 Object 的存储级别，枚举值：STANDARD，STANDARD_IA。默认值：STANDARD
        /// <see cref="Common.CosStorageClass"/>
        /// </summary>
        /// <param name="cosStorageClass"></param>
        public void SetCosStorageClass(CosStorageClass cosStorageClass)
        {
            SetRequestHeader(CosRequestHeaderKey.X_COS_STORAGE_CLASS_, EnumUtils.GetValue(cosStorageClass));
        }
        /// <summary>
        /// 定义 Object 的 ACL 属性。有效值：private，public-read-write，public-read；默认值：private
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
        /// 定义 Object 的 ACL 属性。有效值：private，public-read-write，public-read；默认值：private
        /// <see cref="Common.CosACL"/>
        /// </summary>
        /// <param name="cosACL"></param>
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

        public override void CheckParameters()
        {
            if (copySourceStruct == null)
            {
                throw new CosClientException((int)CosClientError.INVALID_ARGUMENT, "copy source is null");
            }
            else
            {
                copySourceStruct.CheckParameters();
            }
            base.CheckParameters();
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

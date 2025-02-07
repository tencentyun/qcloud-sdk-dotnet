using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Model.Tag;
using COSXML.Common;
using COSXML.CosException;
using COSXML.Network;

namespace COSXML.Model.Object
{
    /// <summary>
    /// 对一个通过 COS 归档为 archive 类型的对象进行恢复
    /// <see href="https://cloud.tencent.com/document/product/436/12633"/>
    /// </summary>
    public sealed class RestoreObjectRequest : ObjectRequest
    {
        /// <summary>
        /// 用于恢复数据的配置
        /// <see href="Model.Tag.RestoreConfigure"/>
        /// </summary>
        private RestoreConfigure restoreConfigure;

        public RestoreObjectRequest(string bucket, string key)
            : base(bucket, key)
        {
            this.method = CosRequestMethod.POST;
            this.queryParameters.Add("restore", null);
            restoreConfigure = new RestoreConfigure();
            restoreConfigure.casJobParameters = new RestoreConfigure.CASJobParameters();
            restoreConfigure.days = 0;
        }

        /// <summary>
        /// 设置临时副本的过期时间
        /// </summary>
        /// <param name="days"></param>
        public void SetExpireDays(int days)
        {
            if (days <= 0)
            {
                throw new CosClientException(-1, "param 'Days' ranges from 1-365, Some storage types do not need call SetExpireDays function to set Days param");
            }
            restoreConfigure.days = days;
        }

        /// <summary>
        /// 恢复数据时，Tier 可以指定为 CAS 支持的三种恢复类型，分别为 Expedited、Standard、Bulk
        /// <see href="Model.Tag.RestoreConfigure.Tier"/>
        /// </summary>
        /// <param name="tier"></param>
        public void SetTier(RestoreConfigure.Tier tier)
        {
            restoreConfigure.casJobParameters.tier = tier;
        }

        public void SetVersionId(string versionId)
        {

            if (versionId != null)
            {
                SetQueryParameter(CosRequestHeaderKey.VERSION_ID, versionId);
            }
        }

        public override Network.RequestBody GetRequestBody()
        {
            if (restoreConfigure.days == 0)
            {
                RestoreConfigureNoDays restoreConfigureNoDays = new RestoreConfigureNoDays();
                restoreConfigureNoDays.casJobParameters = restoreConfigure.casJobParameters;
                return GetXmlRequestBody(restoreConfigureNoDays);
            }
            return GetXmlRequestBody(restoreConfigure);
        }
    }
}

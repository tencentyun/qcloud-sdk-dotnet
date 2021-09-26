using System;
using System.Collections.Generic;

using System.Text;
using System.IO;
using COSXML.Common;
using COSXML.Model.Object;
using COSXML.CosException;

namespace COSXML.Model.CI
{
    /// <summary>
    /// 媒体截图
    /// <see href="https://cloud.tencent.com/document/product/436/55671"/>
    /// </summary>
    public sealed class GetSnapshotRequest : ObjectRequest
    {
        private string localFilePath;

        public GetSnapshotRequest(string bucket, string key, 
            float time, string localFilePath, 
            int width = 0, int height = 0, string format = "jpg", 
            string rotate = "auto", string mode = "exactframe"
            )
            : base(bucket, key)
        {

            if (time < 0)
            {
                throw new CosClientException((int)CosClientError.InvalidArgument, "time less than 0");
            }

            this.method = CosRequestMethod.GET;
            this.queryParameters.Add("ci-process", "snapshot");
            this.queryParameters.Add("time", time.ToString());
            if (width != 0)
            {
                this.queryParameters.Add("width", width.ToString());
            }
            if (height != 0)
            {
                this.queryParameters.Add("height", height.ToString());
            }
            if (format != "jpg")
            {
                this.queryParameters.Add("format", format);
            }
            if (rotate != "auto")
            {
                this.queryParameters.Add("rotate", rotate);
            }
            if (mode != "exactframe")
            {
                this.queryParameters.Add("mode", mode);
            }

            this.localFilePath = localFilePath;
            this.CheckParameters();
        }

        public override void CheckParameters()
        {
            if (localFilePath == null)
            {
                throw new CosClientException((int)CosClientError.InvalidArgument, "localFileName = null");
            }

            base.CheckParameters();
        }

        /// <summary>
        /// 获取本地文件保存路径
        /// </summary>
        /// <returns></returns>
        public string GetSaveFilePath()
        {
            return localFilePath;
        }

    }
}

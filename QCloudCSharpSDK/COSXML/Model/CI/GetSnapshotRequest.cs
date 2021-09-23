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

        private string localDir;
        private string localFileName;

        public GetSnapshotRequest(string bucket, string key, 
            float time, string localDir, string localFileName, 
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

            this.localDir = localDir;
            this.localFileName = localFileName;
            this.CheckParameters();
        }

        public override void CheckParameters()
        {

            if (localDir == null)
            {
                throw new CosClientException((int)CosClientError.InvalidArgument, "localDir = null");
            }

            if (localFileName == null)
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
            string result = localDir;
            DirectoryInfo dirInfo = new DirectoryInfo(localDir);

            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            if (String.IsNullOrEmpty(localFileName))
            {
                localFileName = path;
            }

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

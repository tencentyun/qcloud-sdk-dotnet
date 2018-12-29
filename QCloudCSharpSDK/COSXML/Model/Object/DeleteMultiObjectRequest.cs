using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Model.Tag;
using COSXML.Common;
using COSXML.Network;
using COSXML.CosException;

namespace COSXML.Model.Object
{
    /// <summary>
    /// 实现在指定 Bucket 中批量删除 Object,单次请求最大支持批量删除 1000 个 Object
    /// <see cref="https://cloud.tencent.com/document/product/436/8289"/>
    /// </summary>
    public sealed class DeleteMultiObjectRequest : ObjectRequest
    {
        /// <summary>
        /// 删除对象
        /// <see cref="Model.Tag.Delete"/>
        /// </summary>
        private Delete delete;

        public DeleteMultiObjectRequest(string bucket)
            : base(bucket, null)
        {
            this.method = CosRequestMethod.POST;
            this.path = "/";
            this.needMD5 = true;
            this.queryParameters.Add("delete", null);
            delete = new Delete();
            delete.deleteObjects = new List<Delete.DeleteObject>();
        }

        public override void SetCosPath(string key)
        {
            // do nothing for delete multi objects api
        }
        /// <summary>
        /// 返回模式
        /// verbose 模式和 quiet 模式，默认为 verbose 模式。
        /// verbose 模式返回每个 key 的删除情况，quiet 模式只返回删除失败的 key 的情况；
        /// </summary>
        /// <param name="quiet"></param>
        public void SetDeleteQuiet(bool quiet) 
        {
            delete.quiet = quiet;
        }
        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="key"></param>
        public void SetDeleteKey(string key)
        {
            SetDeleteKey(key, null);
        }
        /// <summary>
        /// 删除指定版本的对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="versionId"></param>
        public void SetDeleteKey(string key, string versionId)
        {
            if (key != null)
            {
                if (key.StartsWith("/"))
                {
                    key = key.Substring(1);
                }
                Delete.DeleteObject deleteObject = new Delete.DeleteObject();
                deleteObject.key = key;
                if (versionId != null)
                {
                    deleteObject.versionId = versionId;
                }
                delete.deleteObjects.Add(deleteObject);
            }
        }
        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="keys"></param>
        public void SetObjectKeys(List<string> keys)
        {
            if (keys != null)
            {
                foreach (string key in keys)
                {
                    SetDeleteKey(key, null);
                }
            }
        }
        /// <summary>
        /// 删除指定版本的对象
        /// </summary>
        /// <param name="versionIdAndKey"></param>
        public void SetObjectKeys(Dictionary<string, string> versionIdAndKey)
        {
            if (versionIdAndKey != null)
            {
                foreach (KeyValuePair<string, string> pair in versionIdAndKey)
                {
                    SetDeleteKey(pair.Value, pair.Key);
                }
            }
        }

        public override void CheckParameters()
        {
            if (delete.deleteObjects.Count == 0) throw new CosClientException((int)CosClientError.INVALID_ARGUMENT, "delete keys = null");
            base.CheckParameters();
        }

        public override Network.RequestBody GetRequestBody()
        {
            string content = Transfer.XmlBuilder.BuildDelete(delete);
            byte[] data = Encoding.UTF8.GetBytes(content);
            ByteRequestBody body = new ByteRequestBody(data);
            return body;
        }

    }
}

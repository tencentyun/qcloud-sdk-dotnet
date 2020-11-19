using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;

namespace COSXML.Model.Bucket
{
    /// <summary>
    /// 查询 Bucket 正在进行中的分块上传
    /// <see cref="https://cloud.tencent.com/document/product/436/7736"/>
    /// </summary>
    public sealed class ListMultiUploadsRequest : BucketRequest
    {
        public ListMultiUploadsRequest(string bucket)
            : base(bucket)
        {
            this.bucket = bucket;
            this.method = CosRequestMethod.GET;
            this.queryParameters.Add("uploads", null);
        }

        /// <summary>
        /// 定界符为一个符号，
        /// 对 Object 名字包含指定前缀且第一次出现 delimiter 字符之间的 Object 作为一组元素：common prefix。
        /// 如果没有 prefix，则从路径起点开始
        /// </summary>
        /// <param name="delimiter"></param>
        public void SetDelimiter(string delimiter)
        {

            if (delimiter != null)
            {
                SetQueryParameter("delimiter", delimiter);
            }
        }

        /// <summary>
        /// 规定返回值的编码格式，合法值：url
        /// </summary>
        /// <param name="encodingType"></param>
        public void SetEncodingType(string encodingType)
        {

            if (encodingType != null)
            {
                SetQueryParameter("Encoding-type", encodingType);
            }
        }

        /// <summary>
        /// 与 upload-id-marker 一起使用.
        /// 当 upload-id-marker 未被指定时，ObjectName 字母顺序大于 key-marker 的条目将被列出.
        /// 当upload-id-marker被指定时，ObjectName 字母顺序大于key-marker的条目被列出，
        /// ObjectName 字母顺序等于 key-marker 同时 UploadID 大于 upload-id-marker 的条目将被列出。
        /// </summary>
        /// <param name="keyMarker"></param>
        public void SetKeyMarker(string keyMarker)
        {

            if (keyMarker != null)
            {
                SetQueryParameter("key-marker", keyMarker);
            }
        }

        /// <summary>
        /// 设置最大返回的 multipart 数量，合法取值从1到1000，默认1000
        /// </summary>
        /// <param name="maxUploads"></param>
        public void SetMaxUploads(string maxUploads)
        {

            if (maxUploads != null)
            {
                SetQueryParameter("max-uploads", maxUploads);
            }
        }

        /// <summary>
        /// 限定返回的 Object key 必须以 Prefix 作为前缀。
        /// 注意使用 prefix 查询时，返回的 key 中仍会包含 Prefix
        /// </summary>
        /// <param name="prefix"></param>
        public void SetPrefix(string prefix)
        {

            if (prefix != null)
            {
                SetQueryParameter("Prefix", prefix);
            }
        }

        /// <summary>
        /// 与 key-marker 一起使用.
        /// 当 key-marker 未被指定时，upload-id-marker 将被忽略.
        /// 当 key-marker 被指定时，ObjectName字母顺序大于 key-marker 的条目被列出，
        /// ObjectName 字母顺序等于 key-marker 同时 UploadID 大于 upload-id-marker 的条目将被列出.
        /// </summary>
        /// <param name="uploadIdMarker"></param>
        public void SetUploadIdMarker(string uploadIdMarker)
        {

            if (uploadIdMarker != null)
            {
                SetQueryParameter("upload-id-marker", uploadIdMarker);
            }
        }
    }
}

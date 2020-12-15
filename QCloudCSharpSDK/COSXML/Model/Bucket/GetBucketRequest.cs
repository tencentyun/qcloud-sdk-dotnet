using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;

namespace COSXML.Model.Bucket
{
    /// <summary>
    /// 获取 Bucket 中对象列表
    /// <see cref="https://cloud.tencent.com/document/product/436/7734"/>
    /// </summary>
    public sealed class GetBucketRequest : BucketRequest
    {
        public GetBucketRequest(string bucket)
            : base(bucket)
        {
            this.method = CosRequestMethod.GET;
            this.queryParameters.Add("max-keys", 1000.ToString());
        }

        /// <summary>
        /// 前缀匹配，用来规定返回的文件前缀地址
        /// </summary>
        /// <param name="prefix"></param>
        public void SetPrefix(string prefix)
        {

            if (prefix != null)
            {
                SetQueryParameter("prefix", prefix);
            }
        }

        /// <summary>
        /// 定界符为一个符号，
        /// 如果有 Prefix，则将 Prefix 到 delimiter 之间的相同路径归为一类，定义为 Common Prefix，然后列出所有 Common Prefix。
        /// 如果没有 Prefix，则从路径起点开始
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
        /// 规定返回值的编码方式，可选值：url
        /// </summary>
        /// <param name="encodingType"></param>
        public void SetEncodingType(string encodingType)
        {

            if (encodingType != null)
            {
                SetQueryParameter("encoding-type", encodingType);
            }
        }

        /// <summary>
        /// 默认以 UTF-8 二进制顺序列出条目，所有列出条目从 marker 开始
        /// </summary>
        /// <param name="marker"></param>
        public void SetMarker(string marker)
        {

            if (marker != null)
            {
                SetQueryParameter("marker", marker);
            }
        }

        /// <summary>
        /// 单次返回最大的条目数量，默认 1000
        /// </summary>
        /// <param name="maxKeys"></param>
        public void SetMaxKeys(string maxKeys)
        {

            if (maxKeys != null)
            {
                SetQueryParameter("max-keys", maxKeys);
            }
        }
    }
}

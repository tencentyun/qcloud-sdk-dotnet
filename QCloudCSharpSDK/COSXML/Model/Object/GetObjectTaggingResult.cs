using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Model.Tag;
using COSXML.Transfer;

namespace COSXML.Model.Object
{
    /// <summary>
    /// 获取存储桶下某个对象的标签结果
    /// <see href="https://cloud.tencent.com/document/product/436/42998"/>
    /// </summary>
    public sealed class GetObjectTaggingResult : CosDataResult<Tagging>
    {
        /// <summary>
        /// 对象标签内容
        /// <see href="Model.Tag.Tagging"/>
        /// </summary>
        public Tagging tagging { 
            get {return _data; } 
        }
    }
}

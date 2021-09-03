using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Model.Tag;
using COSXML.Transfer;

namespace COSXML.Model.Object
{
    /// <summary>
    /// 查询特定分块上传中的已上传的块返回的结果
    /// <see href="https://cloud.tencent.com/document/product/436/7747"/>
    /// </summary>
    public sealed class ListPartsResult : CosDataResult<ListParts>
    {
        /// <summary>
        /// 已上传块的所有信息
        /// <see href="Model.Tag.ListParts"/>
        /// </summary>
        public ListParts listParts { 
            get {return _data; } 
        }
    }
}

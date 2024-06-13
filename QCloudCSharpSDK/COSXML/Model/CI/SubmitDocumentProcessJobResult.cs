using COSXML.Model.Tag;

namespace COSXML.Model.CI
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SubmitDocumentProcessJobResult : CosDataResult<DocProcessResponse>
    {

        /// <summary>
        /// 文档转码结果
        /// </summary>
        /// <value></value>
        public DocProcessResponse docProcessResponse {
            get {return _data; } 
        }
    }
}

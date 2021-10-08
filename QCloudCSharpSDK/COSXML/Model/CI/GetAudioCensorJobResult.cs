using System;
using System.IO;
using System.Collections.Generic;
using COSXML.Model.Tag;
using COSXML.Transfer;

namespace COSXML.Model.CI
{
    /// <summary>
    /// 查询音频审核结果
    /// </summary>
    public sealed class GetAudioCensorJobResult : CosDataResult<AudioCensorResult>
    {
        /// <summary>
        /// 音频审核结果
        /// </summary>
        /// <value></value>
        public AudioCensorResult resultStruct { 
            get {return _data; } 
        }
    }
}

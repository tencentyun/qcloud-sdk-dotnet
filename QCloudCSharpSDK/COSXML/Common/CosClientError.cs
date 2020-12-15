using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;
using COSXML.Utils;

namespace COSXML.Common
{
    public enum CosClientError
    {
        [CosValue("InvalidArgument")]
        InvalidArgument = 10000,

        [CosValue("InvalidCredentials")]
        InvalidCredentials = 10001,

        [CosValue("BadRequest")]
        BadRequest = 10002,

        [CosValue("SinkSourceNotFound")]
        SinkSourceNotFound = 10003,

        [CosValue("InternalError")]
        InternalError = 20000,

        [CosValue("ServerError")]
        ServerError = 20001,

        [CosValue("IOError")]
        IOError = 20002,

        [CosValue("PoorNetwork")]
        PoorNetwork = 20003,

        [CosValue("UserCancelled")]
        UserCancelled = 30000,

        [CosValue("AlreadyFinished")]
        AlredyFinished = 30001,
    }
}

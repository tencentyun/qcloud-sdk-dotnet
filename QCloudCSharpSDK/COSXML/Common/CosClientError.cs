using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;
using COSXML.Utils;
/**
* Copyright (c) 2018 Tencent Cloud. All rights reserved.
* 11/2/2018 8:19:06 PM
* bradyxiao
*/
namespace COSXML.Common
{
    public enum CosClientError
    {
        [CosValue("InvalidArgument")]
        INVALID_ARGUMENT = 10000,

        [CosValue("InvalidCredentials")]
        INVALID_CREDENTIALS = 10001,

        [CosValue("BadRequest")]
        BAD_REQUEST = 10002,

        [CosValue("SinkSourceNotFound")]
        SINK_SOURCE_NOT_FOUND = 10003,

        [CosValue("InternalError")]
        INTERNA_LERROR = 20000,

        [CosValue("ServerError")]
        SERVER_ERROR = 20001,

        [CosValue("IOError")]
        IO_ERROR = 20002,

        [CosValue("PoorNetwork")]
        POOR_NETWORK = 20003,

        [CosValue("UserCancelled")]
        USER_CANCELLED = 30000,

        [CosValue("AlreadyFinished")]
        ALREADY_FINISHED = 30001,
    }
}

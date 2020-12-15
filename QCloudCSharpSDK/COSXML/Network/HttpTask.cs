using COSXML.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tencent.QCloud.Cos.Sdk.Network
{
    public class HttpTask
    {
        internal CosRequest cosRequest;

        internal CosResult cosResult;

        internal bool isSchedue = false;

        internal COSXML.Callback.OnSuccessCallback<CosResult> successCallback;

        internal COSXML.Callback.OnFailedCallback failCallback;
    }
}

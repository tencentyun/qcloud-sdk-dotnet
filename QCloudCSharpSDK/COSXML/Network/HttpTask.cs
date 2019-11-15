using COSXML.Model;
using System;
using System.Collections.Generic;
using System.Text;


/* ============================================================================== 
* Copyright 2016-2019 Tencent Cloud. All Rights Reserved.
* Auth：bradyxiao 
* Date：2019/4/4 16:49:33 
* ==============================================================================*/

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

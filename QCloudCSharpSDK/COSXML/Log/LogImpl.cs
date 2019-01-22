using System;
using System.Collections.Generic;

using System.Text;
using System.Diagnostics;
/**
* Copyright (c) 2018 Tencent Cloud. All rights reserved.
* 11/2/2018 9:52:04 AM
* bradyxiao
*/
namespace COSXML.Log
{
    public sealed class LogImpl : Log
    {

        public void PrintLog(string message)
        {
            Trace.WriteLine(message);
        }
    }
}

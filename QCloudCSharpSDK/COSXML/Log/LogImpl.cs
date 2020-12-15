using System;
using System.Collections.Generic;

using System.Text;
using System.Diagnostics;

namespace COSXML.Log
{
    public sealed class LogImpl : ILog
    {

        public void PrintLog(string message)
        {
            Trace.WriteLine(message);
        }
    }
}

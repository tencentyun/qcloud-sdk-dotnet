using System;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;

using System.Text;

namespace COSXML.Common
{
    public sealed class CosVersion
    {
        private static string SDKVersion = "5.4.18.0";

        public static string GetUserAgent()
        {
            StringBuilder userAgent = new StringBuilder();

            userAgent.Append("cos-net-sdk").Append('.')
                .Append(SDKVersion);

            return userAgent.ToString();
        }

        //public static string GetOsVersion()
        //{
        //    try
        //    {
        //        var os = Environment.OSVersion;
        //        return "windows " + os.Version.Major + "." + os.Version.Minor;
        //    }
        //    catch (InvalidOperationException)
        //    {
        //        return "Unknown OSVersion";
        //    }
        //}

        //public static string GetOsArchitecture()
        //{
        //    return (IntPtr.Size == 8) ? "x86_64" : "x86";
        //}
    }
}

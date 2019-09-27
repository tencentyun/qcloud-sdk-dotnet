using System;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;

using System.Text;
/**
* Copyright (c) 2018 Tencent Cloud. All rights reserved.
* 11/2/2018 2:34:04 PM
* bradyxiao
*/
namespace COSXML.Common
{
    public sealed class CosVersion
    {
        public static string SDKVersion;

        public static string GetUserAgent()
        {
            if (SDKVersion == null) {
                Assembly assembly = Assembly.GetExecutingAssembly();
                FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                SDKVersion = fileVersionInfo.ProductVersion;
            }

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

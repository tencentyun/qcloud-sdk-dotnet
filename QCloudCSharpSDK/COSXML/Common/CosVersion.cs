using System;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;

using System.Text;

namespace COSXML.Common
{
    public sealed class CosVersion
    {
        private static string SDKVersion = "5.4.44.0";
        // 主版本号：第一个数字，产品改动较大，可能无法向后兼容（要看具体项目）
        // 子版本号：第二个数字，增加了新功能，向后兼容 
        // 修正版本号：第三个数字，修复BUG 或优化代码，一般没有添加新功能，向后兼容
        // 编译版本号：通常是系统自动生成，每次代码提交都会导致自动加1.

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

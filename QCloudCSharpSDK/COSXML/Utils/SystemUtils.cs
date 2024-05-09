using System;
using System.Diagnostics;

namespace COSXML.Utils
{
    public class SystemUtils
    {
        /// <summary>
        /// 获取当前占用系统内存大小
        /// </summary>
        public static void getMemorySize()
        {
            Process currentProcess = Process.GetCurrentProcess();
            // 获取当前进程占用的内存大小（以字节为单位）
            long memorySize = currentProcess.WorkingSet64;
            // 将字节转换为兆字节（MB）
            double memorySizeInMB = (double)memorySize / (1024 * 1024);
            Console.WriteLine("内存大小是:" + memorySizeInMB + "MB");
        }
    }
}
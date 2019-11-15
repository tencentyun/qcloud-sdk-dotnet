using System;
using System.Collections.Generic;

using System.Text;
using System.IO;
using System.Threading;
using System.Diagnostics;
/**
* Copyright (c) 2018 Tencent Cloud. All rights reserved.
* 11/2/2018 10:06:17 AM
* bradyxiao
*/
namespace COSXML.Log
{
    public sealed class QLog
    {
        /**
        * log format: [time] [thread]/[application] [Level]/[TAG]: [message]
        */

        private static string timeFormat = "yyyy-MM-dd HH:mm:ss.fff";

        //private static string currentDir = Directory.GetCurrentDirectory();

        private static Process currentProcess = Process.GetCurrentProcess();

        private static List<Log> logImplList = new List<Log>();

        private static LEVEL level = LEVEL.V;

        public static void SetLogLevel(LEVEL level)
        {
            QLog.level = level;
        }

        public static void AddLogAdapter(Log log)
        {
            if (log == null) return;
            foreach (Log logImpl in logImplList)
            {
                if (logImpl.GetType().Name == log.GetType().Name)
                {
                    return;
                }
            }
            logImplList.Add(log);
        }

        public static void V(string tag, string message)
        {
            V(tag, message, null);
        }

        public static void V(string tag, string message, Exception exception)
        {
            if (LEVEL.V >= QLog.level)
            {
                Print(LEVEL.V, tag, message, exception);
            }
            
        }

        public static void D(string tag, string message)
        {
            D(tag, message, null);
        }

        public static void D(string tag, string message, Exception exception)
        {
            if (LEVEL.D >= QLog.level)
            {
                Print(LEVEL.D, tag, message, exception);
            }
            
        }

        public static void I(string tag, string message)
        {
            I(tag, message, null);
        }

        public static void I(string tag, string message, Exception exception)
        {

            if (LEVEL.I >= QLog.level)
            {
                Print(LEVEL.I, tag, message, exception);
            }
            
        }

        public static void W(string tag, string message)
        {
            W(tag, message, null);
        }

        public static void W(string tag, string message, Exception exception)
        {
            if (LEVEL.W >= QLog.level)
            {
                Print(LEVEL.W, tag, message, exception);
            }
           
        }

        public static void E(string tag, string message)
        {
            E(tag, message, null);
        }

        public static void E(string tag, string message, Exception exception)
        {
            if (LEVEL.E >= QLog.level)
            {
                Print(LEVEL.E, tag, message, exception);
            }
           
        }

        private static void Print(LEVEL level, string tag, string message, Exception exception)
        {
            StringBuilder messageBuilder = new StringBuilder();

            messageBuilder.Append(DateTime.Now.ToString(timeFormat))
                .Append(" ")
                .Append(Thread.CurrentThread.ManagedThreadId)
                .Append("-")
                .Append(currentProcess.Id)
                // OSX 系统上不支持
                // .Append("/")
                // .Append(currentProcess.ProcessName)
                .Append(" ")
                .Append(level.ToString())
                .Append("/")
                .Append(tag)
                .Append(": ")
                .Append(message);
            if (exception != null)
            {
                messageBuilder.Append("\n Exception:\n")
                    .Append(exception.ToString());
            }
            messageBuilder.Append("\r\n");
            foreach (Log log in logImplList)
            {
                log.PrintLog(messageBuilder.ToString());
            }  
        }

        //private static string PrintExceptionTrace(Exception exception)
        //{
        //    return exception.ToString();
        //}
    }

    public enum LEVEL
    {
        V = 0,
        D,
        I,
        W,
        E
    }
}

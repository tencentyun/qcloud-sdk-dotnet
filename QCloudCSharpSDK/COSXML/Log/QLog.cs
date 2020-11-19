using System;
using System.Collections.Generic;

using System.Text;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace COSXML.Log
{
    public sealed class QLog
    {
        private static string timeFormat = "yyyy-MM-dd HH:mm:ss.fff";

        //private static string currentDir = Directory.GetCurrentDirectory();

        private static Process currentProcess = Process.GetCurrentProcess();

        private static List<ILog> logImplList = new List<ILog>();

        private static Level level = Level.V;

        public static void SetLogLevel(Level level)
        {
            QLog.level = level;
        }

        public static void AddLogAdapter(ILog log)
        {

            if (log == null)
            {

                return;
            }

            foreach (ILog logImpl in logImplList)
            {

                if (logImpl.GetType().Name == log.GetType().Name)
                {

                    return;
                }
            }

            logImplList.Add(log);
        }

        public static void Verbose(string tag, string message)
        {
            Verbose(tag, message, null);
        }

        public static void Verbose(string tag, string message, Exception exception)
        {

            if (Level.V >= QLog.level)
            {
                Print(Level.V, tag, message, exception);
            }

        }

        public static void Debug(string tag, string message)
        {
            Debug(tag, message, null);
        }

        public static void Debug(string tag, string message, Exception exception)
        {

            if (Level.D >= QLog.level)
            {
                Print(Level.D, tag, message, exception);
            }

        }

        public static void Info(string tag, string message)
        {
            Info(tag, message, null);
        }

        public static void Info(string tag, string message, Exception exception)
        {

            if (Level.I >= QLog.level)
            {
                Print(Level.I, tag, message, exception);
            }

        }

        public static void Warn(string tag, string message)
        {
            Warn(tag, message, null);
        }

        public static void Warn(string tag, string message, Exception exception)
        {

            if (Level.W >= QLog.level)
            {
                Print(Level.W, tag, message, exception);
            }

        }

        public static void Error(string tag, string message)
        {
            Error(tag, message, null);
        }

        public static void Error(string tag, string message, Exception exception)
        {

            if (Level.E >= QLog.level)
            {
                Print(Level.E, tag, message, exception);
            }

        }

        private static void Print(Level level, string tag, string message, Exception exception)
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

            foreach (ILog log in logImplList)
            {
                log.PrintLog(messageBuilder.ToString());
            }
        }

        //private static string PrintExceptionTrace(Exception exception)
        //{
        //    return exception.ToString();
        //}
    }

    public enum Level
    {
        V = 0,

        D,

        I,

        W,

        E
    }
}

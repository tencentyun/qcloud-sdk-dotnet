//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Diagnostics;
//using System.Collections;
//using System.IO;
//using COSXML.Utils;
//using System.Threading;

//namespace COSXML.Log
//{
//    /// <summary>
//    /// 异步写入本地文件日志
//    /// 日志路径：当前应用工程的Log文件夹下
//    /// 后台写入：开启一个后台线程 logThread
//    /// 缓存区域：开辟一块临时日志缓存区域 logBuffer
//    /// 消息循环机制：循环获取->写入 handle -> looper
//    /// </summary>
//    public sealed class FileLogImpl : Log
//    {
//        private string logDir;
//        private const int LOG_BUFFER_SIZE = 32 * 1024; //32kb
//        private const int LOG_FILE_MAX_COUNTS = 30; // 30个文件数 
//        private const int LOG_FILE_MAX_LENGTH = 3 * 1048576; // 文件最大为3M
//        private Queue<string> logFiles;
//        private List<string> buffer;
//        private int currentBufferSize;

//        private Handler handler;
//        public const byte MESSAGE_COMNE_IN = 0;
//        public const byte MESSAGE_FLUSH = 1;

//        public FileLogImpl(string logDir)
//        {
//            if (String.IsNullOrEmpty(logDir))
//            {
//                Trace.WriteLine("log dir = null");
//            }
//            else
//            {
//                this.logDir = logDir;
//                DirectoryInfo dirInfo = new DirectoryInfo(logDir);
//                if (!dirInfo.Exists) dirInfo.Create();
//                logFiles = new Queue<string>(30);
//                buffer = new List<string>(10);
//                currentBufferSize = 0;
//                handler = new Handler();
//                handler.OnHandleMessage = OnHandleMessage;
//                Thread logThread = new Thread(handler.InternalHandle) { Name = "LogThread", IsBackground = true};
//                logThread.Start();
//            }
//        }

//        private void OnHandleMessage(Message message)
//        {
//            Trace.WriteLine(String.Format("currentThread ：name = {0} id = {1} isBackground = {2}", Thread.CurrentThread.Name, Thread.CurrentThread.ManagedThreadId,
//                Thread.CurrentThread.IsBackground));
//            switch (message.what)
//            {
//                case MESSAGE_COMNE_IN:
//                    WriteLogToBuffer((string)message.obj);
//                    break;
//                case MESSAGE_FLUSH:
//                    WriteLogToFile();
//                    break;
//            }
//        }

//        public void PrintLog(string logMessage)
//        {
//            if (handler == null) return;
//            Message message = new Message();
//            message.what = MESSAGE_COMNE_IN;
//            message.obj = logMessage;
//            handler.SendMessage(message);
//        }



//        private void WriteLogToBuffer(string message)
//        {
//            if (message != null)
//            {
//                buffer.Add(message);
//                currentBufferSize += message.Length;
//                if (currentBufferSize > LOG_BUFFER_SIZE)
//                {
//                    handler.SendMessage(MESSAGE_FLUSH);
//                }
//            }
//        }



//        private string GetCurrentLogFilePath()
//        {
//            if (logFiles.Count == 0)
//            {
//                DirectoryInfo dirInfo = new DirectoryInfo(logDir);
//                FileInfo[] fileInfos = dirInfo.GetFiles();
//                Array.Sort(fileInfos, delegate(FileInfo f1, FileInfo f2)
//                {
//                    long prefix1 = 0, prefix2 = 0;
//                    long.TryParse(f1.Name.Split('.')[0], out prefix1);
//                    long.TryParse(f2.Name.Split('.')[0], out prefix2);
//                    if (prefix1 > prefix2) return 1;
//                    if (prefix1 == prefix2) return 0;
//                    if (prefix1 < prefix2) return -1;
//                    return 0;
//                });
//                foreach (FileInfo fileInfo in fileInfos)
//                {
//                    logFiles.Enqueue(fileInfo.FullName);
//                }
//            }
//            int count = logFiles.Count - LOG_FILE_MAX_COUNTS;
//            for (int i = 0; i < count; i++)
//            {
//                logFiles.Dequeue();
//            }
//            if (logFiles.Count > 0)
//            {
//                FileInfo lastFileInfo = new FileInfo(logFiles.ToArray()[logFiles.Count - 1]);
//                if (lastFileInfo.Length < LOG_FILE_MAX_LENGTH)
//                {
//                    return lastFileInfo.FullName;
//                }
//            }
//            string logFileName = String.Format("{0}.log", TimeUtils.GetCurrentTime(TimeUnit.SECONDS));
//            string logFilePath = logDir + System.IO.Path.DirectorySeparatorChar + logFileName;
//            logFiles.Enqueue(logFilePath);
//            return logFilePath;
//        }

//        private void WriteLogToFile()
//        {
//            FileStream fileStream = null;
//            StreamWriter streamWriter = null;
//            try
//            {
//                string logFile = GetCurrentLogFilePath();
//                fileStream = new FileStream(logFile, FileMode.Append, FileAccess.Write);
//                streamWriter = new StreamWriter(fileStream);
//                foreach (string content in buffer)
//                {
//                    streamWriter.Write(content);
//                }
//                streamWriter.Flush();
//                buffer.Clear();
//                currentBufferSize = 0;
//            }
//            catch (Exception)
//            {
//                //Trace.WriteLine(ex.StackTrace);
//            }
//            finally
//            {
//                if (streamWriter != null) streamWriter.Close();
//                //if (fileStream != null)
//                //{
//                //    fileStream.Close();
//                //}
//            }
//        }
//    }

//    public class Handler
//    {
//        private Queue<Message> messageQueue = new Queue<Message>(20);

//        public void SendMessage(Message message)
//        {
//            messageQueue.Enqueue(message);

//        }

//        public void SendMessage(int what)
//        {
//            Message message = new Message();
//            message.what = what;
//            messageQueue.Enqueue(message);
//        }

//        public HandleMessage OnHandleMessage;

//        public void InternalHandle()
//        {
//            long start = TimeUtils.GetCurrentTime(TimeUnit.SECONDS);
//            while (true)
//            {
//                try
//                {
//                    Message message = messageQueue.Dequeue();
//                    OnHandleMessage(message);
//                }
//                catch (Exception)
//                {
//                    //Trace.WriteLine(ex.StackTrace);
//                }
//                long end = TimeUtils.GetCurrentTime(TimeUnit.SECONDS);
//                if (end - start > 10)
//                {
//                    start = end;
//                    SendMessage(FileLogImpl.MESSAGE_FLUSH);
//                }
//            }
//        }

//    }

//    public delegate void HandleMessage(Message message);

//    public class Message
//    {
//        public int what;
//        public Object obj;
//    }
//}

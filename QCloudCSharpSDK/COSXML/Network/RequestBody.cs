using System;
using System.Collections.Generic;

using System.Text;
using System.IO;
using COSXML.Common;
using COSXML.Log;
using COSXML.Utils;
/**
* Copyright (c) 2018 Tencent Cloud. All rights reserved.
* 11/2/2018 4:33:28 PM
* bradyxiao
*/
namespace COSXML.Network
{
    /// <summary>
    /// request body for http request
    /// </summary>
    public abstract class RequestBody
    {
        protected static string TAG = typeof(RequestBody).Name;
        public const int SEGMENT_SIZE = 64 * 1024;// 64kb

        protected long contentLength;
        protected string contentType;
        protected Callback.OnProgressCallback progressCallback;
        /// <summary>
        /// body length
        /// </summary>
        public virtual long ContentLength
        { get{return contentLength;} set{contentLength = value;} }

        /// <summary>
        /// body mime type
        /// </summary>
        public virtual string ContentType
        { get { return contentType; } set { contentType = value; } }

        /// <summary>
        /// calculation content md5
        /// </summary>
        /// <returns></returns>
        public abstract string GetMD5();

        /// <summary>
        /// Synchronization method: write data to outputStream 
        /// </summary>
        /// <param name="outputStream"> output stream for writing data</param>
        public abstract void OnWrite(Stream outputStream);

        /// <summary>
        /// Asynchronous method: handle request body
        /// </summary>
        /// <param name="outputStream"></param>
        /// <param name="EndRequestBody"></param>
        public abstract void StartHandleRequestBody(Stream outputStream, EndRequestBody endRequestBody);

        public Callback.OnProgressCallback ProgressCallback
        {
            get { return progressCallback; }
            set { progressCallback = value; }
        }

        /// <summary>
        /// notify progress is complete!
        /// </summary>
        internal void OnNotifyGetResponse()
        {
            if (progressCallback != null && contentLength >= 0)
            {
                progressCallback(contentLength, contentLength);
            }
        }

        /// <summary>
        /// calculation progress
        /// </summary>
        /// <param name="complete"></param>
        /// <param name="total"></param>
        protected void UpdateProgress(long complete, long total)
        {
            if (total == 0)progressCallback(0, 0);
            else if (complete < total) progressCallback(complete, total);
            else progressCallback(total - 1, total);
        }
    }

    public class RequestBodyState
    {
        public byte[] buffer;
        public long complete;
        public Stream outputStream;
        public EndRequestBody endRequestBody;
    }

    public delegate void EndRequestBody(Exception exception);


    public class ByteRequestBody : RequestBody
    {
        private readonly byte[] data;
        //private RequestBodyState requestBodyState;
        public ByteRequestBody(byte[] data)
        {
            this.data = data;
            contentLength = data.Length;
            contentType = CosRequestHeaderKey.APPLICATION_XML;
        }

        public override void OnWrite(Stream outputStream)
        {
            try
            {
                int completed = 0;
                while (completed + SEGMENT_SIZE < contentLength)
                {
                    outputStream.Write(data, completed, SEGMENT_SIZE);
                    outputStream.Flush();
                    completed += SEGMENT_SIZE;
                    if (progressCallback != null)
                    {
                        UpdateProgress(completed, contentLength);
                    }
                }
                if (completed < contentLength)
                {
                    outputStream.Write(data, completed, (int)(contentLength - completed));//包括本身
                    outputStream.Flush();
                    if (progressCallback != null)
                    {
                        UpdateProgress(contentLength, contentLength);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (outputStream != null)
                {
                    outputStream.Flush();
                    outputStream.Close();
                    outputStream.Dispose();
                    //QLog.D("XIAO", "stream close");
                    outputStream = null;
                }
            }
        }

        public override string GetMD5()
        {
            return DigestUtils.GetMd5ToBase64(data);
        }

        public override void StartHandleRequestBody(Stream outputStream, EndRequestBody endRequestBody)
        {
            try
            {
                int completed = 0;
                while (completed + SEGMENT_SIZE < contentLength)
                {
                    outputStream.Write(data, completed, SEGMENT_SIZE);
                    outputStream.Flush();
                    completed += SEGMENT_SIZE;
                    if (progressCallback != null)
                    {
                        UpdateProgress(completed, contentLength);
                    }
                }
                if (completed < contentLength)
                {
                    outputStream.Write(data, completed, (int)(contentLength - completed));//包括本身
                    outputStream.Flush();
                    if (progressCallback != null)
                    {
                        UpdateProgress(contentLength, contentLength);
                    }
                }
                endRequestBody(null);
            }
            catch (Exception ex)
            {
                endRequestBody(ex);
            }
            finally
            {
               
                if (outputStream != null)
                {
                    outputStream.Flush();
                    outputStream.Close();
                    outputStream.Dispose();
                    //QLog.D("XIAO", "stream close");
                    outputStream = null;
                }
            }

        }

        private void AsyncStreamCallback(IAsyncResult ar)
        {
            RequestBodyState requestBodyState = ar.AsyncState as RequestBodyState;
            Stream outputStream = null;
            try
            {
                outputStream = requestBodyState.outputStream;
                outputStream.EndWrite(ar);
                if (progressCallback != null)
                {
                    UpdateProgress(requestBodyState.complete, contentLength);
                }
                if(requestBodyState.complete < contentLength)
                {
                    long remain = contentLength - requestBodyState.complete;
                    int count = (int)(remain > SEGMENT_SIZE ? SEGMENT_SIZE : remain);
                    int offset = (int)requestBodyState.complete;
                    requestBodyState.complete += count;
                    outputStream.BeginWrite(requestBodyState.buffer, offset, count, AsyncStreamCallback, requestBodyState);
                }
                else
                {
                    if (outputStream != null) outputStream.Close();
                    //write over
                    requestBodyState.endRequestBody(null);
                    requestBodyState = null;
                }
            }
            catch (Exception ex)
            {
                if (outputStream != null) outputStream.Close();
                QLog.E(TAG, ex.Message, ex);
                requestBodyState.endRequestBody(ex);
                requestBodyState = null;
            }

        }
    }

    public class FileRequestBody : RequestBody
    {
        private readonly string srcPath;
        private readonly long fileOffset;
        //private RequestBodyState requestBodyState;
        private FileStream fileStream;

        public FileRequestBody(string srcPath, long fileOffset, long sendContentSize)
        {
            this.srcPath = srcPath;
            this.fileOffset = fileOffset;
            contentLength = sendContentSize;
        }

        public override void OnWrite(Stream outputStream)
        {
            FileStream fileStream = null;
            try
            {
                byte[] buffer = new byte[SEGMENT_SIZE]; 
                int bytesRead = 0;
                long completed = bytesRead;
                fileStream = new FileStream(srcPath, FileMode.Open, FileAccess.Read);
                fileStream.Seek(fileOffset, SeekOrigin.Begin);//seek to designated position
                long remain = contentLength - completed;
                if (remain > 0)
                {
                    while ((bytesRead = fileStream.Read(buffer, 0, (int)(buffer.Length > remain ? remain : buffer.Length))) != 0)
                    {
                        outputStream.Write(buffer, 0, bytesRead);
                        outputStream.Flush();
                        completed += bytesRead;
                        if (progressCallback != null)
                        {
                            UpdateProgress(completed, contentLength);
                        }
                        remain = contentLength - completed;
                        if (remain == 0) break;
                    }
                }
                else
                {
                    if (progressCallback != null)
                    {
                        UpdateProgress(completed, contentLength);
                    }
                }

                buffer = null;
            }
            catch (Exception ex)
            {
                QLog.E(TAG, ex.Message, ex);
                throw ;
            }
            finally
            {

                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                    //QLog.D("XIAO", "stream close");
                    fileStream = null;
                }
                if (outputStream != null)
                {
                    outputStream.Flush();
                    outputStream.Close();
                    outputStream.Dispose();
                    //QLog.D("XIAO", "stream close");
                    outputStream = null;
                }
            }
        }

        public override string GetMD5()
        {
            try
            {
                fileStream = new FileStream(srcPath, FileMode.Open, FileAccess.Read);
                fileStream.Seek(fileOffset, SeekOrigin.Begin);
                return DigestUtils.GetMd5ToBase64(fileStream, contentLength);
            }
            catch (Exception ex)
            {
                QLog.E(TAG, ex.Message, ex);
                throw;
            }
            finally
            {
                if (fileStream != null) fileStream.Close();
            }

        }

        public override void StartHandleRequestBody(Stream outputStream, EndRequestBody endRequestBody)
        {
            FileStream fileStream = null;
            try
            {
                byte[] buffer = new byte[SEGMENT_SIZE];
                int bytesRead = 0;
                long completed = bytesRead;
                fileStream = new FileStream(srcPath, FileMode.Open, FileAccess.Read);
                fileStream.Seek(fileOffset, SeekOrigin.Begin);//seek to designated position
                long remain = contentLength - completed;
                if (remain > 0)
                {
                    while ((bytesRead = fileStream.Read(buffer, 0, (int)(buffer.Length > remain ? remain : buffer.Length))) != 0)
                    {
                        outputStream.Write(buffer, 0, bytesRead);
                        outputStream.Flush();
                        completed += bytesRead;
                        if (progressCallback != null)
                        {
                            UpdateProgress(completed, contentLength);
                        }
                        remain = contentLength - completed;
                        if (remain == 0) break;
                    }
                }
                else
                {
                    if (progressCallback != null)
                    {
                        UpdateProgress(completed, contentLength);
                    }
                }

                buffer = null;
                endRequestBody(null);
            }
            catch (Exception ex)
            {
                QLog.E(TAG, ex.Message, ex);
                endRequestBody(ex);
            }
            finally
            {

                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                    //QLog.D("XIAO", "stream close");
                    fileStream = null;
                }
                if (outputStream != null)
                {
                    outputStream.Flush();
                    outputStream.Close();
                    outputStream.Dispose();
                    //QLog.D("XIAO", "stream close");
                    outputStream = null;
                }
            }

        }

        private void AsyncStreamCallback(IAsyncResult ar)
        {
            RequestBodyState requestBodyState = ar.AsyncState as RequestBodyState;
            Stream outputStream = null;
            try
            {
                outputStream = requestBodyState.outputStream;
                outputStream.EndWrite(ar);
                outputStream.Flush();
                if (progressCallback != null)
                {
                    UpdateProgress(requestBodyState.complete, contentLength);
                }
                if (requestBodyState.complete < contentLength)
                {
                    long remain = contentLength - requestBodyState.complete;
                    int count = fileStream.Read(requestBodyState.buffer, 0, (int)(requestBodyState.buffer.Length > remain ? remain : requestBodyState.buffer.Length));
                    requestBodyState.complete += count;
                    outputStream.BeginWrite(requestBodyState.buffer, 0, count, AsyncStreamCallback, requestBodyState);
                }
                else
                {
                    if (fileStream != null) fileStream.Close();
                    if (outputStream != null) outputStream.Close();
                    //write over
                    requestBodyState.endRequestBody(null);
                    requestBodyState = null;
                }

            }
            catch (Exception ex)
            {
                if (fileStream != null) fileStream.Close();
                if (outputStream != null) outputStream.Close();
                QLog.E(TAG, ex.Message, ex);
                requestBodyState.endRequestBody(ex);
                requestBodyState = null;
            }
            
        }
    }

    /**
     * --boundary\r\n
     * content-type: value\r\n
     * content-disposition: form-data; name="key"\r\n
     * content-transfer-encoding: encoding\r\n
     * \r\n
     * value\r\n
     * --boundary--
     */
    public class MultipartRequestBody : RequestBody
    {
        private readonly string DASHDASH = "--";
        public static string BOUNDARY = "314159265358979323------------";
        private readonly string CRLF = "\r\n";
        private readonly string CONTENT_DISPOSITION = "Content-Disposition: form-data; ";

        private Dictionary<string, string> parameters;

        private string name;
        private string fileName;

        private byte[] data;
        
        private string srcPath;
        private long fileOffset;
        private Stream fileStream;

        private long realContentLength;

        private RequestBodyState requestBodyState;

        public MultipartRequestBody()
        {
            contentType = "multipart/form-data; boundary=" + BOUNDARY;
            parameters = new Dictionary<string, string>();
            contentLength = -1L;
        }

        public void AddParamters(Dictionary<string, string> parameters)
        {
            if (parameters != null)
            {
                foreach (KeyValuePair<string, string> pair in parameters)
                {
                    this.parameters.Add(pair.Key, pair.Value);
                }
            }
            
        }

        public void AddParameter(string key, string value)
        {
            if (key != null)
            {
                parameters.Add(key, value);
            }
        }

        public void AddData(byte[] data, string name, string fileName)
        {
            this.data = data;
            this.name = name;
            this.fileName = fileName;
            this.realContentLength = data.Length;
        }

        public void AddData(string srcPath, long fileOffset, long sendContentSize, string name, string fileName)
        {
            this.srcPath = srcPath;
            this.fileOffset = fileOffset;
            this.name = name;
            this.fileName = fileName;
            realContentLength = sendContentSize;
        }

        //计算长度
        public override long ContentLength
        {
            get
            {
                ComputerContentLength();
                return base.ContentLength;
            }
            set { contentLength = value; }
        }

        private void ComputerContentLength()
        {
            if (contentLength != -1) return;
            contentLength = 0;
            if (parameters != null && parameters.Count > 0)
            {
                StringBuilder parametersBuilder = new StringBuilder();
                foreach (KeyValuePair<string, string> pair in parameters)
                {
                    parametersBuilder.Append(DASHDASH).Append(BOUNDARY).Append(CRLF);
                    parametersBuilder.Append(CONTENT_DISPOSITION).Append("name=\"").Append(pair.Key).Append("\"").Append(CRLF);
                    parametersBuilder.Append(CRLF);
                    parametersBuilder.Append(pair.Value).Append(CRLF);
                }
                string content = parametersBuilder.ToString();
                byte[] data = Encoding.UTF8.GetBytes(content);
                contentLength += data.Length;
            }
            if (name != null)
            {
                StringBuilder parametersBuilder = new StringBuilder();
                parametersBuilder.Append(DASHDASH).Append(BOUNDARY).Append(CRLF);
                parametersBuilder.Append(CONTENT_DISPOSITION).Append("name=\"").Append(name).Append("\"");
                if (!String.IsNullOrEmpty(fileName))
                {
                    parametersBuilder.Append("; filename=").Append("\"").Append(fileName).Append("\""); ;
                }
                parametersBuilder.Append(CRLF);
                parametersBuilder.Append("Content-Type: ").Append("application/octet-stream").Append(CRLF);
                parametersBuilder.Append(CRLF);
                string content = parametersBuilder.ToString();
                byte[] data = Encoding.UTF8.GetBytes(content);
                contentLength += data.Length;
            }
            contentLength += realContentLength;
            
            string endLine = CRLF + DASHDASH + BOUNDARY + DASHDASH + CRLF;
            byte[] endData = Encoding.UTF8.GetBytes(endLine);
            contentLength += endData.Length;
        }


        private void WriteParameters(Stream outputStream)
        {
            if (parameters != null && parameters.Count > 0)
            {
                StringBuilder parametersBuilder = new StringBuilder();
                foreach (KeyValuePair<string, string> pair in parameters)
                {
                    parametersBuilder.Append(DASHDASH).Append(BOUNDARY).Append(CRLF);
                    parametersBuilder.Append(CONTENT_DISPOSITION).Append("name=\"").Append(pair.Key).Append("\"").Append(CRLF);
                    parametersBuilder.Append(CRLF);
                    parametersBuilder.Append(pair.Value).Append(CRLF);
                }
                string content = parametersBuilder.ToString();
                byte[] data = Encoding.UTF8.GetBytes(content);
                outputStream.Write(data, 0, data.Length);
            }
        }

        private void WriteFileParameters(Stream outputStream)
        {
            StringBuilder parametersBuilder = new StringBuilder();
            parametersBuilder.Append(DASHDASH).Append(BOUNDARY).Append(CRLF);
            parametersBuilder.Append(CONTENT_DISPOSITION).Append("name=\"").Append(name).Append("\"");
            if (!String.IsNullOrEmpty(fileName))
            {
                parametersBuilder.Append("; filename=").Append("\"").Append(fileName).Append("\""); ;
            }
            parametersBuilder.Append(CRLF);
            parametersBuilder.Append("Content-Type: ").Append("application/octet-stream").Append(CRLF);
            parametersBuilder.Append(CRLF);
            string content = parametersBuilder.ToString();
            byte[] data = Encoding.UTF8.GetBytes(content);
            outputStream.Write(data, 0, data.Length);
        }

        private void WriteEndLine(Stream outputStream)
        {
            string endLine = CRLF + DASHDASH + BOUNDARY + DASHDASH + CRLF;
            byte[] data = Encoding.UTF8.GetBytes(endLine);
            outputStream.Write(data, 0, data.Length);
        }

        public override void OnWrite(Stream outputStream)
        {
            //write paramters
            WriteParameters(outputStream);

            //写入content-disposition: form-data; name = "file"; filename = "xxx"\r\n
            WriteFileParameters(outputStream);

            outputStream.Flush();

            //wrtie content: file or bintary
            try
            {
                if (data != null)
                {
                    int completed = 0;
                    while (completed + SEGMENT_SIZE < realContentLength)
                    {
                        outputStream.Write(data, completed, SEGMENT_SIZE);
                        outputStream.Flush();
                        completed += SEGMENT_SIZE;
                        if (progressCallback != null)
                        {
                            UpdateProgress(completed, realContentLength);
                        }
                    }

                    if (completed < realContentLength)
                    {
                        outputStream.Write(data, completed, (int)(realContentLength - completed));//包括本身
                        if (progressCallback != null)
                        {
                            UpdateProgress(realContentLength, realContentLength);
                        }
                    }

                    WriteEndLine(outputStream);
                    outputStream.Flush();
                }
                else if (srcPath != null)
                {
                    byte[] buffer = new byte[SEGMENT_SIZE]; // 64kb
                    int bytesRead = 0;
                    long completed = bytesRead;
                    fileStream = new FileStream(srcPath, FileMode.Open, FileAccess.Read);
                    fileStream.Seek(fileOffset, SeekOrigin.Begin);
                    long remain = realContentLength - completed;
                    if (remain > 0)
                    {
                        while ((bytesRead = fileStream.Read(buffer, 0, (int)(buffer.Length > remain ? remain : buffer.Length))) != 0)
                        {
                            outputStream.Write(buffer, 0, bytesRead);
                            outputStream.Flush();
                            completed += bytesRead;
                            if (progressCallback != null)
                            {
                                UpdateProgress(completed, realContentLength);
                            }
                            remain = realContentLength - completed;
                            if (remain == 0) break;
                        }
                    }
                    else
                    {
                        if (progressCallback != null)
                        {
                            completed += bytesRead;
                            UpdateProgress(completed, realContentLength);
                        }
                    }

                    WriteEndLine(outputStream);
                    outputStream.Flush();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                    //QLog.D("XIAO", "stream close");
                    fileStream = null;
                }
                if (outputStream != null)
                {
                    outputStream.Flush();
                    outputStream.Close();
                    outputStream.Dispose();
                    //QLog.D("XIAO", "stream close");
                    outputStream = null;
                }
            }
           
        }

        public override string GetMD5()
        {
            throw new NotImplementedException();
        }

        public override void StartHandleRequestBody(Stream outputStream, EndRequestBody endRequestBody)
        {

            //write paramters
            WriteParameters(outputStream);

            //写入content-disposition: form-data; name = "file"; filename = "xxx"\r\n
            WriteFileParameters(outputStream);

            outputStream.Flush();

            //wrtie content: file or bintary
            try
            {
                if (data != null)
                {
                    int completed = 0;
                    while (completed + SEGMENT_SIZE < realContentLength)
                    {
                        outputStream.Write(data, completed, SEGMENT_SIZE);
                        outputStream.Flush();
                        completed += SEGMENT_SIZE;
                        if (progressCallback != null)
                        {
                            UpdateProgress(completed, realContentLength);
                        }
                    }

                    if (completed < realContentLength)
                    {
                        outputStream.Write(data, completed, (int)(realContentLength - completed));//包括本身
                        if (progressCallback != null)
                        {
                            UpdateProgress(realContentLength, realContentLength);
                        }
                    }

                    WriteEndLine(outputStream);
                    outputStream.Flush();
                }
                else if (srcPath != null)
                {
                    byte[] buffer = new byte[SEGMENT_SIZE]; // 64kb
                    int bytesRead = 0;
                    long completed = bytesRead;
                    fileStream = new FileStream(srcPath, FileMode.Open, FileAccess.Read);
                    fileStream.Seek(fileOffset, SeekOrigin.Begin);
                    long remain = realContentLength - completed;
                    if (remain > 0)
                    {
                        while ((bytesRead = fileStream.Read(buffer, 0, (int)(buffer.Length > remain ? remain : buffer.Length))) != 0)
                        {
                            outputStream.Write(buffer, 0, bytesRead);
                            outputStream.Flush();
                            completed += bytesRead;
                            if (progressCallback != null)
                            {
                                UpdateProgress(completed, realContentLength);
                            }
                            remain = realContentLength - completed;
                            if (remain == 0) break;
                        }
                    }
                    else
                    {
                        if (progressCallback != null)
                        {
                            completed += bytesRead;
                            UpdateProgress(completed, realContentLength);
                        }
                    }

                    WriteEndLine(outputStream);
                    outputStream.Flush();
                }

                endRequestBody(null);
            }
            catch (Exception ex)
            {
                endRequestBody(ex);
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                    //QLog.D("XIAO", "stream close");
                    fileStream = null;
                }
                if (outputStream != null)
                {
                    outputStream.Flush();
                    outputStream.Close();
                    outputStream.Dispose();
                    //QLog.D("XIAO", "stream close");
                    outputStream = null;
                }
            }

        }

        private void AsyncStreamCallback(IAsyncResult ar)
        {
            RequestBodyState requestBodyState = ar.AsyncState as RequestBodyState;
            Stream outputStream = null;
            try
            {
                outputStream = requestBodyState.outputStream;
                outputStream.EndWrite(ar);
                if (progressCallback != null)
                {
                    UpdateProgress(requestBodyState.complete, realContentLength);
                }
                if (requestBodyState.complete < realContentLength)
                {
                    if (srcPath != null)
                    {
                        long remain = realContentLength - requestBodyState.complete;
                        int count = fileStream.Read(requestBodyState.buffer, 0, (int)(requestBodyState.buffer.Length > remain ? remain : requestBodyState.buffer.Length));
                        requestBodyState.complete += count;
                        outputStream.BeginWrite(requestBodyState.buffer, 0, count, AsyncStreamCallback, requestBodyState);
                    }
                    else if (data != null)
                    {
                        long remain = realContentLength - requestBodyState.complete;
                        int count = (int)(remain > SEGMENT_SIZE ? SEGMENT_SIZE : remain);
                        int offset = (int)requestBodyState.complete;
                        requestBodyState.complete += count;
                        outputStream.BeginWrite(requestBodyState.buffer, offset, count, AsyncStreamCallback, requestBodyState);
                    }
                    
                    outputStream.Flush();
                }
                else
                {
                    WriteEndLine(outputStream);
                    outputStream.Flush();
                    if (fileStream != null) fileStream.Close();
                    if (outputStream != null) outputStream.Close();
                    //write over
                    requestBodyState.endRequestBody(null);
                }

            }
            catch (Exception ex)
            {
                if (fileStream != null) fileStream.Close();
                if (outputStream != null) outputStream.Close();
                QLog.E(TAG, ex.Message, ex);
                requestBodyState.endRequestBody(ex);
            }
        }

    }
}

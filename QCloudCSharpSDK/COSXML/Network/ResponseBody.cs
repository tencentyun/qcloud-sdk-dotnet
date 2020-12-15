using System;
using System.IO;
using COSXML.Log;

namespace COSXML.Network
{
    public sealed class ResponseBody
    {
        private const int SEGMENT_SIZE = 4096;

        private long contentLength = -1L;

        private string contentType;

        private COSXML.Callback.OnProgressCallback progressCallback;

        private COSXML.Callback.OnParseStream parseStream;

        private string filePath;

        private long fileOffset;

        private FileStream fileStream;

        private bool isDownload = false;

        private MemoryStream memoryStream;

        public long ContentLength
        {
            get
            {
                return contentLength;
            }
            set { contentLength = value; }
        }

        public string ContentType
        {
            get
            {
                return contentType;
            }
            set { contentType = value; }
        }

        public COSXML.Callback.OnProgressCallback ProgressCallback
        {
            get
            {
                return progressCallback;
            }
            set { progressCallback = value; }
        }

        public COSXML.Callback.OnParseStream ParseStream
        {
            get
            {
                return parseStream;
            }
            set { parseStream = value; }
        }

        public string rawContentBodyString { get; private set; }

        public ResponseBody()
        {

        }

        public ResponseBody(string filePath, long fileOffset)
        {
            this.filePath = filePath;
            this.fileOffset = fileOffset;
            this.isDownload = true;
        }

        /// <summary>
        /// handle cos response
        /// </summary>
        /// <param name="inputStream"></param>
        /// <exception cref="CosServerException"> throw CosServerException </exception>
        /// <exception cref="Exception">throw Exception</exception>
        public void HandleResponseBody(Stream inputStream)
        {

            try
            {

                if (isDownload)
                {
                    fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
                    fileStream.Seek(fileOffset, SeekOrigin.Begin);
                    byte[] buffer = new byte[SEGMENT_SIZE];
                    int recvLen = 0;

                    long completed = recvLen;

                    while ((recvLen = inputStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        fileStream.Write(buffer, 0, recvLen);

                        if (progressCallback != null)
                        {
                            completed += recvLen;
                            progressCallback(completed, contentLength);
                        }
                    }

                    fileStream.Flush();
                }
                else
                {

                    if ("application/xml".Equals(contentType, StringComparison.OrdinalIgnoreCase) &&
                        contentLength > 0 && contentLength < 10 * 1000)
                    {
                        // save raw content
                        memoryStream = new MemoryStream((int)contentLength);
                        inputStream.CopyTo(memoryStream);
                        rawContentBodyString = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());

                        memoryStream.Seek(0, SeekOrigin.Begin);
                        inputStream = memoryStream;
                    }

                    if (parseStream != null)
                    {
                        parseStream(inputStream, contentType, contentLength);
                    }
                }
            }
            catch (Exception ex)
            {
                QLog.Error("ResponseBody", ex.Message, ex);
                throw;
            }
            
            finally
            {

                if (inputStream != null)
                {
                    inputStream.Close();
                    inputStream.Dispose();
                }

                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }
                //if (memoryStream != null) memoryStream.Close();
            }
        }

        public void StartHandleResponseBody(Stream inputStream, EndResponseBody endResponseBody)
        {
            ResponseBodyState responseBodyState = new ResponseBodyState();

            responseBodyState.inputStream = inputStream;
            responseBodyState.endResponseBody = endResponseBody;
            responseBodyState.completed = 0L;

            try
            {
                int count = (int)((contentLength > SEGMENT_SIZE || contentLength <= 0) ? SEGMENT_SIZE : contentLength);

                byte[] buffer = new byte[count];

                responseBodyState.buffer = buffer;

                if (isDownload)
                {
                    fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
                    fileStream.Seek(fileOffset, SeekOrigin.Begin);
                    responseBodyState.inputStream.BeginRead(responseBodyState.buffer, 0, responseBodyState.buffer.Length, AsyncStreamCallback, responseBodyState);
                }
                else
                {
                    memoryStream = new MemoryStream(count);
                    responseBodyState.buffer = buffer;
                    responseBodyState.inputStream.BeginRead(responseBodyState.buffer, 0, responseBodyState.buffer.Length, AsyncStreamCallback, responseBodyState);
                }
            }
            catch (Exception ex)
            {
                responseBodyState.endResponseBody(false, ex);
                responseBodyState.Clear();

                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }

                if (memoryStream != null)
                {
                    memoryStream.Close();
                    memoryStream.Dispose();
                }

                QLog.Error("ResponseBody", ex.Message, ex);
            }

        }

        private void AsyncStreamCallback(IAsyncResult ar)
        {
            ResponseBodyState responseBodyState = ar.AsyncState as ResponseBodyState;

            Stream inputStream = responseBodyState.inputStream;

            try
            {
                int recvLen = inputStream.EndRead(ar);

                responseBodyState.completed += recvLen;

                if (recvLen > 0)
                {

                    if (isDownload)
                    {
                        fileStream.Write(responseBodyState.buffer, 0, recvLen);

                        if (progressCallback != null)
                        {
                            progressCallback(responseBodyState.completed, contentLength);
                        }
                    }
                    else
                    {
                        memoryStream.Write(responseBodyState.buffer, 0, recvLen);
                    }

                    inputStream.BeginRead(responseBodyState.buffer, 0, responseBodyState.buffer.Length, AsyncStreamCallback, responseBodyState);
                }
                else
if (recvLen == 0)
                {

                    if (isDownload)
                    {
                        fileStream.Flush();
                    }
                    else
                    {

                        if ("application/xml".Equals(contentType, StringComparison.OrdinalIgnoreCase) &&
                            memoryStream.Length > 0 && memoryStream.Length < 10 * 1000)
                        {
                            memoryStream.Seek(0, SeekOrigin.Begin);
                            rawContentBodyString = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
                        }

                        memoryStream.Seek(0, SeekOrigin.Begin);
                        parseStream(memoryStream, contentType, responseBodyState.completed);
                    }

                    responseBodyState.endResponseBody(true, null);
                    responseBodyState.Clear();

                    if (fileStream != null)
                    {
                        fileStream.Close();
                        fileStream.Dispose();
                    }

                    if (memoryStream != null)
                    {
                        memoryStream.Close();
                        memoryStream.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                responseBodyState.endResponseBody(false, ex);
                responseBodyState.Clear();

                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }

                if (memoryStream != null)
                {
                    memoryStream.Close();
                    memoryStream.Dispose();
                }

                QLog.Error("ResponseBody", ex.Message, ex);
            }

        }
    }

    public delegate void EndResponseBody(bool isSuccess, Exception ex);

    public class ResponseBodyState
    {
        public Stream inputStream;

        public byte[] buffer;

        public long completed;

        public EndResponseBody endResponseBody;

        public void Clear()
        {

            if (inputStream != null)
            {
                inputStream.Close();
            }

            if (buffer != null)
            {
                buffer = null;
            }
        }

    }
}

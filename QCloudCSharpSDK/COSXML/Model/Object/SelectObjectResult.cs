using System;
using System.Collections.Generic;

using System.Text;
using System.IO;
using COSXML.CosException;
using COSXML.Common;
using System.Xml;

namespace COSXML.Model.Object
{
    public sealed class SelectObjectResult : CosResult
    {

        public String searchContent;

        public String outputFilePath;

        public Stat stat;

        private COSXML.Callback.OnProgressCallback progressCallback;

        public sealed class Stat
        {
            public long BytesScanned;

            public long BytesProcessed;

            public long BytesReturned;

            public override string ToString()
            {

                return string.Format("BytesScanned:{0}, BytesProcessed:{1}, BytesReturned:{2}",
                    BytesScanned, BytesProcessed, BytesReturned);
            }
        }

        internal override void ExternInfo(CosRequest cosRequest)
        {
            this.outputFilePath = ((SelectObjectRequest)cosRequest).outputFilePath;
            this.progressCallback = ((SelectObjectRequest)cosRequest).progressCallback;
        }

        internal override void ParseResponseBody(System.IO.Stream inputStream, string contentType, long contentLength)
        {
            // Read One Message for each loop
            // readToString(inputStream);
            System.IO.Stream outputStream;

            if (outputFilePath != null)
            {
                outputStream = new System.IO.FileStream(outputFilePath, FileMode.Create);
            }
            else
            {
                outputStream = new MemoryStream();
            }

            using (outputStream)
            {
                byte[] tempBuffer = new byte[4];


                while (true)
                {
                    TryRead(inputStream, tempBuffer, 0, 4);
                    long messageEntireLength = Bytes4ToInt(tempBuffer);

                    TryRead(inputStream, tempBuffer, 0, 4);
                    long headerSectionLength = Bytes4ToInt(tempBuffer);

                    TryRead(inputStream, tempBuffer, 0, 4);
                    long preludeCRC = Bytes4ToInt(tempBuffer);

                    Dictionary<String, String> headers = new Dictionary<string, string>();

                    // read header
                    long headerSectionRemainLength = headerSectionLength;

                    while (headerSectionRemainLength > 0)
                    {
                        TryRead(inputStream, tempBuffer, 0, 1);
                        int headerNameLength = Bytes1ToInt(tempBuffer);


                        byte[] headerNameBuffer = new byte[headerNameLength];

                        TryRead(inputStream, headerNameBuffer, 0, headerNameLength);
                        String headerName = Bytes2stringUTF8(headerNameBuffer);

                        // 7
                        inputStream.ReadByte();

                        TryRead(inputStream, tempBuffer, 0, 2);
                        int valueLength = Bytes2ToInt(tempBuffer);


                        byte[] valueBuffer = new byte[valueLength];

                        TryRead(inputStream, valueBuffer, 0, valueLength);
                        String value = Bytes2stringUTF8(valueBuffer);

                        if (headers.ContainsKey(headerName))
                        {
                            headers.Remove(headerName);
                        }

                        headers.Add(headerName, value);

                        headerSectionRemainLength -= 1 + headerNameLength + 3 + valueLength;
                    }

                    long payloadLength = messageEntireLength - headerSectionLength - 16;

                    string messageType;

                    headers.TryGetValue(":message-type", out messageType);
                    string eventType;

                    headers.TryGetValue(":event-type", out eventType);
                    // Console.WriteLine("message = " + messageType + ", event = " + eventType);

                    bool isComplete = false;


                    if ("event".Equals(messageType))
                    {
                        byte[] buffer;


                        switch (eventType)
                        {
                            case "Records":
                                int totalRead = 0;
                                buffer = new byte[1024];

                                while (payloadLength > totalRead)
                                {
                                    int readLength = (int)Math.Min(payloadLength - totalRead, 1024);

                                    int readBytes = TryRead(inputStream, buffer, 0, readLength);

                                    outputStream.Write(buffer, 0, readBytes);
                                    totalRead += readBytes;
                                }
                                break;

                            case "Progress":
                                buffer = new byte[payloadLength];
                                TryRead(inputStream, buffer, 0, (int)payloadLength);
                                Stat stat = ParseStatsBody(buffer);
                                progressCallback(stat.BytesProcessed, stat.BytesScanned);
                                break;

                            case "Cont":
                                buffer = new byte[payloadLength];
                                TryRead(inputStream, buffer, 0, (int)payloadLength);
                                break;

                            case "Stats":
                                buffer = new byte[payloadLength];
                                TryRead(inputStream, buffer, 0, (int)payloadLength);
                                this.stat = ParseStatsBody(buffer);
                                break;

                            case "End":
                            default:
                                isComplete = true;
                                break;
                        }
                    }
                    else
if ("error".Equals(messageType))
                    {
                        string errorCode = null;
                        string errorMessage = null;
                        headers.TryGetValue(":error-code", out errorCode);
                        headers.TryGetValue(":error-message", out errorMessage);

                        throw new System.IO.IOException(string.Format(
                            "search error happends with code :{0} and message: {1}",
                            errorCode, errorMessage));
                    }

                    if (isComplete)
                    {

                        if (outputFilePath == null)
                        {
                            outputStream.Position = 0;
                            searchContent = ReadToString(outputStream);
                        }

                        break;
                    }

                    TryRead(inputStream, tempBuffer, 0, 4);
                    long messageCRC = Bytes4ToInt(tempBuffer);
                }
            }

        }

        private int TryRead(System.IO.Stream inputStream, byte[] buffer, int offset, int count)
        {
            int read = 0;
            int maxRead = 10;
            int remainReadCount = count;


        while (remainReadCount > 0 && maxRead > 0 && inputStream.CanRead)
            {
                maxRead--;
                read = inputStream.Read(buffer, count - remainReadCount, remainReadCount);
                remainReadCount -= read;
            }

            if (remainReadCount > 0)
            {
                throw new System.IO.IOException("input stream is end unexpectly !");
            }

            return count - remainReadCount;
        }

        private Stat ParseStatsBody(byte[] body)
        {
            XmlReader xmlReader = XmlReader.Create(new MemoryStream(body));
            Stat stat = new Stat();

            try
            {

                while (xmlReader.Read())
                {

                    switch (xmlReader.NodeType)
                    {
                        case XmlNodeType.Element:

                            if ("BytesScanned".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                            {
                                xmlReader.Read();
                                stat.BytesScanned = Convert.ToInt64(xmlReader.Value);
                            }
                            else
if ("BytesProcessed".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                            {
                                xmlReader.Read();
                                stat.BytesProcessed = Convert.ToInt64(xmlReader.Value);
                            }
                            else
if ("BytesReturned".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                            {
                                xmlReader.Read();
                                stat.BytesReturned = Convert.ToInt64(xmlReader.Value);
                            }
                            break;
                    }
                }
            }
            catch (XmlException e)
            {
                Console.WriteLine(e.StackTrace);
            }

            return stat;
        }

        private string ReadToString(System.IO.Stream inputStream)
        {
            string content = null;

            using (StreamReader reader = new StreamReader(inputStream))
            {
                content = reader.ReadToEnd();
            }

            return content;
        }

        private string Bytes2stringUTF8(byte[] data)
        {

            return System.Text.Encoding.UTF8.GetString(data);
        }

        private int Bytes2ToInt(byte[] data)
        {

            return ((data[0] & 0xFF) << 8) | (data[1] & 0xFF);
        }

        private int Bytes1ToInt(byte[] data)
        {

            return (data[0] & 0xFF);
        }

        private long Bytes4ToInt(byte[] data)
        {

            return ((data[0] & 0xFF) << 24) | ((data[1] & 0xFF) << 16)
                | ((data[2] & 0xFF) << 8) | (data[3] & 0xFF);
        }
    }

}

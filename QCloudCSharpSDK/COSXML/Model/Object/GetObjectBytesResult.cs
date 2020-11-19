
using System;
using System.Collections.Generic;
using System.IO;

namespace COSXML.Model.Object
{
    public sealed class GetObjectBytesResult : CosResult
    {
        public string eTag;

        public byte[] content;

        private COSXML.Callback.OnProgressCallback progressCallback;


        internal override void ExternInfo(CosRequest cosRequest)
        {
            GetObjectBytesRequest getObjectBytesRequest = cosRequest as GetObjectBytesRequest;

            this.progressCallback = getObjectBytesRequest.GetCosProgressCallback();
        }

        internal override void InternalParseResponseHeaders()
        {
            List<string> values;

            this.responseHeaders.TryGetValue("ETag", out values);

            if (values != null && values.Count > 0)
            {
                eTag = values[0];
            }
        }


        internal override void ParseResponseBody(Stream inputStream, string contentType, long contentLength)
        {
            content = new byte[contentLength];
            int completed = 0;

            while (completed < contentLength)
            {
                int recvLen = inputStream.Read(content, completed, (int)Math.Min(2048, contentLength - completed));

                completed += recvLen;

                if (progressCallback != null)
                {
                    progressCallback(completed, content.Length);
                }
            }
            // Unity 上不支持
            // using(var memoryStream = new MemoryStream())
            // {
            //     inputStream.CopyTo(memoryStream);
            //     content = memoryStream.ToArray();
            // }

        }

    }


}

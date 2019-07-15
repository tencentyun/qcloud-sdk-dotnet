

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
            int recvLen = inputStream.Read(content, 0, content.Length);
            int completed = 0;
            while (recvLen != 0)
            {
                completed += recvLen;
                if (progressCallback != null)
                {
                    progressCallback(completed, content.Length);
                }
                recvLen = inputStream.Read(content, recvLen, content.Length - completed);
            }
           
        }

    }


}

using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Common;
using COSXML.Model.Object;
using COSXML.CosException;

namespace COSXML.Model.CI
{
    public sealed class SensitiveContentRecognitionRequest : ObjectRequest
    {
        public SensitiveContentRecognitionRequest(string bucket, string key, string type)
            : base(bucket, key)
        {

            if (type == null)
            {
                throw new CosClientException((int)CosClientError.InvalidArgument, "type = null");
            }

            this.method = CosRequestMethod.GET;
            this.queryParameters.Add("ci-process", "sensitive-content-recognition");
            this.queryParameters.Add("detect-type", type);
        }

    }
}

using System.Text;
using COSXML.Common;
using COSXML.Network;
using COSXML.Model.Tag;
using COSXML.CosException;

namespace COSXML.Model.Object
{
    public sealed class SelectObjectRequest : ObjectRequest
    {
        private string expression;

        private string expressionType = "SQL";

        private ObjectSelectionFormat inputFormat;

        private ObjectSelectionFormat outputFormat;

        internal COSXML.Callback.OnProgressCallback progressCallback;

        internal string outputFilePath;

        
        public SelectObjectRequest(string bucket, string key)
            : base(bucket, key)
        {
            this.method = CosRequestMethod.POST;
            this.queryParameters.Add("select", null);
            this.queryParameters.Add("select-type", "2");
        }

        public SelectObjectRequest outputToFile(string filePath) {
            outputFilePath = filePath;
            return this;
        }

        public SelectObjectRequest setExpression(string expression) {
            this.expression = expression;
            return this;
        }

        public SelectObjectRequest setExpressionType(string expressionType) {
            this.expressionType = expressionType;
            return this;
        }

        public SelectObjectRequest setInputFormat(ObjectSelectionFormat inputFormat) {
            this.inputFormat = inputFormat;
            return this;
        }

        public SelectObjectRequest setOutputFormat(ObjectSelectionFormat outputFormat) {
            this.outputFormat = outputFormat;
            return this;
        }

        public SelectObjectRequest SetCosProgressCallback(COSXML.Callback.OnProgressCallback progressCallback)
        {
            this.progressCallback = progressCallback;
            return this;
        }

        public override void CheckParameters()
        {
            base.CheckParameters();
            
            if (expression == null) {
              throw new CosClientException((int)CosClientError.INVALID_ARGUMENT, 
                "expression is null");
            }
            if (inputFormat == null) {
              throw new CosClientException((int)CosClientError.INVALID_ARGUMENT, 
                "inputFormat is null");
            }
            if (outputFormat == null) {
              throw new CosClientException((int)CosClientError.INVALID_ARGUMENT, 
                "outputFormat is null");
            }
        }

        public override Network.RequestBody GetRequestBody()
        {
            string content = Transfer.XmlBuilder.BuildSelection(expression, expressionType, inputFormat,
              outputFormat, progressCallback != null);
            byte[] data = Encoding.UTF8.GetBytes(content);
            ByteRequestBody body = new ByteRequestBody(data);
            return body;
        }
    }
}

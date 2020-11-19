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

        public SelectObjectRequest OutputToFile(string filePath)
        {
            outputFilePath = filePath;

            return this;
        }

        public SelectObjectRequest SetExpression(string expression)
        {
            this.expression = expression;

            return this;
        }

        public SelectObjectRequest SetExpressionType(string expressionType)
        {
            this.expressionType = expressionType;

            return this;
        }

        public SelectObjectRequest SetInputFormat(ObjectSelectionFormat inputFormat)
        {
            this.inputFormat = inputFormat;

            return this;
        }

        public SelectObjectRequest SetOutputFormat(ObjectSelectionFormat outputFormat)
        {
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

            if (expression == null)
            {
                throw new CosClientException((int)CosClientError.InvalidArgument,
                  "expression is null");
            }

            if (inputFormat == null)
            {
                throw new CosClientException((int)CosClientError.InvalidArgument,
                  "inputFormat is null");
            }

            if (outputFormat == null)
            {
                throw new CosClientException((int)CosClientError.InvalidArgument,
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

using System.Text;
using COSXML.Common;
using COSXML.Network;
using COSXML.Model.Tag;
using COSXML.CosException;

namespace COSXML.Model.Object
{
    public sealed class SelectObjectRequest : ObjectRequest
    {
        private SelectObject selectObject;

        internal COSXML.Callback.OnProgressCallback progressCallback;

        internal string outputFilePath;


        public SelectObjectRequest(string bucket, string key)
            : base(bucket, key)
        {
            this.method = CosRequestMethod.POST;
            this.queryParameters.Add("select", null);
            this.queryParameters.Add("select-type", "2");
            this.selectObject = new SelectObject();
            selectObject.ExpressionType = "SQL";
        }

        public SelectObjectRequest OutputToFile(string filePath)
        {
            outputFilePath = filePath;

            return this;
        }

        public SelectObjectRequest SetExpression(string expression)
        {
            selectObject.Expression = expression;

            return this;
        }

        public SelectObjectRequest SetExpressionType(string expressionType)
        {
            selectObject.ExpressionType = expressionType;

            return this;
        }

        public SelectObjectRequest SetInputFormat(ObjectSelectionFormat inputFormat)
        {
            selectObject.InputFormat = inputFormat;

            return this;
        }

        public SelectObjectRequest SetOutputFormat(ObjectSelectionFormat outputFormat)
        {
            selectObject.OutputFormat = outputFormat;

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

            if (selectObject.Expression == null)
            {
                throw new CosClientException((int)CosClientError.InvalidArgument,
                  "expression is null");
            }

            if (selectObject.InputFormat == null)
            {
                throw new CosClientException((int)CosClientError.InvalidArgument,
                  "inputFormat is null");
            }

            if (selectObject.OutputFormat == null)
            {
                throw new CosClientException((int)CosClientError.InvalidArgument,
                  "outputFormat is null");
            }
        }

        public override Network.RequestBody GetRequestBody()
        {
            return GetXmlRequestBody(selectObject);
        }
    }
}

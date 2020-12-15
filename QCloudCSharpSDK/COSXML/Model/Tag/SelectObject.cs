using System.Collections.Generic;
using System.Xml.Serialization;

namespace COSXML.Model.Tag
{
    [XmlRoot("SelectRequest")]
    public sealed class SelectObject
    {

        [XmlElement("Expression")]
        public string Expression;

        [XmlElement("ExpressionType")]
        public string ExpressionType;

        [XmlElement("InputSerialization")]
        public ObjectSelectionFormat InputFormat;

        [XmlElement("OutputSerialization")]
        public ObjectSelectionFormat OutputFormat;
    }
}
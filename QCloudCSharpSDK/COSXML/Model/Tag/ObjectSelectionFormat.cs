using System.Collections.Generic;
using System.Xml.Serialization;

namespace COSXML.Model.Tag
{
    public sealed class ObjectSelectionFormat
    {

        [XmlElement("CompressionType")]
        public string compressionType;

        [XmlElement("CSV")]
        public CSVFormat csvFormat;

        [XmlElement("JSON")]
        public JSONFormat jsonFormat;

        public ObjectSelectionFormat()
        {
        }

        public ObjectSelectionFormat(string compressionType, CSVFormat csv)
        {
            this.compressionType = compressionType;
            this.csvFormat = csv;
            this.jsonFormat = null;
        }

        public ObjectSelectionFormat(string compressionType, JSONFormat json)
        {
            this.compressionType = compressionType;
            this.csvFormat = null;
            this.jsonFormat = json;
        }

        public sealed class CSVFormat
        {
            [XmlElement]
            public string FileHeaderInfo;

            [XmlElement]
            public string RecordDelimiter;

            [XmlElement]
            public string FieldDelimiter;

            [XmlElement]
            public string QuoteCharacter;

            [XmlElement]
            public string QuoteEscapeCharacter;

            [XmlElement]
            public string Comments;

            [XmlElement]
            public bool AllowQuotedRecordDelimiter;

            [XmlElement]
            public string QuoteFields;
        }

        public sealed class JSONFormat
        {
            [XmlElement]
            public string Type;

            [XmlElement]
            public string RecordDelimiter;
        }
    }
}
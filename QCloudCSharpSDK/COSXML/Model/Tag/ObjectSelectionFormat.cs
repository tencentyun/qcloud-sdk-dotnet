using System.Collections.Generic;

namespace COSXML.Model.Tag {
  public sealed class ObjectSelectionFormat {

    public readonly string CompressionType;

    public readonly CSVFormat csvFormat;

    public readonly JSONFormat jsonFormat;

    public ObjectSelectionFormat(string CompressionType, CSVFormat csv) {
      this.CompressionType = CompressionType;
      this.csvFormat = csv;
      this.jsonFormat = null;
    }

    public ObjectSelectionFormat(string CompressionType, JSONFormat json) {
      this.CompressionType = CompressionType;
      this.csvFormat = null;
      this.jsonFormat = json;
    }

    public sealed class CSVFormat {
      public string FileHeaderInfo;
      public string RecordDelimiter;
      public string FieldDelimiter;
      public string QuoteCharacter;
      public string QuoteEscapeCharacter;
      public string Comments;
      public bool AllowQuotedRecordDelimiter;

      public string QuoteFields;
    }

    public sealed class JSONFormat {
      public string Type;

      public string RecordDelimiter;
    }
  }
}
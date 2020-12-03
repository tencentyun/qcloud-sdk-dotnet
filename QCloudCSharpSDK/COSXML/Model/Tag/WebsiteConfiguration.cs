using System.Xml.Serialization;
using System.Collections.Generic;

namespace COSXML.Model.Tag
{
    [XmlRoot]
    public sealed class WebsiteConfiguration
    {
        [XmlElement("IndexDocument")]
        public IndexDocument indexDocument;

        [XmlElement("ErrorDocument")]
        public ErrorDocument errorDocument;

        [XmlElement("RedirectAllRequestsTo")]
        public RedirectAllRequestTo redirectAllRequestTo;

        [XmlArray("RoutingRules")]
        public List<RoutingRule> routingRules;

        public sealed class IndexDocument
        {
            [XmlElement("Suffix")]
            public string suffix;
        }

        public sealed class ErrorDocument
        {
            [XmlElement("Key")]
            public string key;
        }

        public sealed class RedirectAllRequestTo
        {
            [XmlElement("Protocol")]
            public string protocol;
        }

        public sealed class RoutingRule
        {
            [XmlElement("Condition")]
            public Contidion contidion;

            [XmlElement("Redirect")]
            public Redirect redirect;
        }

        public sealed class Contidion
        {
            [XmlElement("HttpErrorCodeReturnedEquals")]
            public int httpErrorCodeReturnedEquals;

            [XmlElement("KeyPrefixEquals")]
            public string keyPrefixEquals;
        }

        public sealed class Redirect
        {
            [XmlElement("Protocol")]
            public string protocol;

            [XmlElement("ReplaceKeyWith")]
            public string replaceKeyWith;

            [XmlElement("ReplaceKeyPrefixWith")]
            public string replaceKeyPrefixWith;
        }
    }
}

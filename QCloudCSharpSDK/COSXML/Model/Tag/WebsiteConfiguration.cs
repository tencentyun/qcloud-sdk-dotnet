

namespace COSXML.Model.Tag
{
    public sealed class WebsiteConfiguration
    {
        public IndexDocument indexDocument;
        public ErrorDocument errorDocument;
        public RedirectAllRequestTo redirectAllRequestTo;
        public System.Collections.Generic.List<RoutingRule> routingRules;

        public sealed class IndexDocument
        {
            public string suffix;
        }

        public sealed class ErrorDocument
        {
            public string key;
        }

        public sealed class RedirectAllRequestTo
        {
            public string protocol;
        }

        public sealed class RoutingRule
        {
            public Contidion contidion;
            public Redirect redirect;
        }

        public sealed class Contidion
        {
            public int httpErrorCodeReturnedEquals;
            public string keyPrefixEquals;
        }

        public sealed class Redirect
        {
            public string protocol;
            public string replaceKeyWith;
            public string replaceKeyPrefixWith;
        }
    }
}

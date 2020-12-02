

using COSXML.Common;
using COSXML.Model.Tag;
using COSXML.Network;
using System.Collections.Generic;
using System.Text;

namespace COSXML.Model.Bucket
{
    public sealed class PutBucketWebsiteRequest : BucketRequest
    {
        private WebsiteConfiguration websiteConfiguration;

        public PutBucketWebsiteRequest(string bucket) : base(bucket)
        {
            this.method = CosRequestMethod.PUT;
            this.queryParameters.Add("website", null);
            websiteConfiguration = new WebsiteConfiguration();
        }

        public void SetIndexDocument(string suffix)
        {

            if (suffix != null)
            {

                if (websiteConfiguration.indexDocument == null)
                {
                    websiteConfiguration.indexDocument = new WebsiteConfiguration.IndexDocument();
                }

                websiteConfiguration.indexDocument.suffix = suffix;
            }
        }

        public void SetErrorDocument(string key)
        {

            if (key != null)
            {

                if (websiteConfiguration.errorDocument == null)
                {
                    websiteConfiguration.errorDocument = new WebsiteConfiguration.ErrorDocument();
                }

                websiteConfiguration.errorDocument.key = key;
            }
        }

        public void SetRedirectAllRequestTo(string protocol)
        {

            if (protocol != null)
            {

                if (websiteConfiguration.redirectAllRequestTo == null)
                {
                    websiteConfiguration.redirectAllRequestTo = new WebsiteConfiguration.RedirectAllRequestTo();
                }

                websiteConfiguration.redirectAllRequestTo.protocol = protocol;
            }
        }

        public void SetRoutingRules(List<WebsiteConfiguration.RoutingRule> rules)
        {

            if (rules != null && rules.Count > 0)
            {

                if (websiteConfiguration.routingRules == null)
                {
                    websiteConfiguration.routingRules = new List<WebsiteConfiguration.RoutingRule>();
                }

                foreach (WebsiteConfiguration.RoutingRule rule in rules)
                {
                    websiteConfiguration.routingRules.Add(rule);
                }
            }
        }

        public override Network.RequestBody GetRequestBody()
        {
            return GetXmlRequestBody(websiteConfiguration);
        }
    }
}

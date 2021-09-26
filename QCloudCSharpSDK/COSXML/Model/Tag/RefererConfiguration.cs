using System;
using System.Xml.Serialization;
using System.Text;
using System.Collections.Generic;

namespace COSXML.Model.Tag
{
    [XmlRoot("RefererConfiguration")]
    public sealed class RefererConfiguration
    {
        [XmlElement("Status")]
        public string Status;

        [XmlElement("RefererType")]
        public string RefererType;

        [XmlElement("DomainList")]
        public DomainList domainList;

        [XmlElement("EmptyReferConfiguration")]
        public string EmptyReferConfiguration;

        public string GetInfo()
        {
            StringBuilder stringBuilder = new StringBuilder("{RefererConfiguration:\n");

            stringBuilder.Append("Status:").Append(Status).Append("\n");
            stringBuilder.Append("RefererType:").Append(RefererType).Append("\n");
            stringBuilder.Append(domainList.ToString());
            if(EmptyReferConfiguration == null)
            {
                stringBuilder.Append("EmptyReferConfiguration:").Append(EmptyReferConfiguration).Append("\n");
            }
            stringBuilder.Append("}");
            return stringBuilder.ToString();
        }
    }

    public sealed class DomainList
    {
        [XmlElement("Domain")]
        public List<string> domains;

        public DomainList()
        {
            this.domains = new List<string>();
        }

        public void AddDomain(string domain)
        {
            domains.Add(domain);
        }
        
        public string GetInfo()
        {
            StringBuilder stringBuilder = new StringBuilder("{DomainList:\n");
            foreach (var domain in domains) 
            {
                stringBuilder.Append("Domain:").Append(domain).Append("\n");
            }
            stringBuilder.Append("}");
            return stringBuilder.ToString();
        }
    }

}

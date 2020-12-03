using System.Xml.Serialization;

namespace COSXML.Model.Tag
{
    [XmlRoot("DomainConfiguration")]
    public sealed class DomainConfiguration
    {
        [XmlElement("DomainRule")]
        public DomainRule rule;

        public sealed class DomainRule
        {
            [XmlElement]
            public string Name;

            [XmlElement]
            public string Status;

            [XmlElement]
            public string Type;

            [XmlElement]
            public string Replace;
        }
    }
}

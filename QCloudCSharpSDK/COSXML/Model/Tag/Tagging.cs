using System.Collections.Generic;
using System.Xml.Serialization;

namespace COSXML.Model.Tag
{
    [XmlRoot("Tagging")]
    public sealed class Tagging
    {
        [XmlElement("TagSet")]
        public TagSet tagSet;

        public Tagging()
        {
            this.tagSet = new TagSet();
        }

        public void AddTag(string key, string value)
        {
            this.tagSet.tags.Add(new Tag(key, value));
        }

        public sealed class TagSet
        {
            [XmlElement("Tag")]
            public List<Tag> tags;

            public TagSet()
            {
                this.tags = new List<Tag>();
            }
        }

        public sealed class Tag
        {
            [XmlElement("Key")]
            public string key;

            [XmlElement("Value")]
            public string value;

            public Tag()
            {
                
            }

            public Tag(string key, string value)
            {
                this.key = key;
                this.value = value;
            }
        }
    }
}
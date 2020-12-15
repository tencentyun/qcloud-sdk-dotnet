using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using System.Text;

namespace COSXML.Model.Tag
{
    [XmlRoot("Delete")]
    public sealed class Delete
    {
        [XmlElement("Quiet")]
        public bool quiet;

        [XmlElement("Object")]
        public List<DeleteObject> deleteObjects;

        public sealed class DeleteObject
        {

            [XmlElement("Key")]
            public string key;

            [XmlElement("VersionId")]
            public string versionId;
        }
    }
}

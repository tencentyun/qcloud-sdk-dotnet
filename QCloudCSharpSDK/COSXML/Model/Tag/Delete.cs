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

        public string GetInfo()
        {
            StringBuilder stringBuilder = new StringBuilder("{Delete:\n");

            stringBuilder.Append("Quiet:").Append(quiet).Append("\n");

            if (deleteObjects != null)
            {

                foreach (DeleteObject deleteObject in deleteObjects)
                {
                    stringBuilder.Append(deleteObject.GetInfo());
                }
            }

            stringBuilder.Append("}");

            return stringBuilder.ToString();
        }

        public sealed class DeleteObject
        {

            [XmlElement("Key")]
            public string key;

            [XmlElement("VersionId")]
            public string versionId;

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{Object:\n");

                stringBuilder.Append("Key:").Append(key).Append("\n");
                stringBuilder.Append("VersionId:").Append(versionId).Append("\n");
                stringBuilder.Append("}");

                return stringBuilder.ToString();
            }
        }
    }
}

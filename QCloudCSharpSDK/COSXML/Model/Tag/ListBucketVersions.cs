using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace COSXML.Model.Tag
{
    [XmlRoot("ListVersionsResult")]
    public sealed class ListBucketVersions
    {
        [XmlElement("EncodingType")]
        public string encodingType;

        [XmlElement("Name")]
        public string name;

        [XmlElement("Prefix")]
        public string prefix;

        [XmlElement("KeyMarker")]
        public string keyMarker;

        [XmlElement("VersionIdMarker")]
        public string versionIdMarker;

        [XmlElement("MaxKeys")]
        public long maxKeys;

        [XmlElement("IsTruncated")]
        public bool isTruncated;

        [XmlElement("NextKeyMarker")]
        public string nextKeyMarker;

        [XmlElement("Delimiter")]
        public string delimiter;

        [XmlElement("NextVersionIdMarker")]
        public string nextVersionIdMarker;

        [XmlElement("Version")]
        public List<Version> objectVersionList;

        [XmlElement("CommonPrefixes")]
        public List<CommonPrefixes> commonPrefixesList;

        [XmlElement("DeleteMarker")]
        public List<DeleteMarker> deleteMarkers;

        public string GetInfo()
        {
            StringBuilder stringBuilder = new StringBuilder("{ListVersionsResult:\n");

            stringBuilder.Append("Name:").Append(name).Append("\n");
            stringBuilder.Append("EncodingType:").Append(encodingType).Append("\n");
            stringBuilder.Append("Prefix:").Append(prefix).Append("\n");
            stringBuilder.Append("Delimiter:").Append(delimiter).Append("\n");
            stringBuilder.Append("KeyMarker:").Append(keyMarker).Append("\n");
            stringBuilder.Append("VersionIdMarker:").Append(versionIdMarker).Append("\n");
            stringBuilder.Append("MaxKeys:").Append(maxKeys).Append("\n");
            stringBuilder.Append("IsTruncated:").Append(isTruncated).Append("\n");
            stringBuilder.Append("NextKeyMarker:").Append(nextKeyMarker).Append("\n");
            stringBuilder.Append("NextVersionIdMarker:").Append(nextVersionIdMarker).Append("\n");

            if (objectVersionList != null)
            {

                foreach (Version objectVersion in objectVersionList)
                {

                    if (objectVersion != null)
                    {
                        stringBuilder.Append(objectVersion.GetInfo()).Append("\n");
                    }
                }
            }

            if (commonPrefixesList != null)
            {

                foreach (CommonPrefixes commonPrefixes in commonPrefixesList)
                {

                    if (commonPrefixes != null)
                    {
                        stringBuilder.Append(commonPrefixes.GetInfo()).Append("\n");
                    }
                }
            }

            if (deleteMarkers != null)
            {

                foreach (DeleteMarker deleteMarker in deleteMarkers)
                {

                    if (deleteMarker != null)
                    {
                        stringBuilder.Append(deleteMarker.GetInfo()).Append("\n");
                    }
                }
            }

            stringBuilder.Append("}");

            return stringBuilder.ToString();
        }

        public class ObjectVersion
        {
            [XmlElement("Key")]
            public string key;

            [XmlElement("VersionId")]
            public string versionId;

            [XmlElement("IsLatest")]
            public bool isLatest;

            [XmlElement("LastModified")]
            public string lastModified;

            [XmlElement("Owner")]
            public Owner owner;
        }

        public sealed class Owner
        {
            [XmlElement("ID")]
            public string uid;

            [XmlElement("DisplayName")]
            public string displayName;

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{Owner:\n");

                stringBuilder.Append("Uid:").Append(uid).Append("\n");
                stringBuilder.Append("}");

                return stringBuilder.ToString();
            }
        }

        public sealed class DeleteMarker : ObjectVersion
        {

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{DeleteMarker:\n");

                stringBuilder.Append("Key:").Append(key).Append("\n");
                stringBuilder.Append("VersionId:").Append(versionId).Append("\n");
                stringBuilder.Append("IsLatest:").Append(isLatest).Append("\n");
                stringBuilder.Append("LastModified:").Append(lastModified).Append("\n");

                if (owner != null)
                {
                    stringBuilder.Append(owner.GetInfo()).Append("\n");
                }

                stringBuilder.Append("}");

                return stringBuilder.ToString();
            }
        }

        public sealed class Version : ObjectVersion
        {
            [XmlElement("ETag")]
            public string eTag;

            [XmlElement("Size")]
            public long size;

            [XmlElement("StorageClass")]
            public string storageClass;


            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{Version:\n");

                stringBuilder.Append("Key:").Append(key).Append("\n");
                stringBuilder.Append("VersionId:").Append(versionId).Append("\n");
                stringBuilder.Append("IsLatest:").Append(isLatest).Append("\n");
                stringBuilder.Append("LastModified:").Append(lastModified).Append("\n");
                stringBuilder.Append("ETag:").Append(eTag).Append("\n");
                stringBuilder.Append("Size:").Append(size).Append("\n");
                stringBuilder.Append("StorageClass:").Append(storageClass).Append("\n");

                if (owner != null)
                {
                    stringBuilder.Append(owner.GetInfo()).Append("\n");
                }

                stringBuilder.Append("}");

                return stringBuilder.ToString();
            }
        }

        public sealed class CommonPrefixes
        {
            /// <summary>
            /// 显示具体的 CommonPrefixes
            /// </summary>
            [XmlElement("Prefix")]
            public string prefix;

            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{CommonPrefixes:\n");

                stringBuilder.Append("Prefix:").Append(prefix).Append("\n");
                stringBuilder.Append("}");

                return stringBuilder.ToString();
            }
        }
    }
}

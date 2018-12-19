using System;
using System.Collections.Generic;

using System.Text;

namespace COSXML.Model.Tag
{
    public sealed class ListBucketVersions
    {
        public string name;
        public string prefix;
        public string keyMarker;
        public string versionIdMarker;
        public long maxKeys;
        public bool isTruncated;
        public string nextKeyMarker;
        public string nextVersionIdMarker;
        public List<ObjectVersion> objectVersionList;

        public string GetInfo()
        {
            StringBuilder stringBuilder = new StringBuilder("{ListVersionsResult:\n");
            stringBuilder.Append("Name:").Append(name).Append("\n");
            stringBuilder.Append("Prefix:").Append(prefix).Append("\n");
            stringBuilder.Append("KeyMarker:").Append(keyMarker).Append("\n");
            stringBuilder.Append("VersionIdMarker:").Append(versionIdMarker).Append("\n");
            stringBuilder.Append("MaxKeys:").Append(maxKeys).Append("\n");
            stringBuilder.Append("IsTruncated:").Append(isTruncated).Append("\n");
            stringBuilder.Append("NextKeyMarker:").Append(nextKeyMarker).Append("\n");
            stringBuilder.Append("NextVersionIdMarker:").Append(nextVersionIdMarker).Append("\n");
            if(objectVersionList != null)
            {
                foreach(ObjectVersion objectVersion in objectVersionList)
                {
                    stringBuilder.Append(objectVersion.GetInfo()).Append("\n");
                }
            }
            stringBuilder.Append("}");
            return stringBuilder.ToString();
        }

        public abstract class ObjectVersion
        {
            public string key;
            public string versionId;
            public bool isLatest;
            public string lastModified;
            public Owner owner;

            public abstract string GetInfo();
        }

        public sealed class Owner
        {
            public string uid;
        
            public string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{Owner:\n");
                stringBuilder.Append("Uid:").Append(uid).Append("\n");
                stringBuilder.Append("}");
                return stringBuilder.ToString();
            }
        }

        public sealed class DeleteMarker : ObjectVersion{

            public override string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{DeleteMarker:\n");
                stringBuilder.Append("Key:").Append(key).Append("\n");
                stringBuilder.Append("VersionId:").Append(versionId).Append("\n");
                stringBuilder.Append("IsLatest:").Append(isLatest).Append("\n");
                stringBuilder.Append("LastModified:").Append(lastModified).Append("\n");
                if(owner != null)
                {
                    stringBuilder.Append(owner.GetInfo()).Append("\n");
                }
                stringBuilder.Append("}");
                return stringBuilder.ToString();
            }
        }



        public sealed class Version : ObjectVersion
        {
            public string eTag;
            public long size;
            public string storageClass;


            public override string GetInfo()
            {
                StringBuilder stringBuilder = new StringBuilder("{Version:\n");
                stringBuilder.Append("Key:").Append(key).Append("\n");
                stringBuilder.Append("VersionId:").Append(versionId).Append("\n");
                stringBuilder.Append("IsLatest:").Append(isLatest).Append("\n");
                stringBuilder.Append("LastModified:").Append(lastModified).Append("\n");
                stringBuilder.Append("ETag:").Append(eTag).Append("\n");
                stringBuilder.Append("Size:").Append(size).Append("\n");
                stringBuilder.Append("StorageClass:").Append(storageClass).Append("\n");
                if(owner != null)
                {
                    stringBuilder.Append(owner.GetInfo()).Append("\n");
                }
                stringBuilder.Append("}");
                return stringBuilder.ToString();
            }
        }
    }
}

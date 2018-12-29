using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using System.IO;
using COSXML.Model.Tag;
using COSXML.Log;

namespace COSXML.Transfer
{
    public sealed class XmlParse
    {

        public static void ParseCosError(Stream inStream, CosServerError result)
        {
            XmlReader xmlReader = XmlReader.Create(inStream);
            while (xmlReader.Read())
            {
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        if ("Code".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.code = xmlReader.Value;
                        }
                        else if ("Message".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.message = xmlReader.Value;
                        }
                        else if ("RequestId".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.requestId = xmlReader.Value;
                        }
                        else if ("TraceId".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.traceId = xmlReader.Value;
                        }
                        else if ("Resource".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.resource = xmlReader.Value;
                        }
                        break;
                }
            }
        }

        public static void ParseListAllMyBucketsResult(Stream inStream, ListAllMyBuckets result)
        {
            XmlReader xmlReader = XmlReader.Create(inStream);
            ListAllMyBuckets.Bucket bucket = null;

            while (xmlReader.Read())
            {
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element: // element start
                        if("Owner".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase)) // get element name
                        {
                            result.owner = new ListAllMyBuckets.Owner();
                        }
                        else if ("ID".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.owner.id = xmlReader.Value; // get element value
                        }
                        else if ("DisplayName".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.owner.disPlayName = xmlReader.Value;
                        }
                        else if ("Buckets".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            result.buckets = new List<ListAllMyBuckets.Bucket>();
                        }
                        else if ("Bucket".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            bucket = new ListAllMyBuckets.Bucket();
                        }
                        else if ("Bucket".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            bucket = new ListAllMyBuckets.Bucket();
                        }
                        else if ("Name".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            bucket.name = xmlReader.Value;
                        }
                        else if ("Location".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            bucket.location = xmlReader.Value;
                        }
                        else if ("CreateDate".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            bucket.createDate = xmlReader.Value;
                        }
                        break;
                    case XmlNodeType.EndElement: //end element
                        if ("Bucket".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            result.buckets.Add(bucket);
                            bucket = null;
                        }
                        break;
                }
            }
        }


        public static void ParseListBucket(Stream inStream, ListBucket result)
        {
            XmlReader xmlReader = XmlReader.Create(inStream);
            ListBucket.Owner owner = null;
            ListBucket.CommonPrefixes commonPrefixes = null;
            ListBucket.Contents contents = null;
            result.commonPrefixesList = new List<ListBucket.CommonPrefixes>();
            result.contentsList = new List<ListBucket.Contents>();

            while (xmlReader.Read())
            {
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        if ("Name".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.name = xmlReader.Value;
                        }
                        else if ("Encoding-Type".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.encodingType = xmlReader.Value;
                        }
                        else if ("Marker".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.marker = xmlReader.Value;
                        }
                        else if ("MaxKeys".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            int.TryParse(xmlReader.Value, out result.maxKeys);
                        }
                        else if ("Delimiter".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.delimiter = xmlReader.Value;
                        }
                        else if ("NextMarker".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.nextMarker = xmlReader.Value;
                        }
                        else if ("IsTruncated".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            bool.TryParse(xmlReader.Value, out result.isTruncated);
                        }
                        else if ("Prefix".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            if (commonPrefixes == null)
                            {
                                result.prefix = xmlReader.Value;
                            }
                            else
                            {
                                commonPrefixes.prefix = xmlReader.Value;
                            }
                        }
                        else if ("Contents".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            contents = new ListBucket.Contents();
                        }
                        else if ("Key".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            contents.key = xmlReader.Value;
                        }
                        else if ("LastModified".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            contents.lastModified = xmlReader.Value;
                        }
                        else if ("ETag".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            contents.eTag = xmlReader.Value;
                        }
                        else if ("Size".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            long.TryParse(xmlReader.Value, out contents.size);
                        }
                        else if ("StorageClass".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            contents.storageClass = xmlReader.Value;
                        }
                        else if ("Owner".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            owner = new ListBucket.Owner();
                        }
                        else if ("ID".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            owner.id = xmlReader.Value;
                        }
                        else if ("CommonPrefixes".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            commonPrefixes = new ListBucket.CommonPrefixes();
                        }
                        break;
                    case XmlNodeType.EndElement:
                        if ("Contents".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            result.contentsList.Add(contents);
                            contents = null;
                        }
                        else if ("Owner".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            contents.owner = owner;
                            owner = null;
                        }
                        else if ("CommonPrefixes".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            result.commonPrefixesList.Add(commonPrefixes);
                            commonPrefixes = null;
                        }
                        break;
                }
            }
        }

        public static void ParseLocationConstraint(Stream inStream, LocationConstraint result)
        {
            XmlReader xmlReader = XmlReader.Create(inStream);
            while (xmlReader.Read())
            {
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        if ("LocationConstraint".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.location = xmlReader.Value;
                        }
                        break;
                }
            }
        }


        public static void ParseAccessControlPolicy(Stream inStream, AccessControlPolicy result)
        {
            XmlReader xmlReader = XmlReader.Create(inStream);
            AccessControlPolicy.Owner owner = null;
            AccessControlPolicy.Grant grant = null;
            AccessControlPolicy.Grantee grantee = null;
            result.accessControlList = new AccessControlPolicy.AccessControlList();
            result.accessControlList.grants = new List<AccessControlPolicy.Grant>();

            while (xmlReader.Read())
            {
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        if ("Owner".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            owner = new AccessControlPolicy.Owner();
                        }
                        else if ("ID".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            if (owner != null)
                            {
                                owner.id = xmlReader.Value;
                            }
                            else if (grantee != null)
                            {
                                grantee.id = xmlReader.Value;
                            }
                        }
                        else if ("URI".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            if (grantee != null)
                            {
                                grantee.uri = xmlReader.Value;
                            }
                        }
                        else if ("DisplayName".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            if (owner != null)
                            {
                                owner.displayName = xmlReader.Value;
                            }
                            else if (grantee != null)
                            {
                                grantee.displayName = xmlReader.Value;
                            }
                        }
                        else if ("Grant".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            grant = new AccessControlPolicy.Grant();
                        }
                        else if ("Grantee".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            grantee = new AccessControlPolicy.Grantee();
                        }
                        else if ("Permission".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            grant.permission = xmlReader.Value;
                        }
                        break;
                    case XmlNodeType.EndElement:
                        if ("Owner".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            result.owner = owner;
                            owner = null;
                        }
                        else if ("Grant".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            result.accessControlList.grants.Add(grant);
                            grant = null;
                        }
                        else if ("Grantee".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            grant.grantee = grantee;
                            grantee = null;
                        }
                        break;
                }
            }
        }

        public static void ParseCORSConfiguration(Stream inStream, CORSConfiguration result)
        {
            XmlReader xmlReader = XmlReader.Create(inStream);
            result.corsRules = new List<CORSConfiguration.CORSRule>();
            CORSConfiguration.CORSRule corsRule = null;

            while (xmlReader.Read())
            {
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        if ("CORSRule".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            corsRule = new CORSConfiguration.CORSRule();
                        }
                        else if ("ID".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            corsRule.id = xmlReader.Value;
                        }
                        else if ("AllowedOrigin".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            corsRule.allowedOrigin = xmlReader.Value;
                        }
                        else if ("AllowedMethod".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            if (corsRule.allowedMethods == null)
                            {
                                corsRule.allowedMethods = new List<string>();
                            }
                            corsRule.allowedMethods.Add(xmlReader.Value);
                        }
                        else if ("AllowedHeader".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            if (corsRule.allowedHeaders == null)
                            {
                                corsRule.allowedHeaders = new List<string>();
                            }
                            corsRule.allowedHeaders.Add(xmlReader.Value);
                        }
                        else if ("ExposeHeader".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            if (corsRule.exposeHeaders == null)
                            {
                                corsRule.exposeHeaders = new List<string>();
                            }
                            corsRule.exposeHeaders.Add(xmlReader.Value);
                        }
                        else if ("MaxAgeSeconds".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            int.TryParse(xmlReader.Value, out corsRule.maxAgeSeconds);
                        }
                        break;
                    case XmlNodeType.EndElement:
                        if ("CORSRule".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            result.corsRules.Add(corsRule);
                            corsRule = null;
                        }
                        break;
                }
            }
        }

        public static void ParseLifecycleConfiguration(Stream inStream, LifecycleConfiguration result)
        {
            XmlReader xmlReader = XmlReader.Create(inStream);
            result.rules = new List<LifecycleConfiguration.Rule>();
            LifecycleConfiguration.Rule rule = null;
            LifecycleConfiguration.Filter filter = null;
            LifecycleConfiguration.Transition transition = null;
            LifecycleConfiguration.Expiration expiration = null;
            LifecycleConfiguration.AbortIncompleteMultiUpload abortIncompleteMultiUpload = null;
            LifecycleConfiguration.NoncurrentVersionExpiration noncurrentVersionExpiration = null;
            LifecycleConfiguration.NoncurrentVersionTransition noncurrentVersionTransition = null;

            while (xmlReader.Read())
            {
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        if ("Rule".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            rule = new LifecycleConfiguration.Rule();
                        }
                        else if ("ID".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            rule.id = xmlReader.Value;
                        }
                        else if ("Filter".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            filter = new LifecycleConfiguration.Filter();
                        }
                        else if ("Prefix".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            filter.prefix = xmlReader.Value;
                        }
                        else if ("Status".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            rule.status = xmlReader.Value;
                        }
                        else if ("Transition".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            transition = new LifecycleConfiguration.Transition();
                        }
                        else if ("Expiration".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            expiration = new LifecycleConfiguration.Expiration();
                        }
                        else if ("Days".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            if (transition != null)
                            {
                                int.TryParse(xmlReader.Value, out transition.days);
                            }
                            else if (rule.expiration != null)
                            {
                                int.TryParse(xmlReader.Value, out expiration.days);
                            }
                        }
                        else if ("Date".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            if (transition != null)
                            {
                                transition.date = xmlReader.Value;
                            }
                            else if (expiration != null)
                            {
                                expiration.date = xmlReader.Value;
                            }
                        }
                        else if ("ExpiredObjectDeleteMarker".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            expiration.expiredObjectDeleteMarker = xmlReader.Value;
                        }
                        else if ("AbortIncompleteMultipartUpload".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            abortIncompleteMultiUpload = new LifecycleConfiguration.AbortIncompleteMultiUpload();
                        }
                        else if ("DaysAfterInitiation".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            int.TryParse(xmlReader.Value, out abortIncompleteMultiUpload.daysAfterInitiation);
                        }
                        else if ("NoncurrentVersionExpiration".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            noncurrentVersionExpiration = new LifecycleConfiguration.NoncurrentVersionExpiration();
                        }
                        else if ("NoncurrentVersionTransition".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            noncurrentVersionTransition = new LifecycleConfiguration.NoncurrentVersionTransition();
                        }
                        else if ("NoncurrentDays".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            if (noncurrentVersionExpiration != null)
                            {
                                int.TryParse(xmlReader.Value, out noncurrentVersionExpiration.noncurrentDays);
                            }
                            else if (noncurrentVersionTransition != null)
                            {
                                noncurrentVersionTransition.storageClass = xmlReader.Value;
                            }
                        }
                        else if ("StorageClass".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            if (transition != null)
                            {
                                transition.storageClass = xmlReader.Value;
                            }
                            else if (noncurrentVersionTransition != null)
                            {
                                noncurrentVersionTransition.storageClass = xmlReader.Value;
                            }
                        }

                        break;
                    case XmlNodeType.EndElement:
                        if ("Rule".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            result.rules.Add(rule);
                            rule = null;
                        }
                        else if ("Filter".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            rule.filter = filter;
                            filter = null;
                        }
                        else if ("Transition".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            rule.transition = transition;
                            transition = null;
                        }
                        else if ("NoncurrentVersionExpiration".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            rule.noncurrentVersionExpiration = noncurrentVersionExpiration;
                            noncurrentVersionExpiration = null;
                        }
                        else if ("NoncurrentVersionTransition".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            rule.noncurrentVersionTransition = noncurrentVersionTransition;
                            noncurrentVersionTransition = null;
                        }
                        else if ("Expiration".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            rule.expiration = expiration;
                            expiration = null;
                        }
                        else if ("AbortIncompleteMultipartUpload".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            rule.abortIncompleteMultiUpload = abortIncompleteMultiUpload;
                            abortIncompleteMultiUpload = null;
                        }
                        break;

                }
            }
        }

        public static void ParseReplicationConfiguration(Stream inStream, ReplicationConfiguration result)
        {
            XmlReader xmlReader = XmlReader.Create(inStream);
            result.rules = new List<ReplicationConfiguration.Rule>();
            ReplicationConfiguration.Rule rule = null;
            ReplicationConfiguration.Destination destination = null;

            while (xmlReader.Read())
            {
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        if ("Role".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.role = xmlReader.Value;
                        }
                        else if ("Rule".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            rule = new ReplicationConfiguration.Rule();
                        }
                        else if ("Status".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            rule.status = xmlReader.Value;
                        }
                        else if ("ID".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            rule.id = xmlReader.Value;
                        }
                        else if ("Prefix".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            rule.prefix = xmlReader.Value;
                        }
                        else if ("Destination".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            destination = new ReplicationConfiguration.Destination();
                        }
                        else if ("Bucket".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            destination.bucket = xmlReader.Value;
                        }
                        else if ("StorageClass".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            destination.storageClass = xmlReader.Value;
                        }
                        break;
                    case XmlNodeType.EndElement:
                        if ("Rule".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            result.rules.Add(rule);
                            rule = null;
                        }
                        else if ("Destination".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            rule.destination = destination;
                            destination = null;
                        }
                        break;
                }
            }
        }

        public static void ParseVersioningConfiguration(Stream inStream, VersioningConfiguration result)
        {
            XmlReader xmlReader = XmlReader.Create(inStream);
            while (xmlReader.Read())
            {
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        if ("Status".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.status = xmlReader.Value;
                        }
                        break;
                }
            }
        }

        public static void ParseListBucketVersions(Stream inStream, ListBucketVersions result)
        {
            XmlReader xmlReader = XmlReader.Create(inStream);
            result.objectVersionList = new List<ListBucketVersions.ObjectVersion>();
            ListBucketVersions.ObjectVersion objectVersion = null;
            ListBucketVersions.Owner owner = null;

            while (xmlReader.Read())
            {
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        if ("Name".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.name = xmlReader.Value;
                        }
                        else if ("Prefix".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.prefix = xmlReader.Value;
                        }
                        else if ("KeyMarker".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.keyMarker = xmlReader.Value;
                        }
                        else if ("VersionIdMarker".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.versionIdMarker = xmlReader.Value;
                        }
                        else if ("MaxKeys".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            long.TryParse(xmlReader.Value, out result.maxKeys);
                        }
                        else if ("IsTruncated".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            bool.TryParse(xmlReader.Value, out result.isTruncated);
                        }
                        else if ("NextKeyMarker".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.nextKeyMarker = xmlReader.Value;
                        }
                        else if ("NextVersionIdMarker".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.nextVersionIdMarker = xmlReader.Value;
                        }
                        else if ("DeleteMarker".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            objectVersion = new ListBucketVersions.DeleteMarker();
                        }
                        else if ("Version".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            objectVersion = new ListBucketVersions.Version();
                        }
                        else if ("Key".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            objectVersion.key = xmlReader.Value;
                        }
                        else if ("VersionId".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            objectVersion.versionId = xmlReader.Value;
                        }
                        else if ("IsLatest".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            bool.TryParse(xmlReader.Value, out objectVersion.isLatest);
                        }
                        else if ("LastModified".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            objectVersion.lastModified = xmlReader.Value;
                        }
                        else if ("Owner".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                             owner = new ListBucketVersions.Owner();
                        }
                        else if ("UID".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            owner.uid = xmlReader.Value;
                        }
                        else if ("ETag".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            ((ListBucketVersions.Version)objectVersion).eTag = xmlReader.Value;
                        }
                        else if ("Size".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            long.TryParse(xmlReader.Value, out ((ListBucketVersions.Version)objectVersion).size);
                        }
                        else if ("StorageClass".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            ((ListBucketVersions.Version)objectVersion).storageClass = xmlReader.Value;
                        }
                        break;
                    case XmlNodeType.EndElement:
                        if ("Owner".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            objectVersion.owner = owner;
                            owner = null;
                        }
                        else if ("DeleteMarker".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            result.objectVersionList.Add(objectVersion);
                            objectVersion = null;
                        }
                        else if ("Version".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            result.objectVersionList.Add(objectVersion);
                            objectVersion = null;
                        }
                        break;
                }
            }
        }

        public static void ParseListMultipartUploads(Stream inStream, ListMultipartUploads result)
        {
            XmlReader xmlReader = XmlReader.Create(inStream);
            ListMultipartUploads.CommonPrefixes commonPrefixes = null;
            ListMultipartUploads.Upload upload = null;
            result.uploads = new List<ListMultipartUploads.Upload>();
            result.commonPrefixes = new List<ListMultipartUploads.CommonPrefixes>();
            ListMultipartUploads.Initiator initiator = null;
            ListMultipartUploads.Owner owner = null;

            while (xmlReader.Read())
            {
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        //QLog.D("XIAO", xmlReader.Name);
                        if ("Bucket".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.bucket = xmlReader.Value;
                        }
                        else if ("Encoding-Type".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.encodingType = xmlReader.Value;
                        }
                        else if ("KeyMarker".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.keyMarker = xmlReader.Value;
                        }
                        else if ("UploadIdMarker".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.uploadIdMarker = xmlReader.Value;
                        }
                        else if ("NextKeyMarker".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.nextKeyMarker = xmlReader.Value;
                        }
                        else if ("NextUploadIdMarker".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.nextUploadIdMarker = xmlReader.Value;
                        }
                        else if ("MaxUploads".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.maxUploads = xmlReader.Value;
                        }
                        else if ("IsTruncated".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            bool.TryParse(xmlReader.Value, out result.isTruncated);
                        }
                        else if ("Prefix".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            if (commonPrefixes == null)
                            {
                                result.prefix = xmlReader.Value;
                            }
                            else
                            {
                                commonPrefixes.prefix = xmlReader.Value;
                            }
                        }
                        else if ("Delimiter".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.delimiter = xmlReader.Value;
                        }
                        else if ("Upload".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            upload = new ListMultipartUploads.Upload();
                        }
                        else if ("Key".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            upload.key = xmlReader.Value;
                        }
                        else if ("UploadId".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            upload.uploadID = xmlReader.Value;
                        }
                        else if ("StorageClass".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            upload.storageClass = xmlReader.Value;
                        }
                        else if ("Initiator".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            initiator = new ListMultipartUploads.Initiator();
                        }
                        else if ("UIN".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            initiator.uin = xmlReader.Value;
                        }
                        else if ("Owner".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                           owner = new ListMultipartUploads.Owner();
                        }
                        else if ("UID".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            owner.uid = xmlReader.Value;
                        }
                        else if ("ID".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            if (owner != null)
                            {
                                owner.id = xmlReader.Value;
                            }
                            else if (initiator != null)
                            {
                                initiator.id = xmlReader.Value;
                            }
                        }
                        else if ("DisplayName".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            if (owner != null)
                            {
                                owner.displayName = xmlReader.Value;
                            }
                            else if (initiator != null)
                            {
                                initiator.displayName = xmlReader.Value;
                            }
                        }
                        else if ("Initiated".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            upload.initiated = xmlReader.Value;
                        }
                        else if ("CommonPrefixs".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            commonPrefixes = new ListMultipartUploads.CommonPrefixes();
                        }
                        break;
                    case XmlNodeType.EndElement:
                        if ("Upload".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            result.uploads.Add(upload);
                            upload = null;
                        }
                        else if ("CommonPrefixs".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            result.commonPrefixes.Add(commonPrefixes);
                            commonPrefixes = null;
                        }
                        else if ("Owner".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            upload.owner = owner;
                            owner = null;
                        }
                        else if ("Initiator".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            upload.initiator = initiator;
                            initiator = null;
                        }
                        break;
                }
            }
        }

        public static void ParseCompleteMultipartUploadResult(Stream inStream, CompleteMultipartUploadResult result)
        {
            XmlReader xmlReader = XmlReader.Create(inStream);
            while (xmlReader.Read())
            {
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        if ("Location".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.location = xmlReader.Value;
                        }
                        else if ("Bucket".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.bucket = xmlReader.Value;
                        }
                        else if ("Key".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.key = xmlReader.Value;
                        }
                        else if ("ETag".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.eTag = xmlReader.Value;
                        }
                        break;
                }
            }
        }

        public static void ParseCopyObjectResult(Stream inStream, CopyObject result)
        {
            XmlReader xmlReader = XmlReader.Create(inStream);
            while (xmlReader.Read())
            {
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        if ("ETag".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.eTag = xmlReader.Value;
                        }
                        else if ("LastModified".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.lastModified = xmlReader.Value;
                        }
                        break;
                }
            }       
        }

        public static void ParseInitiateMultipartUpload(Stream inStream, InitiateMultipartUpload result)
        {
            XmlReader xmlReader = XmlReader.Create(inStream);
            while (xmlReader.Read())
            {
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        if ("Bucket".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.bucket = xmlReader.Value;
                         }
                         else if ("Key".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                         {
                             xmlReader.Read();
                             result.key = xmlReader.Value;
                         }
                         else if ("UploadId".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                         {
                            xmlReader.Read();
                            result.uploadId = xmlReader.Value;
                         }
                         break;
                }
            }
        }

        public static void ParseListParts(Stream inStream, ListParts result)
        {
            XmlReader xmlReader = XmlReader.Create(inStream);
            result.parts = new List<ListParts.Part>();
            ListParts.Owner owner = null;
            ListParts.Initiator initiator = null;
            ListParts.Part part = null;

            while (xmlReader.Read())
            {
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        if ("Bucket".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.bucket = xmlReader.Value;
                        }
                        else if ("Encoding-type".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.encodingType = xmlReader.Value;
                        }
                        else if ("Key".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.key = xmlReader.Value;
                        }
                        else if ("UploadId".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.uploadId = xmlReader.Value;
                        }
                        else if ("Owner".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            owner = new ListParts.Owner();
                        }
                        else if ("Initiator".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            initiator = new ListParts.Initiator();
                        }
                        else if ("ID".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            if (owner != null)
                            {
                                owner.id = xmlReader.Value;
                            }
                            else if (initiator != null)
                            {
                                initiator.id = xmlReader.Value;
                            }
                        }
                        else if ("DisplayName".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            if (owner != null)
                            {
                                owner.disPlayName = xmlReader.Value;
                            }
                            else if (initiator != null)
                            {
                                initiator.disPlayName = xmlReader.Value;
                            }
                        }
                        else if ("PartNumberMarker".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.partNumberMarker = xmlReader.Value;
                        }
                        else if ("StorageClass".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.storageClass = xmlReader.Value;
                        }
                        else if ("NextPartNumberMarker".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.nextPartNumberMarker = xmlReader.Value;
                        }
                        else if ("MaxParts".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.maxParts = xmlReader.Value;
                        }
                        else if ("IsTruncated".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            bool.TryParse(xmlReader.Value, out result.isTruncated);
                        }
                        else if ("Part".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            part = new ListParts.Part();
                        }
                        else if ("PartNumber".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            part.partNumber = xmlReader.Value;
                        }
                        else if ("LastModified".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            part.lastModified = xmlReader.Value;
                        }
                        else if ("ETag".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            part.eTag = xmlReader.Value;
                        }
                        else if ("Size".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            part.size = xmlReader.Value;
                        }
                        break;
                    case XmlNodeType.EndElement:
                        if ("Owner".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            result.owner = owner;
                            owner = null;
                        }
                        else if ("Initiator".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            result.initiator = initiator;
                            initiator = null;
                        }
                        else if ("Part".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            result.parts.Add(part);
                            part = null;
                        }
                        break;
                }
            }
        }

        public static void ParseDeleteResult(Stream inStream, DeleteResult result)
        {
            XmlReader xmlReader = XmlReader.Create(inStream);
            result.errorList = new List<DeleteResult.Error>();
            result.deletedList = new List<DeleteResult.Deleted>();
            DeleteResult.Deleted deleted = null;
            DeleteResult.Error error = null;

            while (xmlReader.Read())
            {
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        if ("Deleted".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            deleted = new DeleteResult.Deleted();
                        }
                        else if ("Error".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            error = new DeleteResult.Error();
                        }
                        else if ("Key".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            if (deleted != null)
                            {
                                deleted.key = xmlReader.Value;
                            }
                            else if (error != null)
                            {
                                error.key = xmlReader.Value;
                            }
                        }
                        else if ("VersionId".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            if (deleted != null)
                            {
                                deleted.versionId = xmlReader.Value;
                            }
                            else if (error != null)
                            {
                                error.versionId = xmlReader.Value;
                            }
                        }
                        else if ("DeleteMarker".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            deleted.deleteMarker = xmlReader.Value;
                        }
                        else if ("DeleteMarkerVersionId".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            deleted.deleteMarkerVersionId = xmlReader.Value;
                        }
                        else if ("Message".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            error.message = xmlReader.Value;
                        }
                        break;
                    case XmlNodeType.EndElement:
                        if ("Deleted".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            result.deletedList.Add(deleted);
                            deleted = null;
                        }
                        else if ("Error".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            result.errorList.Add(error);
                            error = null;
                        }
                        break;
                }
            }
        }

        public static void ParsePostResponse(Stream inStream, PostResponse result)
        {
            XmlReader xmlReader = XmlReader.Create(inStream);
            while (xmlReader.Read())
            {
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        if ("Location".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.location = xmlReader.Value;
                        }
                        else if ("Bucket".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.bucket = xmlReader.Value;
                        }
                        else if ("Key".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.key = xmlReader.Value;
                        }
                        else if ("ETag".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.eTag = xmlReader.Value;
                        }
                        break;
                }
            }
        }

    }
}

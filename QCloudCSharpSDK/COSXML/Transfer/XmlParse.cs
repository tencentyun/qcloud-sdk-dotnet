using COSXML.Model.Tag;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

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
                        else
if ("Message".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.message = xmlReader.Value;
                        }
                        else
if ("RequestId".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.requestId = xmlReader.Value;
                        }
                        else
if ("TraceId".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.traceId = xmlReader.Value;
                        }
                        else
if ("Resource".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
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
                                              // get element name
                        if ("Owner".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            result.owner = new ListAllMyBuckets.Owner();
                        }
                        else
                        if ("ID".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            // get element value
                            // get element value
                            result.owner.id = xmlReader.Value;
                        }
                        else
                        if ("DisplayName".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.owner.disPlayName = xmlReader.Value;
                        }
                        else
                        if ("Buckets".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            result.buckets = new List<ListAllMyBuckets.Bucket>();
                        }
                        else
                        if ("Bucket".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            bucket = new ListAllMyBuckets.Bucket();
                        }
                        else
                        if ("Bucket".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            bucket = new ListAllMyBuckets.Bucket();
                        }
                        else
                        if ("Name".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            bucket.name = xmlReader.Value;
                        }
                        else
                        if ("Location".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            bucket.location = xmlReader.Value;
                        }
                        else
                        if ("CreationDate".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
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
                        else
if ("ID".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();

                            if (owner != null)
                            {
                                owner.id = xmlReader.Value;
                            }
                            else
if (grantee != null)
                            {
                                grantee.id = xmlReader.Value;
                            }
                        }
                        else
if ("URI".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();

                            if (grantee != null)
                            {
                                grantee.uri = xmlReader.Value;
                            }
                        }
                        else
if ("DisplayName".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();

                            if (owner != null)
                            {
                                owner.displayName = xmlReader.Value;
                            }
                            else
if (grantee != null)
                            {
                                grantee.displayName = xmlReader.Value;
                            }
                        }
                        else
if ("Grant".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            grant = new AccessControlPolicy.Grant();
                        }
                        else
if ("Grantee".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            grantee = new AccessControlPolicy.Grantee();
                        }
                        else
if ("Permission".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
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
                        else
if ("Grant".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            result.accessControlList.grants.Add(grant);
                            grant = null;
                        }
                        else
if ("Grantee".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
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
                        else
if ("ID".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            corsRule.id = xmlReader.Value;
                        }
                        else
if ("AllowedOrigin".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            corsRule.allowedOrigin = xmlReader.Value;
                        }
                        else
if ("AllowedMethod".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();

                            if (corsRule.allowedMethods == null)
                            {
                                corsRule.allowedMethods = new List<string>();
                            }

                            corsRule.allowedMethods.Add(xmlReader.Value);
                        }
                        else
if ("AllowedHeader".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();

                            if (corsRule.allowedHeaders == null)
                            {
                                corsRule.allowedHeaders = new List<string>();
                            }

                            corsRule.allowedHeaders.Add(xmlReader.Value);
                        }
                        else
if ("ExposeHeader".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();

                            if (corsRule.exposeHeaders == null)
                            {
                                corsRule.exposeHeaders = new List<string>();
                            }

                            corsRule.exposeHeaders.Add(xmlReader.Value);
                        }
                        else
if ("MaxAgeSeconds".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
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
                        else
if ("Rule".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            rule = new ReplicationConfiguration.Rule();
                        }
                        else
if ("Status".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            rule.status = xmlReader.Value;
                        }
                        else
if ("ID".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            rule.id = xmlReader.Value;
                        }
                        else
if ("Prefix".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            rule.prefix = xmlReader.Value;
                        }
                        else
if ("Destination".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            destination = new ReplicationConfiguration.Destination();
                        }
                        else
if ("Bucket".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            destination.bucket = xmlReader.Value;
                        }
                        else
if ("StorageClass".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
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
                        else
if ("Destination".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
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

        public static void ParseTagging(Stream inStream, Tagging tagging)
        {
            XmlReader xmlReader = XmlReader.Create(inStream);
            string key = null;
            string value = null;

            while (xmlReader.Read())
            {

                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:

                        if ("Key".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            key = xmlReader.Value;
                        }
                        else
if ("Value".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            value = xmlReader.Value;
                        }

                        if (key != null && value != null)
                        {
                            tagging.AddTag(key, value);
                            key = null;
                            value = null;
                        }
                        break;
                }
            }
        }

        public static void ParseBucketDomain(Stream inStream, DomainConfiguration domainConfiguration)
        {
            XmlReader xmlReader = XmlReader.Create(inStream);
            domainConfiguration.rule = new DomainConfiguration.DomainRule();

            try
            {

                while (xmlReader.Read())
                {

                    switch (xmlReader.NodeType)
                    {
                        case XmlNodeType.Element:

                            if ("Status".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                            {
                                xmlReader.Read();
                                domainConfiguration.rule.Status = xmlReader.Value;
                            }
                            else if ("Type".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                            {
                                xmlReader.Read();
                                domainConfiguration.rule.Type = xmlReader.Value;
                            }
                            else if ("Name".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                            {
                                xmlReader.Read();
                                domainConfiguration.rule.Name = xmlReader.Value;
                            }
                            break;
                    }
                }
            }
            catch (XmlException e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        public static void ParseCompleteMultipartUploadResult(Stream inStream, CompleteResult result)
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
                        else
if ("Bucket".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.bucket = xmlReader.Value;
                        }
                        else
if ("Key".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.key = xmlReader.Value;
                        }
                        else
if ("ETag".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
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
                        else
if ("LastModified".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.lastModified = xmlReader.Value;
                        }
                        else
if ("VersionId".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.versionId = xmlReader.Value;
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
                        else
if ("Key".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.key = xmlReader.Value;
                        }
                        else
if ("UploadId".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.uploadId = xmlReader.Value;
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
                        else
if ("Error".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            error = new DeleteResult.Error();
                        }
                        else
if ("Key".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();

                            if (deleted != null)
                            {
                                deleted.key = xmlReader.Value;
                            }
                            else
if (error != null)
                            {
                                error.key = xmlReader.Value;
                            }
                        }
                        else
if ("VersionId".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();

                            if (deleted != null)
                            {
                                deleted.versionId = xmlReader.Value;
                            }
                            else
if (error != null)
                            {
                                error.versionId = xmlReader.Value;
                            }
                        }
                        else
if ("DeleteMarker".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            deleted.deleteMarker = xmlReader.Value;
                        }
                        else
if ("DeleteMarkerVersionId".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            deleted.deleteMarkerVersionId = xmlReader.Value;
                        }
                        else
if ("Message".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
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
                        else
if ("Error".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
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
                        else
if ("Bucket".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.bucket = xmlReader.Value;
                        }
                        else
if ("Key".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.key = xmlReader.Value;
                        }
                        else
if ("ETag".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.eTag = xmlReader.Value;
                        }
                        break;
                }
            }
        }

        public static void ParseBucketLoggingStatus(Stream inStream, BucketLoggingStatus result)
        {
            XmlReader xmlReader = XmlReader.Create(inStream);

            while (xmlReader.Read())
            {

                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:

                        if ("LoggingEnabled".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            result.loggingEnabled = new BucketLoggingStatus.LoggingEnabled();
                        }
                        else
if ("TargetBucket".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.loggingEnabled.targetBucket = xmlReader.Value;
                        }
                        else
if ("TargetPrefix".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            xmlReader.Read();
                            result.loggingEnabled.targetPrefix = xmlReader.Value;
                        }
                        break;
                }
            }
        }

        public static IntelligentTieringConfiguration ParseBucketIntelligentTiering(Stream inStream)
        {
            XmlReader xmlReader = XmlReader.Create(inStream);
            IntelligentTieringConfiguration configuration = new IntelligentTieringConfiguration();

            try
            {

                while (xmlReader.Read())
                {

                    switch (xmlReader.NodeType)
                    {
                        case XmlNodeType.Element:

                            if ("Status".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                            {
                                xmlReader.Read();
                                configuration.Status = xmlReader.Value;
                            }
                            else
if ("Days".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                            {
                                xmlReader.Read();
                                Int32.TryParse(xmlReader.Value, out configuration.Days);
                            }
                            else
if ("RequestFrequent".Equals(xmlReader.Name, StringComparison.OrdinalIgnoreCase))
                            {
                                xmlReader.Read();
                                Int32.TryParse(xmlReader.Value, out configuration.RequestFrequent);
                            }
                            break;
                    }
                }
            }
            catch (XmlException e)
            {
                Console.WriteLine(e.StackTrace);
            }

            return configuration;
        }

        public static T Deserialize<T>(Stream inStream) {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (inStream)
            {
                return (T) serializer.Deserialize(inStream);
            }
        }
    }
}

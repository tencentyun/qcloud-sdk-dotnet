using System;
using System.Collections.Generic;

using System.Text;
using COSXML.Model.Tag;
using System.Xml;
using System.IO;
using COSXML.Utils;

namespace COSXML.Transfer
{
    public sealed class XmlBuilder
    {
        public static string BuildCORSConfigXML(CORSConfiguration corsConfiguration)
        {
            StringWriter stringWriter = new StringWriter();
            XmlWriterSettings xmlWriterSetting = new XmlWriterSettings();
            xmlWriterSetting.Indent = true;

            XmlWriter xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSetting);
            xmlWriter.WriteStartDocument();

            //start to write element
            xmlWriter.WriteStartElement("CORSConfiguration");
            if (corsConfiguration.corsRules != null)
            {
                foreach(CORSConfiguration.CORSRule  corsRule in corsConfiguration.corsRules)
                {
                    if (corsRule == null) continue;

                    xmlWriter.WriteStartElement("CORSRule");

                    xmlWriter.WriteElementString("ID", corsRule.id);
                    xmlWriter.WriteElementString("AllowedOrigin", corsRule.allowedOrigin);
                    if (corsRule.allowedMethods != null)
                    {
                        foreach (string method in corsRule.allowedMethods)
                        {
                            xmlWriter.WriteElementString("AllowedMethod", method);
                        }
                    }
                    if (corsRule.allowedHeaders != null)
                    {
                        foreach (string header in corsRule.allowedHeaders)
                        {
                            xmlWriter.WriteElementString("AllowedHeader", header);
                        }
                    }
                    if (corsRule.exposeHeaders != null)
                    {
                        foreach (string exposeHeader in corsRule.exposeHeaders)
                        {
                            xmlWriter.WriteElementString("ExposeHeader", exposeHeader);
                        }
                    }
                    xmlWriter.WriteElementString("MaxAgeSeconds", corsRule.maxAgeSeconds.ToString());

                    xmlWriter.WriteEndElement();
                }
            }

            // end to element
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Flush();
            return RemoveXMLHeader(stringWriter.ToString());
        }

        internal static string BuildWebsiteConfiguration(BucketLoggingStatus bucketLoggingStatus)
        {
            throw new NotImplementedException();
        }

        public static string BuildLifecycleConfiguration(LifecycleConfiguration lifecycleConfiguration)
        {
            StringWriter stringWriter = new StringWriter();
            XmlWriterSettings xmlWriterSetting = new XmlWriterSettings();
            xmlWriterSetting.Indent = true;

            XmlWriter xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSetting);
            xmlWriter.WriteStartDocument();

            //start to write element
            xmlWriter.WriteStartElement("LifecycleConfiguration");

            if (lifecycleConfiguration.rules != null)
            {
                foreach (LifecycleConfiguration.Rule rule in lifecycleConfiguration.rules)
                {
                    if (rule == null) continue;
                    xmlWriter.WriteStartElement("Rule");
                    if(rule.id != null) xmlWriter.WriteElementString("ID", rule.id);
                    if (rule.filter != null)
                    {
                        xmlWriter.WriteStartElement("Filter");
                        if(rule.filter.prefix != null) xmlWriter.WriteElementString("Prefix", rule.filter.prefix);
                        if (rule.filter.filterAnd != null)
                        {
                            xmlWriter.WriteStartElement("And");
                            if (rule.filter.filterAnd.prefix != null) xmlWriter.WriteElementString("Prefix", rule.filter.filterAnd.prefix);
                            xmlWriter.WriteEndElement();
                        }
                        xmlWriter.WriteEndElement();
                    }
                    if(rule.status != null) xmlWriter.WriteElementString("Status", rule.status);
                    if (rule.transition != null)
                    {
                        xmlWriter.WriteStartElement("Transition");
                        if(rule.transition.days > 0) xmlWriter.WriteElementString("Days", rule.transition.days.ToString());
                        if (rule.transition.storageClass != null) xmlWriter.WriteElementString("StorageClass", rule.transition.storageClass);
                        if (rule.transition.date != null) xmlWriter.WriteElementString("Date", rule.transition.date);
                        xmlWriter.WriteEndElement();
                    }
                    if (rule.expiration != null)
                    {
                        xmlWriter.WriteStartElement("Expiration");
                        if (rule.expiration.days > 0) xmlWriter.WriteElementString("Days", rule.expiration.days.ToString());
                        if (rule.expiration.expiredObjectDeleteMarker != null)
                        {
                            if ((bool)rule.expiration.expiredObjectDeleteMarker)
                            {
                                xmlWriter.WriteElementString("ExpiredObjectDeleteMarker", "true");
                            }
                            else
                            {
                                xmlWriter.WriteElementString("ExpiredObjectDeleteMarker", "false");
                            }
                        } 
                        if (rule.expiration.date != null) xmlWriter.WriteElementString("Date", rule.expiration.date);
                        xmlWriter.WriteEndElement();
                    }
                    if (rule.noncurrentVersionTransition != null)
                    {
                        xmlWriter.WriteStartElement("NoncurrentVersionTransition");
                        if(rule.noncurrentVersionTransition.noncurrentDays > 0) xmlWriter.WriteElementString("NoncurrentDays", rule.noncurrentVersionTransition.noncurrentDays.ToString());
                        if(rule.noncurrentVersionTransition.storageClass != null) xmlWriter.WriteElementString("StorageClass", rule.noncurrentVersionTransition.storageClass);
                        xmlWriter.WriteEndElement();
                    }
                    if (rule.noncurrentVersionExpiration != null)
                    {
                        xmlWriter.WriteStartElement("NoncurrentVersionExpiration");
                        if(rule.noncurrentVersionExpiration.noncurrentDays > 0) xmlWriter.WriteElementString("NoncurrentDays", rule.noncurrentVersionExpiration.noncurrentDays.ToString());
                        xmlWriter.WriteEndElement();
                    }
                    if (rule.abortIncompleteMultiUpload != null)
                    {
                        xmlWriter.WriteStartElement("AbortIncompleteMultipartUpload");
                        if (rule.abortIncompleteMultiUpload.daysAfterInitiation > 0) xmlWriter.WriteElementString("DaysAfterInitiation", rule.abortIncompleteMultiUpload.daysAfterInitiation.ToString());
                        xmlWriter.WriteEndElement();
                    }
                }
            }

            // end to element
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndDocument();
            xmlWriter.Flush();
            return RemoveXMLHeader(stringWriter.ToString());
        }

        public static string BuildReplicationConfiguration(ReplicationConfiguration replicationConfiguration)
        {
            StringWriter stringWriter = new StringWriter();
            XmlWriterSettings xmlWriterSetting = new XmlWriterSettings();
            xmlWriterSetting.Indent = true;

            XmlWriter xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSetting);
            xmlWriter.WriteStartDocument();

            //start to write element
            xmlWriter.WriteStartElement("ReplicationConfiguration");

            xmlWriter.WriteElementString("Role", replicationConfiguration.role);

             if(replicationConfiguration.rules != null)
             {
                foreach(ReplicationConfiguration.Rule rule in replicationConfiguration.rules)
                {
                    if(rule == null)continue;

                    xmlWriter.WriteStartElement("Rule");

                    xmlWriter.WriteElementString("Status", rule.status);
                    xmlWriter.WriteElementString("ID", rule.id);
                    xmlWriter.WriteElementString("Prefix", rule.prefix);
                    if(rule.destination != null){
                        xmlWriter.WriteStartElement("Destination");
                        xmlWriter.WriteElementString("Bucket", rule.destination.bucket);
                        if(rule.destination.storageClass != null) xmlWriter.WriteElementString("StorageClass", rule.destination.storageClass);
                        xmlWriter.WriteEndElement();
                    }
                    xmlWriter.WriteEndElement();
                }
            }

            // end to element
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndDocument();
            xmlWriter.Flush();
            return RemoveXMLHeader(stringWriter.ToString());
        }

        public static string BuildVersioningConfiguration(VersioningConfiguration versioningConfiguration)
        {
            StringWriter stringWriter = new StringWriter();
            XmlWriterSettings xmlWriterSetting = new XmlWriterSettings();
            xmlWriterSetting.Indent = true;

            XmlWriter xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSetting);
            xmlWriter.WriteStartDocument();

            //start to write element
            xmlWriter.WriteStartElement("VersioningConfiguration");

            xmlWriter.WriteElementString("Status", versioningConfiguration.status);

            // end to element
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndDocument();
            xmlWriter.Flush();
            return RemoveXMLHeader(stringWriter.ToString());
        }

        public static string BuildCompleteMultipartUpload(CompleteMultipartUpload completeMultipartUpload)
        {
            StringWriter stringWriter = new StringWriter();
            XmlWriterSettings xmlWriterSetting = new XmlWriterSettings();
            xmlWriterSetting.Indent = true;

            XmlWriter xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSetting);
            xmlWriter.WriteStartDocument();

            //start to write element
            xmlWriter.WriteStartElement("CompleteMultipartUpload");

            if(completeMultipartUpload.parts != null)
            {
                foreach(CompleteMultipartUpload.Part part in completeMultipartUpload.parts)
                {
                    if(part == null)continue;
                    xmlWriter.WriteStartElement("Part");
                    xmlWriter.WriteElementString("PartNumber", part.partNumber.ToString());
                    xmlWriter.WriteElementString("ETag", part.eTag);
                    xmlWriter.WriteEndElement();
                }
            }

            // end to element
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndDocument();
            xmlWriter.Flush();
            return RemoveXMLHeader(stringWriter.ToString());
        }

        public static string BuildDelete(Delete delete)
        {
            StringWriter stringWriter = new StringWriter();
            XmlWriterSettings xmlWriterSetting = new XmlWriterSettings();
            xmlWriterSetting.Indent = true;

            XmlWriter xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSetting);
            xmlWriter.WriteStartDocument();

            //start to write element
            xmlWriter.WriteStartElement("Delete");
            xmlWriter.WriteElementString("Quiet", delete.quiet ? "true" : "false");
            if (delete.deleteObjects != null)
            {
                foreach(Delete.DeleteObject deleteObject in delete.deleteObjects)
                {
                    if(deleteObject == null)continue;
                    xmlWriter.WriteStartElement("Object");
                    xmlWriter.WriteElementString("Key", deleteObject.key);
                    if(deleteObject.versionId != null) xmlWriter.WriteElementString("VersionId", deleteObject.versionId);
                    xmlWriter.WriteEndElement();
                }
            }

            // end to element
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndDocument();
            xmlWriter.Flush();
            return RemoveXMLHeader(stringWriter.ToString());
        }

        public static string BuildRestoreConfigure(RestoreConfigure restoreConfigure)
        {
            StringWriter stringWriter = new StringWriter();
            XmlWriterSettings xmlWriterSetting = new XmlWriterSettings();
            xmlWriterSetting.Indent = true;

            XmlWriter xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSetting);
            xmlWriter.WriteStartDocument();

            //start to write element
            xmlWriter.WriteStartElement("RestoreRequest");

            xmlWriter.WriteElementString("Days", restoreConfigure.days.ToString());
            if (restoreConfigure.casJobParameters != null)
            {
                xmlWriter.WriteStartElement("CASJobParameters");
                xmlWriter.WriteElementString("Tier", EnumUtils.GetValue(restoreConfigure.casJobParameters.tier));
                xmlWriter.WriteEndElement();
            }

            // end to element
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndDocument();
            xmlWriter.Flush();
            return RemoveXMLHeader(stringWriter.ToString());
        }

        public static string BuildWebsiteConfiguration(WebsiteConfiguration websiteConfiguration)
        {
            StringWriter stringWriter = new StringWriter();
            XmlWriterSettings xmlWriterSetting = new XmlWriterSettings();
            xmlWriterSetting.Indent = true;

            XmlWriter xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSetting);
            xmlWriter.WriteStartDocument();

            if (websiteConfiguration.indexDocument != null)
            {
                xmlWriter.WriteStartElement("IndexDocument");
                xmlWriter.WriteElementString("Suffix", websiteConfiguration.indexDocument.suffix);
                xmlWriter.WriteEndElement();
            }

            if (websiteConfiguration.errorDocument != null)
            {
                xmlWriter.WriteStartElement("ErrorDocument");
                xmlWriter.WriteElementString("Key", websiteConfiguration.errorDocument.key);
                xmlWriter.WriteEndElement();
            }

            if (websiteConfiguration.redirectAllRequestTo != null)
            {
                xmlWriter.WriteStartElement("RedirectAllRequestTo");
                xmlWriter.WriteElementString("Protocol", websiteConfiguration.redirectAllRequestTo.protocol);
                xmlWriter.WriteEndElement();
            }

            if (websiteConfiguration.routingRules != null && websiteConfiguration.routingRules.Count > 0)
            {
                xmlWriter.WriteStartElement("RoutingRules");
                foreach (WebsiteConfiguration.RoutingRule routingRule in websiteConfiguration.routingRules)
                {
                    xmlWriter.WriteStartElement("RoutingRule");
                    if (routingRule.contidion != null)
                    {
                        xmlWriter.WriteStartElement("Condition");
                        xmlWriter.WriteElementString("HttpErrorCodeReturnedEquals", routingRule.contidion.httpErrorCodeReturnedEquals.ToString());
                        xmlWriter.WriteElementString("KeyPrefixEquals", routingRule.contidion.keyPrefixEquals);
                        xmlWriter.WriteEndElement();
                    }
                    if (routingRule.redirect != null)
                    {
                        xmlWriter.WriteStartElement("Redirect");
                        xmlWriter.WriteElementString("Protocol", routingRule.redirect.protocol);
                        xmlWriter.WriteElementString("ReplaceKeyPrefixWith", routingRule.redirect.replaceKeyPrefixWith);
                        xmlWriter.WriteElementString("ReplaceKeyWith", routingRule.redirect.replaceKeyWith);
                        xmlWriter.WriteEndElement();
                    }
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
            }
            // end to element
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndDocument();
            xmlWriter.Flush();
            return RemoveXMLHeader(stringWriter.ToString());
        }

        public static string BuildBucketLogging(BucketLoggingStatus bucketLoggingStatus)
        {
            StringWriter stringWriter = new StringWriter();
            XmlWriterSettings xmlWriterSetting = new XmlWriterSettings();
            xmlWriterSetting.Indent = true;

            XmlWriter xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSetting);
            xmlWriter.WriteStartDocument();

            //start to write element
            xmlWriter.WriteStartElement("BucketLoggingStatus");
            if (bucketLoggingStatus.loggingEnabled != null)
            {
                xmlWriter.WriteStartElement("LoggingEnabled");
                if(bucketLoggingStatus.loggingEnabled.targetBucket != null)
                    xmlWriter.WriteElementString("TargetBucket", bucketLoggingStatus.loggingEnabled.targetBucket);
                if(bucketLoggingStatus.loggingEnabled.targetPrefix != null)
                    xmlWriter.WriteElementString("TargetPrefix", bucketLoggingStatus.loggingEnabled.targetPrefix);
                xmlWriter.WriteEndElement();
            }
          
            // end to element
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndDocument();
            xmlWriter.Flush();
            return RemoveXMLHeader(stringWriter.ToString());
        }

        public static string BuildInventoryConfiguration(InventoryConfiguration inventoryConfiguration)
        {
            StringWriter stringWriter = new StringWriter();
            XmlWriterSettings xmlWriterSetting = new XmlWriterSettings();
            xmlWriterSetting.Indent = true;

            XmlWriter xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSetting);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("InventoryConfiguration");

            if (inventoryConfiguration.id != null)
                xmlWriter.WriteElementString( "Id", inventoryConfiguration.id);
            xmlWriter.WriteElementString("IsEnabled", inventoryConfiguration.isEnabled ? "True" : "False");
            if (inventoryConfiguration.destination != null)
            {
                xmlWriter.WriteStartElement("Destination");
                if (inventoryConfiguration.destination.cosBucketDestination != null)
                {
                    xmlWriter.WriteStartElement("COSBucketDestination");
                    if (inventoryConfiguration.destination.cosBucketDestination.format != null)
                        xmlWriter.WriteElementString("Format", inventoryConfiguration.destination.cosBucketDestination.format);
                    if (inventoryConfiguration.destination.cosBucketDestination.accountId != null)
                        xmlWriter.WriteElementString("AccountId", inventoryConfiguration.destination.cosBucketDestination.accountId);
                    if (inventoryConfiguration.destination.cosBucketDestination.bucket != null)
                        xmlWriter.WriteElementString("Bucket", inventoryConfiguration.destination.cosBucketDestination.bucket);
                    if (inventoryConfiguration.destination.cosBucketDestination.prefix != null)
                    {
                        xmlWriter.WriteElementString("Prefix", inventoryConfiguration.destination.cosBucketDestination.prefix);
                    }
                    if (inventoryConfiguration.destination.cosBucketDestination.encryption != null)
                    {
                        xmlWriter.WriteStartElement("Encryption");
                        xmlWriter.WriteElementString("SSE-COS", inventoryConfiguration.destination.cosBucketDestination.encryption.sSECOS);
                        xmlWriter.WriteEndElement();
                    }
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
            }
            if (inventoryConfiguration.schedule != null && inventoryConfiguration.schedule.frequency != null)
            {
                xmlWriter.WriteStartElement("Schedule");
                xmlWriter.WriteElementString("Frequency", inventoryConfiguration.schedule.frequency);
                xmlWriter.WriteEndElement();
            }
            if (inventoryConfiguration.filter != null && inventoryConfiguration.filter.prefix != null)
            {
                xmlWriter.WriteStartElement("Filter");
                xmlWriter.WriteElementString("Prefix", inventoryConfiguration.filter.prefix);
                xmlWriter.WriteEndElement();
            }
            if (inventoryConfiguration.includedObjectVersions != null)
            {
                xmlWriter.WriteElementString("IncludeObjectVersions", inventoryConfiguration.includedObjectVersions);
            }
            if (inventoryConfiguration.optionalFields != null && inventoryConfiguration.optionalFields.fields != null)
            {
                xmlWriter.WriteStartElement("OptionalFields");
                foreach (string field in inventoryConfiguration.optionalFields.fields)
                {
                    xmlWriter.WriteElementString("Field", field);
                }
                xmlWriter.WriteEndElement();
            }

            // end to element
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndDocument();
            xmlWriter.Flush();
            return RemoveXMLHeader(stringWriter.ToString());

        }

        private static string RemoveXMLHeader(string xmlContent)
        {
            if (xmlContent != null)
            {
                if (xmlContent.StartsWith("<?xml"))
                {
                    int end = xmlContent.IndexOf("?>");
                    xmlContent = xmlContent.Substring(end + 2);
                }
            }
            return xmlContent;
        }

    }
}

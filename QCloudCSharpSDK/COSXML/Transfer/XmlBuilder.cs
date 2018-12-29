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
                    xmlWriter.WriteElementString("ID", rule.id);
                    if (rule.filter != null)
                    {
                        xmlWriter.WriteStartElement("Filter");
                        xmlWriter.WriteElementString("Prefix", rule.filter.prefix);
                        xmlWriter.WriteEndElement();
                    }
                    xmlWriter.WriteElementString("Status", rule.status);
                    if (rule.transition != null)
                    {
                        xmlWriter.WriteStartElement("Transition");
                        xmlWriter.WriteElementString("Days", rule.transition.days.ToString());
                        xmlWriter.WriteElementString("StorageClass", rule.transition.storageClass);
                        xmlWriter.WriteElementString("Date", rule.transition.date);
                        xmlWriter.WriteEndElement();
                    }
                    if (rule.expiration != null)
                    {
                        xmlWriter.WriteStartElement("Expiration");
                        xmlWriter.WriteElementString("Days", rule.expiration.days.ToString());
                        xmlWriter.WriteElementString("ExpiredObjectDeleteMarker", rule.expiration.expiredObjectDeleteMarker);
                        xmlWriter.WriteElementString("Date", rule.expiration.date);
                        xmlWriter.WriteEndElement();
                    }
                    if (rule.noncurrentVersionTransition != null)
                    {
                        xmlWriter.WriteStartElement("NoncurrentVersionTransition");
                        xmlWriter.WriteElementString("NoncurrentDays", rule.noncurrentVersionTransition.noncurrentDays.ToString());
                        xmlWriter.WriteElementString("StorageClass", rule.noncurrentVersionTransition.storageClass);
                        xmlWriter.WriteEndElement();
                    }
                    if (rule.noncurrentVersionExpiration != null)
                    {
                        xmlWriter.WriteStartElement("NoncurrentVersionExpiration");
                        xmlWriter.WriteElementString("NoncurrentDays", rule.noncurrentVersionExpiration.noncurrentDays.ToString());
                        xmlWriter.WriteEndElement();
                    }
                    if (rule.abortIncompleteMultiUpload != null)
                    {
                        xmlWriter.WriteStartElement("AbortIncompleteMultipartUpload");
                        xmlWriter.WriteElementString("DaysAfterInitiation", rule.abortIncompleteMultiUpload.daysAfterInitiation.ToString());
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
                        xmlWriter.WriteElementString("StorageClass", rule.destination.storageClass);
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
                    xmlWriter.WriteElementString("VersionId", deleteObject.versionId);
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

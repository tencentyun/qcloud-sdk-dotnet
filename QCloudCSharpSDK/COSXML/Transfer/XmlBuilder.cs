using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;
using COSXML.Model.Tag;
using System.Xml;
using System.IO;
using COSXML.Utils;

namespace COSXML.Transfer
{
    public sealed class XmlBuilder
    {
        public static String Serialize(object o) 
        {
            var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            XmlSerializer serializer = new XmlSerializer(o.GetType());

            using (var stream = new StringWriter())
            using (var writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, o, emptyNs);
                return stream.ToString();
            }
        }

        // private static string RemoveXMLHeader(string xmlContent)
        // {

        //     if (xmlContent != null)
        //     {

        //         if (xmlContent.StartsWith("<?xml"))
        //         {
        //             int end = xmlContent.IndexOf("?>");

        //             xmlContent = xmlContent.Substring(end + 2);
        //         }
        //     }

        //     return xmlContent;
        // }

    }
}

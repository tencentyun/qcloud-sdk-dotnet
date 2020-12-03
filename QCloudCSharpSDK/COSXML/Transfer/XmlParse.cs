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
        public static T Deserialize<T>(Stream inStream) 
        {
            using (inStream)
            {
                using (XmlTextReader reader = new XmlTextReader(inStream))
                {
                    reader.Namespaces = false;
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    return (T) serializer.Deserialize(reader);
                }
            }
        }
    }
}

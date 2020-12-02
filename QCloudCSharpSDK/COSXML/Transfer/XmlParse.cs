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
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (inStream)
            {
                return (T) serializer.Deserialize(inStream);
            }
        }
    }
}

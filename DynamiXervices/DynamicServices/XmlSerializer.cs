using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Taga.DynamicServices
{
    public class XmlSerializer<T> where T : class
    {
        private XmlSerializer _serializer;
        private XmlSerializer Serializer
        {
            get { return _serializer ?? (_serializer = CreateXmlSerializer()); }
        }

        private static XmlSerializer CreateXmlSerializer()
        {
            return new XmlSerializer(typeof(T));
        }

        private XmlSerializerNamespaces _namespaces;
        private XmlSerializerNamespaces Namespaces
        {
            get
            {
                if (_namespaces == null)
                {
                    _namespaces = new XmlSerializerNamespaces();
                    _namespaces.Add(String.Empty, String.Empty);
                }
                return _namespaces;
            }
        }

        public T Deserialize(string filePath)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                return Deserialize(fs);
        }

        public T Deserialize(Stream stream)
        {
            return Serializer.Deserialize(stream) as T;
        }

        public T Deserialize(StringBuilder stringBuilder)
        {
            using (var sr = new StringReader(stringBuilder.ToString()))
                return Deserialize(sr);
        }

        public T Deserialize(TextReader textReader)
        {
            return Serializer.Deserialize(textReader) as T;
        }

        public void Serialize(T obj, string filePath)
        {
            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                Serialize(obj, fs);
        }

        public void Serialize(T obj, Stream stream)
        {
            Serializer.Serialize(stream, obj, Namespaces);
        }

        public void Serialize(T obj, StringBuilder stringBuilder)
        {
            using (var sw = new StringWriter(stringBuilder))
                Serialize(obj, sw);
        }

        public void Serialize(T obj, TextWriter textWriter)
        {
            Serializer.Serialize(textWriter, obj, Namespaces);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace DiagramDesigner.AdventureWorld.Extensions
{
    public static class SerializationExtensions
    {
        public static string SerializeToString<T>(T instance, IEnumerable<Type> knownTypes)
        {
            var serializer = new DataContractSerializer(typeof(T), knownTypes);
            using (var stringWriter = new StringWriter())
            using (var writer = new XmlTextWriter(stringWriter))
            {
                serializer.WriteObject(writer, instance);
                return stringWriter.ToString();
            }
        }

        public static string SerializeToString<T>(T instance)
        {
            return SerializeToString(instance, new List<Type>());
        }
        
        public static T CreateInstance<T>(string xmlData, IEnumerable<Type> knownTypes) where T: class
        {
            using (Stream stream = new MemoryStream())
            {
                var data = Encoding.UTF8.GetBytes(xmlData);
                stream.Write(data, 0, data.Length);
                stream.Position = 0;
                var deserializer = new DataContractSerializer(typeof(T), knownTypes);

                return deserializer.ReadObject(stream) as T;
            }
        }
    }
}

using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace CryptoSystemDissertation.BusinessLogic
{
    public class Helper
    {
        public static string Serialize<T>(T item)
        {
            string result;
            var serializer = new XmlSerializer(typeof(T));
            var xmlWriterSettings = new XmlWriterSettings
            {
                CloseOutput = false,
                OmitXmlDeclaration = true,
                Indent = true
            };
            var xmlNameSpace = new XmlSerializerNamespaces();
            xmlNameSpace.Add("", "");

            using (var stringWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings))
                {
                    serializer.Serialize(xmlWriter, item, xmlNameSpace);
                    result = stringWriter.ToString();
                }
            }

            return result;
        }

        public static T Deserialize<T>(string serializedItem)
        {
            T item;
            var serializer = new XmlSerializer(typeof(T));

            using (var stringReader = new StringReader(serializedItem))
            {
                item = (T)serializer.Deserialize(stringReader);
            }
            return item;
        }
    }
}
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;


namespace NotificationServer.Core.Utilities
{
    public static class XmlSerializerHelper
    {
        /// <summary>
        /// Serializa una clase a un archivo xml
        /// </summary>
        /// <param name="type"></param>
        /// <param name="target"></param>
        /// <param name="fileName"></param>
        public static string Serialize(object target)
        {
            // this line removes extra namespaces added by the framework. without this line it would render as follows:
            //  <facture xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            // TODO: a futuro agregar aque la xsd que usemos nosotros para validar
            ns.Add("", "");

            StringBuilder sb = new StringBuilder();
            XmlSerializer serializer = new XmlSerializer(target.GetType());
            using (TextWriter writer = new StringWriter(sb))
            { serializer.Serialize(writer, target, ns); }
            return sb.ToString();
        }

        /// <summary>
        /// Serializa una clase a un archivo xml
        /// </summary>
        /// <param name="type"></param>
        /// <param name="target"></param>
        /// <param name="fileName"></param>
        public static void Serialize(object target, string xmlFileName)
        {
            FileSystemHelper.CreateParentFolder(xmlFileName); // se asegura que el folder padre exista
            // this line removes extra namespaces added by the framework. without this line it would render as follows:
            //  <facture xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
            // TODO: a futuro agregar aque la xsd que usemos nosotros para validar
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            XmlSerializer serializer = new XmlSerializer(target.GetType());
            using (TextWriter writer = new StreamWriter(xmlFileName))
            { serializer.Serialize(writer, target, ns); }
        }


        /* If the XML document has been altered with unknown nodes or attributes, handle them with the UnknownNode and UnknownAttribute events.
        serializer.UnknownNode += new XmlNodeEventHandler(serializer_UnknownNode);
        serializer.UnknownAttribute += new XmlAttributeEventHandler(serializer_UnknownAttribute);
            
        private void serializer_UnknownNode(object sender, XmlNodeEventArgs e)
        {
            Console.WriteLine("Unknown Node:" + e.Name + "\t" + e.Text);
        }

        private void serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            System.Xml.XmlAttribute attr = e.Attr;
            Console.WriteLine("Unknown attribute " +
            attr.Name + "='" + attr.Value + "'");
        }
        */

        public static T DeserializeFile<T>(string xmlFileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            // read the XML document and restore object state
            T item = (T)serializer.Deserialize(new FileStream(xmlFileName, FileMode.Open));
            return item;
        }

        public static T Deserialize<T>(string XmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(new StringReader(XmlString));
        }

        public static T Deserialize<T>(XDocument doc)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            // read the XML document and restore object state
            using (var reader = doc.CreateReader())
            { return (T)serializer.Deserialize(reader); }
        }

        public static XDocument Serialize<T>(T value)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

            //Add an empty namespace and empty value
            ns.Add("", "");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            XDocument doc = new XDocument();
            using (var writer = doc.CreateWriter())
            {
                xmlSerializer.Serialize(writer, value, ns);
            }

            return doc;
        }
    }
}

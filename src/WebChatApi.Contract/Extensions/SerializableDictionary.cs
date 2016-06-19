using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ifunction.WebChatApi
{

    /// <summary>
    /// Class for Serializable Dictionary, which is inherited from <see cref="Dictionary{T1,T2}"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    [XmlRoot("Dictionary")]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
    {
        /// <summary>
        /// The XML_ key
        /// </summary>
        const string xml_Key = "Key";

        /// <summary>
        /// The XML_ root
        /// </summary>
        const string xml_Root = "Dictionary";

        /// <summary>
        /// The XML_ value
        /// </summary>
        const string xml_Value = "Value";

        /// <summary>
        /// The XML_ item
        /// </summary>
        const string xml_Item = "Item";


        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableDictionary{TKey, TValue}"/> class.
        /// </summary>
        public SerializableDictionary()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableDictionary{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        public SerializableDictionary(IDictionary<TKey, TValue> dictionary)
            : base(dictionary)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableDictionary{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Generic.Dictionary`2" /> can contain.</param>
        public SerializableDictionary(int capacity)
            : base(capacity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableDictionary{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        public SerializableDictionary(IEqualityComparer<TKey> comparer)
            : base(comparer)
        {
        }

        /// <summary>
        /// This method is reserved and should not be used. When implementing the IXmlSerializable interface, you should return null (Nothing in Visual Basic) from this method, and instead, if specifying a custom schema is required, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute" /> to the class.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Xml.Schema.XmlSchema" /> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)" /> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)" /> method.
        /// </returns>
        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Reads the XML.
        /// </summary>
        /// <param name="xmlReader">The XML reader.</param>
        public void ReadXml(XmlReader xmlReader)
        {
            XmlSerializer keyXmlSerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueXmlSerializer = new XmlSerializer(typeof(TValue));

            if (xmlReader.IsEmptyElement)
                return;

            xmlReader.ReadStartElement(xml_Root);

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    xmlReader.ReadStartElement(xml_Item);
                    xmlReader.ReadStartElement(xml_Key);

                    TKey key = (TKey)keyXmlSerializer.Deserialize(xmlReader);

                    xmlReader.ReadEndElement();
                    xmlReader.ReadStartElement(xml_Value);
                    TValue value = (TValue)valueXmlSerializer.Deserialize(xmlReader);
                    xmlReader.ReadEndElement();

                    this.Add(key, value);
                    xmlReader.ReadEndElement();
                }
                else
                {
                    xmlReader.Read();
                }
            }

            xmlReader.ReadEndElement();
        }

        /// <summary>
        /// Reads from string.
        /// </summary>
        /// <param name="xmlString">The XML string.</param>
        /// <returns></returns>
        public void FromXmlString(string xmlString)
        {
            StringReader stringReader = new StringReader(xmlString);
            XmlTextReader xmlTextReader = new XmlTextReader(stringReader);

            ReadXml(xmlTextReader);

            xmlTextReader.Close();
            stringReader.Close();
        }

        /// <summary>
        /// Writes the XML.
        /// </summary>
        /// <param name="xmlWriter">The XML writer.</param>
        public void WriteXml(XmlWriter xmlWriter)
        {
            XmlSerializer keyXMLSerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueXMLSerializer = new XmlSerializer(typeof(TValue));

            foreach (TKey key in this.Keys)
            {
                xmlWriter.WriteStartElement(xml_Item);

                xmlWriter.WriteStartElement(xml_Key);
                keyXMLSerializer.Serialize(xmlWriter, key);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement(xml_Value);
                TValue value = this[key];
                valueXMLSerializer.Serialize(xmlWriter, value);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement();
            }
        }

        /// <summary>
        /// Serializes to string.
        /// </summary>
        /// <returns></returns>
        public string ToXmlString()
        {
            string result = null;
            MemoryStream memoryStream = new MemoryStream();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            xmlTextWriter.Namespaces = true;

            WriteXml(xmlTextWriter);

            xmlTextWriter.Close();
            memoryStream.Close();

            result = Encoding.UTF8.GetString(memoryStream.GetBuffer());
            int index = result.IndexOf(Convert.ToChar(60));
            if (index > -1)
            {
                result = result.Substring(index);
                result = result.Substring(0, (result.LastIndexOf(Convert.ToChar(62)) + 1));
            }
            else
            {
                result = result.Trim('\0');
            }
            result = "<" + xml_Root + ">" + result + "</" + xml_Root + ">";

            return result;
        }
        /// <summary>
        /// To the XML.
        /// </summary>
        /// <returns></returns>
        public XElement ToXml()
        {
            XDocument document = XDocument.Parse(ToXmlString());
            return document.Root;
        }

        /// <summary>
        /// Loads from XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        public void LoadFromXml(XElement xml)
        {
            if (xml != null)
            {
                FromXmlString(xml.ToString());
            }
        }

    }
}

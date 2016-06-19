using System;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace ifunction.WebChatApi.Contract
{
    /// <summary>
    /// Class ContentMessage.
    /// </summary>
    [DataContract]
    [KnownType(typeof(Message))]
    public class ContentMessage : Message
    {
        /// <summary>
        /// The node name_ content
        /// </summary>
        protected const string nodeName_Content = "Content";

        [DataMember]
        public string Content { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentMessage"/> class.
        /// </summary>
        /// <param name="content">The content.</param>
        public ContentMessage()
            : base(MessageType.Text)
        {
        }

        /// <summary>
        /// Fills the object data.
        /// </summary>
        /// <param name="xml">The XML.</param>
        protected override void FillObjectData(XElement xml)
        {
            base.FillObjectData(xml);

            this.Content = xml.GetValue(nodeName_Content);
        }

        /// <summary>
        /// Fills the XML data.
        /// </summary>
        /// <param name="xml">The XML.</param>
        protected override void FillXmlData(XElement xml)
        {
            base.FillXmlData(xml);

            xml.SetValue(nodeName_Content, this.Content);
        }
    }
}

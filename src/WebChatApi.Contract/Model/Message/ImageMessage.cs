using System;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace ifunction.WebChatApi.Contract
{
    /// <summary>
    /// Class ImageMessage.
    /// </summary>
    [DataContract]
    [KnownType(typeof(Message))]
    public class ImageMessage : Message
    {
        #region Constants

        /// <summary>
        /// The node name_ image URL
        /// </summary>
        protected const string nodeName_ImageUrl = "PicUrl";

        #endregion

        /// <summary>
        /// Gets or sets the image URL.
        /// </summary>
        /// <value>The image URL.</value>
        [DataMember]
        public string ImageUrl { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageMessage"/> class.
        /// </summary>
        public ImageMessage()
            : base(MessageType.Image)
        {
        }

        /// <summary>
        /// Fills the object data.
        /// </summary>
        /// <param name="xml">The XML.</param>
        protected override void FillObjectData(XElement xml)
        {
            base.FillObjectData(xml);

            this.ImageUrl =xml.GetValue( nodeName_ImageUrl);
        }

        /// <summary>
        /// Fills the XML data.
        /// </summary>
        /// <param name="xml">The XML.</param>
        protected override void FillXmlData(XElement xml)
        {
            base.FillXmlData(xml);

            xml.SetValue( nodeName_ImageUrl, this.ImageUrl);
        }
    }
}

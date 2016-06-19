using System;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace ifunction.WebChatApi.Contract
{
    /// <summary>
    /// Class LinkMessage.
    /// </summary>
    [DataContract]
    [KnownType(typeof(Message))]
    public class LinkMessage : Message
    {
        #region Constants

        /// <summary>
        /// The node name_ URL
        /// </summary>
        protected const string nodeName_Url = "Url";

        /// <summary>
        /// The node name_ title
        /// </summary>
        protected const string nodeName_Title = "Title";

        /// <summary>
        /// The node name_ description
        /// </summary>
        protected const string nodeName_Description = "Description";

        #endregion

        #region Property

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        [DataMember]
        public string Url { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkMessage" /> class.
        /// </summary>
        public LinkMessage()
            : base(MessageType.Link)
        {
        }

        #endregion

        /// <summary>
        /// Fills the object data.
        /// </summary>
        /// <param name="xml">The XML.</param>
        protected override void FillObjectData(XElement xml)
        {
            base.FillObjectData(xml);

            this.Url = xml.GetValue(nodeName_Url);
            this.Title = xml.GetValue(nodeName_Title);
            this.Description = xml.GetValue(nodeName_Description);
        }

        /// <summary>
        /// Fills the XML data.
        /// </summary>
        /// <param name="xml">The XML.</param>
        protected override void FillXmlData(XElement xml)
        {
            base.FillXmlData(xml);

            xml.SetValue(nodeName_Url, this.Url);
            xml.SetValue(nodeName_Title, this.Title);
            xml.SetValue(nodeName_Description, this.Description);
        }
    }
}

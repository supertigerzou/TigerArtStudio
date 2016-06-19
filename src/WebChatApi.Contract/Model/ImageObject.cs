using System;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace ifunction.WebChatApi.Contract
{
    /// <summary>
    /// Class ImageObject.
    /// </summary>
    [DataContract]
    public class ImageObject
    {
        #region Constants

        /// <summary>
        /// The node name_ title
        /// </summary>
        protected const string nodeName_Title = "Title";

        /// <summary>
        /// The node name_ description
        /// </summary>
        protected const string nodeName_Description = "Description";

        /// <summary>
        /// The node name_ music URL
        /// </summary>
        protected const string nodeName_ImageUrl = "PicUrl";

        /// <summary>
        /// The node name_ hq music URL
        /// </summary>
        protected const string nodeName_Url = "Url";

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
        /// Gets or sets the image URL.
        /// </summary>
        /// <value>The image URL.</value>
        [DataMember]
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        [DataMember]
        public string Url { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageObject"/> class.
        /// </summary>
        public ImageObject()
        {
        }

        /// <summary>
        /// Automatics the XML.
        /// </summary>
        /// <returns>XElement.</returns>
        public XElement ToXml()
        {
            XElement xml = this.CreateXmlNode();

            xml.SetValue(nodeName_Title, this.Title);
            xml.SetValue(nodeName_Description, this.Description);
            xml.SetValue(nodeName_ImageUrl, this.ImageUrl);
            xml.SetValue(nodeName_Url, this.Url);

            return xml;
        }
    }
}

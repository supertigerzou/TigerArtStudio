using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace ifunction.WebChatApi.Contract
{
    /// <summary>
    /// Class ImageObject.
    /// </summary>
    [DataContract]
    [KnownType(typeof(ImageObject))]
    public class ImageObjectCollection : List<ImageObject>
    {
        #region Constants

        /// <summary>
        /// The node name_ music
        /// </summary>
        public const string nodeName_Articles = "Articles";

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageObjectCollection"/> class.
        /// </summary>
        public ImageObjectCollection()
            : base()
        {
        }

        /// <summary>
        /// Automatics the XML.
        /// </summary>
        /// <returns>XElement.</returns>
        public XElement ToXml()
        {
            XElement xml = this.CreateXmlNode(nodeName_Articles);

            foreach (var one in this)
            {
                xml.Add(one.ToXml());
            }

            return xml;
        }
    }
}

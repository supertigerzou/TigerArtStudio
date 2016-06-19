using System;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace ifunction.WebChatApi.Contract
{
    /// <summary>
    /// Class MusicObject.
    /// </summary>
    [DataContract]
    public class MusicObject
    {
        #region Constants

        /// <summary>
        /// The node name_ music
        /// </summary>
        public const string nodeName_Music = "Music";

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
        protected const string nodeName_MusicUrl = "MusicUrl";

        /// <summary>
        /// The node name_ hq music URL
        /// </summary>
        protected const string nodeName_HQMusicUrl = "HQMusicUrl";

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
        /// Gets or sets the music URL.
        /// </summary>
        /// <value>The music URL.</value>
        [DataMember]
        public string MusicUrl { get; set; }

        /// <summary>
        /// Gets or sets the hq music URL.
        /// </summary>
        /// <value>The hq music URL.</value>
        [DataMember]
        public string HQMusicUrl { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="MusicObject"/> class.
        /// </summary>
        public MusicObject()
        {
        }

        /// <summary>
        /// Automatics the XML.
        /// </summary>
        /// <returns>XElement.</returns>
        public XElement ToXml()
        {
            XElement xml = this.CreateXmlNode(nodeName_Music);

            xml.SetValue(nodeName_Title, this.Title);
            xml.SetValue(nodeName_MusicUrl, this.MusicUrl);
            xml.SetValue(nodeName_Description, this.Description);
            xml.SetValue(nodeName_HQMusicUrl, this.HQMusicUrl);

            return xml;
        }

        /// <summary>
        /// Converts the automatic music object.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns>MusicObject.</returns>
        public static MusicObject ConvertToMusicObject(XElement xml)
        {
            MusicObject result = null;

            if (xml != null && xml.Name.LocalName == nodeName_Music)
            {
                result = new MusicObject();

                result.HQMusicUrl = xml.GetValue(nodeName_HQMusicUrl);
                result.MusicUrl = xml.GetValue(nodeName_MusicUrl);
                result.Title = xml.GetValue(nodeName_Title);
                result.Description = xml.GetValue(nodeName_Description);
            }

            return result;
        }
    }
}

using System;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace ifunction.WebChatApi.Contract
{
    /// <summary>
    /// Class MusicMessage.
    /// </summary>
    [DataContract]
    [KnownType(typeof(Message))]
    [KnownType(typeof(MusicObject))]
    public class MusicMessage : Message
    {
        #region Property

        /// <summary>
        /// Gets or sets the event.
        /// </summary>
        /// <value>The event.</value>
        [DataMember]
        public MusicObject Music { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MusicMessage" /> class.
        /// </summary>
        public MusicMessage()
            : base(MessageType.Music)
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

            this.Music = MusicObject.ConvertToMusicObject(xml.Element(MusicObject.nodeName_Music));
        }

        /// <summary>
        /// Fills the XML data.
        /// </summary>
        /// <param name="xml">The XML.</param>
        protected override void FillXmlData(XElement xml)
        {
            base.FillXmlData(xml);

            if (this.Music != null)
            {
                xml.Add(this.Music.ToXml());
            }
        }
    }
}

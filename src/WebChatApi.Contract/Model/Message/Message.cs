using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace ifunction.WebChatApi.Contract
{
    /// <summary>
    /// Class Message.
    /// </summary>
    [DataContract]
    [KnownType(typeof(MessageType))]
    public abstract class Message
    {
        #region Constants

        /// <summary>
        /// The node name_ message type
        /// </summary>
        protected const string nodeName_MessageType = "MsgType";

        /// <summary>
        /// The node name_ automatic user name
        /// </summary>
        protected const string nodeName_ToUserName = "ToUserName";

        /// <summary>
        /// The node name_ from user name
        /// </summary>
        protected const string nodeName_FromUserName = "FromUserName";

        /// <summary>
        /// The node name_ created stamp
        /// </summary>
        protected const string nodeName_CreatedStamp = "CreateTime";

        /// <summary>
        /// The node name_ message unique identifier
        /// </summary>
        protected const string nodeName_MessageId = "MsgId";

        /// <summary>
        /// The node name_ XML
        /// </summary>
        protected const string nodeName_Xml = "xml";

        #endregion

        #region Property

        /// <summary>
        /// Gets or sets the message unique identifier.
        /// </summary>
        /// <value>The message unique identifier.</value>
        [DataMember]
        public long MessageId { get; set; }

        /// <summary>
        /// Gets or sets the name of the automatic user.
        /// </summary>
        /// <value>The name of the automatic user.</value>
        [DataMember]
        public string ToUserName { get; set; }

        /// <summary>
        /// Gets or sets the name of from user.
        /// </summary>
        /// <value>The name of from user.</value>
        [DataMember]
        public string FromUserName { get; set; }

        /// <summary>
        /// Gets or sets the created stamp.
        /// </summary>
        /// <value>The created stamp.</value>
        [DataMember]
        public DateTime CreatedStamp { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        [DataMember]
        public MessageType Type { get; protected set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Message" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public Message(MessageType type)
        {
            this.CreatedStamp = DateTime.UtcNow;
        }

        #endregion

        /// <summary>
        /// Fills the object data.
        /// </summary>
        /// <param name="xml">The XML.</param>
        protected virtual void FillObjectData(XElement xml)
        {
            FillBasicInformation(this, xml);
        }

        /// <summary>
        /// Fills the XML data.
        /// </summary>
        /// <param name="xml">The XML.</param>
        protected virtual void FillXmlData(XElement xml)
        {
            //Do nothing 
        }


        /// <summary>
        /// Automatics the XML.
        /// </summary>
        /// <returns>XElement.</returns>
        public XElement ToXml()
        {
            XElement xml = this.CreateXmlNode(nodeName_Xml);

            xml.SetValue(nodeName_FromUserName, this.FromUserName);
            xml.SetValue(nodeName_ToUserName, this.ToUserName);
            if (this.MessageId > 0)
            {
                xml.SetValue(nodeName_MessageId, this.MessageId.ToString());
            }
            xml.SetValue(nodeName_CreatedStamp, this.CreatedStamp.ToMilliseconds().ToString(), true);
            xml.SetValue(nodeName_MessageType, this.Type.ToString().ToLowerInvariant());
            FillXmlData(xml);

            return xml;
        }

        #region Reply

        /// <summary>
        /// Replies the content.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns>ContentMessage.</returns>
        public ContentMessage ReplyContent(string content)
        {
            var result = new ContentMessage { Content = content };

            result.SetBasicReplyInformation(this);

            return result;

        }

        /// <summary>
        /// Replies the music.
        /// </summary>
        /// <param name="musicObject">The music object.</param>
        /// <returns>MusicMessage.</returns>
        public MusicMessage ReplyMusic(MusicObject musicObject)
        {
            var result = new MusicMessage { Music = musicObject };

            result.SetBasicReplyInformation(this);

            return result;
        }

        /// <summary>
        /// Replies the media.
        /// </summary>
        /// <param name="imageObjects">The image objects.</param>
        /// <returns>MediaMessage.</returns>
        public MediaMessage ReplyMedia(IEnumerable<ImageObject> imageObjects)
        {
            MediaMessage message = new MediaMessage();

            if (imageObjects != null)
            {
                foreach (var one in imageObjects)
                {
                    message.ImageCollection.Add(one);
                }
            }

            message.SetBasicReplyInformation(this);

            return message;
        }

        /// <summary>
        /// Sets the basic reply information.
        /// </summary>
        /// <param name="message">The message.</param>
        protected void SetBasicReplyInformation(Message message)
        {
            this.FromUserName = message.ToUserName;
            this.ToUserName = message.FromUserName;
            this.MessageId = 0;
        }

        #endregion

        #region Static methods

        /// <summary>
        /// Converts the message.
        /// </summary>
        /// <param name="xElement">The executable element.</param>
        /// <returns>Message.</returns>
        public static Message ConvertMessage(XElement xElement)
        {
            Message message = null;

            if (xElement != null && xElement.Name.LocalName == nodeName_Xml)
            {
                string type = xElement.GetValue(nodeName_MessageType);

                if (!string.IsNullOrWhiteSpace(type))
                {
                    MessageType messageType;
                    if (Enum.TryParse<MessageType>(type, true, out messageType))
                    {
                        switch (messageType)
                        {
                            case MessageType.Text:
                                message = new ContentMessage();
                                break;
                            case MessageType.Event:
                                message = new EventMessage();
                                break;
                            case MessageType.Location:
                                message = new GeographyMessage();
                                break;
                            default:
                                break;
                        }

                        if (message != null)
                        {
                            message.FillObjectData(xElement);
                        }
                    }
                }
            }

            return message;
        }

        /// <summary>
        /// Fills the basic information.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="xElement">The executable element.</param>
        protected static void FillBasicInformation(Message message, XElement xElement)
        {
            if (message != null && xElement != null)
            {
                message.FromUserName = xElement.GetValue(nodeName_FromUserName);
                message.ToUserName = xElement.GetValue(nodeName_ToUserName);
                message.MessageId = xElement.GetValue(nodeName_MessageId).DBToLong();
                message.CreatedStamp = xElement.GetValue(nodeName_CreatedStamp).DBToLong().ToDateTime();
            }
        }

        #endregion
    }
}

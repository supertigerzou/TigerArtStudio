using System;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace ifunction.WebChatApi.Contract
{
    /// <summary>
    /// Class EventMessage.
    /// </summary>
    [DataContract]
    [KnownType(typeof(Message))]
    public class EventMessage : Message
    {
        #region Constants

        /// <summary>
        /// The node name_ event
        /// </summary>
        protected const string nodeName_Event = "Event";

        /// <summary>
        /// The node name_ event key
        /// </summary>
        protected const string nodeName_EventKey = "EventKey";


        #endregion

        #region Property

        /// <summary>
        /// Gets or sets the event.
        /// </summary>
        /// <value>The event.</value>
        [DataMember]
        public string Event { get; set; }

        /// <summary>
        /// Gets or sets the event key.
        /// </summary>
        /// <value>The event key.</value>
        [DataMember]
        public string EventKey { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="EventMessage" /> class.
        /// </summary>
        public EventMessage()
            : base(MessageType.Event)
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

            this.Event =xml.GetValue( nodeName_Event);
            this.EventKey =xml.GetValue( nodeName_EventKey);
        }

        /// <summary>
        /// Fills the XML data.
        /// </summary>
        /// <param name="xml">The XML.</param>
        protected override void FillXmlData(XElement xml)
        {
            base.FillXmlData(xml);

            xml.SetValue( nodeName_Event, this.Event);
            xml.SetValue( nodeName_EventKey, this.EventKey);
        }
    }
}

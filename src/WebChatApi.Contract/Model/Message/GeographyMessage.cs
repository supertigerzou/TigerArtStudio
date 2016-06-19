using System;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace ifunction.WebChatApi.Contract
{
    /// <summary>
    /// Class GeographyMessage.
    /// </summary>
    [DataContract]
    [KnownType(typeof(Message))]
    public class GeographyMessage : Message
    {
        #region Constants

        /// <summary>
        /// The node name_ label
        /// </summary>
        protected const string nodeName_Label = "Label";

        /// <summary>
        /// The node name_ scale
        /// </summary>
        protected const string nodeName_Scale = "Scale";

        /// <summary>
        /// The node name_ latitude
        /// </summary>
        protected const string nodeName_Latitude = "Location_X";

        /// <summary>
        /// The node name_ longitude
        /// </summary>
        protected const string nodeName_Longitude = "Location_Y";

        #endregion

        #region Property

        /// <summary>
        /// Gets or sets the scale.
        /// </summary>
        /// <value>The scale.</value>
        [DataMember]
        public int Scale
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>The latitude.</value>
        [DataMember]
        public double Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>The longitude.</value>
        [DataMember]
        public double Longitude { get; set; }

        /// <summary>
        /// Gets or sets the image URL.
        /// </summary>
        /// <value>The image URL.</value>
        [DataMember]
        public string Label { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="GeographyMessage"/> class.
        /// </summary>
        public GeographyMessage()
            : base(MessageType.Location)
        {
        }

        /// <summary>
        /// Fills the object data.
        /// </summary>
        /// <param name="xml">The XML.</param>
        protected override void FillObjectData(XElement xml)
        {
            base.FillObjectData(xml);

            this.Label =xml.GetValue( nodeName_Label);
            this.Latitude =xml.GetValue( nodeName_Latitude).ToDouble();
            this.Longitude =xml.GetValue( nodeName_Longitude).ToDouble();
            this.Scale =xml.GetValue( nodeName_Scale).ToInt32();
        }

        /// <summary>
        /// Fills the XML data.
        /// </summary>
        /// <param name="xml">The XML.</param>
        protected override void FillXmlData(XElement xml)
        {
            base.FillXmlData(xml);

            xml.SetValue( nodeName_Label, this.Label);
            xml.SetValue( nodeName_Latitude, this.Latitude.ToString());
            xml.SetValue( nodeName_Longitude, this.Longitude.ToString());
            xml.SetValue( nodeName_Scale, this.Scale.ToString());

        }
    }
}

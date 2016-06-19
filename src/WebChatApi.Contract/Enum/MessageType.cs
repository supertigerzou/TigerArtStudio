using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ifunction.WebChatApi.Contract
{
    /// <summary>
    /// Enum MessageType
    /// </summary>
    [DataContract]
    public enum MessageType
    {
        /// <summary>
        /// The text
        /// </summary>
        [DataMember]
        Text = 0,
        /// <summary>
        /// The image
        /// </summary>
        [DataMember]
        Image,
        /// <summary>
        /// The location
        /// </summary>
        Location,
        /// <summary>
        /// The link
        /// </summary>
        [DataMember]
        Link,
        /// <summary>
        /// The event
        /// </summary>
        [DataMember]
        Event,
        /// <summary>
        /// The music
        /// </summary>
        [DataMember]
        Music = 10,
        /// <summary>
        /// The news
        /// </summary>
        [DataMember]
        News
    }
}

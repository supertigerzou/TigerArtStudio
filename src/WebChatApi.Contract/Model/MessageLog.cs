using System;
using System.Runtime.Serialization;
using WebChatApi.Contract.Interface;

namespace ifunction.WebChatApi.Contract
{
    /// <summary>
    /// Class MessageLog.
    /// </summary>
    [DataContract]
    public class MessageLog : IIdentifiable
    {
        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key.</value>
        [DataMember]
        public Guid Key { get; set; }

        /// <summary>
        /// Gets or sets the content of the original.
        /// </summary>
        /// <value>The content of the original.</value>
        [DataMember]
        public string OriginalContent { get; set; }

        /// <summary>
        /// Gets or sets the request message.
        /// </summary>
        /// <value>The message.</value>
        [DataMember]
        public Message RequestMessage { get; set; }

        /// <summary>
        /// Gets or sets the response message.
        /// </summary>
        /// <value>The response message.</value>
        [DataMember]
        public Message ResponseMessage { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        [DataMember]
        public DateTime CreatedStamp { get; set; }

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageLog" /> class.
        /// </summary>
        public MessageLog()
        {
            this.CreatedStamp = DateTime.UtcNow;
            this.Key = Guid.NewGuid();
        }

        #endregion
    }
}

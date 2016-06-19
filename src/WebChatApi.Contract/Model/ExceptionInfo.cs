using System.Runtime.Serialization;

namespace ifunction.WebChatApi.Contract
{
    /// <summary>
    /// Class for exception info.
    /// </summary>
    [DataContract]
    public class ExceptionInfo
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [DataMember]
        public int Code { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        [DataMember]
        public string Message { get; set; }

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionInfo"/> class.
        /// </summary>
        /// <param name="faultCode">The fault code.</param>
        /// <param name="message">The message.</param>
        public ExceptionInfo(BaseException.FaultCode faultCode, string message)
            : this()
        {
            Code = (int)faultCode;
            Message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionInfo"/> class.
        /// </summary>
        public ExceptionInfo()
        {
        }

        #endregion

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Code: {0}, Message: {1}", this.Code, this.Message);
        }
    }
}

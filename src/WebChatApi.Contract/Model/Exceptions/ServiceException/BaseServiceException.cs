using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ifunction.WebChatApi;
using ifunction.WebChatApi.Contract.Resources;



namespace ifunction.WebChatApi.Contract
{
    /// <summary>
    /// Model class for exception, which is inherited from the <seealso cref="System.Exception" /> class, and add <seealso cref="Gbi.BaseException.FaultCode" /> values.
    /// </summary>
    [DataContract]
    [KnownType(typeof(FaultCode))]
    public abstract class BaseServiceException : System.Exception
    {
        /// <summary>
        /// Fault code for error.
        /// </summary>
        [DataContract]
        public enum FaultCode
        {
            /// <summary>
            /// Value indicating it is an unknown error.
            /// </summary>
            [EnumMember]
            Unknown = 0,
            /// <summary>
            /// Value indicating it is an error when failed to operate in a method.
            /// </summary>
            [EnumMember]
            ServiceError = 102,
            /// <summary>
            /// Value indicating it is an error when data conflicts in a method.
            /// </summary>
            [EnumMember]
            DataConflictException = 103,
            /// <summary>
            /// Value indicating it is an error when object is invalid or null.
            /// </summary>
            [EnumMember]
            InvalidObject = 201,
            /// <summary>
            /// Value indicating it is an error when operation is unauthorized.
            /// </summary>
            [EnumMember]
            UnauthorizedOperation = 401,
            /// <summary>
            /// Value indicating it is an error when specific resource is not found.
            /// </summary>
            [EnumMember]
            ResourceNotFound = 404
        }

        #region Properties

        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        /// <value>
        /// The error code.
        /// </value>
        [DataMember]
        public FaultCode ErrorCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        [DataMember]
        public Guid Key
        {
            get;
            set;
        }

        #endregion

        #region Constructor


        /// <summary>
        /// Initializes a new instance of the <see cref="BaseServiceException"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        public BaseServiceException(FaultCode code)
            : base(GetMessage(code))
        {
            this.ErrorCode = code;
            this.Key = Guid.NewGuid();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseServiceException"/> class.
        /// </summary>
        public BaseServiceException()
            : base()
        {
            this.ErrorCode = FaultCode.Unknown;
            this.Key = Guid.NewGuid();
        }

        #endregion


        /// <summary>
        /// To the exception info.
        /// </summary>
        /// <returns></returns>
        public ExceptionInfo ToExceptionInfo()
        {
            ExceptionInfo info = new ExceptionInfo();

            info.Code = (int)this.ErrorCode;
            info.Message = this.Message;

            return info;
        }

        #region Resource protected methods.

        /// <summary>
        /// Gets the resource manager.
        /// </summary>
        /// <value>
        /// The resource manager.
        /// </value>
        protected static ResourceManager ResourceManager
        {
            get
            {
                return OuterExceptionMessages.ResourceManager;
            }
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <param name="faultCode">The fault code.</param>
        /// <returns></returns>
        protected static string GetMessage(FaultCode faultCode)
        {
            return ResourceManager.GetString("E" + ((int)faultCode).ToString());
        }

        #endregion
    }
}

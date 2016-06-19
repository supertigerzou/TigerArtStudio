using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using ifunction.WebChatApi.Contract.Resources;



/// <summary>
/// The Gbi namespace.
/// </summary>
namespace ifunction.WebChatApi.Contract
{
    /// <summary>
    /// Model class for exception, which is inherited from the <seealso cref="System.Exception" /> class, and add <seealso cref="System.BaseException.FaultCode" /> values
    /// and  Reference data of <seealso cref="System.Object" /> class .
    /// </summary>
    public class BaseException : System.Exception
    {
        /// <summary>
        /// Fault code for error.
        /// </summary>
        public enum FaultCode
        {
            /// <summary>
            /// Value indicating it is an unknown error.
            /// </summary>
            Unknown = 0,
            /// <summary>
            /// Value indicating it is a Thread about message.
            /// </summary>
            ThreadAbortMessage = 1,
            #region Basic
            /// <summary>
            /// Value indicating it is an error when failed to initialize object or service.
            /// </summary>
            InitializationFailureException = 101,
            /// <summary>
            /// Value indicating it is an error when failed to operate in a method.
            /// </summary>
            OperationFailureException = 102,
            /// <summary>
            /// Value indicating it is an error when data conflicts in a method.
            /// </summary>
            DataConflictException = 103,
            /// <summary>
            /// Value indicating it is an error when failed to operation in HTTP methods.
            /// </summary>
            HttpOperationException = 110,
            /// <summary>
            /// Value indicating it is an error when calling remote service.
            /// </summary>
            RemoteServiceOperationFailureException = 120,
            #endregion
            /// <summary>
            /// Value indicating it is an error when object or parameter is null.
            /// </summary>
            NullObjectOrParameter = 201,
            /// <summary>
            /// Value indicating it is an error when object is invalid value of format.
            /// </summary>
            InvalidFormatOrValue = 202,
            //---------------------------------------
            /// <summary>
            /// Value indicating it is an error when operation is unauthorized.
            /// </summary>
            UnauthorizedOperation = 401,
            //---------------------------------------
            /// <summary>
            /// Value indicating it is an error when specific resource is not found.
            /// </summary>
            ResourceNotFound = 404
        }

        #region Properties

        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        /// <value>The error code.</value>
        public FaultCode ErrorCode
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public Guid Key
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the reference data.
        /// </summary>
        /// <value>The reference data.</value>
        public object ReferenceData
        {
            get;
            set;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseException" /> class.
        /// </summary>
        /// <param name="exceptionMessage">The exception message.</param>
        public BaseException(string exceptionMessage)
            : this(exceptionMessage, FaultCode.Unknown)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseException" /> class.
        /// </summary>
        /// <param name="exceptionMessage">The exception message.</param>
        /// <param name="faultCode">The fault code.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="data">The data.</param>
        public BaseException(string exceptionMessage, FaultCode faultCode, Exception innerException = null, object data = null)
            : base(exceptionMessage, innerException)
        {
            this.ErrorCode = faultCode;
            this.Key = Guid.NewGuid();
            this.ReferenceData = data;
        }

        #endregion

        /// <summary>
        /// To the service exception.
        /// </summary>
        /// <returns>BaseServiceException.</returns>
        public BaseServiceException ToServiceException()
        {
            BaseServiceException result = null;

            switch (this.ErrorCode)
            {
                case FaultCode.ThreadAbortMessage:
                case FaultCode.OperationFailureException:
                case FaultCode.InitializationFailureException:
                    result = new ServiceErrorException();
                    break;
                case FaultCode.InvalidFormatOrValue:
                case FaultCode.NullObjectOrParameter:
                    result = new InvalidObjectServiceException();
                    break;
                case FaultCode.ResourceNotFound:
                    result = new ResourceNotFoundServiceException();
                    break;
                case FaultCode.UnauthorizedOperation:
                    result = new UnauthorizedOperationServiceException();
                    break;
                case FaultCode.DataConflictException:
                    result = new DataConflictServiceException();
                    break;
                case FaultCode.Unknown:
                default:
                    result = new UnknownServiceException();
                    break;

            }

            return result;
        }

        #region Resource protected methods.

        /// <summary>
        /// Gets the resource manager.
        /// </summary>
        /// <value>The resource manager.</value>
        protected static ResourceManager ResourceManager
        {
            get
            {
                return InnerExceptionMessages.ResourceManager;
            }
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <param name="faultCode">The fault code.</param>
        /// <returns>System.String.</returns>
        protected static string GetMessage(FaultCode faultCode)
        {
            return ResourceManager.GetString("E" + ((int)faultCode).ToString());
        }

        /// <summary>
        /// Gets the full message.
        /// </summary>
        /// <param name="faultCode">The fault code.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns>System.String.</returns>
        protected static string GetFullMessage(FaultCode faultCode, params string[] arguments)
        {
            return string.Format(GetMessage(faultCode), arguments);
        }

        #endregion
    }
}

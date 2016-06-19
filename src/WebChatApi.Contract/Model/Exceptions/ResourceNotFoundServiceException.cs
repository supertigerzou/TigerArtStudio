using System;

namespace ifunction.WebChatApi.Contract
{
    /// <summary>
    /// Model class for service exception, which is inherited from the <seealso cref="BaseException" /> class.
    /// </summary>
    public class ResourceNotFoundException : BaseException
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceNotFoundException" /> class.
        /// </summary>
        /// <param name="scopeIdentity">The scope identity.</param>
        /// <param name="objectIdentity">The object identity.</param>
        /// <param name="innerException">The inner exception.</param>
        public ResourceNotFoundException(string scopeIdentity, string objectIdentity, Exception innerException = null)
            : base(GetFullMessage(FaultCode.ResourceNotFound, scopeIdentity, objectIdentity), FaultCode.OperationFailureException, innerException, null)
        {
        }

        #endregion
    }
}

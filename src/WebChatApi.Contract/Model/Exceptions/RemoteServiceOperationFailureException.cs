using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// The Gbi namespace.
/// </summary>
namespace ifunction.WebChatApi.Contract
{
    /// <summary>
    /// Class RemoteServiceOperationFailureException.
    /// </summary>
    public class RemoteServiceOperationFailureException : BaseException
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationFailureException" /> class.
        /// </summary>
        /// <param name="objectIdentity">The object identity.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="data">The data.</param>
        public RemoteServiceOperationFailureException(string objectIdentity, Exception innerException = null, object data = null)
            : base(GetFullMessage(FaultCode.RemoteServiceOperationFailureException, objectIdentity), FaultCode.RemoteServiceOperationFailureException, innerException, data)
        {
        }

        #endregion
    }
}

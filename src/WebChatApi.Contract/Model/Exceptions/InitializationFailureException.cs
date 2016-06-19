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
    /// Exception class for Initialization Failure
    /// </summary>
    public class InitializationFailureException : BaseException
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="InitializationFailureException" /> class.
        /// </summary>
        /// <param name="objectIdentity">The object identity.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="data">The data.</param>
        public InitializationFailureException(string objectIdentity, Exception innerException = null, object data = null)
            : base(GetFullMessage(FaultCode.InitializationFailureException, objectIdentity), FaultCode.InitializationFailureException, innerException, data)
        {
        }

        #endregion
    }
}

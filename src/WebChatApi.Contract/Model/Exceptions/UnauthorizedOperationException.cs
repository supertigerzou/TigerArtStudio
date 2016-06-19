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
    /// Class for unauthorized operation exception
    /// </summary>
    public class UnauthorizedOperationException : BaseException
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UnauthorizedOperationException" /> class.
        /// </summary>
        /// <param name="operationIdentity">The operation identity.</param>
        /// <param name="operatorIdentity">The operator identity.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="data">The data.</param>
        public UnauthorizedOperationException(string operationIdentity, string operatorIdentity, Exception innerException = null, object data = null)
            : base(GetFullMessage(FaultCode.UnauthorizedOperation, operationIdentity, operatorIdentity), FaultCode.UnauthorizedOperation, innerException, data)
        {
        }

        #endregion
    }
}

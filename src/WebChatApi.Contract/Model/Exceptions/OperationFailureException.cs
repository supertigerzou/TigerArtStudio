using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ifunction.WebChatApi.Contract
{
    /// <summary>
    /// Class OperationFailureException.
    /// </summary>
    public class OperationFailureException : BaseException
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationFailureException" /> class.
        /// </summary>
        /// <param name="objectIdentity">The object identity.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="data">The data.</param>
        public OperationFailureException(string objectIdentity, Exception innerException = null, object data = null)
            : base(GetFullMessage(FaultCode.OperationFailureException, objectIdentity), FaultCode.OperationFailureException, innerException, data)
        {
        }

        #endregion
    }
}

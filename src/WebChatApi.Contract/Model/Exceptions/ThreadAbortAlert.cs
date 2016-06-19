using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// The Gbi namespace.
/// </summary>
namespace ifunction.WebChatApi.Contract
{
    /// <summary>
    /// Class for thread abort alert, which is based on <see cref="ThreadAbortException" /> class object.
    /// </summary>
    public class ThreadAbortAlert : BaseException
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadAbortAlert" /> class.
        /// </summary>
        /// <param name="operationIdentity">The operation identity.</param>
        /// <param name="data">The data.</param>
        public ThreadAbortAlert(string operationIdentity, ThreadAbortException data = null)
            : base(GetFullMessage(FaultCode.ThreadAbortMessage, operationIdentity, Thread.CurrentThread.ManagedThreadId.ToString()), FaultCode.UnauthorizedOperation, data, null)
        {
        }

        #endregion
    }
}

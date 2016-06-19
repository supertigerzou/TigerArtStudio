using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ifunction.WebChatApi.Contract
{
    /// <summary>
    /// Exception class for data conflicts
    /// </summary>
    public class DataConflictException : BaseException
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DataConflictException" /> class.
        /// </summary>
        /// <param name="scopeIdentity">The scope identity.</param>
        /// <param name="objectIdentity">The object identity.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="data">The data.</param>
        public DataConflictException(string scopeIdentity, string objectIdentity, Exception innerException = null, object data = null)
            : base(GetFullMessage(FaultCode.DataConflictException, scopeIdentity, objectIdentity), FaultCode.DataConflictException, innerException, data)
        {
        }

        #endregion
    }
}

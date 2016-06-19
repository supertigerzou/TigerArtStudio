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
    /// Exception Class for Null object.
    /// </summary>
    public class NullObjectException : BaseException
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="NullObjectException" /> class.
        /// </summary>
        /// <param name="objectIdentity">The object identity.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="data">The data.</param>
        public NullObjectException(string objectIdentity, Exception innerException = null, object data = null)
            : base(GetFullMessage(FaultCode.InvalidFormatOrValue, objectIdentity), FaultCode.InvalidFormatOrValue, innerException, data)
        {
        }

        #endregion
    }
}

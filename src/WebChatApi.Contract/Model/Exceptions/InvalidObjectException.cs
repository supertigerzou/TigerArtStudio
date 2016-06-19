using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ifunction.WebChatApi.Contract
{
    /// <summary>
    /// Exception for object validation, which is occurred when object is in invalid format or value.
    /// In case of null object, please use <see cref="NullObjectException" /> class instead.
    /// </summary>
    public class InvalidObjectException : BaseException
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidObjectException" /> class.
        /// </summary>
        /// <param name="objectIdentity">The object identity.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="data">The data.</param>
        public InvalidObjectException(string objectIdentity, Exception innerException = null, object data = null)
            : base(GetFullMessage(FaultCode.InvalidFormatOrValue, objectIdentity), FaultCode.InvalidFormatOrValue, innerException, data)
        {
        }

        #endregion
    }
}

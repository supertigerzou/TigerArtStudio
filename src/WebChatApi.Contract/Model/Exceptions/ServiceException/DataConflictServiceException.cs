using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ifunction.WebChatApi.Contract
{
    /// <summary>
    /// 
    /// </summary>
    public class DataConflictServiceException : BaseServiceException
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DataConflictException" /> class.
        /// </summary>
        public DataConflictServiceException()
            : base(FaultCode.DataConflictException)
        {
        }

        #endregion
    }
}

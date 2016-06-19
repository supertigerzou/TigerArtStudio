using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;


using System.Net;

/// <summary>
/// The Gbi namespace.
/// </summary>
namespace ifunction.WebChatApi.Contract
{
    /// <summary>
    /// Class HttpOperationException.
    /// </summary>
    public class HttpOperationException : BaseException
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DataConflictException" /> class.
        /// </summary>
        /// <param name="targetUrl">The target URL.</param>
        /// <param name="status">The status.</param>
        public HttpOperationException(string targetUrl, HttpStatusCode status)
            : base(GetFullMessage(FaultCode.HttpOperationException, targetUrl, status.ToString(), ((int)status).ToString()), FaultCode.HttpOperationException, null, null)
        {
        }

        #endregion
    }
}

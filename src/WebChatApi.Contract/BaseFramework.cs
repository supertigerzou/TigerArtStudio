using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ifunction.WebChatApi.Contract;

namespace ifunction.WebChatApi
{
    /// <summary>
    /// Abstract singleton class for framework.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseFramework<T> : Singleton<T> where T : BaseFramework<T>, new()
    {
        #region Properties

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <value>The logger.</value>
        public Logger Logger
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the application identity.
        /// </summary>
        /// <value>The application identity.</value>
        public string ApplicationIdentity
        {
            get;
            protected set;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseFramework{T}" /> class.
        /// </summary>
        public BaseFramework()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseFramework{T}" /> class.
        /// </summary>
        /// <param name="applicationIdentity">The application identity.</param>
        protected BaseFramework(string applicationIdentity)
        {
            this.ApplicationIdentity = applicationIdentity;
            InitializeLogger();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();

            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fvi.FileVersion;
        }

        #endregion

        #region Initialize methods

        /// <summary>
        /// Initializes the logger.
        /// </summary>
        /// <exception cref="InitializationFailureException">Logger</exception>
        protected virtual void InitializeLogger()
        {
            try
            {
                this.Logger = new Logger(this.ApplicationIdentity, true, false, this.ApplicationIdentity);
            }
            catch (Exception ex)
            {
                throw new InitializationFailureException("Logger", ex);
            }
        }


        #endregion

        /// <summary>
        /// Gets the request log.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <returns>System.String.</returns>
        public static string GetRequestLog(HttpRequest httpRequest)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (httpRequest != null)
            {
                stringBuilder.AppendLine("Address: [" + httpRequest.Url.ToString() + "]");
                stringBuilder.AppendLine("Refer:" + (httpRequest.UrlReferrer == null ? "<null>" : httpRequest.UrlReferrer.ToString()));
                stringBuilder.AppendLine("User Address: [" + httpRequest.UserHostAddress + "]");

                byte[] buffer = new byte[httpRequest.InputStream.Length];
                httpRequest.InputStream.Read(buffer, 0, buffer.Length);
                httpRequest.InputStream.Position = 0;
                string soapMessage = Encoding.ASCII.GetString(buffer);

                stringBuilder.AppendLine("Data: ");
                stringBuilder.AppendLine(soapMessage.GetStringValue());
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="operationIdentity">The operation identity.</param>
        /// <param name="operatorIdentity">The operator identity.</param>
        /// <param name="data">The data.</param>
        /// <returns>BaseServiceException.</returns>
        public BaseServiceException HandleException(BaseException exception, string operationIdentity, string operatorIdentity = null, object data = null)
        {
            if (exception != null)
            {
                BaseException baseException = null;

                if (exception.ErrorCode == BaseException.FaultCode.UnauthorizedOperation)
                {
                    baseException = new UnauthorizedOperationException(operationIdentity.GetStringValue(), operatorIdentity.GetStringValue(), exception, data);
                }
                else
                {
                    baseException = new OperationFailureException(operationIdentity, exception, data);
                }

                Logger.LogException(baseException);
                return baseException.ToServiceException();
            }
            else
            {
                return new UnknownServiceException();
            }
        }
    }
}

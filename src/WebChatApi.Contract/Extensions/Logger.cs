using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ifunction.WebChatApi.Contract
{
    /// <summary>
    /// Class Logger. This class cannot be inherited.
    /// </summary>
    public sealed class Logger : IDisposable
    {
        #region Fields

        /// <summary>
        /// The daily mode locker
        /// </summary>
        private object dailyModeLocker = new object();
        /// <summary>
        /// The log locker
        /// </summary>
        private object logLocker = new object();

        /// <summary>
        /// The event log
        /// </summary>
        private EventLog eventLog;

        /// <summary>
        /// The log file writer
        /// </summary>
        private StreamWriter logFileWriter;

        /// <summary>
        /// The using daily mode
        /// </summary>
        private bool usingDailyMode;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether [use event log].
        /// </summary>
        /// <value><c>true</c> if [use event log]; otherwise, <c>false</c>.</value>
        public bool UseEventLog { get; private set; }

        /// <summary>
        /// Gets the name of the application.
        /// </summary>
        /// <value>The name of the application.</value>
        public string ApplicationName { get; private set; }

        /// <summary>
        /// Gets the log file folder.
        /// </summary>
        /// <value>The log file folder.</value>
        public string LogFileFolder { get; private set; }

        /// <summary>
        /// Gets the log file path.
        /// </summary>
        /// <value>The log file path.</value>
        public string LogFilePath { get; private set; }

        /// <summary>
        /// Gets the name of the source.
        /// </summary>
        /// <value>The name of the source.</value>
        public string SourceName { get; private set; }

        /// <summary>
        /// Gets the success count.
        /// </summary>
        /// <value>The success count.</value>
        public int SuccessCount { get; private set; }

        /// <summary>
        /// Gets the error count.
        /// </summary>
        /// <value>The error count.</value>
        public int ErrorCount { get; private set; }

        /// <summary>
        /// Gets the current dictionary.
        /// </summary>
        /// <value>The current dictionary.</value>
        private string CurrentDictionary
        {
            get
            {
                return Assembly.GetExecutingAssembly().Location;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger" /> class.
        /// </summary>
        /// <param name="applicationName">Name of the application.</param>
        /// <param name="usingDailyMode">if set to <c>true</c> [using daily mode].</param>
        /// <param name="useEventLog">if set to <c>true</c> [use event log].</param>
        /// <param name="sourceName">Name of the source.</param>
        public Logger(string applicationName, bool usingDailyMode, bool useEventLog, string sourceName)
            : this(applicationName, null, usingDailyMode, useEventLog, sourceName)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Logger" /> class.
        /// </summary>
        /// <param name="applicationName">Name of the application.</param>
        /// <param name="logFileFolder">The log file folder.</param>
        /// <param name="usingDailyMode">if set to <c>true</c> [using daily mode].</param>
        /// <param name="useEventLog">if set to <c>true</c> [use event log].</param>
        /// <param name="sourceName">Name of the source.</param>
        public Logger(string applicationName, string logFileFolder, bool usingDailyMode, bool useEventLog, string sourceName)
        {
            this.ApplicationName = applicationName;
            if (string.IsNullOrWhiteSpace(logFileFolder))
            {
                logFileFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            }
            this.LogFileFolder = logFileFolder;

            this.UseEventLog = useEventLog;
            this.SourceName = sourceName;

            this.usingDailyMode = usingDailyMode;
            Initialize();
        }

        #endregion

        /// <summary>
        /// Increments the success count.
        /// </summary>
        /// <param name="count">The count.</param>
        public void IncrementSuccessCount(int count)
        {
            SuccessCount += count;
        }

        /// <summary>
        /// Increments the error count.
        /// </summary>
        /// <param name="count">The count.</param>
        public void IncrementErrorCount(int count)
        {
            ErrorCount += count;
        }

        /// <summary>
        /// Logs the message with timestamp.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        public void LogMessageWithTimestamp(string msg)
        {
            if (!string.IsNullOrWhiteSpace(msg))
            {
                WriteContent(string.Format(CultureInfo.InvariantCulture, "[{0}] {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), msg));
            }
        }

        /// <summary>
        /// Logs the blank line.
        /// </summary>
        public void LogBlankLine()
        {
            WriteContent(string.Empty);
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        public void LogError(string msg)
        {
            ErrorCount++;
            LogBlankLine();
            WriteContent("************************");
            WriteContent("***      ERROR       ***");
            WriteContent(msg);
            WriteContent("************************");
            LogBlankLine();
        }

        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="data">The data.</param>
        /// <param name="code">The code.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public void LogException(Exception ex, object data = null, int code = 0)
        {
            LogException(string.Empty, ex, data, code);
        }

        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="data">The data.</param>
        /// <param name="code">The code.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public void LogException(string msg, Exception ex, object data = null, int code = 0)
        {
            const string format = "\r\n ********** Log (Thread:#{0}; Stamp:{1}) **********\r\nCode:{2}. Message:{3}\r\n{4}";
            var nowStampString = DateTime.Now.ToLogStampString();
            string threadId = Thread.CurrentThread.ManagedThreadId.ToString("00000");
            LogError(String.Format(CultureInfo.InvariantCulture, format,
                threadId,
                nowStampString,
                code,
                msg.GetStringValue(), FormatException(ex, data)));
            if (UseEventLog)
            {
                LogEvent(EventLogEntryType.Error, msg, ex, data, code);
            }
        }

        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public void LogException(BaseException ex)
        {
            const string format = "\r\n ********** Log (Thread:#{0}; Stamp:{1}) **********\r\nCode:{2}. Key:{3}\r\n{4}";
            var nowStampString = DateTime.Now.ToLogStampString();
            string threadId = Thread.CurrentThread.ManagedThreadId.ToString("00000");
            LogError(String.Format(CultureInfo.InvariantCulture, format,
                threadId,
                nowStampString,
                (int)ex.ErrorCode,
                ex.Key.ToString(), FormatException(ex)));
            if (UseEventLog)
            {
                LogEvent(EventLogEntryType.Error, ex.Message, ex, ex.ReferenceData, (int)ex.ErrorCode);
            }
        }

        /// <summary>
        /// Logs the event.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="data">The data.</param>
        /// <param name="code">The code.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public void LogEvent(EventLogEntryType type, Exception ex, object data = null, int code = 0)
        {
            LogEvent(type, string.Empty, ex, data, code);
        }

        /// <summary>
        /// Logs the event.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="message">The message.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public void LogEvent(EventLogEntryType type, string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                eventLog.WriteEntry(message, type);
            }
        }

        /// <summary>
        /// Logs the event.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="msg">The MSG.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="data">The data.</param>
        /// <param name="code">The code.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public void LogEvent(EventLogEntryType type, string msg, Exception ex, object data = null, int code = 0)
        {
            if (UseEventLog)
            {
                try
                {
                    string fullMsg = String.Concat(msg, ex != null ? String.Concat("\r\n\r\n", FormatException(ex, data)) : "");
                    eventLog.WriteEntry(fullMsg, type, code);
                }
                catch
                {
                    //unable to log event to event log so there is nothing else that we can do to log this error 
                    //so we will just continue
                }
            }
        }

        #region Private methods

        /// <summary>
        /// Initialize for logger.
        /// </summary>
        private void Initialize()
        {
            if (string.IsNullOrWhiteSpace(SourceName))
            {
                SourceName = AppDomain.CurrentDomain.ApplicationIdentity.FullName;
                if (string.IsNullOrWhiteSpace(SourceName))
                {
                    SourceName = AppDomain.CurrentDomain.FriendlyName;
                }
            }
            if (string.IsNullOrWhiteSpace(ApplicationName))
            {
                ApplicationName = SourceName;
            }
            #region changed
            ////// Remove if statement for UseEventLog.
            ////// Even it is no need to log in file as well as in Event Log, the method of log in Event Log can be called standalone.
            //if (!EventLog.SourceExists(SourceName))
            //{
            //    EventLog.CreateEventSource(SourceName, ApplicationName);
            //    eventLog = new EventLog(ApplicationName);
            //    eventLog.Source = SourceName;

            //    eventLog.WriteEntry("Event log created", EventLogEntryType.Information);
            //}
            //else
            //{
            // eventLog = new EventLog();
            // eventLog.Source = SourceName;
            //}
            eventLog = new EventLog();
            eventLog.Source = SourceName;
            #endregion

            if (usingDailyMode)
            {
                LogFilePath = GenerateDailyFilePath();
            }
            else if (string.IsNullOrWhiteSpace(LogFileFolder))
            {
                LogFilePath = Path.Combine(CurrentDictionary, ApplicationName);
            }
            else
            {
                LogFilePath = Path.Combine(LogFileFolder, ApplicationName);
            }

            if (!Directory.Exists(LogFileFolder))
            {
                Directory.CreateDirectory(LogFileFolder);
            }

            if (File.Exists(LogFilePath))
            {
                logFileWriter = new StreamWriter(LogFilePath, true);
            }
            else
            {
                logFileWriter = new StreamWriter(LogFilePath);
            }
        }

        /// <summary>
        /// Core method for writing log content.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        private void WriteContent(string msg)
        {
            CheckDailyModeFilePath();

            lock (logLocker)
            {
                try
                {
                    logFileWriter.WriteLine(msg);
                    logFileWriter.Flush();
                }
                catch (Exception ex)
                {
                    if (UseEventLog)
                    {
                        LogEvent(EventLogEntryType.Error, "Error writing to log file", ex);
                    }
                }
            }
        }

        /// <summary>
        /// Checks the daily mode file path.
        /// </summary>
        private void CheckDailyModeFilePath()
        {
            if (usingDailyMode)
            {
                string newFilePath = GenerateDailyFilePath();
                if (!newFilePath.Equals(LogFilePath, StringComparison.InvariantCultureIgnoreCase))
                {
                    lock (dailyModeLocker)
                    {
                        if (!newFilePath.Equals(LogFilePath, StringComparison.InvariantCultureIgnoreCase))
                        {
                            logFileWriter.Close();
                            LogFilePath = newFilePath;
                            if (File.Exists(LogFilePath))
                            {
                                logFileWriter = new StreamWriter(LogFilePath, true);
                            }
                            else
                            {
                                logFileWriter = new StreamWriter(LogFilePath);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Generates the daily file path.
        /// </summary>
        /// <returns>System.String.</returns>
        private string GenerateDailyFilePath()
        {
            return GenerateDailyFilePath(DateTime.Now);
        }

        /// <summary>
        /// Generates the daily file path.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>System.String.</returns>
        public string GenerateDailyFilePath(DateTime dateTime)
        {
            return Path.Combine(LogFileFolder, this.ApplicationName + "_" + dateTime.ToString("yyyyMMdd") + ".log");
        }

        /// <summary>
        /// Closes the log file.
        /// </summary>
        private void CloseLogFile()
        {
            try
            {
                if (logFileWriter != null)
                {
                    logFileWriter.Flush();
                    logFileWriter.Close();
                }
            }
            catch (Exception ex)
            {
                if (this.UseEventLog)
                {
                    LogEvent(EventLogEntryType.Error, "Unable to close the log file", ex);
                }
            }
        }

        /// <summary>
        /// Formats the exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="obj">The obj.</param>
        /// <returns>System.String.</returns>
        private static string FormatException(Exception ex, object obj = null)
        {
            StringBuilder exDetails = new StringBuilder();

            exDetails.AppendFormat(CultureInfo.InvariantCulture, "Exception Type: {0}\r\n", ex.GetType().ToString());

            SqlException sqlEx = ex as SqlException;
            if (sqlEx != null)
            {
                int i = 0;

                exDetails.AppendFormat(CultureInfo.InvariantCulture, "\r\n Errors.Count = {0}", sqlEx.Errors.Count);
                foreach (SqlError sqlE in sqlEx.Errors)
                {
                    i++;
                    exDetails.AppendFormat(CultureInfo.InvariantCulture, "\r\n ********** Error #{0} **********", i);
                    exDetails.AppendFormat(CultureInfo.InvariantCulture, "\r\n ->Class: {0}", sqlE.Class);
                    exDetails.AppendFormat(CultureInfo.InvariantCulture, "\r\n ->Number: {0}", sqlE.Number);
                    exDetails.AppendFormat(CultureInfo.InvariantCulture, "\r\n ->Server: {0}", sqlE.Server);
                    exDetails.AppendFormat(CultureInfo.InvariantCulture, "\r\n ->Source: {0}", sqlE.Source);
                    exDetails.AppendFormat(CultureInfo.InvariantCulture, "\r\n ->Procedure: {0}", sqlE.Procedure);
                    exDetails.AppendFormat(CultureInfo.InvariantCulture, "\r\n ->LineNumber: {0}", sqlE.LineNumber);
                    exDetails.AppendFormat(CultureInfo.InvariantCulture, "\r\n ->State: {0}", sqlE.State);
                    exDetails.AppendFormat(CultureInfo.InvariantCulture, "\r\n ->Message: {0}", sqlE.Message);
                }
            }

            exDetails.AppendFormat(CultureInfo.InvariantCulture, "\r\nException Message: {0}", ex.Message);
            exDetails.AppendFormat(CultureInfo.InvariantCulture, "\r\n\r\nSource: {0}", ex.Source);
            exDetails.AppendFormat(CultureInfo.InvariantCulture, "\r\nSite: {0}", ex.TargetSite);
            exDetails.AppendFormat(CultureInfo.InvariantCulture, "\r\nStackTrace: {0}\r\n", ex.StackTrace);
            exDetails.AppendFormat(CultureInfo.InvariantCulture, "\r\nData Reference: {0}\r\n", GenerateDataString(obj));

            if (ex.InnerException != null)
                exDetails.AppendFormat(CultureInfo.InvariantCulture, "\r\n***** Inner Exception *****\r\n{0}", FormatException(ex.InnerException));

            return exDetails.ToString();
        }


        /// <summary>
        /// Formats the exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns>System.String.</returns>
        private static string FormatException(BaseException ex)
        {
            StringBuilder exDetails = new StringBuilder();
            exDetails.AppendFormat(CultureInfo.InvariantCulture, "Exception Type: {0}\r\n", ex.GetType().ToString());

            exDetails.AppendFormat(CultureInfo.InvariantCulture, "\r\nException Message: {0}", ex.Message);
            exDetails.AppendFormat(CultureInfo.InvariantCulture, "\r\n\r\nSource: {0}", ex.Source);
            exDetails.AppendFormat(CultureInfo.InvariantCulture, "\r\nSite: {0}", ex.TargetSite);
            exDetails.AppendFormat(CultureInfo.InvariantCulture, "\r\nStackTrace: {0}\r\n", ex.StackTrace);
            exDetails.AppendFormat(CultureInfo.InvariantCulture, "\r\nData Reference: {0}\r\n", GenerateDataString(ex.ReferenceData));

            if (ex.InnerException != null)
            {
                BaseException innerException = ex.InnerException as BaseException;
                if (innerException != null)
                {
                    exDetails.AppendFormat(CultureInfo.InvariantCulture, "\r\n***** Inner Exception *****\r\n{0}", FormatException(innerException));
                }
                else
                {
                    exDetails.AppendFormat(CultureInfo.InvariantCulture, "\r\n***** Inner Exception *****\r\n{0}", FormatException(ex.InnerException));
                }
            }

            return exDetails.ToString();
        }

        /// <summary>
        /// Generates the data string.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>System.String.</returns>
        private static string GenerateDataString(object obj)
        {
            string result = "<null>";

            if (obj != null)
            {
                //StringBuilder stringBuilder = new StringBuilder();

                //XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ////Add an empty namespace and empty value
                //ns.Add(string.Empty, string.Empty);
                //XmlWriterSettings settings = new XmlWriterSettings();

                //settings.OmitXmlDeclaration = true; // Remove the <?xml version="1.0" encoding="utf-8"?>

                //XmlWriter writer = XmlWriter.Create(stringBuilder, settings);
                ////Create the serializer
                //XmlSerializer slz = new XmlSerializer(obj.GetType());

                ////Serialize the object with our own namespaces (notice the overload)
                //slz.Serialize(writer, obj, ns);
                //writer.Flush();

                //result = stringBuilder.ToString();


                result = JsonConvert.SerializeObject(obj);
            }

            return result;
        }

        #endregion

        #region IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            CloseLogFile();
        }

        #endregion
    }
}

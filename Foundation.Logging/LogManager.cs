#region
using Common.Logging;
using System;
using System.Configuration;
#endregion

namespace Foundation.Logging
{
    /// <summary>
    /// Log Manager
    /// </summary>
    public static class LogManager
    {
        internal static ILog GetLogger<T>()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     ILog Provider
        /// </summary>
        private static readonly ILog DefaultLogProvider;

        /// <summary>
        ///     Set the default provider based on deployment
        /// </summary>
        static LogManager()
        {
            try
            {

                if (!String.IsNullOrEmpty("LoggerName"))
                {
                    switch ("LoggerName")
                    {
                        case "Log4net":
                            DefaultLogProvider = Common.Logging.LogManager.GetLogger("Log4net");
                            DefaultLogProvider.Info("Log4net logger created.");
                            break;
                        case "NLog":
                            DefaultLogProvider = Common.Logging.LogManager.GetLogger("NLog");
                            DefaultLogProvider.Info("NLog logger created.");
                            break;
                        default:
                            DefaultLogProvider = Common.Logging.LogManager.GetLogger("Log4net");
                            DefaultLogProvider.Info(" Default logger selected as Log4net. Use Logger object for logging.");
                            break;
                    }
                }
                else
                {
                    DefaultLogProvider = Common.Logging.LogManager.GetLogger("Log4net");
                    DefaultLogProvider.Info(" Default logger selected as Log4Net. Use Logger object for logging.");
                }
            }
            catch (Exception exception)
            {
                DefaultLogProvider = Common.Logging.LogManager.GetLogger("Log4net");
                DefaultLogProvider.Error(" Default logger selected as Log4Net", exception);
            }
        }

        /// <summary>
        ///     Logs the error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">The ex.</param>
        public static void Error(string message, Exception ex)
        {
            var updatedMessage = message == "" ? (ex?.Message ?? string.Empty) : message;
            DefaultLogProvider.Error(updatedMessage, ex);
        }

        /// <summary>
        ///     Logs the information.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Info(string message)
        {
            DefaultLogProvider.Info(message);
        }

        /// <summary>
        ///     Logs the debug.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">The ex.</param>
        public static void Debug(string message, Exception ex)
        {
            var updatedMessage = message == "" ? (ex?.Message ?? string.Empty) : message;
            DefaultLogProvider.Debug(updatedMessage, ex);
        }

        /// <summary>
        ///     Logs the warning.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Warning(string message)
        {
            DefaultLogProvider.Warn(message);
        }

        /// <summary>
        ///     Logs the exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public static void Exception(Exception ex)
        {
            DefaultLogProvider.Error(string.Empty, ex);
        }

        /// <summary>
        ///     Falat the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public static void Fatal(Exception ex)
        {
            DefaultLogProvider.Fatal(ex);
        }
    }
}
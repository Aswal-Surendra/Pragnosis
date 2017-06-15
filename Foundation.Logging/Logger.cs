// /* Copyright (c) 2017 Publicis.Sapient.  All rights reserved. */

#region

using System;

#endregion

namespace RapidSitecore.Foundation.Logging
{
    /// <summary>
    ///     Logger
    /// </summary>
    public static class SeriLogger
    {
        /// <summary>
        ///     The cache provider type name
        /// </summary>
        public const string LoggingProviderTypeAndName = "LoggingProvider";


        /// <summary>
        ///     ICache Provider
        /// </summary>
        private static readonly ILogger DefaultLogProvider;

        /// <summary>
        ///     Set the default provider based on deployment
        /// </summary>
        static SeriLogger()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(LoggingProviderTypeAndName))
                {
                    DefaultLogProvider = DefaultLoggingProvider.Instance;
                    return;
                }
                else
                {
                    var logProviderType = Type.GetType(LoggingProviderTypeAndName, false);

                    if (logProviderType != null)
                        DefaultLogProvider = Activator.CreateInstance(logProviderType) as ILogger;

                    if (DefaultLogProvider == null)
                        DefaultLogProvider = DefaultLoggingProvider.Instance;
                }
            }
            catch (Exception exception)
            {
                DefaultLogProvider = DefaultLoggingProvider.Instance;

                Info("Exception when Initalizing cache." + exception.Message + " " + exception.InnerException);

                Info("InMemoryCacheProvider Activated");
            }
        }


        /// <summary>
        ///     Errors the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">The ex.</param>
        public static void Error(string message, Exception ex)
        {
            DefaultLogProvider.Error(message, ex);
        }

        /// <summary>
        ///     Informations the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Info(string message)
        {
            DefaultLogProvider.Info(message);
        }


        /// <summary>
        ///     Debugs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">The ex.</param>
        public static void Debug(string message, Exception ex = null)
        {
            DefaultLogProvider.Debug(message, ex);
        }


        /// <summary>
        ///     Warnings the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Warning(string message)
        {
            DefaultLogProvider.Warning(message);
        }


        /// <summary>
        ///     Exceptions the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public static void Exception(Exception ex)
        {
            DefaultLogProvider.Exception(ex);
        }

        /// <summary>
        ///     Fatal the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public static void Fatal(Exception ex)
        {
            DefaultLogProvider.Exception(ex);
        }
    }
}
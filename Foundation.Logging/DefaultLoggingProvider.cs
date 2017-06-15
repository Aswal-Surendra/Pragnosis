// /* Copyright (c) 2017 Publicis.Sapient.  All rights reserved. */

#region

using System;
using Serilog;

#endregion

namespace RapidSitecore.Foundation.Logging
{
    public sealed class DefaultLoggingProvider : ILogger, IDisposable
    {
        /// <summary>
        ///     The logger
        /// </summary>
        private const String _path = "C:\\logs\\serilog.txt";

        /// <summary>
        ///     The logger
        /// </summary>
        private readonly Serilog.Core.Logger _logger;

        /// <summary>
        ///     The disposed
        /// </summary>
        private bool _disposed;

        /// <summary>
        ///     Prevents a default instance of the <see cref="DefaultLoggingProvider" /> class from being created.
        /// </summary>
        private DefaultLoggingProvider()
        {
            //setup our DI
            // Create Logger
            if (!String.IsNullOrEmpty("SeriLogFilePath"))
            {
                _logger = new LoggerConfiguration()
                    .WriteTo.File("SeriLogFilePath")
                    .CreateLogger();
            }
            else
            {
                _logger = new LoggerConfiguration()
                   .WriteTo.File(_path)
                   .CreateLogger();
            }
        }

        /// <summary>
        ///     Gets the instance.
        /// </summary>
        /// <value>
        ///     The instance.
        /// </value>
        public static DefaultLoggingProvider Instance { get; } = new DefaultLoggingProvider();

        /// <summary>
        ///     Dispose method
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Logs the error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">The ex.</param>
        public void Error(string message, Exception ex)
        {
            _logger.Error(ex, message);
        }

        /// <summary>
        ///     Logs the information.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Info(string message)
        {
            _logger.Information(message);
        }

        /// <summary>
        ///     Logs the debug.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">The ex.</param>
        public void Debug(string message, Exception ex)
        {
            _logger.Debug(ex, "[Debug]");
        }

        /// <summary>
        ///     Logs the warning.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Warning(string message)
        {
            _logger.Warning(message);
        }

        /// <summary>
        ///     Logs the exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public void Exception(Exception ex)
        {
            _logger.Error(ex, "[Error]");
        }

        /// <summary>
        ///     Logs the fatel.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public void Fatal(Exception ex)
        {
            _logger.Error(ex, "[Error]");
        }

        /// <summary>
        ///     Setting dispose
        /// </summary>
        /// <param name="disposing">Disposing value</param>
        private void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
                if (_logger != null)
                {
                    _logger.Dispose();
                    _disposed = true;
                }
        }

        /// <summary>
        ///     We want the remove object to be disposed only once the static object instance loses scope.
        /// </summary>
        ~DefaultLoggingProvider()
        {
            Dispose(true);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace IntraWeb.Middleware.ErrorHandling
{
    /// <summary>
    /// Logger which formats and logs Unhandled Exceptions catched by <see cref="GlobalErrorHandling"/>.
    /// </summary>
    public class UnhandledExceptionLogger : IUnhandledExceptionLogger
    {
        private readonly ILogger _logger;

        public UnhandledExceptionLogger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("UnhandledExceptions");
        }

        /// <summary>
        /// Log Unhandled Exception
        /// </summary>
        public void LogException(Exception exception)
        {
            _logger.LogCritical("Unhandled Exception", exception);
        }
    }
}

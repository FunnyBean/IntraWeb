using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntraWeb.Middleware.ErrorHandling
{
    /// <summary>
    /// Logger which formats and logs Unhandled Exceptions catched by <see cref="GlobalErrorHandling"/>.
    /// </summary>
    public interface IUnhandledExceptionLogger
    {
        /// <summary>
        /// Log Unhandled Exception
        /// </summary>
        void LogException(Exception exception);
    }
}

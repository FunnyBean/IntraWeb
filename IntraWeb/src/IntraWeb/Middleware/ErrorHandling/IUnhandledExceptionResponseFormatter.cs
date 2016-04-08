using Microsoft.AspNet.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntraWeb.Middleware.ErrorHandling
{
    /// <summary>
    /// Response Formatter for Unhandled Exceptions catched by <see cref="GlobalErrorHandling"/>.
    /// </summary>
    public interface IUnhandledExceptionResponseFormatter
    {
        /// <summary>
        /// Action that formats response with requested format
        /// </summary>
        /// <param name="response">Response instance to be modified</param>
        /// <param name="exception">Exception instance</param>
        void FormatResponse(HttpResponse response, Exception exception);
    }        
}

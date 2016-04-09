using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Newtonsoft.Json;


namespace IntraWeb.Middleware.ErrorHandling
{
    /// <summary>
    /// Response Formatter for Unhandled Exceptions catched by <see cref="GlobalErrorHandling"/>.
    /// </summary>
    public class UnhandledExceptionApiResponseFormatter : IUnhandledExceptionApiResponseFormatter
    {

        /// <summary>
        /// Action that formats response with requested format
        /// </summary>
        /// <param name="response">Response instance to be modified</param>
        /// <param name="exception">Exception instance</param>
        public async void FormatResponse(HttpResponse response, Exception exception)
        {
            response.StatusCode = 500;
            response.ContentType = "application/json";

            var errorInfo = new
            {
                exceptionMessage = exception.Message,
                exceptionData = exception.Data
            };

            await response.WriteAsync(JsonConvert.SerializeObject(errorInfo));
        }        
    }
}

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
    public class UnhandledExceptionResponseFormatter : IUnhandledExceptionResponseFormatter
    {

        /// <summary>
        /// Action that formats response with requested format
        /// </summary>
        /// <param name="requestedFormat">Requested response format</param>
        /// <param name="response">Response instance to be modified</param>
        /// <param name="exception">Exception instance</param>
        public void FormatResponse(UnhandledExceptionResponseFormat requestedFormat, HttpResponse response, Exception exception)
        {
            switch (requestedFormat)
            {
                case UnhandledExceptionResponseFormat.HtmlPage:
                    FormatHtmlResponse(response, exception);
                    break;
                case UnhandledExceptionResponseFormat.JsonObject:
                    FormatJsonResponse(response, exception);
                    break;
                default:
                    throw new ArgumentException();
            }                        
        }


        private async void FormatJsonResponse(HttpResponse response, Exception exception)
        {
            response.Clear();
            response.StatusCode = 500;
            response.ContentType = "application/json";

            var errorInfo = new
            {
                exceptionMessage = exception.Message,
                exceptionData = exception.Data
            };

            await response.WriteAsync(JsonConvert.SerializeObject(errorInfo));
        }


        private async void FormatHtmlResponse(HttpResponse response, Exception exception)
        {
            response.Clear();
            response.StatusCode = 500;
            response.ContentType = "text/html";
            await response.WriteAsync("<html><body>\r\n");
            await response.WriteAsync("We're sorry, we encountered an un-expected issue with your application.<br>\r\n");
            await response.WriteAsync("<br><a href=\"/\">Home</a><br>\r\n");
            await response.WriteAsync("</body></html>\r\n");
            await response.WriteAsync(new string(' ', 512)); // Padding for IE
        }
    }
}

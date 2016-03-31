using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;


namespace IntraWeb.Middleware.ErrorHandling
{
    /// <summary>
    /// Global Error Handling Middleware
    /// </summary>
    public class GlobalErrorHandling
    {

        #region Fields

        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly GlobalErrorHandlingOptions _options;

        #endregion


        #region Ctor


        public GlobalErrorHandling(RequestDelegate next, 
                                   ILoggerFactory loggerFactory, 
                                   IOptions<GlobalErrorHandlingOptions> options)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<GlobalErrorHandling>();
            _options = options.Value;
        }


        #endregion


        #region Middleware methods


        public async Task Invoke(HttpContext context)
        {            
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {                
                _logger.LogCritical(ex.ToString());
                FormatExceptionResponse(context.Response);
            }            
        }


        private void FormatExceptionResponse(HttpResponse response)
        {
            response.Clear();
            response.StatusCode = 500;            
        }


        #endregion

    }
}
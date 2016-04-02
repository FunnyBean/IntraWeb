using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.Logging;


namespace IntraWeb.Middleware.ErrorHandling
{
    /// <summary>
    /// Global Error Handling Middleware
    /// </summary>
    public class GlobalErrorHandling
    {

        #region Fields

        private readonly RequestDelegate _next;        
        private readonly IUnhandledExceptionLogger _exceptionLogger;
        private readonly IUnhandledExceptionResponseFormatter _responseFormatter;
                
        #endregion


        #region Constructor
        
        public GlobalErrorHandling(RequestDelegate next, 
                                   IUnhandledExceptionLogger exceptionLogger, 
                                   IUnhandledExceptionResponseFormatter responseFormatter)
        {
            _next = next;            
            _exceptionLogger = exceptionLogger;
            _responseFormatter = responseFormatter;
        }
        
        #endregion
                       
        
        public async Task Invoke(HttpContext context)
        {            
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                _exceptionLogger.LogException(ex);
                
                if (IsApiRequest(context))
                {
                    _responseFormatter.FormatResponse(
                        UnhandledExceptionResponseFormat.JsonObject, context.Response, ex);
                }
                else
                {
                    _responseFormatter.FormatResponse(
                        UnhandledExceptionResponseFormat.HtmlPage, context.Response, ex);
                }
            }
        }
                                       
  
        private bool IsApiRequest(HttpContext context)
        {            
            // TODO: Rozlíšiť request na API od hlavného requestu na zobrazenie stránky
            return true;
        }

    }
}
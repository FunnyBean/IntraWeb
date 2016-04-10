using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Microsoft.AspNet.Diagnostics;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNet.Http.Features;

namespace IntraWeb.Middleware.ErrorHandling
{
    /// <summary>
    /// Global Error Handling Middleware
    /// </summary>
    public class GlobalErrorHandling
    {

        #region Fields

        private readonly RequestDelegate _next;        
        private readonly ILogger _exceptionLogger;
        private readonly IUnhandledExceptionApiResponseFormatter _responseFormatter;
        private readonly GlobalErrorHandlingOptions _options;
        private readonly Func<object, Task> _clearCacheHeadersDelegate;
        private readonly DiagnosticSource _diagnosticSource;

        #endregion


        #region Constructor

        public GlobalErrorHandling(RequestDelegate next, 
                                   ILoggerFactory loggerFactory, 
                                   IUnhandledExceptionApiResponseFormatter responseFormatter,
                                   GlobalErrorHandlingOptions options,
                                   DiagnosticSource diagnosticSource)
        {
            _next = next;
            _exceptionLogger = loggerFactory.CreateLogger<GlobalErrorHandling>();
            _responseFormatter = responseFormatter;
            _options = options;
            _clearCacheHeadersDelegate = ClearCacheHeaders;
            _diagnosticSource = diagnosticSource;
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
                // logging error
                _exceptionLogger.LogError("An unhandled exception has occurred: " + ex.Message, ex);

                // We can't do anything if the response has already started, just abort.
                if (context.Response.HasStarted)
                {
                    _exceptionLogger.LogWarning("The response has already started, the error handler will not be executed.");
                    throw;
                }

                // WTF???
                PathString originalPath = context.Request.Path;
                if (_options.NonApiExceptionPath.HasValue)
                {
                    context.Request.Path = _options.NonApiExceptionPath;
                }

                try
                {
                    context.Response.Clear();

                    var errorHandlerFeature = new GlobalErrorHandlingFeature()
                    {
                        Error = ex,
                    };
                    context.Features.Set<IGlobalErrorHandlingFeature>(errorHandlerFeature);

                    context.Response.StatusCode = 500;
                    context.Response.OnStarting(_clearCacheHeadersDelegate, context.Response);

                    await _next(context);

                    // ???
                    if (_diagnosticSource.IsEnabled("Microsoft.AspNet.Diagnostics.HandledException"))
                    {
                        _diagnosticSource.Write("Microsoft.AspNet.Diagnostics.HandledException", new { httpContext = context, exception = ex });
                    }
                                        
                    return;
                }
                catch (Exception ex2)
                {
                    // Suppress secondary exceptions, re-throw the original.
                    _exceptionLogger.LogError("An exception was thrown attempting to execute the error handler.", ex2);
                }
                finally
                {
                    context.Request.Path = originalPath;
                }

                // Re-throw the original if we couldn't handle it
                throw;                                 
            }
        }


        private Task ClearCacheHeaders(object state)
        {
            var response = (HttpResponse)state;
            response.Headers[HeaderNames.CacheControl] = "no-cache";
            response.Headers[HeaderNames.Pragma] = "no-cache";
            response.Headers[HeaderNames.Expires] = "-1";
            response.Headers.Remove(HeaderNames.ETag);
            return Task.FromResult(0);
        }


        private bool IsApiRequest(HttpContext context)
        {            
            var requestPath = context.Request.Path.Value ?? string.Empty;
            return requestPath.Contains("/api/");
        }

    }
}
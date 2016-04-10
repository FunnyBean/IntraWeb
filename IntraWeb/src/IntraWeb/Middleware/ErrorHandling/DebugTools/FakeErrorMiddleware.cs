using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntraWeb.Middleware.ErrorHandling.DebugTools
{
    public class FakeErrorMiddleware
    {
        private readonly RequestDelegate _next;

        private FakeErrorMiddlewareOptions _options;

        
        private static int _counter = 0;
        private static object _locker = new object();
        

        #region Constructor
        
        public FakeErrorMiddleware(RequestDelegate next, FakeErrorMiddlewareOptions options)
        {
            _next = next;
            _options = options;
        }

        #endregion
                
        public async Task Invoke(HttpContext context)
        {
            var canThrow = false;

            // if path meets filter requirements, increment counter
            if (IsRequestedPath(context))
            {
                lock (_locker)
                {
                    _counter++;
                    if (_counter >= _options.ErrorTriggerDelay)
                    {
                        canThrow = true;
                        _counter = 0;
                    }
                }
            }
            
            // throw before request delegation
            if (canThrow && _options.TimingOptions.HasFlag(FakeErrorTimingOptions.BeforeRequestDelegated))
            {
                ThrowException(context);
            }
            
            // request delegation
            await _next.Invoke(context);

            // throw after request delegation
            if (canThrow && _options.TimingOptions.HasFlag(FakeErrorTimingOptions.AfterRequestDelegated))
            {
                ThrowException(context);
            }
        }

        /// <summary>
        /// Exception throwing
        /// </summary>
        private void ThrowException(HttpContext context)
        {
            throw new Exception("Boom!");
        }

        /// <summary>
        /// Verifying current request path
        /// </summary>        
        private bool IsRequestedPath(HttpContext context)
        {
            var pathValue = context.Request.Path.Value ?? string.Empty;
            return (_options.PathFilter.IsMatch(pathValue));
        }
    }
}
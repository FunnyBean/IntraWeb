using Microsoft.AspNet.Builder;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace IntraWeb.Middleware.ErrorHandling
{
    public static class GlobalErrorHandlingExtensions
    {
        /// <summary>
        /// Captures all unhandled exceptions
        /// </summary>
        public static IApplicationBuilder UseGlobalErrorHandling(this IApplicationBuilder builder)
        {            
            return builder.UseMiddleware<GlobalErrorHandling>();
        }
    }
}

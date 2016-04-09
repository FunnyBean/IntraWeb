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
        /// <param name="apiRoute">
        /// Route for Api Controllers
        /// </param>
        /// <param name="nonApiExceptionPath">
        /// Error Path for unhandled non-api exceptions
        /// </param>
        public static IApplicationBuilder UseGlobalErrorHandling(this IApplicationBuilder builder, 
                string apiRoute, string nonApiExceptionPath)
        {
            var options = new GlobalErrorHandlingOptions { NonApiExceptionPath = nonApiExceptionPath,
                                                           ApiRoute = apiRoute };
            return builder.UseMiddleware<GlobalErrorHandling>(options);
        }
    }
}

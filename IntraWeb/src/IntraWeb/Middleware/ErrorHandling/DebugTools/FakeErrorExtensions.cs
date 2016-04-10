using Microsoft.AspNet.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IntraWeb.Middleware.ErrorHandling.DebugTools
{
    public static class FakeErrorExtensions
    {
        /// <summary>
        /// Error generator Middleware
        /// </summary>        
        /// <param name="timing">
        /// Whether Exception will be thrown before or after Request delegation to next Middleware in chain
        /// </param>
        /// <param name="pathFilter">
        /// RegEx path filter
        /// </param>
        /// <param name="delay">
        /// Requests count before Exception will be thrown
        /// </param>
        /// <remarks>
        /// For Debugging purposes only!!!
        /// </remarks>
        public static IApplicationBuilder UseErrorGenerator(this IApplicationBuilder builder, 
            FakeErrorTimingOptions timing,
            string pathFilter, 
            int delay)
        {
            var options = new FakeErrorMiddlewareOptions
            {
                TimingOptions = timing,
                PathFilter = new Regex(pathFilter),
                ErrorTriggerDelay = delay
            };
            return builder.UseMiddleware<FakeErrorMiddleware>(options);
        }
    }
}

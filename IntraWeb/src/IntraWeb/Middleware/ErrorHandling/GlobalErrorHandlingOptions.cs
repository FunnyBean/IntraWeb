using Microsoft.AspNet.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntraWeb.Middleware.ErrorHandling
{
    public class GlobalErrorHandlingOptions
    {
        /// <summary>
        /// Error Path for unhandled non-api exceptions
        /// </summary>
        public PathString NonApiExceptionPath { get; set; }

        /// <summary>
        /// Route for Api Controllers
        /// </summary>
        public PathString ApiRoute { get; set; }                
    }
}

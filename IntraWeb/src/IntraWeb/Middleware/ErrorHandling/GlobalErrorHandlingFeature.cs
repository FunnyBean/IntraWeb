using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntraWeb.Middleware.ErrorHandling
{
    public class GlobalErrorHandlingFeature : IGlobalErrorHandlingFeature 
    {
        public Exception Error { get; set; }
    }
}

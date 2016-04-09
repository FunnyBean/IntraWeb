using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntraWeb.Middleware.ErrorHandling
{
    interface IGlobalErrorHandlingFeature
    {
        Exception Error { get; }
    }
}

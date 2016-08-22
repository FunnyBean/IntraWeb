using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Krosbook.Filters
{
    /// <summary>
    /// Action filter attribute for checking null arguments.
    /// </summary>
    /// <remarks>
    /// If any of arguments contains null value, then action result is set to badRequest.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class CheckArgumentsForNullAttribute : ActionFilterAttribute
    {

        /// <summary>
        /// Called when [action executing].
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (actionContext.ActionArguments.Any(p => p.Value == null))
            {
                actionContext.Result = new BadRequestObjectResult("Arguments cannot be null.");
            }

        }
    }
}

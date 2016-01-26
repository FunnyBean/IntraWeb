using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;
using NSubstitute;

namespace IntraWeb.UnitTests.Filters
{
    /// <summary>
    /// Class for checking Action filter attributes
    /// </summary>
    public static  class ActionFilterCheckerExtensions
    {
        /// <summary>
        /// Has method custom action filter attribute?
        /// </summary>
        /// <typeparam name="T">Type of acton filter attribute.</typeparam>
        /// <param name="method">Method for checking.</param>
        /// <returns>
        /// True - method hes custom attribute; otherwise false;
        /// </returns>
        public static bool HasMethodActionFilterAttribute<T>(this Delegate method) where T : ActionFilterAttribute
        {
            var methodInfo = method.GetMethodInfo();

            var attibutes = methodInfo.GetCustomAttributes(typeof(T), true);

            return attibutes.Any();
        }

        /// <summary>
        /// Creates the action executing context for testing.
        /// </summary>
        /// <param name="initActionArguments">Action for init action arguments.</param>
        /// <returns>The action executing context for testing.</returns>
        public static ActionExecutingContext CreateActionExecutingContext(Action<Dictionary<string, object>> initActionArguments)
        {
            var actionArguments = new Dictionary<string, object>();

            initActionArguments(actionArguments);

            var executingContext = new ActionExecutingContext(new ActionContext
            {
                HttpContext = Substitute.For<HttpContext>(),
                RouteData = new Microsoft.AspNet.Routing.RouteData(),
                ActionDescriptor = new Microsoft.AspNet.Mvc.Abstractions.ActionDescriptor()
            }, new List<IFilterMetadata>(), actionArguments, new object());

            return executingContext;
        }
    }
}

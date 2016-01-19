using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc.Filters;

namespace IntraWeb.UnitTests.Extensions
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
    }
}

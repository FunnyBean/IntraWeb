using Microsoft.AspNet.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IntraWeb.Middleware.ErrorHandling.DebugTools
{
    public class FakeErrorMiddlewareOptions
    {
        /// <summary>
        /// Whether Exception will be thrown before or after Request delegation to next Middleware in chain
        /// </summary>
        public FakeErrorTimingOptions TimingOptions { get; set; }

        /// <summary>
        /// Filtered route
        /// </summary>
        public Regex PathFilter { get; set; }

        /// <summary>
        /// Filtered Requests count before Exception will be thrown
        /// </summary>
        public int ErrorTriggerDelay { get; set; }
    }

    [Flags]
    public enum FakeErrorTimingOptions
    {        
        BeforeRequestDelegated = 1,
        AfterRequestDelegated = 2
    }
}

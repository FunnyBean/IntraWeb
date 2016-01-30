using System;
using Microsoft.Extensions.Logging;

namespace IntraWeb.UnitTests.Service
{
    /// <summary>
    /// Fake fogger for unit testing.
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.Logging.ILogger{T}" />
    public class StubLogger<T> : ILogger<T>
    {
        /// <summary>
        /// Do nothing.
        /// </summary>
        public void Log(LogLevel logLevel, int eventId, object state, Exception exception, Func<object, Exception, string> formatter)
        {
            // no op
        }

        /// <summary>
        /// Always false.
        /// </summary>
        public bool IsEnabled(LogLevel logLevel)
        {
            return false;
        }

        ///<summary>
        /// <Always null.
        /// </summary>
        public IDisposable BeginScopeImpl(object state)
        {
            return null;
        }
    }
}

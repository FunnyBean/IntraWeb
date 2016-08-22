using System;
using System.Runtime.Serialization;

namespace Krosbook.Services.Template
{
    /// <summary>
    /// Base class for template engine exceptions.
    /// </summary>
    public class TemplateException
        : Exception
    {
        public TemplateException()
        {
        }

        public TemplateException(string message) : base(message)
        {
        }

        public TemplateException(string message, Exception innerException) : base(message, innerException)
        {
        }

    }
}

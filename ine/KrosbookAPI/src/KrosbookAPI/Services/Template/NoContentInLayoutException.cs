using System;
using System.Runtime.Serialization;

namespace Krosbook.Services.Template
{
    /// <summary>
    /// Exception indicating, that there is no <c>{include #content}</c> macro in the layout template.
    /// </summary>
    public class NoContentInLayoutException
        : TemplateException
    {

        public NoContentInLayoutException(string layoutName)
            : base($"There is no \"{{include #content}}\" macro in layout template \"{layoutName}\".")
        {
            this.LayoutName = layoutName;
        }

        /// <summary>
        /// Name of the layout template.
        /// </summary>
        public string LayoutName { get; }
    }
}

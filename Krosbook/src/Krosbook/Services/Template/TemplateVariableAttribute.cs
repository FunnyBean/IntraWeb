using System;

namespace Krosbook.Services.Template
{
    /// <summary>
    /// Indicates that a public property represents a data for template variable.
    /// </summary>
    /// <remarks>
    /// This attribute makes strongly typed classes data classes for templates. Data for templates are simple dictionaries
    /// with string keys. It is more convenient and safer to have strongly typed classes for data. This attribute maps properties
    /// to variables in template. Data classes can be converted to data dictionaries using
    /// <see cref="TemplateDataConverter">TemplateDataConverter</see>.
    /// </remarks>
    /// <example>
    /// The following example shows, that value of the <c>UserName</c> property will be set to variable <c>Name</c> in template.
    /// <code>
    /// public class TemplateData {
    ///     [TemplateVariable("Name")]
    ///     public string UserName { get; set; }
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Property)]
    public class TemplateVariableAttribute
        : Attribute
    {
        /// <summary>
        /// Initializes a new instance of <see cref="TemplateVariableAttribute">TemplateVariableAttribute</see> class and
        /// specifies a name of the variable name in template.
        /// </summary>
        /// <param name="variableName">Name of a variable in a template.</param>
        public TemplateVariableAttribute(string variableName)
        {
            this.VariableName = variableName;
        }

        /// <summary>
        /// Name of a variable in a template.
        /// </summary>
        public string VariableName { get; set; }

    }
}

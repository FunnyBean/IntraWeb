using System;

namespace IntraWeb.Services.Template
{
    /// <summary>
    /// Exception for template formatter (<see cref="ITemplateFormatter">ITemplateFormatter</see>), when there is a variable
    /// in template, for which there are no data.
    /// </summary>
    public class UnknownTemplateVariableException
        : Exception
    {

        public UnknownTemplateVariableException(string templateName, string variableName)
            : base($"Unknown variable \"{variableName}\" in template \"{templateName}\".")
        {
            this.TemplateName = templateName;
            this.VariableName = variableName;
        }


        /// <summary>
        /// Template name.
        /// </summary>
        public string TemplateName { get; }

        /// <summary>
        /// Variable name for which there are no data.
        /// </summary>
        public string VariableName { get; }
    }
}

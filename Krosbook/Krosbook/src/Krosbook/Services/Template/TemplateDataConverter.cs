using System;
using System.Collections.Generic;
using System.Reflection;

namespace Krosbook.Services.Template
{
    /// <summary>
    /// Class for converting strongly typed data classes to data dictionaries.
    /// </summary>
    /// <remarks>
    /// Resulting dictionary is filled with values from properties annotated with
    /// <see cref="TemplateVariableAttribute">TemplateVariable</see> attribute. Keys in dictionary are variable names specified
    /// by <see cref="TemplateVariableAttribute.VariableName">TemplateVariableAttribute.VariableName</see>.
    /// </remarks>
    public class TemplateDataConverter
    {

        /// <summary>
        /// Converts strongly typed class <paramref name="data" /> to data dictionary.
        /// </summary>
        /// <param name="data">Strongly typed data class, which is converted to dictionary.</param>
        /// <returns>Data dictionary, where keys are the names of variables from <paramref name="data" />.</returns>
        /// <exception cref="ArgumentNullException">Value of <paramref name="data" /> is null.</exception>
        /// <exception cref="InvalidOperationException">Variable name is defined more than once - it is defined on more
        /// than one property.</exception>
        public IDictionary<string, object> Convert(object data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var result = new Dictionary<string, object>();

            var props = data.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                ConvertProperty(result, data, prop);
            }

            return result;
        }


        private void ConvertProperty(IDictionary<string, object> result, object data, PropertyInfo prop)
        {
            foreach (var attr in prop.GetCustomAttributes<TemplateVariableAttribute>(true))
            {
                if (result.ContainsKey(attr.VariableName)) {
                    throw new InvalidOperationException($"Variable name \"{attr.VariableName}\" is defined more than once.");
                }
                result.Add(attr.VariableName, prop.GetValue(data));
            }
        }

    }
}

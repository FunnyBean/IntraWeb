using System;
using System.Collections.Generic;
using System.Reflection;

namespace IntraWeb.Services.Email
{
    public class EmailDataConverter
    {

        public virtual IDictionary<string, object> Convert(object data)
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
                if (result.ContainsKey(attr.Name)) {
                    throw new InvalidOperationException($"Kľúč {attr.Name} je definovaný viackrát.");
                }
                result.Add(attr.Name, prop.GetValue(data));
            }
        }

    }
}

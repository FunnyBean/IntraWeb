using System;
using System.Collections.Generic;
using System.Reflection;

namespace IntraWeb.Services.Email
{
    public class EmailDataConverter
    {

        public virtual IDictionary<string, string> Convert(IEmailData data)
        {
            var result = new Dictionary<string, string>();

            if (data != null)
            {
                var props = data.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var prop in props)
                {
                    ConvertProperty(result, data, prop);
                }
            }

            return result;
        }


        private void ConvertProperty(IDictionary<string, string> result, IEmailData data, PropertyInfo prop)
        {
            foreach (var attr in prop.GetCustomAttributes<TemplateVariableAttribute>(true))
            {
                if (result.ContainsKey(attr.Name)) {
                    throw new InvalidOperationException($"Kľúč {attr.Name} je definovaný viackrát.");
                }
                var value = prop.GetValue(data);
                result.Add(attr.Name, (value == null) ? string.Empty : value.ToString());
            }
        }

    }
}

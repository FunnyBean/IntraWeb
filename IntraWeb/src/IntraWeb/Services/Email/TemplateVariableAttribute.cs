using System;

namespace IntraWeb.Services.Email
{
    [AttributeUsage(AttributeTargets.Property)]
    public class TemplateVariableAttribute
        : Attribute
    {

        public TemplateVariableAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }

    }
}

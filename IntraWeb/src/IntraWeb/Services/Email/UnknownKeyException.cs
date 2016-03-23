using System;

namespace IntraWeb.Services.Email
{
    public class UnknownKeyException
        : Exception
    {

        public UnknownKeyException(string templateName, string key)
            : base("Neznámy kľúč v šablóne.")
        {
            this.TemplateName = templateName;
            this.Key = key;
        }


        public string TemplateName { get; }
        public string Key { get; }
    }
}

using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntraWeb.Services.Email
{
    [AttributeUsage(AttributeTargets.Property)]
    public class EmailDataKeyAttribute
        : Attribute
    {

        public EmailDataKeyAttribute(string key)
        {
            this.Key = key;
        }

        public string Key { get; set; }

    }
}

using Krosbook.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Krosbook.Models.Users
{
    public class RememberMe : IModel
    {
        public static int BCryptWorkFactor { get; set; } = 10;

        public int Id { get; set; }

        public int userId { get; set; }

        public User user { get; set; }

        public string Selector { get; set; }

        public string Validator { set { ValidatorHash = BCrypt.Net.BCrypt.HashPassword(value, BCryptWorkFactor); } }

        public string ValidatorHash { get; set; }

    }
}

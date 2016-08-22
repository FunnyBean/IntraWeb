using Krosbook.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Krosbook.Models.Users
{
    public interface IRememberMeRepository : IRepository<RememberMe>
    {
        RememberMe GetSingleByUserId(int userId);
        RememberMe GetSingleBySelector(string selector);
    }
}

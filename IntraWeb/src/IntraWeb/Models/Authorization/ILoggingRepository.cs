using IntraWeb.Models.Base;
using IntraWeb.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntraWeb.Models.Authorization
{
    public interface ILoggingRepository :  IRepository<User>
    {
    }
}

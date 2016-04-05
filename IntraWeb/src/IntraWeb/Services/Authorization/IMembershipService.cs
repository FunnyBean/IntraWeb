using IntraWeb.Models.Authorization;
using IntraWeb.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntraWeb.Services.Authorization
{
    public interface IMembershipService
    {
        MembershipContext ValidateUser(string username, string password);

        User GetUser(int userId);

        List<Role> GetUserRoles(string username);
    }
}

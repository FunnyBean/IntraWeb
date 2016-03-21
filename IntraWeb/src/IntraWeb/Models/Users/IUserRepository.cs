using IntraWeb.Models.Base;
using System.Collections.Generic;

namespace IntraWeb.Models.Users
{
    /// <summary>
    /// Interface, which describe repository for CRUD operations on the User model.
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        User GetSingleByUsername(string username);

        IEnumerable<Role> GetUserRoles(string username);
    }
}

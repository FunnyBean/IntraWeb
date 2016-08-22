using Krosbook.Models.Base;
using System.Collections.Generic;

namespace Krosbook.Models.Users
{
    /// <summary>
    /// Interface, which describe repository for CRUD operations on the User model.
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        User GetSingleByUsername(string username);

        User GetSingleByEmail(string email);

        IEnumerable<Role> GetUserRoles(string username);

        IEnumerable<Role> GetUserRolesById(int id); 
    }
}

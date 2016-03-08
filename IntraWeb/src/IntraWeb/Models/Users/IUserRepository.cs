using IntraWeb.Models.Base;

namespace IntraWeb.Models.Users
{
    /// <summary>
    /// Interface, which describe repository for CRUD operations on the User model.
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
    }
}

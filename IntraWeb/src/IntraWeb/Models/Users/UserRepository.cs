using IntraWeb.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;

namespace IntraWeb.Models.Users
{
    /// <summary>
    /// Users repository with CRUD operations
    /// </summary>
    /// <seealso cref="IntraWeb.Models.Base.BaseRepository{T}" />
    /// <seealso cref="IntraWeb.Models.Users.IUserRepository " />
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        /// <summary>
        /// Constructor of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Edits the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public override void Edit(User item)
        {
            var userRoles = item.UserRoles;

            item.UserRoles = null;
            base.Edit(item);

            var oldRoles = _dbContext.Set<UserRole>().Where(p => p.UserId == item.Id);

            foreach (var role in userRoles.Where(p => !HasUserRole(item.Id, p.RoleId)))
            {
                _dbContext.Entry(role).State = EntityState.Added;
            }

            foreach (var role in oldRoles.Where((r) => !userRoles.Any(p => (p.RoleId == r.RoleId))))
            {
                _dbContext.Entry(role).State = EntityState.Deleted;
            }

        }

        private bool HasUserRole(int userId, int roleId)
        {
            return _dbContext.Set<UserRole>().Any(p => p.UserId == userId && p.RoleId == roleId);
        }
    }
}

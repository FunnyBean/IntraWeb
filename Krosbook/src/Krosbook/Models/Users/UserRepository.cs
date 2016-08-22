using Krosbook.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Krosbook.Models.Users
{
    /// <summary>
    /// Users repository with CRUD operations
    /// </summary>
    /// <seealso cref="IntraWeb.Models.Base.BaseRepository{T}" />
    /// <seealso cref="IntraWeb.Models.Users.IUserRepository " />
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        IRoleRepository _roleReposistory;

        /// <summary>
        /// Constructor of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public UserRepository(ApplicationDbContext dbContext, IRoleRepository roleReposistory) : base(dbContext)
        {
            _roleReposistory = roleReposistory;
        }


        public User GetSingleByUsername(string username)
        {
            return this.GetSingle(x => x.Name == username);
        }

        public User GetSingleByEmail(string email)
        {
            return this.GetSingle(x => x.Email == email);
        }

        
        public IEnumerable<Role> GetUserRoles(string username)
        {
            List<Role> _roles = null;

            User _user = this.GetSingle(u => u.Name == username, u => u.Roles);
            if (_user != null)
            {
                _roles = new List<Role>();
                foreach (var _userRole in _user.Roles)
                    _roles.Add(_roleReposistory.GetItem(_userRole.RoleId));
            }

            return _roles;
        }

        public IEnumerable<Role> GetUserRolesById(int id)
        {
            List<Role> _roles = null;
            User _user = this.GetSingle(u => u.Id == id, u => u.Roles);
            if (_user != null)
            {
                _roles = new List<Role>();
                foreach (var _userRole in _user.Roles)
                    _roles.Add(_roleReposistory.GetItem(_userRole.RoleId));
            }
            return _roles;
        }

        /// <summary>
        /// Gets the user by Id with roles.
        /// </summary>
        /// <param name="userId">The user identifier</param>
        /// <returns>
        /// Return user with roles; otherwise null.
        /// </returns>
        public override User GetItem(int userId)
        {
            return _dbContext.Set<User>().
                Include(r => r.Roles).
                AsNoTracking().
                FirstOrDefault(r => r.Id == userId);
        }

        /// <summary>
        /// Gets all users with roles.
        /// </summary>
        /// <returns>
        /// Return all users with roles; otherwise null.
        /// </returns>
        public override IQueryable<User> GetAll()
        {
            return _dbContext.Set<User>().
                Include(r => r.Roles).
                AsNoTracking();
        }

        /// <summary>
        /// Edits the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public override void Edit(User user)
        {
            var userRoles = user.Roles;

            user.Roles = null;
            base.Edit(user);

            if (userRoles != null)
            {
                var oldRole = _dbContext.Set<UserRole>().Where(p => p.RoleId == user.Id);

                SetAddOrModifiedStatesForRole(user, userRoles);

                SetDeleteStateForNotUsedRole(userRoles, oldRole);
            }

        }

        private void SetAddOrModifiedStatesForRole(User user, ICollection<UserRole> userRole)
        {
            foreach (var role in userRole)
            {
                _dbContext.Entry(role).State =
                    HasUserRole(user.Id, role.RoleId) ? EntityState.Modified : EntityState.Added;
            }
        }

        private void SetDeleteStateForNotUsedRole(ICollection<UserRole> userRole, IQueryable<UserRole> oldRole)
        {
            foreach (var role in oldRole.Where((r) => !userRole.Any(p => (p.RoleId == r.RoleId))))
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

using IntraWeb.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntraWeb.Models.Users
{
    /// <summary>
    /// USer's roles repository with CRUD operations
    /// </summary>
    /// <seealso cref="IntraWeb.Models.Base.BaseRepository{T}" />
    /// <seealso cref="IntraWeb.Models.Users.IUserRoleRepository " />
    public class UserRoleRepository : BaseRepository<UserRole>, IUserRoleRepository
    {
        /// <summary>
        /// Constructor of the <see cref="UserRoleRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public UserRoleRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}

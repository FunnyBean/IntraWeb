using Krosbook.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Krosbook.Models.Users
{
    /// <summary>
    /// Roles repository with CRUD operations
    /// </summary>
    /// <seealso cref="Krosbook.Models.Base.BaseRepository{T}" />
    /// <seealso cref="Krosbook.Models.Users.IRoleRepository " />
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        /// <summary>
        /// Constructor of the <see cref="RoleRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public RoleRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}

using IntraWeb.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}

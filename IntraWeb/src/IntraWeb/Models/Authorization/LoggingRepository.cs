using IntraWeb.Models.Base;
using IntraWeb.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntraWeb.Models.Authorization
{
    public class LoggingRepository : BaseRepository<User>, ILoggingRepository
    {

        /// <summary>
        /// Constructor of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public LoggingRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}

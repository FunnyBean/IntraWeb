using Krosbook.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Krosbook.Models.Users
{
    public class RememberMeRepository : BaseRepository<RememberMe>, IRememberMeRepository
    {
        public RememberMeRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public RememberMe GetSingleByUserId(int userId)
        {
            return this.GetSingle(x => x.userId == userId);
        }

        public RememberMe GetSingleBySelector(string selector)
        {
            return this.GetSingle(x => x.Selector == selector);
        }
    }
}

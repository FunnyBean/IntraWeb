using System.Collections.Generic;
using IntraWeb.Models.Base;

namespace IntraWeb.Models.Users
{
    /// <summary>
    /// UserRole dummy repository for testing
    /// </summary>
    /// <seealso cref="IntraWeb.Models.IUserRoleRepository" />
    /// <seealso cref="IntraWeb.Models.Base.BaseDummyRepository{T}" />
    public class UserRoleDummyRepository : BaseDummyRepository<UserRole>, IUserRoleRepository
    {

        /// <summary>
        /// Initializes the dummy data.
        /// </summary>
        /// <param name="dummyData">The dummy data.</param>
        protected override void InitDummyData(IList<UserRole> dummyData)
        {
            base.InitDummyData(dummyData);

            dummyData.Add(new UserRole() { RoleId = 0, UserId = 1 });
            dummyData.Add(new UserRole() { RoleId = 0, UserId = 2 });
            dummyData.Add(new UserRole() { RoleId = 1, UserId = 2 });
            dummyData.Add(new UserRole() { RoleId = 0, UserId = 3 });
            dummyData.Add(new UserRole() { RoleId = 0, UserId = 4 });
        }
    }
}

using System.Collections.Generic;
using IntraWeb.Models.Base;

namespace IntraWeb.Models.Users
{
    /// <summary>
    /// Role dummy repository for testing
    /// </summary>
    /// <seealso cref="IntraWeb.Models.IRoleRepository" />
    /// <seealso cref="IntraWeb.Models.Base.BaseDummyRepository{T}" />
    public class RoleDummyRepository : BaseDummyRepository<Role>, IRoleRepository
    {

        /// <summary>
        /// Initializes the dummy data.
        /// </summary>
        /// <param name="dummyData">The dummy data.</param>
        protected override void InitDummyData(IList<Role> dummyData)
        {
            base.InitDummyData(dummyData);

            dummyData.Add(new Role() { Id = 1, Name = "Admin" });
            dummyData.Add(new Role() { Id = 2, Name = "User" });
        }
    }
}

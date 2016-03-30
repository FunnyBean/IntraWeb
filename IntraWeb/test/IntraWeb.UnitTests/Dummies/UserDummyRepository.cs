using System;
using System.Collections.Generic;
using System.Linq;
using IntraWeb.Models.Base;

namespace IntraWeb.Models.Users
{
    /// <summary>
    /// User dummy repository for testing
    /// </summary>
    /// <seealso cref="IntraWeb.Models.IUserRepository" />
    /// <seealso cref="IntraWeb.Models.Base.BaseDummyRepository{T}" />
    public class UserDummyRepository : BaseDummyRepository<User>, IUserRepository
    {

        /// <summary>
        /// Gets user with specific email.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>User with specific name; if doesn't exist then return null.</returns>
        public User GetItem(string email)
        {
            return this.GetItem(p => p.Name.Equals(email, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Adds the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        public override void Add(User user)
        {
            if (this.GetAll().Any(p => p.Email.Equals(user.Email, System.StringComparison.CurrentCultureIgnoreCase)))
            {
                throw new System.InvalidProgramException($"User with email {user.Name}, already exist.");
            }

            base.Add(user);
        }

        /// <summary>
        /// Initializes the dummy data.
        /// </summary>
        /// <param name="dummyData">The dummy data.</param>
        protected override void InitDummyData(IList<User> dummyData)
        {
            base.InitDummyData(dummyData);

            dummyData.Add(new User() { Id = 0, UserName = "Janko", Surname = "Hraško", Email = "janko.hrasko@gmail.com" });
            dummyData.Add(new User() { Id = 1, UserName = "Juraj", Surname = "Dlhý", Email = "dlhy@gmail.com" });
            dummyData.Add(new User() { Id = 2, UserName = "Ďurko", Surname = "Široký", Email = "sikory@gmail.com" });
            dummyData.Add(new User() { Id = 3, UserName = "Jožko", Surname = "Bistrozraký", Email = "bistrozraky@gmail.com" });
        }
    }
}

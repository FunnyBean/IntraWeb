using IntraWeb.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IntraWeb.Models.Users
{
    /// <summary>
    /// User dummy repository for testing
    /// </summary>
    /// <seealso cref="IntraWeb.Models.IUserRepository" />
    /// <seealso cref="IntraWeb.Models.Base.BaseDummyRepository{T}" />
    public class UserDummyRepository : BaseDummyRepository<User>, IUserRepository
    {

        protected override void InitDummyData(IList<User> dummyData)
        {
            base.InitDummyData(dummyData);

            dummyData.Add(CreateUser("gabo", "Gabriel", "Archanjel", "stano@tanopetko.eu", "21gabo12"));
            dummyData.Add(CreateUser("jankohrasko", "Janko", "Hraško", "janko.hrasko@example.com", "21hrasko12"));
            dummyData.Add(CreateUser("dlhy", "Juraj", "Dlhý", "dlhy@example.com", "21dlhy12"));
            dummyData.Add(CreateUser("siroky", "Ďurko", "Široký", "siroky@example.com", "21siroky12"));
            dummyData.Add(CreateUser("bystrozraky", "Jožko", "Bystrozraký", "bystrozraky@example.com", "21bystrozraky12"));
        }

        private int idGenerator = 0;

        private User CreateUser(string userName, string name, string surname, string email, string password)
        {
            idGenerator++;
            return new User()
            {
                Id = idGenerator,
                Nickname = userName,
                Name = name,
                Surname = surname,
                Email = email,
                Password = password,
            };
        }


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

        public User GetSingleByUsername(string userName)
        {
            return _data.FirstOrDefault(u => u.Nickname.Equals(userName, StringComparison.CurrentCultureIgnoreCase));
        }

        public IEnumerable<Role> GetUserRoles(string username)
        {
            throw new NotImplementedException();
        }

    }
}

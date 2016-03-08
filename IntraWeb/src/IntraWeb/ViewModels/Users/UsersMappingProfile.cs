using AutoMapper;
using IntraWeb.Models;
using IntraWeb.Models.Users;

namespace IntraWeb.ViewModels.Users
{
    /// <summary>
    /// Initialization model to view model and revers mapping for users.
    /// </summary>
    public class UsersMappingProfile : Profile
    {

        protected override void Configure()
        {
            this.CreateMap<User, UserViewModel>().ReverseMap();
            this.CreateMap<Role, RoleViewModel>().ReverseMap();
            this.CreateMap<UserRole, UserRoleViewModel>().ReverseMap();
        }
    }
}

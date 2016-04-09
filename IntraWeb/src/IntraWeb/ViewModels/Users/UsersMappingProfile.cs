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
            this.CreateMap<User, UserViewModel>().ReverseMap().
                AfterMap((m, vm) =>
                {
                    foreach (var role in vm.Roles)
                    {
                        role.UserId = m.Id;
                    }
                });
            this.CreateMap<Role, RoleViewModel>().ReverseMap();
            this.CreateMap<UserRole, UserRoleViewModel>().ReverseMap();
        }
    }
}

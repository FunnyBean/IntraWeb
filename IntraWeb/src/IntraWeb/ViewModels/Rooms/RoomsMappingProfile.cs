using AutoMapper;
using IntraWeb.Models;
using IntraWeb.Models.Rooms;

namespace IntraWeb.ViewModels.Rooms
{
    /// <summary>
    /// Initialization model to view model and revers mapping for rooms.
    /// </summary>
    public class RoomsMappingProfile : Profile
    {

        protected override void Configure()
        {
            this.CreateMap<Room, RoomViewModel>().ReverseMap();
            this.CreateMap<Equipment, EquipmentViewModel>().ReverseMap();
        }
    }
}

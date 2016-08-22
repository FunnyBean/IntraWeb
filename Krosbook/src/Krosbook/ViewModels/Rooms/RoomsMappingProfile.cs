using AutoMapper;
using Krosbook.Models;
using Krosbook.Models.Reservation;
using Krosbook.Models.Rooms;
using Krosbook.Models.Users;
using Krosbook.ViewModels.Users;

namespace Krosbook.ViewModels.Rooms
{
    /// <summary>
    /// Initialization model to view model and revers mapping for rooms.
    /// </summary>
    public class RoomsMappingProfile : Profile
    {

        protected override void Configure()
        {
            this.CreateMap<Room, RoomViewModel>().ReverseMap().
                AfterMap((m, vm) =>
                {
                    foreach (var equipment in vm.Equipment)
                    {
                        equipment.RoomId = m.Id;
                    }
                });
            this.CreateMap<Equipment, EquipmentViewModel>().ReverseMap();
            this.CreateMap<RoomEquipment, RoomEquipmentViewModel>().
                ForMember(vm => vm.Description, map => map.MapFrom(m => m.Equipment.Description)).
                ReverseMap();



            this.CreateMap<Room, RoomViewModel>().ReverseMap();

            this.CreateMap<User, UserViewModel>().ReverseMap();

            this.CreateMap<RoomReservation, RoomReservationViewModel>().ReverseMap();
             //   ForMember(vm => vm.dateTime, map => map.MapFrom(m => m.User.)).
            //    ReverseMap();
        }
    }
}

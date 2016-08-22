using IntraWeb.Models;

namespace IntraWeb.ViewModels.Administration
{
    /// <summary>
    /// Class for initialize model to view model and revers mapping in Administration section.
    /// </summary>
    public static class AdministrationModelMapping
    {
        /// <summary>
        /// Configures mapping for Room.
        /// </summary>
        public static void ConfigureRoomMapping()
        {
            AutoMapper.Mapper.Initialize(conf =>
            {
                conf.CreateMap<Room, RoomViewModel>().ReverseMap();
            });
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace IntraWeb.ViewModels.Users
{
    /// <summary>
    /// Role View Model for Role model
    /// </summary>
    public class RoleViewModel
    {
        [Required()]
        public int Id { get; set; }

        [Required()]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}

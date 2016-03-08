using IntraWeb.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace IntraWeb.Models.Users
{
    /// <summary>
    /// Model, which represent UserRole.
    /// </summary>
    public class UserRole : IModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}

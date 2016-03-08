using IntraWeb.Models.Base;

namespace IntraWeb.Models.Users
{
    /// <summary>
    /// Model, which represent Role.
    /// </summary>
    public class Role : IModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

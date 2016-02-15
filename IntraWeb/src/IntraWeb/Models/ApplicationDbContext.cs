using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;

namespace IntraWeb.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
        {
            Database.EnsureCreated();
        }       

        /// <summary>
        /// DbSet for rooms.
        /// </summary>
        public DbSet<Room> Rooms { get; set; }

        /// <summary>
        /// Gets or sets the equipments.
        /// </summary>
        public DbSet<Equipment> Equipments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            OnRoomModelCreating(builder);
        }

        private void OnRoomModelCreating(ModelBuilder builder)
        {
            builder.Entity<Room>().HasIndex(r => r.Name).IsUnique();
            builder.Entity<Equipment>().HasIndex(e => e.Description).IsUnique();

            builder.Entity<RoomEquipment>()
                .HasOne(re => re.Equipment)
                .WithMany(e => e.Rooms)
                .HasForeignKey(re => re.EquipmentId);

            builder.Entity<RoomEquipment>()
                .HasOne(re => re.Room)
                .WithMany(r => r.Equipments)
                .HasForeignKey(re => re.RoomId);
        }
    }
}

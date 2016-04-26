using IntraWeb.Models.Rooms;
using IntraWeb.Models.Users;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;

namespace IntraWeb.Models
{
    public class ApplicationDbContext : DbContext
    {

        #region Constructor

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        #endregion


        #region DbSets

        /// <summary>
        /// DbSet for rooms.
        /// </summary>
        public DbSet<Room> Rooms { get; set; }

        /// <summary>
        /// Gets or sets the equipment.
        /// </summary>
        public DbSet<Equipment> Equipment { get; set; }

        /// <summary>
        /// Gets or sets the room equipments.
        /// </summary>
        public DbSet<RoomEquipment> RoomEquipments { get; set; }

        /// <summary>
        /// DbSet for users.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// DbSet for roles.
        /// </summary>
        public DbSet<Role> Roles { get; set; }

        /// <summary>
        /// DbSet for user's roles.
        /// </summary>
        public DbSet<UserRole> UserRoles { get; set; }

        #endregion


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            OnRoomModelCreating(builder);
            OnUserModelCreating(builder);
        }

        private void OnRoomModelCreating(ModelBuilder builder)
        {
            builder.Entity<Room>().HasIndex(r => r.Name).IsUnique();
            builder.Entity<Equipment>().HasIndex(e => e.Description).IsUnique();

            builder.Entity<RoomEquipment>().HasKey(re => new { re.EquipmentId, re.RoomId });

            builder.Entity<RoomEquipment>()
                .HasOne(re => re.Equipment)
                .WithMany(e => e.Rooms)
                .HasForeignKey(re => re.EquipmentId);

            builder.Entity<RoomEquipment>()
                .HasOne(re => re.Room)
                .WithMany(r => r.Equipment)
                .HasForeignKey(re => re.RoomId);
        }

        private void OnUserModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().HasIndex(u => u.UserName).IsUnique();
            builder.Entity<User>().HasIndex(u => u.Email).IsUnique();

            builder.Entity<Role>().HasIndex(r => r.Name).IsUnique();

            builder.Entity<UserRole>().HasKey(x => new { x.UserId, x.RoleId });

            builder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(ur => ur.Roles)
                .HasForeignKey(ur => ur.UserId);
            builder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(ur => ur.Users)
                .HasForeignKey(ur => ur.RoleId);
        }
    }

}

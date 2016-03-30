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
        /// Gets or sets the equipments.
        /// </summary>
        public DbSet<Equipment> Equipments { get; set; }

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
                .WithMany(r => r.Equipments)
                .HasForeignKey(re => re.RoomId);
        }

        private void OnUserModelCreating(ModelBuilder builder)
        {
            // User
            builder.Entity<User>().Property(u => u.UserName).HasMaxLength(100);
            builder.Entity<User>().Property(u => u.Email).IsRequired().HasMaxLength(200);
            builder.Entity<User>().Property(u => u.Name).HasMaxLength(100);
            builder.Entity<User>().Property(u => u.Surname).HasMaxLength(100);
            builder.Entity<User>().Property(u => u.HashedPassword).HasMaxLength(200);
            builder.Entity<User>().Property(u => u.Salt).HasMaxLength(50);

            // Role
            builder.Entity<Role>().Property(r => r.Name).IsRequired().HasMaxLength(50);

            // UserRole
            builder.Entity<UserRole>().Property(ur => ur.UserId).IsRequired();
            builder.Entity<UserRole>().Property(ur => ur.RoleId).IsRequired();
            builder.Entity<UserRole>().HasKey(x => new { x.UserId, x.RoleId });
        }
    }
}

using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using IntraWeb.Models;

namespace IntraWeb.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20160417174951_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("IntraWeb.Models.Rooms.Equipment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 100);

                    b.HasKey("Id");

                    b.HasIndex("Description")
                        .IsUnique();
                });

            modelBuilder.Entity("IntraWeb.Models.Rooms.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 25);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();
                });

            modelBuilder.Entity("IntraWeb.Models.Rooms.RoomEquipment", b =>
                {
                    b.Property<int>("EquipmentId");

                    b.Property<int>("RoomId");

                    b.Property<decimal>("Amount");

                    b.HasKey("EquipmentId", "RoomId");
                });

            modelBuilder.Entity("IntraWeb.Models.Users.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.HasKey("Id");
                });

            modelBuilder.Entity("IntraWeb.Models.Users.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 200);

                    b.Property<string>("HashedPassword")
                        .HasAnnotation("MaxLength", 200);

                    b.Property<bool>("IsLocked");

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("Nickname")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<byte[]>("Photo");

                    b.Property<string>("Salt")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("Surname")
                        .HasAnnotation("MaxLength", 100);

                    b.HasKey("Id");
                });

            modelBuilder.Entity("IntraWeb.Models.Users.UserRole", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("RoleId");

                    b.HasKey("UserId", "RoleId");
                });

            modelBuilder.Entity("IntraWeb.Models.Rooms.RoomEquipment", b =>
                {
                    b.HasOne("IntraWeb.Models.Rooms.Equipment")
                        .WithMany()
                        .HasForeignKey("EquipmentId");

                    b.HasOne("IntraWeb.Models.Rooms.Room")
                        .WithMany()
                        .HasForeignKey("RoomId");
                });

            modelBuilder.Entity("IntraWeb.Models.Users.UserRole", b =>
                {
                    b.HasOne("IntraWeb.Models.Users.Role")
                        .WithMany()
                        .HasForeignKey("RoleId");

                    b.HasOne("IntraWeb.Models.Users.User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });
        }
    }
}

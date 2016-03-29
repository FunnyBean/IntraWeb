using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using IntraWeb.Models;

namespace IntraWeb.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20160215203315_AddRoomEquipments")]
    partial class AddRoomEquipments
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);       

            modelBuilder.Entity("IntraWeb.Models.Equipment", b =>
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

            modelBuilder.Entity("IntraWeb.Models.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();
                });

            modelBuilder.Entity("IntraWeb.Models.RoomEquipment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("EquipmentId");

                    b.Property<int>("RoomId");

                    b.HasKey("Id");
                });     
        }
    }
}

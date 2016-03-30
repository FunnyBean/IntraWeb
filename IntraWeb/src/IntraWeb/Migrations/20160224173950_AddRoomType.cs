using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace IntraWeb.Migrations
{
    public partial class AddRoomType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "RoomEquipment",
                nullable: false,
                defaultValue: 0m);
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Room",
                nullable: true);

            migrationBuilder.Sql("Update Room Set Type = 'default'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "Amount", table: "RoomEquipment");
            migrationBuilder.DropColumn(name: "Type", table: "Room");
        }
    }
}

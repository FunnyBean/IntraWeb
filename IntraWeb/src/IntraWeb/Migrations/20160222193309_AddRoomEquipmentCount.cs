using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace IntraWeb.Migrations
{
    public partial class AddRoomEquipmentCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "RoomEquipment",
                nullable: false,
                defaultValue: 0);         
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {         
            migrationBuilder.DropColumn(name: "Count", table: "RoomEquipment");          
        }
    }
}

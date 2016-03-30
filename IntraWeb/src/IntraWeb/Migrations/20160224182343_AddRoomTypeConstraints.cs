using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace IntraWeb.Migrations
{
    public partial class AddRoomTypeConstraints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Room",
                nullable: false);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Room",
                nullable: true);
        }
    }
}

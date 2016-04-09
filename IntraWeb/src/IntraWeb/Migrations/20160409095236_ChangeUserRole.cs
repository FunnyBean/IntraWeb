using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.Metadata;

namespace IntraWeb.Migrations
{
    public partial class ChangeUserRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_RoomEquipment_Equipment_EquipmentId", table: "RoomEquipment");
            migrationBuilder.DropForeignKey(name: "FK_RoomEquipment_Room_RoomId", table: "RoomEquipment");
            migrationBuilder.DropForeignKey(name: "FK_UserRole_Role_RoleId", table: "UserRole");
            migrationBuilder.DropForeignKey(name: "FK_UserRole_User_UserId", table: "UserRole");
            migrationBuilder.DropColumn(name: "Id", table: "UserRole");
            migrationBuilder.DropColumn(name: "UserName", table: "User");
            migrationBuilder.AddColumn<string>(
                name: "Nickname",
                table: "User",
                nullable: true);
            migrationBuilder.AddForeignKey(
                name: "FK_RoomEquipment_Equipment_EquipmentId",
                table: "RoomEquipment",
                column: "EquipmentId",
                principalTable: "Equipment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_RoomEquipment_Room_RoomId",
                table: "RoomEquipment",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_Role_RoleId",
                table: "UserRole",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_User_UserId",
                table: "UserRole",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_RoomEquipment_Equipment_EquipmentId", table: "RoomEquipment");
            migrationBuilder.DropForeignKey(name: "FK_RoomEquipment_Room_RoomId", table: "RoomEquipment");
            migrationBuilder.DropForeignKey(name: "FK_UserRole_Role_RoleId", table: "UserRole");
            migrationBuilder.DropForeignKey(name: "FK_UserRole_User_UserId", table: "UserRole");
            migrationBuilder.DropColumn(name: "Nickname", table: "User");
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserRole",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "User",
                nullable: true);
            migrationBuilder.AddForeignKey(
                name: "FK_RoomEquipment_Equipment_EquipmentId",
                table: "RoomEquipment",
                column: "EquipmentId",
                principalTable: "Equipment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_RoomEquipment_Room_RoomId",
                table: "RoomEquipment",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_Role_RoleId",
                table: "UserRole",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_User_UserId",
                table: "UserRole",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

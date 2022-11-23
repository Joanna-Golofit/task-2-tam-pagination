using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class Roles : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Roles",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                Created = table.Column<DateTime>(nullable: false),
                Updated = table.Column<DateTime>(nullable: false),
                Name = table.Column<string>(maxLength: 50, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Roles", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "RoleObjects",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                Created = table.Column<DateTime>(nullable: false),
                Updated = table.Column<DateTime>(nullable: false),
                ParentRoleId = table.Column<Guid>(nullable: false),
                AllowBuildingId = table.Column<Guid>(nullable: true),
                AllowFloorId = table.Column<Guid>(nullable: true),
                AllowRoomId = table.Column<Guid>(nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RoleObjects", x => x.Id);
                table.ForeignKey(
                    name: "FK_RoleObjects_Buildings_AllowBuildingId",
                    column: x => x.AllowBuildingId,
                    principalTable: "Buildings",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_RoleObjects_Floors_AllowFloorId",
                    column: x => x.AllowFloorId,
                    principalTable: "Floors",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_RoleObjects_Rooms_AllowRoomId",
                    column: x => x.AllowRoomId,
                    principalTable: "Rooms",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_RoleObjects_Roles_ParentRoleId",
                    column: x => x.ParentRoleId,
                    principalTable: "Roles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "UserRoles",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                Created = table.Column<DateTime>(nullable: false),
                Updated = table.Column<DateTime>(nullable: false),
                Username = table.Column<string>(nullable: false),
                RoleId = table.Column<Guid>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserRoles", x => x.Id);
                table.ForeignKey(
                    name: "FK_UserRoles_Roles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "Roles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_RoleObjects_AllowBuildingId",
            table: "RoleObjects",
            column: "AllowBuildingId");

        migrationBuilder.CreateIndex(
            name: "IX_RoleObjects_AllowFloorId",
            table: "RoleObjects",
            column: "AllowFloorId");

        migrationBuilder.CreateIndex(
            name: "IX_RoleObjects_AllowRoomId",
            table: "RoleObjects",
            column: "AllowRoomId");

        migrationBuilder.CreateIndex(
            name: "IX_RoleObjects_ParentRoleId",
            table: "RoleObjects",
            column: "ParentRoleId");

        migrationBuilder.CreateIndex(
            name: "IX_Roles_Name",
            table: "Roles",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_UserRoles_RoleId_Username",
            table: "UserRoles",
            columns: new[] { "RoleId", "Username" },
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "RoleObjects");

        migrationBuilder.DropTable(
            name: "UserRoles");

        migrationBuilder.DropTable(
            name: "Roles");
    }
}

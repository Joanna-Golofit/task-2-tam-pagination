using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class RemoveRoleObjects : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.UsePreDeploymentScript(this);

		migrationBuilder.DropTable(name: "RoleObjects");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable(
			name: "RoleObjects",
			columns: table => new
			{
				Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
				AllowBuildingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
				AllowFloorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
				AllowRoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
				Created = table.Column<DateTime>(type: "datetime2", nullable: false),
				ParentRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
				Updated = table.Column<DateTime>(type: "datetime2", nullable: false)
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
	}
}

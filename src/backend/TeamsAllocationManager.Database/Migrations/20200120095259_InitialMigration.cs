using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class InitialMigration : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Buildings",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                Created = table.Column<DateTime>(nullable: false),
                Updated = table.Column<DateTime>(nullable: false),
                Name = table.Column<string>(maxLength: 50, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Buildings", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Equipments",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                Created = table.Column<DateTime>(nullable: false),
                Updated = table.Column<DateTime>(nullable: false),
                Name = table.Column<string>(maxLength: 100, nullable: false),
                AdditionalInfo = table.Column<string>(maxLength: 200, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Equipments", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "TeamLeaders",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                Created = table.Column<DateTime>(nullable: false),
                Updated = table.Column<DateTime>(nullable: false),
                Name = table.Column<string>(maxLength: 100, nullable: false),
                Surname = table.Column<string>(maxLength: 100, nullable: false),
                Email = table.Column<string>(maxLength: 100, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TeamLeaders", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Rooms",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                Created = table.Column<DateTime>(nullable: false),
                Updated = table.Column<DateTime>(nullable: false),
                BuildingId = table.Column<Guid>(nullable: false),
                Area = table.Column<decimal>(type: "decimal", nullable: false),
                Name = table.Column<string>(maxLength: 50, nullable: false),
                Floor = table.Column<int>(nullable: false),
                BlueprintNumber = table.Column<string>(maxLength: 50, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Rooms", x => x.Id);
                table.ForeignKey(
                    name: "FK_Rooms_Buildings_BuildingId",
                    column: x => x.BuildingId,
                    principalTable: "Buildings",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Projects",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                Created = table.Column<DateTime>(nullable: false),
                Updated = table.Column<DateTime>(nullable: false),
                Name = table.Column<string>(maxLength: 100, nullable: false),
                TeamLeaderId = table.Column<Guid>(nullable: false),
                EndDate = table.Column<DateTime>(nullable: false),
                PeopleCount = table.Column<int>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Projects", x => x.Id);
                table.ForeignKey(
                    name: "FK_Projects_TeamLeaders_TeamLeaderId",
                    column: x => x.TeamLeaderId,
                    principalTable: "TeamLeaders",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "RoomEquipments",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                Created = table.Column<DateTime>(nullable: false),
                Updated = table.Column<DateTime>(nullable: false),
                RoomId = table.Column<Guid>(nullable: false),
                EquipmentId = table.Column<Guid>(nullable: false),
                Count = table.Column<int>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RoomEquipments", x => x.Id);
                table.ForeignKey(
                    name: "FK_RoomEquipments_Equipments_EquipmentId",
                    column: x => x.EquipmentId,
                    principalTable: "Equipments",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_RoomEquipments_Rooms_RoomId",
                    column: x => x.RoomId,
                    principalTable: "Rooms",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Desks",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                Created = table.Column<DateTime>(nullable: false),
                Updated = table.Column<DateTime>(nullable: false),
                RoomId = table.Column<Guid>(nullable: false),
                ProjectId = table.Column<Guid>(nullable: true),
                Number = table.Column<int>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Desks", x => x.Id);
                table.ForeignKey(
                    name: "FK_Desks_Projects_ProjectId",
                    column: x => x.ProjectId,
                    principalTable: "Projects",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Desks_Rooms_RoomId",
                    column: x => x.RoomId,
                    principalTable: "Rooms",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Desks_ProjectId",
            table: "Desks",
            column: "ProjectId");

        migrationBuilder.CreateIndex(
            name: "IX_Desks_RoomId",
            table: "Desks",
            column: "RoomId");

        migrationBuilder.CreateIndex(
            name: "IX_Projects_TeamLeaderId",
            table: "Projects",
            column: "TeamLeaderId");

        migrationBuilder.CreateIndex(
            name: "IX_RoomEquipments_EquipmentId",
            table: "RoomEquipments",
            column: "EquipmentId");

        migrationBuilder.CreateIndex(
            name: "IX_RoomEquipments_RoomId",
            table: "RoomEquipments",
            column: "RoomId");

        migrationBuilder.CreateIndex(
            name: "IX_Rooms_BuildingId",
            table: "Rooms",
            column: "BuildingId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Desks");

        migrationBuilder.DropTable(
            name: "RoomEquipments");

        migrationBuilder.DropTable(
            name: "Projects");

        migrationBuilder.DropTable(
            name: "Equipments");

        migrationBuilder.DropTable(
            name: "Rooms");

        migrationBuilder.DropTable(
            name: "TeamLeaders");

        migrationBuilder.DropTable(
            name: "Buildings");
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class IntroducingFloorEntity : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Rooms_Buildings_BuildingId",
            table: "Rooms");

        migrationBuilder.DropIndex(
            name: "IX_Rooms_BuildingId",
            table: "Rooms");

        migrationBuilder.DropColumn(
            name: "BuildingId",
            table: "Rooms");

        migrationBuilder.DropColumn(
            name: "Floor",
            table: "Rooms");

        migrationBuilder.AddColumn<Guid>(
            name: "FloorId",
            table: "Rooms",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.CreateTable(
            name: "Floor",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                Created = table.Column<DateTime>(nullable: false),
                Updated = table.Column<DateTime>(nullable: false),
                BuildingId = table.Column<Guid>(nullable: false),
                FloorNumber = table.Column<int>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Floor", x => x.Id);
                table.ForeignKey(
                    name: "FK_Floor_Buildings_BuildingId",
                    column: x => x.BuildingId,
                    principalTable: "Buildings",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Rooms_FloorId",
            table: "Rooms",
            column: "FloorId");

        migrationBuilder.CreateIndex(
            name: "IX_Floor_BuildingId",
            table: "Floor",
            column: "BuildingId");

        migrationBuilder.AddForeignKey(
            name: "FK_Rooms_Floor_FloorId",
            table: "Rooms",
            column: "FloorId",
            principalTable: "Floor",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Rooms_Floor_FloorId",
            table: "Rooms");

        migrationBuilder.DropTable(
            name: "Floor");

        migrationBuilder.DropIndex(
            name: "IX_Rooms_FloorId",
            table: "Rooms");

        migrationBuilder.DropColumn(
            name: "FloorId",
            table: "Rooms");

        migrationBuilder.AddColumn<Guid>(
            name: "BuildingId",
            table: "Rooms",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.AddColumn<int>(
            name: "Floor",
            table: "Rooms",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.CreateIndex(
            name: "IX_Rooms_BuildingId",
            table: "Rooms",
            column: "BuildingId");

        migrationBuilder.AddForeignKey(
            name: "FK_Rooms_Buildings_BuildingId",
            table: "Rooms",
            column: "BuildingId",
            principalTable: "Buildings",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}

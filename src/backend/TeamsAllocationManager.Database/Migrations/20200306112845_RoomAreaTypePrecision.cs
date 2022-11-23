using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class RoomAreaTypePrecision : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Floor_Buildings_BuildingId",
            table: "Floor");

        migrationBuilder.DropForeignKey(
            name: "FK_Rooms_Floor_FloorId",
            table: "Rooms");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Floor",
            table: "Floor");

        migrationBuilder.RenameTable(
            name: "Floor",
            newName: "Floors");

        migrationBuilder.RenameIndex(
            name: "IX_Floor_BuildingId",
            table: "Floors",
            newName: "IX_Floors_BuildingId");

        migrationBuilder.AlterColumn<decimal>(
            name: "Area",
            table: "Rooms",
            type: "decimal(6,2)",
            nullable: false,
            oldClrType: typeof(decimal),
            oldType: "decimal");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Floors",
            table: "Floors",
            column: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Floors_Buildings_BuildingId",
            table: "Floors",
            column: "BuildingId",
            principalTable: "Buildings",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Rooms_Floors_FloorId",
            table: "Rooms",
            column: "FloorId",
            principalTable: "Floors",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Floors_Buildings_BuildingId",
            table: "Floors");

        migrationBuilder.DropForeignKey(
            name: "FK_Rooms_Floors_FloorId",
            table: "Rooms");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Floors",
            table: "Floors");

        migrationBuilder.RenameTable(
            name: "Floors",
            newName: "Floor");

        migrationBuilder.RenameIndex(
            name: "IX_Floors_BuildingId",
            table: "Floor",
            newName: "IX_Floor_BuildingId");

        migrationBuilder.AlterColumn<decimal>(
            name: "Area",
            table: "Rooms",
            type: "decimal",
            nullable: false,
            oldClrType: typeof(decimal),
            oldType: "decimal(6,2)");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Floor",
            table: "Floor",
            column: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Floor_Buildings_BuildingId",
            table: "Floor",
            column: "BuildingId",
            principalTable: "Buildings",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Rooms_Floor_FloorId",
            table: "Rooms",
            column: "FloorId",
            principalTable: "Floor",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}

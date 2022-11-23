using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class UniqueNumberOfDeskInRoom : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_Desks_RoomId",
            table: "Desks");

        migrationBuilder.CreateIndex(
            name: "IX_Desks_RoomId_Number",
            table: "Desks",
            columns: new[] { "RoomId", "Number" },
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_Desks_RoomId_Number",
            table: "Desks");

        migrationBuilder.CreateIndex(
            name: "IX_Desks_RoomId",
            table: "Desks",
            column: "RoomId");
    }
}

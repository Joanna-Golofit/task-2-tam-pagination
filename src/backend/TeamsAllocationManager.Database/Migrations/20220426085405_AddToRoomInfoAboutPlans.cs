using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class AddToRoomInfoAboutPlans : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "RoomPlanInfo",
            table: "Rooms",
            nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "RoomPlanInfo",
            table: "Rooms");
    }
}

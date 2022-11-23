using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamsAllocationManager.Database.Migrations
{
    public partial class equipmentEmployeeHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "EmployeeEquipmentHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "EmployeeEquipmentHistory");
        }
    }
}

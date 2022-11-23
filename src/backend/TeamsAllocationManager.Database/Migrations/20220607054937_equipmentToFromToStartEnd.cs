using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamsAllocationManager.Database.Migrations
{
    public partial class equipmentToFromToStartEnd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "To",
                table: "EmployeeEquipmentHistory",
                newName: "ReservationEnd");

            migrationBuilder.RenameColumn(
                name: "From",
                table: "EmployeeEquipmentHistory",
                newName: "ReservationStart");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReservationStart",
                table: "EmployeeEquipmentHistory",
                newName: "From");

            migrationBuilder.RenameColumn(
                name: "ReservationEnd",
                table: "EmployeeEquipmentHistory",
                newName: "To");
        }
    }
}

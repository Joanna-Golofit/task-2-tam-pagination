using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class UpadeEmployeeWorkingTypeHistoryDBConfig : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_EmployeeWorkingTypeHistory_Employees_EmployeeId",
            table: "EmployeeWorkingTypeHistory");

        migrationBuilder.AddForeignKey(
            name: "FK_EmployeeWorkingTypeHistory_Employees_EmployeeId",
            table: "EmployeeWorkingTypeHistory",
            column: "EmployeeId",
            principalTable: "Employees",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_EmployeeWorkingTypeHistory_Employees_EmployeeId",
            table: "EmployeeWorkingTypeHistory");

        migrationBuilder.AddForeignKey(
            name: "FK_EmployeeWorkingTypeHistory_Employees_EmployeeId",
            table: "EmployeeWorkingTypeHistory",
            column: "EmployeeId",
            principalTable: "Employees",
            principalColumn: "Id");
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class UpdatedDeskHistoryDbConfig : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_EmployeeDeskHistory_Employees_EmployeeId",
            table: "EmployeeDeskHistory");

        migrationBuilder.AddForeignKey(
            name: "FK_EmployeeDeskHistory_Employees_EmployeeId",
            table: "EmployeeDeskHistory",
            column: "EmployeeId",
            principalTable: "Employees",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_EmployeeDeskHistory_Employees_EmployeeId",
            table: "EmployeeDeskHistory");

        migrationBuilder.AddForeignKey(
            name: "FK_EmployeeDeskHistory_Employees_EmployeeId",
            table: "EmployeeDeskHistory",
            column: "EmployeeId",
            principalTable: "Employees",
            principalColumn: "Id");
    }
}

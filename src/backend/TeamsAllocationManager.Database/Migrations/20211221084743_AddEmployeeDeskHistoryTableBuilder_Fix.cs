using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class AddEmployeeDeskHistoryTableBuilder_Fix : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_EmployeeDeskHistory_Desks_DeskId",
            table: "EmployeeDeskHistory");

        migrationBuilder.AddForeignKey(
            name: "FK_EmployeeDeskHistory_Desks_DeskId",
            table: "EmployeeDeskHistory",
            column: "DeskId",
            principalTable: "Desks",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_EmployeeDeskHistory_Desks_DeskId",
            table: "EmployeeDeskHistory");

        migrationBuilder.AddForeignKey(
            name: "FK_EmployeeDeskHistory_Desks_DeskId",
            table: "EmployeeDeskHistory",
            column: "DeskId",
            principalTable: "Desks",
            principalColumn: "Id");
    }
}

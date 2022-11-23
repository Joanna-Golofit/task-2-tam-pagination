using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class DeleteTeamLeaderDerivedClass : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "EmployeeType",
            table: "Employees");

        migrationBuilder.AddColumn<int>(
            name: "Type",
            table: "Employees",
            nullable: false,
            defaultValue: 0);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Type",
            table: "Employees");

        migrationBuilder.AddColumn<int>(
            name: "EmployeeType",
            table: "Employees",
            type: "int",
            nullable: false,
            defaultValue: 0);
    }
}

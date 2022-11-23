using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class EmployeeEntityProjectEntityRemoveIsSynced : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "IsSynced",
            table: "Projects");

        migrationBuilder.DropColumn(
            name: "IsSynced",
            table: "Employees");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "IsSynced",
            table: "Projects",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<bool>(
            name: "IsSynced",
            table: "Employees",
            type: "bit",
            nullable: false,
            defaultValue: false);
    }
}

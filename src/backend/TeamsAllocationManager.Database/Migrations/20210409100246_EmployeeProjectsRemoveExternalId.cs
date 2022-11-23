using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class EmployeeProjectsRemoveExternalId : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "ExternalId",
            table: "EmployeeProjects");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "ExternalId",
            table: "EmployeeProjects",
            type: "int",
            nullable: false,
            defaultValue: 0);
    }
}

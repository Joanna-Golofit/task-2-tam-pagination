using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class RemoveEmployeeProjectRole : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropColumn(
			name: "Role",
			table: "EmployeeProjects");

		migrationBuilder.AddColumn<bool>(
			name: "IsTeamLeaderProjectRole",
			table: "EmployeeProjects",
			nullable: false,
			defaultValue: false);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropColumn(
			name: "IsTeamLeaderProjectRole",
			table: "EmployeeProjects");

		migrationBuilder.AddColumn<int>(
			name: "Role",
			table: "EmployeeProjects",
			type: "int",
			nullable: false,
			defaultValue: 0);
	}
}

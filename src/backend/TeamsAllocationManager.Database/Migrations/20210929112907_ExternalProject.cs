using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class ExternalProject : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AddColumn<bool>(
			name: "IsExternal",
			table: "Projects",
			nullable: false,
			defaultValue: false);

		migrationBuilder.UsePostDeploymentScript(this);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropColumn(
			name: "IsExternal",
			table: "Projects");
	}
}

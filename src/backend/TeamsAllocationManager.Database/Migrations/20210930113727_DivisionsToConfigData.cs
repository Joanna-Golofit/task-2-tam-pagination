using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class DivisionsToConfigData : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.UsePreDeploymentScript(this);

		migrationBuilder.AddColumn<int>(
			name: "DivisionExternalId",
			table: "Projects",
			nullable: true);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropColumn(
			name: "DivisionExternalId",
			table: "Projects");
	}
}

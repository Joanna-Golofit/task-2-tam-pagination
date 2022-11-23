using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class AddITSupportRole : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.UsePreDeploymentScript(this);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{

	}
}

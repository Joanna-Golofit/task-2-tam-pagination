using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class UniqueEmployeeProject : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.UsePreDeploymentScript(this);

		migrationBuilder.DropIndex(
			name: "IX_EmployeeProjects_EmployeeId",
			table: "EmployeeProjects");

		migrationBuilder.CreateIndex(
			name: "IX_EmployeeProjects_EmployeeId_ProjectId",
			table: "EmployeeProjects",
			columns: new[] { "EmployeeId", "ProjectId" },
			unique: true);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropIndex(
			name: "IX_EmployeeProjects_EmployeeId_ProjectId",
			table: "EmployeeProjects");

		migrationBuilder.CreateIndex(
			name: "IX_EmployeeProjects_EmployeeId",
			table: "EmployeeProjects",
			column: "EmployeeId");
	}
}

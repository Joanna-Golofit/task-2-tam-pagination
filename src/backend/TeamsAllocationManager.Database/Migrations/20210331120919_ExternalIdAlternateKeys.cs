using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class ExternalIdAlternateKeys : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_Projects_ExternalId",
            table: "Projects");

        migrationBuilder.DropIndex(
            name: "IX_Employees_ExternalId",
            table: "Employees");

        migrationBuilder.AddUniqueConstraint(
            name: "AK_Projects_ExternalId",
            table: "Projects",
            column: "ExternalId");

        migrationBuilder.AddUniqueConstraint(
            name: "AK_Employees_ExternalId",
            table: "Employees",
            column: "ExternalId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropUniqueConstraint(
            name: "AK_Projects_ExternalId",
            table: "Projects");

        migrationBuilder.DropUniqueConstraint(
            name: "AK_Employees_ExternalId",
            table: "Employees");

        migrationBuilder.CreateIndex(
            name: "IX_Projects_ExternalId",
            table: "Projects",
            column: "ExternalId",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Employees_ExternalId",
            table: "Employees",
            column: "ExternalId",
            unique: true);
    }
}

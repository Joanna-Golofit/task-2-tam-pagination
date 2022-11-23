using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class DropExternalIdKey : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropUniqueConstraint(
            name: "AK_Projects_ExternalId",
            table: "Projects");

        migrationBuilder.DropUniqueConstraint(
            name: "AK_Employees_ExternalId",
            table: "Employees");

        migrationBuilder.AlterColumn<int>(
            name: "ExternalId",
            table: "Projects",
            nullable: true,
            oldClrType: typeof(int),
            oldType: "int");

        migrationBuilder.AlterColumn<int>(
            name: "ExternalId",
            table: "Employees",
            nullable: true,
            oldClrType: typeof(int),
            oldType: "int");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<int>(
            name: "ExternalId",
            table: "Projects",
            type: "int",
            nullable: false,
            oldClrType: typeof(int),
            oldNullable: true);

        migrationBuilder.AlterColumn<int>(
            name: "ExternalId",
            table: "Employees",
            type: "int",
            nullable: false,
            oldClrType: typeof(int),
            oldNullable: true);

        migrationBuilder.AddUniqueConstraint(
            name: "AK_Projects_ExternalId",
            table: "Projects",
            column: "ExternalId");

        migrationBuilder.AddUniqueConstraint(
            name: "AK_Employees_ExternalId",
            table: "Employees",
            column: "ExternalId");
    }
}

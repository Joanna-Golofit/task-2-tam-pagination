using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class WorkspaceTypeNullable : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<int>(
            name: "WorkspaceType",
            table: "Employees",
            nullable: true,
            oldClrType: typeof(int),
            oldType: "int");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<int>(
            name: "WorkspaceType",
            table: "Employees",
            type: "int",
            nullable: false,
            oldClrType: typeof(int),
            oldNullable: true);
    }
}

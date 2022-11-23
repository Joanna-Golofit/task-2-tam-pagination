using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class IsSyncedAndUserLoginColumns : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "IsSynced",
            table: "Projects",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<bool>(
            name: "IsSynced",
            table: "Employees",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<string>(
            name: "UserLogin",
            table: "Employees",
            maxLength: 255,
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_Employees_UserLogin",
            table: "Employees",
            column: "UserLogin",
            unique: true,
            filter: "[UserLogin] IS NOT NULL");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_Employees_UserLogin",
            table: "Employees");

        migrationBuilder.DropColumn(
            name: "IsSynced",
            table: "Projects");

        migrationBuilder.DropColumn(
            name: "IsSynced",
            table: "Employees");

        migrationBuilder.DropColumn(
            name: "UserLogin",
            table: "Employees");
    }
}

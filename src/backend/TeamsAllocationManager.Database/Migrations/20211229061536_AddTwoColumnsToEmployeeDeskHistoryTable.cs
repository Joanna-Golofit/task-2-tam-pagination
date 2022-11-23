using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class AddTwoColumnsToEmployeeDeskHistoryTable : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<DateTime>(
            name: "From",
            table: "EmployeeDeskHistory",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.AddColumn<DateTime>(
            name: "To",
            table: "EmployeeDeskHistory",
            nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "From",
            table: "EmployeeDeskHistory");

        migrationBuilder.DropColumn(
            name: "To",
            table: "EmployeeDeskHistory");
    }
}

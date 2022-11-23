using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class AddTwoColumnsToEmployeeWorkingTypeHistoryTable : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<DateTime>(
            name: "From",
            table: "EmployeeWorkingTypeHistory",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.AddColumn<DateTime>(
            name: "To",
            table: "EmployeeWorkingTypeHistory",
            nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "From",
            table: "EmployeeWorkingTypeHistory");

        migrationBuilder.DropColumn(
            name: "To",
            table: "EmployeeWorkingTypeHistory");
    }
}

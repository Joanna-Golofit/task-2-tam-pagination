using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class AddEmployeeWorkingTypeHistoryTable : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "EmployeeWorkingTypeHistory",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                Created = table.Column<DateTime>(nullable: false),
                Updated = table.Column<DateTime>(nullable: false),
                EmployeeId = table.Column<Guid>(nullable: false),
                WorkspaceType = table.Column<int>(nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_EmployeeWorkingTypeHistory", x => x.Id);
                table.ForeignKey(
                    name: "FK_EmployeeWorkingTypeHistory_Employees_EmployeeId",
                    column: x => x.EmployeeId,
                    principalTable: "Employees",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_EmployeeWorkingTypeHistory_EmployeeId",
            table: "EmployeeWorkingTypeHistory",
            column: "EmployeeId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "EmployeeWorkingTypeHistory");
    }
}

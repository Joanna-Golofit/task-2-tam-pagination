using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class AddEmployeeDeskHistoryTableBuilder : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "EmployeeDeskHistory",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                Created = table.Column<DateTime>(nullable: false),
                Updated = table.Column<DateTime>(nullable: false),
                EmployeeId = table.Column<Guid>(nullable: true),
                DeskId = table.Column<Guid>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_EmployeeDeskHistory", x => x.Id);
                table.ForeignKey(
                    name: "FK_EmployeeDeskHistory_Desks_DeskId",
                    column: x => x.DeskId,
                    principalTable: "Desks",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_EmployeeDeskHistory_Employees_EmployeeId",
                    column: x => x.EmployeeId,
                    principalTable: "Employees",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_EmployeeDeskHistory_DeskId",
            table: "EmployeeDeskHistory",
            column: "DeskId");

        migrationBuilder.CreateIndex(
            name: "IX_EmployeeDeskHistory_EmployeeId",
            table: "EmployeeDeskHistory",
            column: "EmployeeId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "EmployeeDeskHistory");
    }
}

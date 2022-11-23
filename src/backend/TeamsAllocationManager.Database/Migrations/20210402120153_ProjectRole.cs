using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class ProjectRole : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Projects_Employees_TeamLeaderExternalId",
            table: "Projects");

        migrationBuilder.DropIndex(
            name: "IX_Projects_TeamLeaderExternalId",
            table: "Projects");

        migrationBuilder.AddColumn<Guid>(
            name: "TeamLeaderId",
            table: "Projects",
            nullable: true);

        migrationBuilder.AddColumn<int>(
            name: "Role",
            table: "EmployeeProjects",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.CreateIndex(
            name: "IX_Projects_TeamLeaderId",
            table: "Projects",
            column: "TeamLeaderId");

        migrationBuilder.AddForeignKey(
            name: "FK_Projects_Employees_TeamLeaderId",
            table: "Projects",
            column: "TeamLeaderId",
            principalTable: "Employees",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Projects_Employees_TeamLeaderId",
            table: "Projects");

        migrationBuilder.DropIndex(
            name: "IX_Projects_TeamLeaderId",
            table: "Projects");

        migrationBuilder.DropColumn(
            name: "TeamLeaderId",
            table: "Projects");

        migrationBuilder.DropColumn(
            name: "Role",
            table: "EmployeeProjects");

        migrationBuilder.CreateIndex(
            name: "IX_Projects_TeamLeaderExternalId",
            table: "Projects",
            column: "TeamLeaderExternalId");

        migrationBuilder.AddForeignKey(
            name: "FK_Projects_Employees_TeamLeaderExternalId",
            table: "Projects",
            column: "TeamLeaderExternalId",
            principalTable: "Employees",
            principalColumn: "ExternalId",
            onDelete: ReferentialAction.Restrict);
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class TeamLeaderAndDesks : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Projects_Employees_EmployeeEntityId",
            table: "Projects");

        migrationBuilder.DropForeignKey(
            name: "FK_Projects_Employees_TeamLeaderEntityId",
            table: "Projects");

        migrationBuilder.DropIndex(
            name: "IX_Projects_EmployeeEntityId",
            table: "Projects");

        migrationBuilder.DropIndex(
            name: "IX_Projects_TeamLeaderEntityId",
            table: "Projects");

        migrationBuilder.DropColumn(
            name: "EmployeeEntityId",
            table: "Projects");

        migrationBuilder.DropColumn(
            name: "TeamLeaderEntityId",
            table: "Projects");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "EmployeeEntityId",
            table: "Projects",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "TeamLeaderEntityId",
            table: "Projects",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_Projects_EmployeeEntityId",
            table: "Projects",
            column: "EmployeeEntityId");

        migrationBuilder.CreateIndex(
            name: "IX_Projects_TeamLeaderEntityId",
            table: "Projects",
            column: "TeamLeaderEntityId");

        migrationBuilder.AddForeignKey(
            name: "FK_Projects_Employees_EmployeeEntityId",
            table: "Projects",
            column: "EmployeeEntityId",
            principalTable: "Employees",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_Projects_Employees_TeamLeaderEntityId",
            table: "Projects",
            column: "TeamLeaderEntityId",
            principalTable: "Employees",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }
}

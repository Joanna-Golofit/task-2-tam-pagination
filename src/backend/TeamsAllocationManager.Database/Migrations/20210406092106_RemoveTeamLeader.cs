using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class RemoveTeamLeader : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Projects_Employees_TeamLeaderId",
            table: "Projects");

        migrationBuilder.DropIndex(
            name: "IX_Projects_TeamLeaderId",
            table: "Projects");

        migrationBuilder.DropColumn(
            name: "TeamLeaderExternalId",
            table: "Projects");

        migrationBuilder.DropColumn(
            name: "TeamLeaderId",
            table: "Projects");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "TeamLeaderExternalId",
            table: "Projects",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<Guid>(
            name: "TeamLeaderId",
            table: "Projects",
            type: "uniqueidentifier",
            nullable: true);

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
}

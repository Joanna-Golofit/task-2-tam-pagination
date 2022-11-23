using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class DeskRemoveProjectAndProjectRemoveDesks : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Desks_Projects_ProjectId",
            table: "Desks");

        migrationBuilder.DropIndex(
            name: "IX_Desks_ProjectId",
            table: "Desks");

        migrationBuilder.DropColumn(
            name: "ProjectId",
            table: "Desks");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "ProjectId",
            table: "Desks",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_Desks_ProjectId",
            table: "Desks",
            column: "ProjectId");

        migrationBuilder.AddForeignKey(
            name: "FK_Desks_Projects_ProjectId",
            table: "Desks",
            column: "ProjectId",
            principalTable: "Projects",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }
}

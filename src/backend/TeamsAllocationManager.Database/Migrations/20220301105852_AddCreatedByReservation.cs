using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class AddCreatedByReservation : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "CreatedById",
            table: "DeskReservations",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_DeskReservations_CreatedById",
            table: "DeskReservations",
            column: "CreatedById");

        migrationBuilder.AddForeignKey(
            name: "FK_DeskReservations_Employees_CreatedById",
            table: "DeskReservations",
            column: "CreatedById",
            principalTable: "Employees",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_DeskReservations_Employees_CreatedById",
            table: "DeskReservations");

        migrationBuilder.DropIndex(
            name: "IX_DeskReservations_CreatedById",
            table: "DeskReservations");

        migrationBuilder.DropColumn(
            name: "CreatedById",
            table: "DeskReservations");
    }
}

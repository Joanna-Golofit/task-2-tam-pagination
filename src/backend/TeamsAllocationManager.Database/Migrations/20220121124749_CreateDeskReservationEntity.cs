using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class CreateDeskReservationEntity : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "DeskReservations",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                Created = table.Column<DateTime>(nullable: false),
                Updated = table.Column<DateTime>(nullable: false),
                DeskId = table.Column<Guid>(nullable: false),
                EmployeeId = table.Column<Guid>(nullable: false),
                ReservationStart = table.Column<DateTime>(nullable: false),
                ReservationEnd = table.Column<DateTime>(nullable: false),
                IsSchedule = table.Column<bool>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_DeskReservations", x => x.Id);
                table.ForeignKey(
                    name: "FK_DeskReservations_Desks_DeskId",
                    column: x => x.DeskId,
                    principalTable: "Desks",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_DeskReservations_Employees_EmployeeId",
                    column: x => x.EmployeeId,
                    principalTable: "Employees",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_DeskReservations_DeskId",
            table: "DeskReservations",
            column: "DeskId");

        migrationBuilder.CreateIndex(
            name: "IX_DeskReservations_EmployeeId",
            table: "DeskReservations",
            column: "EmployeeId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "DeskReservations");
    }
}

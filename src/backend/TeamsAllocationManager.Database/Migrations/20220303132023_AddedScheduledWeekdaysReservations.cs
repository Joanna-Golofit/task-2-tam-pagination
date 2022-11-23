using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class AddedScheduledWeekdaysReservations : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
		var employeeDeskMigrationScript = @"
DECLARE @employeesToMigrate TABLE (employeeId UNIQUEIDENTIFIER, deskId UNIQUEIDENTIFIER)

INSERT INTO @employeesToMigrate
SELECT [EmployeeId], [Id] from [Desks] d where d.[EmployeeId] IS NOT NULL

WHILE EXISTS (SELECT 1 FROM @employeesToMigrate)
BEGIN

INSERT INTO [DeskReservations] (
	Id,
	Created,
	Updated,
	DeskId,
	EmployeeId,
	ReservationStart,
	ReservationEnd,
	IsSchedule
)
SELECT TOP 1
	NEWID(),
	GETDATE(),
	GETDATE(),
	deskId,
	employeeId,
	GETDATE(),
	NULL,
	1
FROM @employeesToMigrate

DELETE TOP(1) FROM @employeesToMigrate
		
END";

		migrationBuilder.AlterColumn<DateTime>(
			name: "ReservationEnd",
			table: "DeskReservations",
			nullable: true,
			oldClrType: typeof(DateTime),
			oldType: "datetime2");

		migrationBuilder.Sql(employeeDeskMigrationScript);

		migrationBuilder.DropForeignKey(
            name: "FK_Desks_Employees_EmployeeId",
            table: "Desks");

        migrationBuilder.DropIndex(
            name: "IX_Desks_EmployeeId",
            table: "Desks");

        migrationBuilder.DropColumn(
            name: "EmployeeId",
            table: "Desks");

        migrationBuilder.AddColumn<string>(
            name: "ScheduledWeekdays",
            table: "DeskReservations",
            nullable: false,
            defaultValue: "");

		migrationBuilder.Sql("UPDATE [DeskReservations] SET [ScheduledWeekdays] = 'Monday,Tuesday,Wednesday,Thursday,Friday' where [IsSchedule] = 1");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "ScheduledWeekdays",
            table: "DeskReservations");

        migrationBuilder.AddColumn<Guid>(
            name: "EmployeeId",
            table: "Desks",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AlterColumn<DateTime>(
            name: "ReservationEnd",
            table: "DeskReservations",
            type: "datetime2",
            nullable: false,
            oldClrType: typeof(DateTime),
            oldNullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_Desks_EmployeeId",
            table: "Desks",
            column: "EmployeeId");

        migrationBuilder.AddForeignKey(
            name: "FK_Desks_Employees_EmployeeId",
            table: "Desks",
            column: "EmployeeId",
            principalTable: "Employees",
            principalColumn: "Id");
    }
}

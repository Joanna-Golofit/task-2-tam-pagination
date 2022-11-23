using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class EmployeeEntity : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Projects_TeamLeaders_TeamLeaderId",
            table: "Projects");

        migrationBuilder.DropPrimaryKey(
            name: "PK_TeamLeaders",
            table: "TeamLeaders");

        migrationBuilder.RenameTable(
            name: "TeamLeaders",
            newName: "Employees");

        migrationBuilder.RenameIndex(
            name: "IX_TeamLeaders_Email",
            table: "Employees",
            newName: "IX_Employees_Email");

        migrationBuilder.AddColumn<Guid>(
            name: "EmployeeEntityId",
            table: "Projects",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "ExternalId",
            table: "Projects",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.AddColumn<Guid>(
            name: "TeamLeaderEntityId",
            table: "Projects",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "EmployeeId",
            table: "Desks",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Discriminator",
            table: "Employees",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<Guid>(
            name: "ExternalId",
            table: "Employees",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.AddColumn<string>(
            name: "PhoneNumber",
            table: "Employees",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddPrimaryKey(
            name: "PK_Employees",
            table: "Employees",
            column: "Id");

        migrationBuilder.CreateTable(
            name: "EmployeeProjects",
            columns: table => new
            {
                Id = table.Column<Guid>(nullable: false),
                Created = table.Column<DateTime>(nullable: false),
                Updated = table.Column<DateTime>(nullable: false),
                EmployeeId = table.Column<Guid>(nullable: false),
                ProjectId = table.Column<Guid>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_EmployeeProjects", x => x.Id);
                table.ForeignKey(
                    name: "FK_EmployeeProjects_Employees_EmployeeId",
                    column: x => x.EmployeeId,
                    principalTable: "Employees",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_EmployeeProjects_Projects_ProjectId",
                    column: x => x.ProjectId,
                    principalTable: "Projects",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Projects_EmployeeEntityId",
            table: "Projects",
            column: "EmployeeEntityId");

        migrationBuilder.CreateIndex(
            name: "IX_Projects_TeamLeaderEntityId",
            table: "Projects",
            column: "TeamLeaderEntityId");

        migrationBuilder.CreateIndex(
            name: "IX_Desks_EmployeeId",
            table: "Desks",
            column: "EmployeeId");

        migrationBuilder.CreateIndex(
            name: "IX_EmployeeProjects_EmployeeId",
            table: "EmployeeProjects",
            column: "EmployeeId");

        migrationBuilder.CreateIndex(
            name: "IX_EmployeeProjects_ProjectId",
            table: "EmployeeProjects",
            column: "ProjectId");

        migrationBuilder.AddForeignKey(
            name: "FK_Desks_Employees_EmployeeId",
            table: "Desks",
            column: "EmployeeId",
            principalTable: "Employees",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

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
            name: "FK_Desks_Employees_EmployeeId",
            table: "Desks");

        migrationBuilder.DropForeignKey(
            name: "FK_Projects_Employees_EmployeeEntityId",
            table: "Projects");

        migrationBuilder.DropForeignKey(
            name: "FK_Projects_Employees_TeamLeaderEntityId",
            table: "Projects");

        migrationBuilder.DropForeignKey(
            name: "FK_Projects_Employees_TeamLeaderId",
            table: "Projects");

        migrationBuilder.DropTable(
            name: "EmployeeProjects");

        migrationBuilder.DropIndex(
            name: "IX_Projects_EmployeeEntityId",
            table: "Projects");

        migrationBuilder.DropIndex(
            name: "IX_Projects_TeamLeaderEntityId",
            table: "Projects");

        migrationBuilder.DropIndex(
            name: "IX_Desks_EmployeeId",
            table: "Desks");

        migrationBuilder.DropPrimaryKey(
            name: "PK_Employees",
            table: "Employees");

        migrationBuilder.DropColumn(
            name: "EmployeeEntityId",
            table: "Projects");

        migrationBuilder.DropColumn(
            name: "ExternalId",
            table: "Projects");

        migrationBuilder.DropColumn(
            name: "TeamLeaderEntityId",
            table: "Projects");

        migrationBuilder.DropColumn(
            name: "EmployeeId",
            table: "Desks");

        migrationBuilder.DropColumn(
            name: "Discriminator",
            table: "Employees");

        migrationBuilder.DropColumn(
            name: "ExternalId",
            table: "Employees");

        migrationBuilder.DropColumn(
            name: "PhoneNumber",
            table: "Employees");

        migrationBuilder.RenameTable(
            name: "Employees",
            newName: "TeamLeaders");

        migrationBuilder.RenameIndex(
            name: "IX_Employees_Email",
            table: "TeamLeaders",
            newName: "IX_TeamLeaders_Email");

        migrationBuilder.AddPrimaryKey(
            name: "PK_TeamLeaders",
            table: "TeamLeaders",
            column: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_Projects_TeamLeaders_TeamLeaderId",
            table: "Projects",
            column: "TeamLeaderId",
            principalTable: "TeamLeaders",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}

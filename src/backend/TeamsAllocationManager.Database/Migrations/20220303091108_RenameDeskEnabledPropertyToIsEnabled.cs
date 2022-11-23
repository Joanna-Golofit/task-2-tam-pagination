using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class RenameDeskEnabledPropertyToIsEnabled : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Enabled",
            table: "Desks");

        migrationBuilder.AddColumn<bool>(
            name: "IsEnabled",
            table: "Desks",
            nullable: false,
            defaultValue: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "IsEnabled",
            table: "Desks");

        migrationBuilder.AddColumn<bool>(
            name: "Enabled",
            table: "Desks",
            type: "bit",
            nullable: false,
            defaultValue: true);
    }
}

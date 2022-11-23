using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class DropExternalIdConstraints : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AlterColumn<int>(
			name: "ExternalId",
			table: "Employees",
			nullable: true,
			oldNullable: false);

		migrationBuilder.AlterColumn<int>(
			name: "ExternalId",
			table: "Projects",
			nullable: true,
			oldNullable: false);
	}

    protected override void Down(MigrationBuilder migrationBuilder)
    {
	    migrationBuilder.AlterColumn<int>(
		    name: "ExternalId",
		    table: "Employees",
		    nullable: false,
		    oldNullable: true);

	    migrationBuilder.AlterColumn<int>(
		    name: "ExternalId",
		    table: "Projects",
		    nullable: false,
		    oldNullable: true);
	}
}

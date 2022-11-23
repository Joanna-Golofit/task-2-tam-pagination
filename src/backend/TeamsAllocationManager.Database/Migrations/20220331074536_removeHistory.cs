using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class removeHistory : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
	    migrationBuilder.Sql("DELETE FROM [dbo].[EmployeeDeskHistory]");
    }

	protected override void Down(MigrationBuilder migrationBuilder)
    {
    }
}

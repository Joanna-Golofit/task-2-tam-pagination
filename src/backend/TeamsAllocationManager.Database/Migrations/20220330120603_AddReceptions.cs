using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsAllocationManager.Database.Migrations;

public partial class AddReceptions : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
	    migrationBuilder.Sql(@"DECLARE @RECEPTIONTOMIGRATE TABLE(FLOORID UNIQUEIDENTIFIER)
	    DECLARE @GUIDRECEPPTION UNIQUEIDENTIFIER = NEWID()
	    INSERT INTO
	    @RECEPTIONTOMIGRATE
	    SELECT
		    [Floors].Id
	    FROM
		    [Floors]
	    INNER JOIN[Buildings] ON[Buildings].ID = [BuildingId]
	    WHERE
	    [Buildings].NAME = 'F4'
	    AND[Floors].FLOORNUMBER = 0;
	    WHILE EXISTS(
		    SELECT
	    1
	    FROM
		    @RECEPTIONTOMIGRATE
		    ) 
	    BEGIN
		    DECLARE @GUIDRECEPPTIONID UNIQUEIDENTIFIER = (
		    SELECT
			    [Floors].ID
	    FROM
		    [Floors]
	    INNER JOIN[Buildings] ON[Buildings].ID = [BuildingId] WHERE[Buildings].NAME = 'F4'
	    AND[Floors].FLOORNUMBER = 0
		    ) IF NOT EXISTS(
		    SELECT
	    1
	    FROM
		    [Rooms]
	    WHERE
		    [Name] = 'Recepcja'
	    AND[FloorId] = @GUIDRECEPPTIONID
		    )
	    BEGIN
		    INSERT INTO
		    [Rooms](ID, AREA, FLOORID, NAME, CREATED, UPDATED)
	    VALUES
	    (
		    @GUIDRECEPPTION,
		    205,
		    @GUIDRECEPPTIONID,
		    'Recepcja',
		    GETDATE(),
		    GETDATE()
	    )
	    INSERT INTO
		    [Desks] (ID, NUMBER, ROOMID, CREATED, UPDATED)
	    VALUES
		    (NEWID(), 1, @GUIDRECEPPTION, GETDATE(), GETDATE())
	    INSERT INTO
		    [Desks] (ID, NUMBER, ROOMID, CREATED, UPDATED)
	    VALUES
		    (NEWID(), 2, @GUIDRECEPPTION, GETDATE(), GETDATE())
	    END
		    DELETE TOP(1)
	    FROM
		    @RECEPTIONTOMIGRATE
	    END");
	}

    protected override void Down(MigrationBuilder migrationBuilder)
    {

    }
}

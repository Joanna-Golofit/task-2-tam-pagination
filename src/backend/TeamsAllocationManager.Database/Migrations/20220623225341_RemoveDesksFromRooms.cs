using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TeamsAllocationManager.Database.Migrations
{
    public partial class RemoveDesksFromRooms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(
			@"DECLARE @F4BuildingId UNIQUEIDENTIFIER = (SELECT [Id] FROM [dbo].[Buildings] WHERE [Buildings].[Name] = 'F4');
			DECLARE @F4CBuildingId UNIQUEIDENTIFIER = (SELECT [Id] FROM [dbo].[Buildings] WHERE [Buildings].[Name] = 'F4C');

			IF @F4BuildingId IS NOT NULL
			BEGIN
				DECLARE @F4FloorId UNIQUEIDENTIFIER = (SELECT [Id] FROM [dbo].[Floors] WHERE [Floors].[BuildingId] = @F4BuildingId AND [Floors].FloorNumber = '2');
				IF @F4FloorId IS NOT NULL
				BEGIN
				DECLARE @F4RoomIds TABLE(FloorId UNIQUEIDENTIFIER);
				INSERT INTO @F4RoomIds SELECT [Id] FROM [dbo].[Rooms] WHERE [Rooms].[FloorId] = @F4FloorId AND [Rooms].Name IN ('204', '209');
				IF EXISTS (SELECT FloorId FROM @F4RoomIds)
					BEGIN
					DELETE FROM [dbo].[Desks] WHERE [Desks].[RoomId] IN (SELECT FloorId FROM @F4RoomIds);
					END;
				END;
			END;

			IF @F4CBuildingId IS NOT NULL
			BEGIN
				DECLARE @F4CFloorId UNIQUEIDENTIFIER = (SELECT [Id] FROM [dbo].[Floors] WHERE [Floors].[BuildingId] = @F4CBuildingId AND [Floors].FloorNumber = '2');
				IF @F4FloorId IS NOT NULL
				BEGIN
				DECLARE @F4CRoomId UNIQUEIDENTIFIER = (SELECT [Id] FROM [dbo].[Rooms] WHERE [Rooms].[FloorId] = @F4CFloorId AND [Rooms].Name= '225');
				IF @F4CRoomId IS NOT NULL
					BEGIN
					DELETE FROM [dbo].[Desks] WHERE [Desks].[RoomId] = @F4CRoomId;
					END;
				END;
			END;"
			);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

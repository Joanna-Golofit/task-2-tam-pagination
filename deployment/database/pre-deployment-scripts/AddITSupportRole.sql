INSERT INTO [dbo].[Roles] (
	[Id]
	,[Created]
	,[Updated]
	,[Name]
	)
VALUES (
	NEWID()
	,GETDATE()
	,GETDATE()
	,'Standard'
	);
	
INSERT INTO [dbo].[Roles] (
	[Id]
	,[Created]
	,[Updated]
	,[Name]
	)
VALUES (
	NEWID()
	,GETDATE()
	,GETDATE()
	,'ITSupport'
	);
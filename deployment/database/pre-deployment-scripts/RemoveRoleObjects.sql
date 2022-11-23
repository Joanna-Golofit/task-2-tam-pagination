UPDATE [dbo].[Roles]
SET [Name] = 'Admin'
WHERE [Name] = 'admin';

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
	,'TeamLeader'
	);

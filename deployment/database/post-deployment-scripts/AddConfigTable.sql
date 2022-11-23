INSERT INTO [dbo].[Configs] (
	[Id]
	,[Created]
	,[Updated]
	,[Key]
	,[Data]
	)
VALUES (
	NEWID()
	,GETDATE()
	,GETDATE()
	,'IgnoredProjects'
	,'{"711":"Long Leave - Teams","1174":"Long Leave","1796":"Long Leave - PB","1114":"Strefa Sportu i Relaksu"}'
	);
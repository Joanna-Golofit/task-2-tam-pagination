BEGIN TRANSACTION

BEGIN TRY
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
		,'Divisions'
		,'{"157":"Other","1117":"Shared Services","1841":"Ventures","2001":"Nearshoring","2012":"Board","2208":"Unassigned"}'
		);

	COMMIT TRANSACTION
END TRY

BEGIN CATCH
	ROLLBACK TRANSACTION;

	SELECT 'SQL ERROR: ' + ERROR_MESSAGE() AS ErrorMessage;

	THROW
END CATCH

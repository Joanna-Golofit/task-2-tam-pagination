BEGIN TRANSACTION

BEGIN TRY
	-- set Future Processing Healthcare SP. Z O.O. as external one
	UPDATE [dbo].[Projects]
	SET [DivisionExternalId] = 1841 -- Ventures
	WHERE ExternalId = 1000000

	COMMIT TRANSACTION
END TRY

BEGIN CATCH
	ROLLBACK TRANSACTION;

	SELECT 'SQL ERROR: ' + ERROR_MESSAGE() AS ErrorMessage;

	THROW
END CATCH
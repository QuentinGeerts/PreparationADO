CREATE PROCEDURE [dbo].[AddSection]
	@SectionId INT,
	@SectionName NVARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY

		INSERT INTO SECTION (ID, SectionName)
		VALUES (@SectionId, @SectionName);

	END TRY
	BEGIN CATCH

		DECLARE @ErrorMessage NVARCHAR(MAX);
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;

		SELECT 
			@ErrorMessage = ERROR_MESSAGE(), 
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE();

		RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);

	END CATCH

END;
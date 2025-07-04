CREATE PROCEDURE [dbo].[AddTask]
	@Title NVARCHAR(50),
	@Description NVARCHAR(255) = NULL,
	@UserId INT = NULL,
	@ErrorMessage NVARCHAR(4000) OUTPUT,
	@TaskId INT OUTPUT
AS
	BEGIN TRY
		SET NOCOUNT ON;
		SET @ErrorMessage = NULL;

		-- Check if the user exists
		IF @UserId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM [dbo].[User] WHERE [Id] = @UserId AND [IsActive] = 1)
		BEGIN
			SET @ErrorMessage = 'User does not exist or is inactive.';
			RETURN -1;
		END
		
		-- Insert the new task
		INSERT INTO [dbo].[Task] (Title, Description, UserId)
		VALUES (@Title, @Description, @UserId);

		SET @TaskId = SCOPE_IDENTITY();
		RETURN @TaskId;

	END TRY
	BEGIN CATCH
		SET @ErrorMessage = ERROR_MESSAGE();
		DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
		DECLARE @ErrorState INT = ERROR_STATE();
		
		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
	END CATCH;
RETURN 0

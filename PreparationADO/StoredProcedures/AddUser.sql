CREATE PROCEDURE [dbo].[AddUser]
	@UserName NVARCHAR(50),
	@Email NVARCHAR(100),
	@Password NVARCHAR(255),
	@LastName NVARCHAR(50) = NULL,
	@FirstName NVARCHAR(50) = NULL,
	@UserId INT OUTPUT,
	@ErrorMessage NVARCHAR(4000) OUTPUT
AS
	SET NOCOUNT ON;

	BEGIN TRY

		SET @UserId = NULL;
		SET @ErrorMessage = NULL;

		IF EXISTS (SELECT 1 FROM [dbo].[User] WHERE [UserName] = @UserName)
		BEGIN
			SET @ErrorMessage = 'UserName already exists.';
			RETURN -1;
		END

		IF EXISTS (SELECT 1 FROM [dbo].[User] WHERE [Email] = @Email)
		BEGIN
			SET @ErrorMessage = 'Email already exists.';
			RETURN -1;
		END


		INSERT INTO [dbo].[User] (UserName, Email, PasswordHash, LastName, FirstName)
		VALUES (@UserName, @Email, HASHBYTES('SHA2_256', @Password), @LastName, @FirstName);

		SET @UserId = SCOPE_IDENTITY();
		RETURN @UserId;
		
	END TRY
	BEGIN CATCH
		-- Handle the error, log it or rethrow it as needed
		SET @ErrorMessage = ERROR_MESSAGE();
		DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
		DECLARE @ErrorState INT = ERROR_STATE();
		
		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
	END CATCH;
RETURN 0

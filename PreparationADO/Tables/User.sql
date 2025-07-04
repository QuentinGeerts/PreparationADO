CREATE TABLE [dbo].[User]
(
	[Id] INT NOT NULL IDENTITY, 
    [UserName] NVARCHAR(50) NOT NULL,
    [Email] NVARCHAR(100) NOT NULL,
    [PasswordHash] NVARCHAR(255) NOT NULL, 
    [LastName] NVARCHAR(50) NULL, 
    [FirstName] NVARCHAR(50) NULL, 
    [CreatedAt] DATETIME2 NULL DEFAULT GETDATE(), 
    [UpdatedAt] DATETIME2 NULL DEFAULT GETDATE(), 
    [IsActive] BIT NULL DEFAULT 1, 

    CONSTRAINT [PK_User] PRIMARY KEY ([Id]), 
    CONSTRAINT [CK_User_Email_Format] CHECK (Email LIKE '_%@_%.__%'), 
    CONSTRAINT [CK_User_UserName] CHECK (LEN(TRIM(UserName)) >= 3),
)

GO

CREATE TRIGGER [dbo].[TR_User_UpdatedAt]
    ON [dbo].[User]
    AFTER UPDATE
    AS
    BEGIN
        UPDATE [dbo].[User]
        SET [UpdatedAt] = GETDATE()
        WHERE [Id] IN (SELECT DISTINCT [Id] FROM inserted);
    END
GO

CREATE TRIGGER [dbo].[TR_User_IsActive]
    ON [dbo].[User]
    INSTEAD OF DELETE
    AS
    BEGIN
        UPDATE [dbo].[User]
        SET [IsActive] = 0
        WHERE [Id] IN (SELECT DISTINCT [Id] FROM deleted);
    END
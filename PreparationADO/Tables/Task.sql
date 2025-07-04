CREATE TABLE [dbo].[Task]
(
	[Id] INT NOT NULL IDENTITY, 
    [Title] NVARCHAR(50) NOT NULL, 
    [Description] NVARCHAR(255) NULL, 
    [Status] NVARCHAR(20) NOT NULL,
    [CompletedAt] DATETIME2 NULL,

    [UserId] INT NULL,

    [CreatedAt] DATETIME2 NULL DEFAULT GETDATE(), 
    [UpdatedAt] DATETIME2 NULL DEFAULT GETDATE(), 
    [IsActive] BIT NULL DEFAULT 1,  

    CONSTRAINT [PK_Task] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_Task_User] FOREIGN KEY ([UserId]) REFERENCES [User]([Id]),
)

GO

CREATE INDEX [IX_Task_UserId] ON [dbo].[Task] ([UserId])

GO

CREATE TRIGGER [dbo].[TR_Task_IsActive]
    ON [dbo].[Task]
    INSTEAD OF DELETE
    AS
    BEGIN
        UPDATE [dbo].[Task]
        SET [IsActive] = 0
        WHERE [Id] IN (SELECT DISTINCT [Id] FROM deleted);
    END
GO

CREATE TRIGGER [dbo].[TR_Task_UpdatedAt]
    ON [dbo].[Task]
    AFTER UPDATE
    AS
    BEGIN
        UPDATE [dbo].[Task]
        SET [UpdatedAt] = GETDATE()
        WHERE [Id] IN (SELECT DISTINCT [Id] FROM inserted);
    END
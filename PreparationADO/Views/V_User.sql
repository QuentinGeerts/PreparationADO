CREATE VIEW [dbo].[V_User]
	AS SELECT Id, Email, LastName, FirstName FROM [User] WHERE [IsActive] = 1;

CREATE VIEW [dbo].[V_Student]
	AS SELECT * FROM [dbo].[Student] WHERE [Active] = 1;

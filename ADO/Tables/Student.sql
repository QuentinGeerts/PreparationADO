CREATE TABLE [dbo].[Student]
(
	[ID] INT NOT NULL IDENTITY,
	[FirstName] NVARCHAR(50) NOT NULL,
	[LastName] NVARCHAR(50) NOT NULL,
	[BirthDate] DATETIME2 NOT NULL,
	[YearResult] INT NOT NULL,
	[SectionID] INT NOT NULL,
	[Active] BIT NULL DEFAULT 1,

	CONSTRAINT PK_Student PRIMARY KEY ([ID]),
	CONSTRAINT FK_Student_Section FOREIGN KEY ([SectionID]) REFERENCES [dbo].[Section] ([ID]),
	CONSTRAINT CK_YearResult_BetweenZeroAndTwenty CHECK ([YearResult] BETWEEN 0 AND 20),
	CONSTRAINT CK_BirthDate_GreaterThanOrEqualTo CHECK ([BirthDate] >= '1930-01-01')
)

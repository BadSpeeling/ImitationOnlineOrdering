CREATE TABLE [dbo].[Franchise](
	[FranchiseID] [int] IDENTITY(1,1) NOT NULL,
	[FranchiseName] [nvarchar](100) NOT NULL,
	[FranchiseOwnerUserID] [uniqueidentifier] NULL,
	CONSTRAINT [PK_Franchise] PRIMARY KEY CLUSTERED 
	(
		[FranchiseID] ASC
	)
)
GO
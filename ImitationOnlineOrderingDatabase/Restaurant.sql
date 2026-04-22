CREATE TABLE [dbo].[Restaurant](
	[RestaurantID] [int] IDENTITY(1,1) NOT NULL,
	[RestaurantName] [nvarchar](200) NOT NULL,
	[FranchiseID] [int] NOT NULL,
	[RestaurantManagerUserID] [uniqueidentifier] NULL,
	CONSTRAINT [PK_Restuarant] PRIMARY KEY CLUSTERED 
	(
		[RestaurantID] ASC
	)
)
GO

ALTER TABLE [dbo].[Restaurant] ADD CONSTRAINT [FK_Restaurant_Franchise] FOREIGN KEY([FranchiseID])
REFERENCES [dbo].[Franchise] ([FranchiseID])
ON DELETE CASCADE
GO
CREATE TABLE [dbo].[Restaurant](
	[RestaurantID] [int] IDENTITY(1,1) NOT NULL,
	[FranchiseID] [int] NOT NULL,
	[RestaurantManagerUserID] [uniqueidentifier] NULL,
	[StreetAddress] [nvarchar](100) NOT NULL,
	[City] [nvarchar](50) NOT NULL,
	[State] [char](2) NOT NULL,
	[Zip] [char](5) NOT NULL,
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
CREATE TABLE [dbo].[MenuItem](
	[MenuItemID] [int] IDENTITY(1,1) NOT NULL,
	[MenuItemName] [nvarchar](100) NOT NULL,
	[Price] [decimal](19, 4) NOT NULL,
	[RestaurantID] [int] NOT NULL,
	CONSTRAINT [PK_MenuItem] PRIMARY KEY CLUSTERED 
	(
		[MenuItemID] ASC
	)
)
GO

ALTER TABLE [dbo].[MenuItem] ADD CONSTRAINT [FK_MenuItem_Restaurant] FOREIGN KEY([RestaurantID])
REFERENCES [dbo].[Restaurant] ([RestaurantID]) 
ON DELETE CASCADE
GO

CREATE TABLE [dbo].[Restaurant](
	[RestaurantID] [int] IDENTITY(1,1) NOT NULL,
	[RestaurantName] [nvarchar](200) NOT NULL,
	[UserID] [uniqueidentifier] NOT NULL,
	CONSTRAINT [PK_Restuarant] PRIMARY KEY CLUSTERED 
	(
		[RestaurantID] ASC
	)
)
GO
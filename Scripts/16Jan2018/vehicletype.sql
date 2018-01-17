CREATE TABLE [dbo].[vehicletype](
	[vehicletype] [int] IDENTITY(1,1) NOT NULL,
	[type] [int] NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[fee] [numeric](15, 2) NOT NULL,
 CONSTRAINT [PK_vehicletype] PRIMARY KEY CLUSTERED 
(
	[vehicletype] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
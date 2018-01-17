CREATE TABLE [dbo].[vehiclefeeitem](
	[vehiclefeeitem] [int] IDENTITY(1,1) NOT NULL,
	[vehiclefee] [int] NOT NULL,
	[invoiceno] [nvarchar](30) NOT NULL,
	[vehicle] [nvarchar](100) NOT NULL,
	[plateno] [nvarchar](50) NOT NULL,
	[vehicletype] [int] NOT NULL,
	[fee] [numeric](15, 2) NOT NULL,
 CONSTRAINT [PK_vehicleitem] PRIMARY KEY CLUSTERED 
(
	[vehiclefeeitem] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[vehiclefeeitem]  WITH CHECK ADD  CONSTRAINT [FK_vehiclefeeitem_vehiclefee] FOREIGN KEY([vehiclefee])
REFERENCES [dbo].[vehiclefee] ([vehiclefee])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[vehiclefeeitem] CHECK CONSTRAINT [FK_vehiclefeeitem_vehiclefee]
GO

ALTER TABLE [dbo].[vehiclefeeitem]  WITH CHECK ADD  CONSTRAINT [FK_vehiclefeeitem_vehicletype] FOREIGN KEY([vehicletype])
REFERENCES [dbo].[vehicletype] ([vehicletype])
GO

ALTER TABLE [dbo].[vehiclefeeitem] CHECK CONSTRAINT [FK_vehiclefeeitem_vehicletype]
GO
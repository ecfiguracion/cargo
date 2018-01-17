CREATE TABLE [dbo].[vehiclefee](
	[vehiclefee] [int] IDENTITY(1,1) NOT NULL,
	[feeno] [nvarchar](50) NOT NULL,
	[date] [datetime] NOT NULL,
	[ppatotal] [numeric](15, 2) NULL,
	[status] [int] NOT NULL,
	[createdby] [int] NULL,
	[datecreated] [datetime] NULL,
	[updatedby] [int] NULL,
	[dateupdated] [datetime] NULL,
 CONSTRAINT [PK_vehiclefee] PRIMARY KEY CLUSTERED 
(
	[vehiclefee] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
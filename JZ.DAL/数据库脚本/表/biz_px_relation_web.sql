USE [SYGLDB_XiCheng]
GO

/****** Object:  Table [dbo].[biz_px_relation_web]    Script Date: 2013/8/15 8:49:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[biz_px_relation_web](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ModelIndex] [varchar](50) NULL,
	[SGDataID] [nvarchar](50) NOT NULL,
	[PXDataID] [nvarchar](50) NOT NULL,
	[SGTestRoomCode] [nvarchar](50) NOT NULL,
	[PXTestRoomCode] [nvarchar](50) NOT NULL,
	[PXTime] [datetime] NOT NULL,
	[PXBGRQ] [datetime] NULL,
	[SGBGRQ] [datetime] NULL,
 CONSTRAINT [PK_biz_px_relation_web] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO



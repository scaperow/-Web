USE [SYGLDB_XiCheng]
GO

/****** Object:  Table [dbo].[biz_ZJ_JZ_Summary]    Script Date: 2013/8/15 8:50:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[biz_ZJ_JZ_Summary](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[BGRQ] [datetime] NOT NULL,
	[ModelIndex] [varchar](50) NOT NULL,
	[ModelName] [varchar](50) NULL,
	[TestRoomCode] [varchar](50) NOT NULL,
	[ZjCount] [int] NOT NULL,
	[JzCount] [int] NOT NULL,
	[JLCompanyName] [varchar](50) NULL,
	[JLCompanyCode] [varchar](50) NULL,
 CONSTRAINT [PK_biz_ZJ_JZ_Summary] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[biz_ZJ_JZ_Summary] ADD  CONSTRAINT [DF_biz_ZJ_JZ_Summary_ZjCount]  DEFAULT ((0)) FOR [ZjCount]
GO

ALTER TABLE [dbo].[biz_ZJ_JZ_Summary] ADD  CONSTRAINT [DF_biz_ZJ_JZ_Summary_JzCount]  DEFAULT ((0)) FOR [JzCount]
GO



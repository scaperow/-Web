USE [SYGLDB_XiCheng]
GO

/****** Object:  View [dbo].[v_bs_machineSummary]    Script Date: 2013/8/8 17:14:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO







ALTER view [dbo].[v_bs_machineSummary] 
  AS
  


select
ID,
substring(SCPT,1,Len(SCPT)-4) as 试验室编码,
试验室名称,
单位名称,
单位编码,
管理编号,
标段名称,
标段编码,
设备名称,
生产厂家,
规格型号,
购置日期,
数量,
备注
from [biz_norm_试验室设备列表]  a JOIN dbo.v_bs_codeName b ON substring(SCPT,1,Len(SCPT)-4)=b.试验室编码
where 管理编号 is not null or 设备名称 is not null




GO



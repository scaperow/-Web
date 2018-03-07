USE [SYGLDB_XiCheng]
GO

/****** Object:  View [dbo].[v_bs_reminder_stadiumData]    Script Date: 2013/8/8 17:14:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO







ALTER view [dbo].[v_bs_reminder_stadiumData] 
  AS
  

 

  SELECT f.id, m.标段名称 , m.单位名称 , m.试验室名称 ,m.试验室编码 AS testroomcode,f.F_Name as 名称,f.F_PH as 批号,f.F_ZJRQ as 制件日期,f.F_SJBH as 试件编号,f.F_SJSize as 试件尺寸,f.F_SYXM as 试验项目,f.F_BGBH as 报告编号,f.F_WTBH as 委托编号   FROM 
  dbo.sys_biz_reminder_stadiumInfo e JOIN   
  dbo.sys_biz_reminder_stadiumData f ON    f.ModelIndex=e.ID AND CAST( DATEDIFF(day, DATEADD(DAY, f.DateSpan, f.F_ZJRQ), GETDATE()) AS NVARCHAR(50)) IN (e.SearchRange) JOIN 
 dbo.v_bs_codeName m ON LEFT(f.ModelCode,16)=m.试验室编码 
            




GO



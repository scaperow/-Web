USE [SYGLDB_XiCheng]
GO

/****** Object:  View [dbo].[v_bs_userSummary]    Script Date: 2013/8/8 17:14:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






ALTER view [dbo].[v_bs_userSummary] 
  AS
  
 select ID,
substring(SCPT,1,Len(SCPT)-4) as 试验室编码,
b.标段名称,
b.单位名称,
b.试验室名称,
b.单位编码,
col_norm_D6 as '姓名',
col_norm_G6 as '性别',
col_norm_K6 as '年龄',
col_norm_D7 as '技术职称',
col_norm_G7 as '职务',
col_norm_K7 as '工作年限',
col_norm_D9 as '联系电话',
col_norm_G8 as '学历',
col_norm_G9 as '毕业学校',
col_norm_D8 as '专业',
1 AS num
from 
biz_norm_试验人员档案 a JOIN dbo.v_bs_codeName b ON substring(SCPT,1,Len(SCPT)-4)=b.试验室编码





GO



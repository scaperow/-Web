USE [SYGLDB_XiCheng]
GO
/****** Object:  StoredProcedure [dbo].[spweb_userSummary_chart_pop]    Script Date: 2013/8/15 8:47:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





-- =============================================
-- Author:		张宏伟
-- Create date: 2013-7-24
-- Description:	人员情况
-- =============================================
ALTER PROCEDURE [dbo].[spweb_userSummary_chart_pop]
	
	@testcode  VARCHAR(5000),
	@startdate VARCHAR(50),
	@enddate VARCHAR(50),
	@ftype TINYINT
	
AS
BEGIN

		CREATE TABLE #tmp
		(
		id bigint identity(1,1)  primary key,
		 testname  nvarchar(50),
		 Testcode NVARCHAR(50),
		 Companycode NVARCHAR(50),
		 company NVARCHAR(50),
		 ncount INT
		 
		) 
 
 IF	@ftype=1  --按照是试验室分类统计
 BEGIN
 
		 INSERT #tmp
				 ( 
				   testname ,
				   Testcode ,
				   Companycode ,
				   company,
				   ncount
				 )
		 SELECT MAX(试验室名称), 试验室编码,MAX(单位编码),MAX(单位名称),COUNT(1) FROM dbo.v_bs_userSummary WHERE 单位编码=@testcode  GROUP BY  试验室编码
 END
 
  IF	@ftype=2  --按照是职称分类统计
 BEGIN
 
		 INSERT #tmp
				 ( 
				   testname ,
				   Testcode ,
				   Companycode ,
				   company,
				   ncount
				 )
		 SELECT 技术职称, MAX(试验室编码),MAX(单位编码),MAX(单位名称),COUNT(1) FROM dbo.v_bs_userSummary WHERE 单位编码=@testcode  GROUP BY  技术职称
 END
 
   IF	@ftype=3  --按照是学历分类统计
 BEGIN
 
		 INSERT #tmp
				 ( 
				   testname ,
				   Testcode ,
				   Companycode ,
				   company,
				   ncount
				 )
		 SELECT 学历, MAX(试验室编码),MAX(单位编码),MAX(单位名称),COUNT(1) FROM dbo.v_bs_userSummary WHERE 单位编码=@testcode  GROUP BY  学历
 END
 
    IF	@ftype=4  --按照是工作年限分类统计
 BEGIN
 
		 INSERT #tmp
				 ( 
				   testname ,
				   Testcode ,
				   Companycode ,
				   company,
				   ncount
				 )
		 SELECT REPLACE(工作年限,'年','')+'年', MAX(试验室编码),MAX(单位编码),MAX(单位名称),COUNT(1) FROM dbo.v_bs_userSummary WHERE 单位编码=@testcode  GROUP BY  REPLACE(工作年限,'年','')
 END

	SELECT * FROM #tmp
END

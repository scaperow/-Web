USE [SYGLDB_XiCheng]
GO
/****** Object:  StoredProcedure [dbo].[spweb_evaluatedata_chart]    Script Date: 2013/8/15 8:42:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





-- =============================================
-- Author:		张宏伟
-- Create date: 2013-7-24
-- Description:	不合格报告
-- =============================================
ALTER PROCEDURE [dbo].[spweb_evaluatedata_chart]
	
	@testcode  VARCHAR(5000),
	@startdate VARCHAR(50),
	@enddate VARCHAR(50),
	@ftype TINYINT
	
AS
BEGIN

		CREATE TABLE #tmp
		(
		id bigint identity(1,1)  primary key,
		 单位名称  nvarchar(50),
		 单位编码 NVARCHAR(50),
		 ncount INT
		 
		) 
		DECLARE 
	@code VARCHAR(50),
	@companyname VARCHAR(50),
	@num INT,
			@sqls nvarchar(4000)
 
 	CREATE TABLE #t1
		(
		 testcode NVARCHAR(50)
		)  

	  IF @testcode!='' 
  BEGIN
			SET @sqls='INSERT #t1 ( testcode ) SELECT   LEFT(NodeCode,12)    FROM sys_engs_Tree where LEFT(NodeCode,16) IN ('+@testcode+')    ORDER BY LEFT(NodeCode,12)'
		 EXEC	sp_executesql @sqls
	END

		DECLARE cur CURSOR FOR	
SELECT 单位编码,单位名称 FROM dbo.v_bs_codeName a JOIN #t1 b ON 单位编码=testcode  GROUP BY 单位编码,单位名称

	OPEN cur
	FETCH NEXT FROM cur INTO @code,@companyname
	WHILE @@FETCH_STATUS = 0
	BEGIN		
	
	SELECT @num=COUNT(*) FROM  dbo.v_bs_evaluateData WHERE  CompanyCode=@code
	
	IF	@num=0
	BEGIN
  INSERT #tmp
        (  单位名称, 单位编码, ncount )
VALUES(@companyname,@code,0) 
  END  
	ELSE
  BEGIN  
INSERT #tmp
        (  单位名称, 单位编码, ncount )
SELECT  MAX(CompanyName)  ,MAX(CompanyCode) , COUNT(*) FROM  dbo.v_bs_evaluateData WHERE     CompanyCode=@code
END
	
	  FETCH NEXT FROM cur INTO @code,@companyname
	END
	CLOSE cur
	DEALLOCATE cur


	SELECT 单位名称 AS companyname, 单位编码 AS companycode, ncount FROM  #tmp
	    
    

END


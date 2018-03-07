USE [SYGLDB_XiCheng]
GO
/****** Object:  StoredProcedure [dbo].[spweb_machineSummary_chart_pop]    Script Date: 2013/8/15 8:44:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





-- =============================================
-- Author:		张宏伟
-- Create date: 2013-7-24
-- Description:	人员情况
-- =============================================
ALTER PROCEDURE [dbo].[spweb_machineSummary_chart_pop]
	
	
	@testcode  VARCHAR(5000),
	@startdate VARCHAR(50),
	@enddate VARCHAR(50),
	@ftype TINYINT
	
AS
BEGIN

		CREATE TABLE #tmp
		(
		id bigint identity(1,1)  primary key,
		 试验室名称  nvarchar(50),
		 试验室编码 NVARCHAR(50),
		 ncount INT
		 
		) 
		DECLARE 
	@code VARCHAR(50),
	@companyname VARCHAR(50),
	@num INT
 


		DECLARE cur CURSOR FOR	
SELECT 试验室编码,试验室名称 FROM dbo.v_bs_codeName WHERE 单位编码=@testcode

	OPEN cur
	FETCH NEXT FROM cur INTO @code,@companyname
	WHILE @@FETCH_STATUS = 0
	BEGIN		
	
	SELECT @num=COUNT(*) FROM  dbo.v_bs_machineSummary WHERE  试验室编码=@code
	
	IF	@num=0
	BEGIN
  INSERT #tmp
        (  试验室名称, 试验室编码, ncount )
VALUES(@companyname,@testcode,0) 
  END  
	ELSE
  BEGIN  
INSERT #tmp
        (  试验室名称, 试验室编码, ncount )
SELECT  MAX(试验室名称)  ,MAX(试验室编码) , COUNT(*) FROM  dbo.v_bs_machineSummary WHERE    试验室编码=@code
END
	
	  FETCH NEXT FROM cur INTO @code,@companyname
	END
	CLOSE cur
	DEALLOCATE cur


	SELECT 试验室名称 AS testname, 试验室编码 AS companycode, ncount FROM  #tmp

    

END

USE [SYGLDB_XiCheng]
GO
/****** Object:  StoredProcedure [dbo].[spweb_machineSummary_chart]    Script Date: 2013/8/15 8:43:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		张宏伟
-- Create date: 2013-7-24
-- Description:	设备
-- =============================================
ALTER PROCEDURE [dbo].[spweb_machineSummary_chart]
	
	@testcode  VARCHAR(5000),
	@startdate VARCHAR(50),
	@enddate VARCHAR(50),
	@ftype TINYINT
	
AS
BEGIN

		CREATE TABLE #tmp
		(
		id bigint identity(1,1)  primary key,
		试验室名称   nvarchar(50),
		 试验室编码 NVARCHAR(50),
		 ncount INT
		 
		) 
		DECLARE 
	@testid VARCHAR(50),
	@testname VARCHAR(50),
	@num INT,
		@sqls nvarchar(4000)
 


		DECLARE cur CURSOR FOR	
SELECT  试验室编码, 试验室名称 FROM dbo.v_bs_codeName  GROUP BY 试验室编码, 试验室名称

	OPEN cur
	FETCH NEXT FROM cur INTO @testid,@testname
	WHILE @@FETCH_STATUS = 0
	BEGIN		
	
	SELECT @num=COUNT(*) FROM  dbo.v_bs_machineSummary WHERE 试验室编码=@testid
	
	IF	@num=0
	BEGIN
  INSERT #tmp
        (  试验室名称, 试验室编码, ncount )
VALUES(@testname,@testid,0) 
  END  
	ELSE
  BEGIN  
INSERT #tmp
        (  试验室名称, 试验室编码, ncount )
SELECT  MAX(试验室名称)  ,MAX(试验室编码) , COUNT(*) FROM  dbo.v_bs_machineSummary WHERE    试验室编码=@testid
END
	
	  FETCH NEXT FROM cur INTO @testid,@testname
	END
	CLOSE cur
	DEALLOCATE cur

	    
IF @ftype=1
		begin
  
		  IF @testcode!=''
		  BEGIN
					SET @sqls='SELECT SUM(a.ncount) as ncount,b.单位名称  companyname,b.单位编码 AS companycode, b.标段名称 as segment FROM  #tmp a JOIN dbo.v_bs_codeName b ON a.试验室编码 = b.试验室编码 WHERE a.试验室编码 IN('+@testcode+')  GROUP BY b.单位编码,b.单位名称,b.标段名称'
					EXEC	sp_executesql @sqls
			END
		  ELSE
		  BEGIN
		  SELECT '' as ncount,'' as  companyname,'' AS companycode, '' as segment FROM  #tmp WHERE 1=0
		  END
    

		END   
IF @ftype=2
		begin

		IF  @testcode!=''
		  BEGIN
			 SET @sqls=' SELECT SUM(a.ncount) as ncount,b.试验室名称 AS companyname,b.试验室编码 AS companycode,b.标段名称 as segment  FROM  #tmp a JOIN dbo.v_bs_codeName b ON a.试验室编码 = b.试验室编码 WHERE a.试验室编码 IN('+@testcode+')  GROUP BY b.试验室编码,b.试验室名称,b.标段名称 '
					 EXEC	sp_executesql @sqls
					 END
		   ELSE
		  BEGIN
		  SELECT '' as ncount,'' as  companyname,'' AS companycode, '' as segment FROM  #tmp WHERE 1=0
		  END           
		END
   ELSE
	  BEGIN
   SELECT '' as ncount,'' as  companyname,'' AS companycode, '' as segment FROM  #tmp WHERE 1=0
	  END   
 


END

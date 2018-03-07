USE [SYGLDB_XiCheng]
GO
/****** Object:  StoredProcedure [dbo].[spweb_pxreport_Chart_pop]    Script Date: 2013/8/15 8:45:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		张宏伟
-- Create date: 2013-7-4
-- Description:	获取平行、见证的Chart
-- =============================================
ALTER PROCEDURE [dbo].[spweb_pxreport_Chart_pop] 
	-- Add the parameters for the stored procedure here
	@testcode VARCHAR(50), --标准试验室id
	@ftype TINYINT, -- 频率类型1=平行，2=见证
	@startdate varchar(30),
	@enddate varchar(30),
	@modelID varchar(50)
AS
BEGIN


	
		CREATE TABLE #tmp
	(
	chartDate VARCHAR(20),
	zjCount INT,
	pxjzCount INT,	
	)
	
		CREATE TABLE #tmp1
	(
	chartDate VARCHAR(20),
	countnum INT	
	)
	
		CREATE TABLE #tmp2
	(
	chartDate VARCHAR(20),
	countnum INT	
	)
	
	CREATE TABLE #tmp3
	(
	chartDate1 VARCHAR(20),
	chartDate2 VARCHAR(20),
	zjCount INT,
	pxjzCount INT,	
	)
 
 DECLARE 

	@sqls nvarchar(4000)

		
	   SET @sqls='INSERT #tmp1
				   ( chartDate, countnum ) select REPLACE(CONVERT(varchar(12) , SCTS, 111 ) ,''/'',''-''),count(*) from [biz_norm_extent_'+@modelID+'] a 
	   where a.trytype=''自检'' AND SUBSTRING(a.SCPT,0,17)='''+@testcode+''' AND a.SCTS>='''+@startdate+''' AND a.SCTS<'''+@enddate+'''
	   GROUP by REPLACE(CONVERT(varchar(12) , SCTS, 111 )  ,''/'',''-'') '
	   
	  
	   EXEC(@sqls)
	   
	   IF(@ftype=1)
	   begin
			 SET @sqls='INSERT #tmp2
				   ( chartDate, countnum ) select REPLACE(CONVERT(varchar(12) , b.PXTime, 111 )  ,''/'',''-''),count(*) from [biz_norm_extent_'+@modelID+'] a 
		   join dbo.biz_px_relation b on a.ID=b.SGDataID
		   where a.trytype=''自检'' AND SUBSTRING(a.SCPT,0,17)='''+@testcode+''' AND a.SCTS>='''+@startdate+''' AND a.SCTS<'''+@enddate+'''
		   GROUP by REPLACE(CONVERT(varchar(12) , b.PXTime, 111 )  ,''/'',''-'') '
		
			  
		   EXEC(@sqls)
		END
		
		 IF(@ftype=2)
	   begin
			 SET @sqls='  INSERT #tmp2
				   ( chartDate, countnum ) select REPLACE(CONVERT(varchar(12) , SCTS, 111 )  ,''/'',''-''),count(*) from [biz_norm_extent_'+@modelID+'] a 
		   where trytype=''见证'' AND SUBSTRING(a.SCPT,0,17)='''+@testcode+''' AND a.SCTS>='''+@startdate+''' AND a.SCTS<'''+@enddate+'''
		   GROUP by REPLACE(CONVERT(varchar(12) , SCTS, 111 )  ,''/'',''-'') '
		
			
		   EXEC(@sqls)
		END
		
   
		
		--SELECT a.chartDate,b.chartDate,a.countnum,b.countnum FROM #tmp1 a FULL JOIN #tmp2 b ON a.chartDate = b.chartDate
		
		
		INSERT #tmp3
		        ( chartDate1 ,
		          chartDate2 ,
		          zjCount ,
		          pxjzCount
		        )
		SELECT a.chartDate,b.chartDate,a.countnum,b.countnum FROM #tmp1 a FULL JOIN #tmp2 b ON a.chartDate = b.chartDate
		
		UPDATE #tmp3 SET chartDate1=chartDate2 WHERE chartDate1 IS NULL
		
		
		INSERT #tmp
		        ( chartDate, zjCount, pxjzCount )
		SELECT chartDate1,zjCount,pxjzCount FROM  #tmp3

		update #tmp set zjCount=0 where zjCount is null
		update #tmp set pxjzCount=0 where pxjzCount is null
		select * from #tmp
			
END


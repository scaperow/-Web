USE [SYGLDB_XiCheng]
GO
/****** Object:  StoredProcedure [dbo].[spweb_tqdreport]    Script Date: 2013/8/15 8:46:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		张宏伟
-- Create date: 2013-7-31
-- Description:	砼强度
-- =============================================
ALTER PROCEDURE [dbo].[spweb_tqdreport]
	-- Add the parameters for the stored procedure here	
	@testcode  VARCHAR(5000),
	@modelID VARCHAR(50),
	@startdate VARCHAR(50),
	@enddate VARCHAR(50),
	@type VARCHAR(50)

AS

BEGIN

 
DECLARE 
	@sqls nvarchar(4000),
	@curid VARCHAR(50)

	CREATE TABLE #t1
	(
	ID VARCHAR(50)
	)
    
	

IF @testcode!=''
BEGIN
 SET @sqls='INSERT #t1 ( ID ) select ID from [biz_norm_extent_'+@modelID+'] a 
	   where  SUBSTRING(a.SCPT,0,17) IN ('+@testcode+') '
	  EXEC	sp_executesql @sqls
END

	  CREATE TABLE #tmp
	  (
	  cType NVARCHAR(80),
	  sDate DATETIME,
	  sPlace NVARCHAR(4000),
	  sValue1 NVARCHAR(80),
	  sValue2 NVARCHAR(80),
	  sValue3 NVARCHAR(80),
	  sValue4 NVARCHAR(80),
	  sTestCode VARCHAR(50)
	  )



	IF @modelID='f34c2b8b-ddbe-4c04-bd01-f08b0f479ae8'
	BEGIN
		
		  INSERT #tmp
	          ( cType ,
	            sDate ,
	            sPlace ,
	            sValue1 ,
	            sValue2 ,
	            sValue3 ,
	            sValue4,
				sTestCode
	          )
	  	   	   SELECT a.col_norm_C9,a.col_norm_C22,b.col_norm_D7,b.col_norm_S34,b.col_norm_S37,b.col_norm_S40,b.col_norm_S43,a.SCPT FROM dbo.biz_norm_新混凝土试验任务委托单 a JOIN dbo.biz_norm_混凝土检查试件抗压强度试验报告三级配 b ON a.ID = b.ID  AND a.col_norm_C9=@type  AND a.col_norm_C22>=@startdate AND a.col_norm_C22<@enddate AND a.ID IN (SELECT ID FROM #t1)
  
	 END  		
	  ELSE
	  begin
	   
	   	  INSERT #tmp
	          ( cType ,
	            sDate ,
	            sPlace ,
	            sValue1 ,
	            sValue2 ,
	            sValue3 ,
	            sValue4,
				sTestCode
	          )
	   SELECT a.col_norm_C9,a.col_norm_C21,b.col_norm_D7,b.col_norm_S33,b.col_norm_S36,b.col_norm_S39,b.col_norm_S42,a.SCPT FROM dbo.biz_norm_混凝土试验任务委托单 a JOIN biz_norm_混凝土检查试件抗压强度试验报告 b ON a.ID = b.ID   AND a.col_norm_C9=@type  AND a.col_norm_C21>=@startdate AND a.col_norm_C21<@enddate AND a.ID IN (SELECT ID FROM #t1)

	 END
	   


		DELETE #tmp WHERE ISNUMERIC(sValue1)=0    AND ISNUMERIC(sValue2)=0  AND ISNUMERIC(sValue3)=0  AND ISNUMERIC(sValue4)=0  
		
		DELETE #tmp WHERE  cType='/'

	SELECT cType ,
	            sDate ,
	            sPlace ,
	            sValue1 ,
	            sValue2 ,
	            sValue3 ,
	            sValue4,
				 b.标段名称 AS segment,
				 b.单位名称 AS company,
				 b.试验室名称 AS testroom

				 FROM #tmp a JOIN dbo.v_bs_codeName b ON  LEFT(a.sTestCode,16)=b.试验室编码 
				
				ORDER BY sDate

  
  

			
END

 



 




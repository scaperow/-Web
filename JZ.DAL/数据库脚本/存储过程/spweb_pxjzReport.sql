USE [SYGLDB_XiCheng]
GO
/****** Object:  StoredProcedure [dbo].[spweb_pxjzReport]    Script Date: 2013/8/15 8:45:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		张宏伟
-- Create date: 2013-7-24
-- Description:	获取平行、见证的报表
-- =============================================
ALTER PROCEDURE [dbo].[spweb_pxjzReport]
	@testcode varchar(5000), --有权限的试验室id集合
	@ftype TINYINT, -- 频率类型1=平行，2=见证
	@startdate varchar(30),
	@enddate varchar(30),
	@pageSize int=10,
	@page        int, 
	@fldSort    varchar(200) = null,    ----排序字段列表或条件
	@Sort        bit = 0,    
	@pageCount    int = 1 output,           
	@Counts    int = 1 output
AS
BEGIN

 	CREATE TABLE #tmp1
	(
	id bigint identity(1,1)  primary key,
	modelName NVARCHAR(50),
	condition FLOAT,
	zjCount FLOAT,
	pxCount FLOAT,
	frequency FLOAT,
	result VARCHAR(10),
	segment VARCHAR(100),
	jl VARCHAR(100),
	sg VARCHAR(100),
	testroom VARCHAR(100),
	modelID NVARCHAR(50),
	testroomID NVARCHAR(50),
	pxqulifty FLOAT
	)
 


	SELECT ModelIndex,MAX(ModelName) ModelName ,TestRoomCode,SUM(ZjCount) ZjCount,SUM(JzCount) JzCount,MAX(JLCompanyName) JLCompanyName INTO #t1 FROM biz_ZJ_JZ_Summary  WHERE BGRQ>@startdate AND BGRQ<@enddate GROUP BY TestRoomCode,ModelIndex



INSERT #tmp1
        ( 
          modelName ,
          zjCount ,
          pxCount ,
          jl ,
          modelID ,
          testroomID 
        )	
 SELECT ModelName,ZjCount, dbo.Fweb_ReturnPXCount(ModelIndex,TestRoomCode,@startdate,@enddate),JLCompanyName,ModelIndex,TestRoomCode FROM #t1

 
 
	 SELECT  
           a.modelName ,
             condition ,
           zjCount ,
           pxCount ,
          ROUND((pxCount/zjCount),2) AS frequency ,
           result ,
           b.标段名称 segment ,
           jl ,
           b.单位名称 sg ,
           b.试验室名称 testroom ,
           modelID ,
           a.testroomID ,
           pxqulifty INTO #t3 FROM #tmp1 a JOIN dbo.v_bs_codeName b
		   ON a.testroomID=b.试验室编码
		     ORDER BY segment,sg,testroom



		   TRUNCATE TABLE #tmp1
		   INSERT #tmp1
		           ( 
		             modelName ,
		             condition ,
		             zjCount ,
		             pxCount ,
		             frequency ,
		             result ,
		             segment ,
		             jl ,
		             sg ,
		             testroom ,
		             modelID ,
		             testroomID ,
		             pxqulifty
		           )SELECT * FROM #t3


	 UPDATE #tmp1 SET condition=(b.Frequency*100) FROM #tmp1 a JOIN sys_biz_reminder_Itemfrequency b
			 ON a.modelID=b.ModelIndex AND a.testroomID=b.TestRoomCode AND b.IsActive=1  


    --取得分页总数
  declare @totalcounts int
  select @totalcounts=count(ID) from #tmp1
	  declare @pageIndex int --总数/页大小
    declare @lastcount int --总数%页大小  

	 set @pageIndex = @totalcounts/@pageSize
    set @lastcount = @totalcounts%@pageSize
    if @lastcount > 0
        set @pageCount = @pageIndex + 1
    else
        set @pageCount = @pageIndex 


		SET @Counts=@totalcounts
	SELECT id, modelName ,
		             condition ,
		             zjCount ,
		             pxCount ,
		             frequency ,
		             result ,
		             segment ,
		             jl ,
		             sg ,
		             testroom ,
		             modelID ,
		             testroomID ,
		             dbo.Fweb_ReturnPXQualityCount(modelID,testroomID,@startdate,@enddate) pxqulifty from #tmp1  	
	 where id>(@pageSize*(@page-1)) and id<=(@pageSize*@page)   
 
    
END






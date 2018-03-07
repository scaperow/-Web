USE [SYGLDB_XiCheng]
GO
/****** Object:  StoredProcedure [dbo].[spweb_failreport]    Script Date: 2013/8/15 8:43:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		张宏伟
-- Create date: 2013-7-22
-- Description:	全线资料汇总表和不合格报告
-- =============================================
ALTER PROCEDURE [dbo].[spweb_failreport]
	-- Add the parameters for the stored procedure here	
	@testcode  VARCHAR(5000),
	@ftype TINYINT,--没有任何意义为了能和平行见证使用同一方法，特加了一个参数
	@startdate VARCHAR(50),
	@enddate VARCHAR(50),
	@pageSize int=10,
	@page        int, 
	@fldSort    varchar(200) = null,    ----排序字段列表或条件
	@Sort        bit = 0,    
	@pageCount    int = 0 output,           
	@Counts    int = 0 output
AS

BEGIN

 

	CREATE TABLE #tmp
	(	
	ncount INT,
	testroomid VARCHAR(100),
	modelid NVARCHAR(100)	
	)
 
 DECLARE 
	@modelID varchar(50),--游标中使用
	@testroomid VARCHAR(50),
	@sqls nvarchar(4000)
 
 CREATE TABLE #t1
 (
 modelID VARCHAR(50),
 NodeCode VARCHAR(50)
 )
	 IF	@testcode!=''
	 BEGIN
SET @sqls=' INSERT #t1 ( modelID, NodeCode ) SELECT  DISTINCT b.ID AS modelID,LEFT(a.NodeCode,16) NodeCode   FROM sys_engs_Tree  a JOIN sys_biz_module b ON a.RalationID=b.ID WHERE LEFT(a.NodeCode,16) IN ('+@testcode+') ORDER BY LEFT(a.NodeCode,16) '
 EXEC	sp_executesql @sqls

	END
	 

	DECLARE cur CURSOR FOR
    
 SELECT * FROM #t1

	OPEN cur
	FETCH NEXT FROM cur INTO @modelID,@testroomid
	WHILE @@FETCH_STATUS = 0
	BEGIN
		
	   SET @sqls='select count(1) as count,'''+@testroomid+''' as testroomid,'''+@modelID+''' modelid  from 

[biz_norm_extent_'+@modelID+'] a 
	   where  LEFT(SCPT,16)='''+@testroomid+''' and    a.SCTS>='''+@startdate+''' AND a.SCTS<'''+@enddate+''''  

	INSERT #tmp
	        ( ncount, testroomid, modelid )
			  EXEC	sp_executesql @sqls
			    
	   
	   FETCH NEXT FROM cur INTO  @modelID,@testroomid
	END

	CLOSE cur
	DEALLOCATE cur	


 SELECT b.工程名称,b.标段名称,b.单位名称,b.试验室名称,a.modelid,a.ncount,b.试验室编码 AS testcode INTO #t2 FROM #tmp a JOIN dbo.v_bs_codeName b ON 

a.testroomid=b.试验室编码

 create table #tmp1
 (
id bigint identity(1,1)  primary key,
project varchar(50),
segment varchar(50),
company varchar(50),
testroom varchar(50),
testname varchar(50),
ncount INT,
testcode VARCHAR(50),
modelid VARCHAR(50)
 )


   insert into #tmp1
 ( project,segment,company,testroom,testname,ncount,testcode,modelid)
SELECT a.工程名称,a.标段名称,a.单位名称,a.试验室名称,b.Description,a.ncount,a.testcode,a.modelid FROM #t2 a JOIN dbo.sys_biz_Module b ON 

a.modelid=b.ID AND a.ncount>0   order by a.单位名称


  /*给没有的试验室添加0值*/


 CREATE TABLE #t5
 (
 NodeCode VARCHAR(50)
 )
 IF	@testcode!=''
	 BEGIN	 
SET @sqls=' INSERT #t5 (  NodeCode ) SELECT  DISTINCT LEFT(a.NodeCode,16) NodeCode   FROM sys_engs_Tree  a  WHERE LEFT(a.NodeCode,16) IN ('+@testcode+') ORDER BY LEFT(a.NodeCode,16) '
  EXEC	sp_executesql @sqls
  END
INSERT #tmp1
        (
          project ,
          segment ,
          company ,
          testroom ,
          testname ,
          ncount,
		  testcode,
		  modelid
        )
 SELECT 工程名称,标段名称,单位名称,试验室名称,'',0,试验室编码,''  FROM #t5 a JOIN dbo.v_bs_codeName b ON a.NodeCode=b.试验室编码 WHERE a.NodeCode NOT IN (SELECT testcode FROM #tmp1)


   /*给没有的试验室添加0值*/


  create table #tmp2
 (
id bigint identity(1,1)  primary key,
project varchar(50),
segment varchar(50),
company varchar(50),
testroom varchar(50),
testname varchar(50),
ncount INT,
testcode VARCHAR(50),
modelid VARCHAR(50)
 )


 INSERT #tmp2
         ( 
           project ,
           segment ,
           company ,
           testroom ,
           testname ,
           ncount,
		   testcode,
		   modelid
         )
SELECT project,segment,company,testroom,testname,ncount,testcode,modelid FROM  #tmp1  ORDER BY project,segment,company,testroom


  create table #tmp3
 (
id bigint identity(1,1)  primary key,
project varchar(50),
segment varchar(50),
company varchar(50),
testroom varchar(50),
testcode VARCHAR(50),
totalncount INT
 )

 INSERT #tmp3
         ( 
           project ,
           segment ,
           company ,
           testroom ,
		   testcode,
           totalncount
         )
SELECT   project ,
           segment ,
           company ,
           MAX(testroom) ,
		   testcode,
           SUM(ncount) FROM  #tmp2 GROUP BY project,segment,company,testcode


		   CREATE TABLE #t6
		   (
		   testcode NVARCHAR(50),
		   counts INT
		   )
 
 

  IF	@testcode!=''
	 BEGIN
 SET @sqls='INSERT #t6 ( testcode, counts )  SELECT TestRoomCode,COUNT(1) AS counts FROM v_bs_evaluateData a JOIN dbo.sys_engs_Tree ON a.TestRoomCode=LEFT(NodeCode,16) AND LEFT(NodeCode,16)  IN ('+@testcode+') GROUP BY TestRoomCode'
 END

   EXEC	sp_executesql @sqls



     create table #tmp4
 (
id bigint identity(1,1)  primary key,
project varchar(50),
segment varchar(50),
company varchar(50),
testroom varchar(50),
testcode VARCHAR(50),
totalncount INT,
counts INT
 )

    INSERT #tmp4
         ( 
           project ,
           segment ,
           company ,
           testroom ,
           totalncount,
		   counts
         )
		 SELECT a.project,a.segment,a.company,a.testroom,a.totalncount,b.counts FROM  #tmp3 a JOIN #t6 b ON a.testcode = b.testcode ORDER BY a.segment,a.company,a.testcode,a.totalncount,b.counts  DESC



  declare @totalcounts int
  select @totalcounts=count(ID) from #tmp4

    --取得分页总数

	  declare @pageIndex INT --总数/页大小
    declare @lastcount INT --总数%页大小  


	SET @pageIndex=0
	SET @lastcount=0

	 set @pageIndex = @totalcounts/@pageSize
    set @lastcount = @totalcounts%@pageSize
    if @lastcount > 0
        set @pageCount = @pageIndex + 1
    else
        set @pageCount = @pageIndex 

 

		SET @Counts=@totalcounts

	select * from #tmp4 where id>(@pageSize*(@page-1)) and id<=(@pageSize*(@page)) 
			
END

 



 




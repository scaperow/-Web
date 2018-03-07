USE [SYGLDB_XiCheng]
GO
/****** Object:  StoredProcedure [dbo].[spweb_userSummary]    Script Date: 2013/8/15 8:46:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		张宏伟
-- Create date: 2013-7-24
-- Description:	人员情况
-- =============================================
ALTER PROCEDURE [dbo].[spweb_userSummary]
	@testcode varchar(5000), --为空
	@ftype TINYINT, -- 为空
	@startdate varchar(30),--为空
	@enddate varchar(30),--为空
	@pageSize int=10,
	@page        int, 
	@fldSort    varchar(200) = null,    ----排序字段列表或条件
	@Sort        bit = 0,    
	@pageCount    int = 1 output,           
	@Counts    int = 1 output
AS
BEGIN

		CREATE TABLE #tmp
		(
		id bigint identity(1,1)  primary key,
		userid VARCHAR(50),
		 试验室编码  varchar(36),
		 标段名称  nvarchar(50),
		 单位名称  nvarchar(50),
		 试验室名称  nvarchar(50),
		 姓名  nvarchar(80),
		 性别  nvarchar(80),
		 年龄  nvarchar(80),
		 技术职称  nvarchar(80),
		 职务  nvarchar(80),
		 工作年限  nvarchar(80),
		 联系电话  nvarchar(80),
		 学历  nvarchar(80),
		 毕业学校  nvarchar(80),
		 专业  nvarchar(80),
		num  int
		) 
		DECLARE 
	@modelID varchar(50),--游标中使用
	@count INT,
	@sqls  NVARCHAR(4000)




	CREATE TABLE #t1
	(
	testcode VARCHAR(50)
	)
	IF @testcode !='' 
  
  BEGIN  
SET @sqls='INSERT #t1 ( testcode ) SELECT  DISTINCT (LEFT(NodeCode,16) )   FROM sys_engs_Tree where LEFT(NodeCode,16) IN ('+@testcode+')    ORDER BY LEFT(NodeCode,16)'

 	  EXEC	sp_executesql @sqls

	  END

		

		DECLARE cur CURSOR FOR	

 SELECT * FROM #t1

	OPEN cur
	FETCH NEXT FROM cur INTO @modelID
	WHILE @@FETCH_STATUS = 0
	BEGIN		


	SELECT @count=COUNT(*) FROM  dbo.v_bs_userSummary WHERE   试验室编码=@modelID

	IF @count=0
	BEGIN
  INSERT #tmp
          (
		  userid,
		   试验室编码 ,
            标段名称 ,
            单位名称 ,
            试验室名称 ,
            姓名 ,
            性别 ,
            年龄 ,
            技术职称 ,
            职务 ,
            工作年限 ,
            联系电话 ,
            学历 ,
            毕业学校 ,
            专业 ,
            num
          )
		  SELECT '',试验室编码,标段名称,单位名称,试验室名称,'','','' ,'' ,'' ,'' ,'' ,'' ,'' ,'' ,0 FROM dbo.v_bs_codeName WHERE 试验室编码=@modelID
	END
  ELSE
	BEGIN
		INSERT #tmp
		        (
				userid, 
				试验室编码 ,
		          标段名称 ,
		          单位名称 ,
		          试验室名称 ,
		          姓名 ,
		          性别 ,
		          年龄 ,
		          技术职称 ,
		          职务 ,
		          工作年限 ,
		          联系电话 ,
		          学历 ,
		          毕业学校 ,
		          专业 ,
		          num
		        )
SELECT  ID,试验室编码,标段名称,单位名称,试验室名称,姓名,性别,年龄,技术职称,职务,工作年限,联系电话,学历,毕业学校,专业,1 FROM  dbo.v_bs_userSummary WHERE 试验室编码=@modelID

	END
    
 
	
	  FETCH NEXT FROM cur INTO @modelID
	END
	CLOSE cur
	DEALLOCATE cur


  declare @totalcounts int
  select @totalcounts=count(id) from #tmp

    --取得分页总数

	  declare @pageIndex int --总数/页大小
    declare @lastcount int --总数%页大小  

	 set @pageIndex = @totalcounts/@pageSize
    set @lastcount = @totalcounts%@pageSize
    if @lastcount > 0
        set @pageCount = @pageIndex + 1
    else
        set @pageCount = @pageIndex 

 
	--select @Counts=count(ID) from #tmp where  id>(@pageSize*(@page-1)) and id<=(@pageSize*(@page))  

		SET @Counts=@totalcounts

	select * from #tmp where id>(@pageSize*(@page-1)) and id<=(@pageSize*@page)  

END

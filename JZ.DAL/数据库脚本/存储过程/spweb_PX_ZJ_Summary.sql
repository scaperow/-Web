USE [SYGLDB_XiCheng]
GO
/****** Object:  StoredProcedure [dbo].[spweb_PX_ZJ_Summary]    Script Date: 2013/8/15 8:44:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		张宏伟
-- Create date: 2013-7-22
-- Description:	平行
-- =============================================
ALTER PROCEDURE [dbo].[spweb_PX_ZJ_Summary]	
AS

BEGIN

 

	
 
 DECLARE 
	@modelID varchar(50),--游标中使用
	@testroomid VARCHAR(50),
	@sqls nvarchar(4000)	
 


CREATE TABLE #t1
        ( BGRQ DATETIME,
          ModelIndex VARCHAR(50) ,
          TestRoomCode VARCHAR(50),
          ZjCount INT
        )
		 
		 CREATE TABLE #t2
        ( BGRQ DATETIME,
          ModelIndex VARCHAR(50) ,
          TestRoomCode VARCHAR(50),
          JzCount INT
        )


 
	DECLARE cur CURSOR FOR



SELECT DISTINCT ModelIndex FROM  sys_biz_reminder_Itemfrequency WHERE IsActive=1

	OPEN cur
	FETCH NEXT FROM cur INTO @modelID
	WHILE @@FETCH_STATUS = 0
	BEGIN
	 
  
    SET @sqls='INSERT #t1
		        ( BGRQ ,
		          ModelIndex ,
		          TestRoomCode ,
		          ZjCount
		        ) select a.BGRQ,'''+@modelID+''',LEFT(a.SCPT,16),count(1) from [biz_norm_extent_'+@modelID+'] a 
	   where (a.trytype=''自检'' or a.trytype=''见证'') and a.BGRQ is not null GROUP BY a.BGRQ, LEFT(a.SCPT,16) '
	   EXEC	sp_executesql @sqls 


	   SET @sqls='INSERT #t2
		        ( BGRQ ,
		          ModelIndex ,
		          TestRoomCode ,
		          JzCount
		        ) select a.BGRQ,'''+@modelID+''',LEFT(a.SCPT,16),count(1) from [biz_norm_extent_'+@modelID+'] a 
	   where trytype=''见证'' and a.BGRQ is not null  GROUP BY a.BGRQ, LEFT(a.SCPT,16) '
		EXEC sp_executesql @sqls 
		
		
		  
	 
	   
	   FETCH NEXT FROM cur INTO  @modelID
	END

	CLOSE cur
	DEALLOCATE cur	


	--SELECT COUNT(*) FROM #t1

	--SELECT COUNT(*) FROM #t2



SELECT a.*, b.BGRQ AS bgrq1, b.ModelIndex AS modelindex1, b.TestRoomCode AS
testroomcode1, b.JzCount INTO #t3 FROM #t1 a
FULL JOIN #t2 b ON a.BGRQ = b.BGRQ AND 
a.ModelIndex = b.ModelIndex AND 
a.TestRoomCode = b.TestRoomCode

 

UPDATE #t3 SET BGRQ=bgrq1,ModelIndex=modelindex1,TestRoomCode=testroomcode1,ZjCount=0
WHERE BGRQ IS NULL

UPDATE #t3 SET JzCount=0 WHERE JzCount IS NULL

TRUNCATE TABLE dbo.biz_ZJ_JZ_Summary

INSERT dbo.biz_ZJ_JZ_Summary
        ( BGRQ ,
          ModelIndex ,
          TestRoomCode ,
          ZjCount ,
          JzCount,
		  JLCompanyName,
		  JLCompanyCode,
		  ModelName
        )
SELECT 
 a.BGRQ ,
   a.ModelIndex ,
        a.TestRoomCode ,           
		a.ZjCount ,
        a.JzCount,c.Description,d.NodeCode,e.Description FROM #t3 a JOIN dbo.v_codeName b ON LEFT(a.TestRoomCode,12)=b.NodeCode
		JOIN  dbo.sys_engs_CompanyInfo c
		ON CHARINDEX(b.RalationID,c.ConstructionCompany)>0
		JOIN dbo.sys_engs_Tree d
		ON c.ID=d.RalationID
		JOIN dbo.sys_biz_Module e 
		ON a.ModelIndex=e.ID



END

 



 





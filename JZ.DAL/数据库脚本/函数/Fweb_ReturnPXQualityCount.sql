USE [SYGLDB_XiCheng]
GO
/****** Object:  UserDefinedFunction [dbo].[Fweb_ReturnPXQualityCount]    Script Date: 2013/8/15 8:41:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER FUNCTION [dbo].[Fweb_ReturnPXQualityCount]
 (
     @ModelIndex VARCHAR(50),
	 @SGRoomCode VARCHAR(50),
	 @StartDate DATETIME,
	 @EndDate DATETIME
    )

	RETURNS INT
	BEGIN
  
		DECLARE @count INT  

	DECLARE @tmp_px_0 TABLE
	(
	chartDate VARCHAR(20),
	zjCount INT,
	pxjzCount INT	
	)

	DECLARE @tmp_px_1 TABLE
	(
	chartDate VARCHAR(20),
	countnum INT	
	)

	DECLARE @tmp_px_2 TABLE
	(
	chartDate VARCHAR(20),
	countnum INT	
	)
	
	DECLARE @tmp_px_3 TABLE
	(
	chartDate1 VARCHAR(20),
	chartDate2 VARCHAR(20),
	zjCount INT,
	pxjzCount INT
	)

INSERT @tmp_px_1
        ( chartDate, countnum )
SELECT BGRQ,ZjCount FROM dbo.biz_ZJ_JZ_Summary WHERE ModelIndex=@ModelIndex AND TestRoomCode=@SGRoomCode AND BGRQ>@StartDate AND BGRQ<@EndDate

INSERT @tmp_px_2
        ( chartDate, countnum )
SELECT PXBGRQ,COUNT(1) FROM  dbo.biz_px_relation_web WHERE
  ModelIndex=@ModelIndex
   AND  LEFT(SGTestRoomCode,16)=@SGRoomCode
   AND   SGBGRQ>@StartDate 
   AND   SGBGRQ<@EndDate 
   AND  PXBGRQ>@StartDate 
   AND  PXBGRQ<@EndDate GROUP BY PXBGRQ



   INSERT @tmp_px_3
		        ( chartDate1 ,
		          chartDate2 ,
		          zjCount ,
		          pxjzCount
		        )
		SELECT a.chartDate,b.chartDate,a.countnum,b.countnum FROM @tmp_px_1 a FULL JOIN @tmp_px_2 b ON a.chartDate = b.chartDate
		
		UPDATE @tmp_px_3 SET chartDate1=chartDate2 WHERE chartDate1 IS NULL
		
		
		INSERT @tmp_px_0
		        ( chartDate, zjCount, pxjzCount )
		SELECT chartDate1,zjCount,pxjzCount FROM  @tmp_px_3 

		update @tmp_px_0 set zjCount=0 where zjCount is null
		update @tmp_px_0 set pxjzCount=0 where pxjzCount is null
  

  SELECT @count=COUNT(pxjzCount) FROM @tmp_px_0


	DECLARE @n INT--判断dt是否为空  
    SELECT @n=COUNT(1) FROM @tmp_px_0
	IF @n>0
		BEGIN
			  DECLARE @maxdate DATETIME--获取dt中最大的时间
			  DECLARE @mindate DATETIME--获取dt中最小的时间			  
			  SELECT TOP 1 @mindate=chartDate FROM @tmp_px_0  ORDER BY chartDate ASC    
			  SELECT TOP 1 @maxdate=chartDate FROM @tmp_px_0  ORDER BY chartDate DESC   
			  
			  DECLARE @CountList INT--计算间隔几天
			  SET @CountList=DATEDIFF(day,@mindate, @maxdate )

			  DECLARE @NAverage INT--最大时间减去最小时间除以3,得到等分
			  SET @NAverage=@CountList/3


			  DECLARE @px1 FLOAT
			  DECLARE @px2 FLOAT
			  DECLARE @px3 FLOAT 
			  
			  DECLARE @zj1 FLOAT
			  DECLARE @zj2 FLOAT
			  DECLARE @zj3 FLOAT            


			  SELECT @px1=SUM(pxjzCount),@zj1=SUM(zjCount) FROM @tmp_px_0 WHERE chartDate>=@mindate AND chartDate<DATEADD(dd,@NAverage,@mindate)

			   SELECT @px2=SUM(pxjzCount),@zj2=SUM(zjCount) FROM @tmp_px_0 WHERE chartDate>=DATEADD(dd,@NAverage,@mindate) AND chartDate<DATEADD(dd,@NAverage*2,@mindate)

			    SELECT @px3=SUM(pxjzCount),@zj3=SUM(zjCount) FROM @tmp_px_0 WHERE chartDate>=DATEADD(dd,@NAverage*2,@mindate) AND chartDate<@maxdate
			
			DECLARE @B1 FLOAT
			DECLARE @B2 FLOAT
			DECLARE @B3 FLOAT     
			
				set @B1=0.0
				set @B2=0.0
				set @B3=0.0

			IF @zj1!=0
				BEGIN
				SET	@B1=(@px1/@zj1)*10
				END
			IF @zj2!=0
				BEGIN              
				SET	@B2=(@px2/@zj2)*10
				END
			IF @zj3!=0
				BEGIN              
				SET	@B3=(@px3/@zj3)*10
				END

			DECLARE @C3 FLOAT
  
			SET @C3=(@B1+@B2+@B3)/3

			DECLARE @E1 FLOAT
			DECLARE @E2 FLOAT
			DECLARE @E3 FLOAT     

			SET	@E1=(@B1-@C3)*(@B1-@C3)
			SET	@E2=(@B2-@C3)*(@B2-@C3)
			SET	@E3=(@B3-@C3)*(@B3-@C3)
			  
			  DECLARE @G3 FLOAT
			  
			  SET @G3=(@E1+@E2+@E3)/3

				DECLARE @F4 FLOAT
				SET @F4=SQRT(@G3)              
			  
			  IF @C3=0  
				  BEGIN
					SET @count=0             
				 END   
			  ELSE
				  BEGIN
					  SET @count=100-10*@F4                
				  END         
		END 
	ELSE
		BEGIN
			
			SET @count=0
		END
	
		RETURN @count
	END
    